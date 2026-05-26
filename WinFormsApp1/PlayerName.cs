using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace WinFormsApp1
{
    // This form handles player name entry and acts as the container/host that embeds the
    // Unity game window directly inside our WinForms layout.
    public partial class PlayerName : Form
    {
        // ── Win32 API Imports ────────────────────────────────────────────────
        // We use low-level Windows APIs here to hook the Unity game window and redirect 
        // user controls (mouse clicks/keyboard presses) into it.
        
        [DllImport("user32.dll")] static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll")] static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")] static extern IntPtr SetFocus(IntPtr hWnd);
        [DllImport("user32.dll")] static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")] static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);
        [DllImport("user32.dll")] static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
        [DllImport("kernel32.dll")] static extern uint GetCurrentThreadId();
        
        // PostMessage sends window messages (like key presses or mouse clicks) directly 
        // to the Unity process queue asynchronously.
        [DllImport("user32.dll")] static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        // Windows Message codes we use to tell Unity what controls are being pressed
        const uint WM_KEYDOWN = 0x0100;
        const uint WM_KEYUP = 0x0101;
        const uint WM_MOUSEMOVE = 0x0200;
        const uint WM_LBUTTONDOWN = 0x0201;
        const uint WM_LBUTTONUP = 0x0202;

        // Virtual key codes for player movement (A, D, Space, Left, Right)
        const int VK_A = 0x41;
        const int VK_D = 0x44;
        const int VK_SPACE = 0x20;
        const int VK_LEFT = 0x25;
        const int VK_RIGHT = 0x27;

        // File path where we save player profile details (name and chosen difficulty)
        public static readonly string PlayerDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "BridgeGame", "playerdata.txt"
        );

        // ── State Variables ──────────────────────────────────────────────────
        private Process unityProcess; // Holds the process reference to the embedded Unity game
        private IntPtr unityHwnd = IntPtr.Zero; // Window handle of the Unity game
        private bool gameRunning = false; // Flag to track if the game is active inside this container
        private string selectedDifficulty; // Game difficulty selected by the player
        private HomeTab parentHome; // Saved reference to return to the home screen
        private System.Windows.Forms.Timer unityWatchTimer; // Timer that polls for signals from Unity

        // File path used as a communication bridge between C# and Unity.
        // Unity writes messages here (like "BACK_TO_HOME"), and C# reads it to respond.
        private static readonly string SignalPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "BridgeGame", "signal.txt"
        );

        // Constructor: sets the parent home reference and difficulty level.
        public PlayerName(HomeTab home, string difficulty = "Easy")
        {
            InitializeComponent();
            parentHome = home;
            selectedDifficulty = difficulty;
            this.KeyPreview = true; // Allows the Form to intercept keyboard inputs before child controls see them
            this.Load += PlayerName_Load;
        }

        // Helper to resolve the correct file path to the built Unity executable based on difficulty
        private string GetGamePath()
        {
            switch (selectedDifficulty)
            {
                case "Easy":
                    return @"C:\Users\oreto\source\repos\WinFormsApp1\GameSceneEasyMode\My project (2).exe";
                case "Medium":
                    return @"C:\Users\oreto\source\repos\WinFormsApp1\GameSceneMediumMode\My project (2).exe";
                case "Hard":
                    return @"C:\Users\oreto\source\repos\WinFormsApp1\GameSceneHardMode\My project (2).exe";
                default:
                    return @"C:\Users\oreto\source\repos\WinFormsApp1\GameSceneEasyMode\My project (2).exe";
            }
        }

        // Setup textbox placeholding and picturebox transparency on screen load
        private void PlayerName_Load(object sender, EventArgs e)
        {
            this.ActiveControl = pictureBox1; // Set focus away from textbox initially

            // Remove visual backgrounds from graphics and apply transparency keying
            pictureBox2.BackColor = Color.Transparent;
            if (pictureBox2.Image != null)
            {
                Bitmap bmp = new Bitmap(pictureBox2.Image);
                bmp.MakeTransparent(Color.White);
                pictureBox2.Image = bmp;
            }

            // Textbox placeholder behavior: "Enter Name"
            textBox1.Text = "Enter Name";
            textBox1.ForeColor = Color.Gray;

            textBox1.GotFocus += (s, ev) =>
            {
                if (textBox1.Text == "Enter Name")
                {
                    textBox1.Text = "";
                    textBox1.ForeColor = Color.White;
                }
            };

            textBox1.LostFocus += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    textBox1.Text = "Enter Name";
                    textBox1.ForeColor = Color.Gray;
                }
            };
        }

        // Handles the Play button (pictureBox3) click: saves data and launches Unity.
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (gameRunning) return;

            string playerName = textBox1.Text.Trim();

            // Validate that a player name has actually been entered
            if (string.IsNullOrWhiteSpace(playerName) || playerName == "Enter Name")
            {
                MessageBox.Show("Please enter your name before playing!",
                                "Name Required",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }

            // Save player name and difficulty to text file so Unity can load it
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(PlayerDataPath));
                File.WriteAllText(PlayerDataPath,
                    $"Name={playerName}{Environment.NewLine}" +
                    $"Difficulty={selectedDifficulty}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not save player data: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string gamePath = GetGamePath();

            if (!File.Exists(gamePath))
            {
                MessageBox.Show("Game executable not found at:\n" + gamePath,
                                "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Set up arguments to tell Unity to start inside our window container
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(gamePath)
                {
                    Arguments =
                        "-parentHWND " + this.Handle.ToInt32() +
                        " -screen-width " + this.ClientSize.Width +
                        " -screen-height " + this.ClientSize.Height +
                        " -difficulty " + selectedDifficulty,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                // Launch Unity and wait briefly for its main window to initialize
                unityProcess = Process.Start(psi);
                unityProcess.WaitForInputIdle();
                Thread.Sleep(1000);

                // Embed the Unity window into this WinForms Form container
                SetParent(unityProcess.MainWindowHandle, this.Handle);
                MoveWindow(unityProcess.MainWindowHandle, 0, 0,
                           this.ClientSize.Width, this.ClientSize.Height, true);

                unityHwnd = unityProcess.MainWindowHandle;
                gameRunning = true;

                // Hide player name entry UI controls so only the game is visible
                textBox1.Visible = false;
                pictureBox1.Visible = false;
                pictureBox2.Visible = false;
                pictureBox3.Visible = false;

                FocusUnity();

                // Start watching for file signals from the Unity game
                unityWatchTimer = new System.Windows.Forms.Timer();
                unityWatchTimer.Interval = 1000;
                unityWatchTimer.Tick += CheckUnitySignal;
                unityWatchTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error launching game: " + ex.Message,
                                "Launch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Monitors for back-to-home signals from Unity or game crashes.
        private void CheckUnitySignal(object sender, EventArgs e)
        {
            // Case 1: Check if Unity wrote the "BACK_TO_HOME" command to signal.txt
            if (File.Exists(SignalPath))
            {
                string signal = File.ReadAllText(SignalPath).Trim();

                if (signal == "BACK_TO_HOME")
                {
                    File.Delete(SignalPath); // Clear the signal file
                    unityWatchTimer.Stop();
                    gameRunning = false;

                    if (unityProcess != null && !unityProcess.HasExited)
                        unityProcess.Kill();

                    parentHome.Show(); // Return to original home menu
                    this.Close(); // Close this container
                    return;
                }
            }

            // Case 2: Check if the Unity process crashed or exited on its own
            if (unityProcess != null && unityProcess.HasExited && gameRunning)
            {
                unityWatchTimer.Stop();
                gameRunning = false;

                parentHome.Show(); // Return to original home menu
                this.Close(); // Close this container
            }
        }

        // Brings focus back to Unity window handle
        private void FocusUnity()
        {
            if (unityHwnd == IntPtr.Zero) return;
            uint currentThread = GetCurrentThreadId();
            uint unityThread = GetWindowThreadProcessId(unityHwnd, IntPtr.Zero);
            AttachThreadInput(currentThread, unityThread, true);
            SetForegroundWindow(unityHwnd);
            SetFocus(unityHwnd);
            AttachThreadInput(currentThread, unityThread, false);
        }

        // Helper to translate x, y coordinates to lParam format for Win32 API calls
        private IntPtr MakeLParam(int x, int y) =>
            (IntPtr)(((y & 0xFFFF) << 16) | (x & 0xFFFF));

        // Redirect mouse movement coordinates to the child Unity process
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (gameRunning && unityHwnd != IntPtr.Zero)
                PostMessage(unityHwnd, WM_MOUSEMOVE, IntPtr.Zero, MakeLParam(e.X, e.Y));
            base.OnMouseMove(e);
        }

        // Translates clicks on the WinForms wrapper to keyboard/mouse signals for the embedded game.
        // Specifically redirects quiz choice clicks in the bottom half of the display.
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (gameRunning && unityHwnd != IntPtr.Zero && e.Button == MouseButtons.Left)
            {
                FocusUnity();

                float xPct = (float)e.X / ClientSize.Width;
                float yPct = (float)e.Y / ClientSize.Height;

                // Relative boundaries of the quiz container in Unity's screen layout
                float quizTop = 0.55f;
                float quizBot = 0.92f;
                float quizLeft = 0.18f;
                float quizRight = 0.82f;
                float quizMidH = (quizTop + quizBot) / 2f;
                float quizMidW = (quizLeft + quizRight) / 2f;

                // Detect click coordinates and translate them to keyboard key simulation
                if (xPct >= quizLeft && xPct <= quizRight &&
                    yPct >= quizTop && yPct <= quizBot)
                {
                    bool leftCol = xPct < quizMidW;
                    bool topRow = yPct < quizMidH;

                    int vk;
                    if (topRow && leftCol) vk = 0x31;      // Key '1'
                    else if (topRow && !leftCol) vk = 0x32; // Key '2'
                    else if (!topRow && leftCol) vk = 0x33;  // Key '3'
                    else vk = 0x34;                         // Key '4'

                    // Post keystrokes to Unity window handle
                    PostMessage(unityHwnd, WM_KEYDOWN, (IntPtr)vk, IntPtr.Zero);
                    Thread.Sleep(50);
                    PostMessage(unityHwnd, WM_KEYUP, (IntPtr)vk, IntPtr.Zero);
                }
            }
            base.OnMouseDown(e);
        }

        // Redirect mouse button up events to Unity
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (gameRunning && unityHwnd != IntPtr.Zero)
                if (e.Button == MouseButtons.Left)
                    PostMessage(unityHwnd, WM_LBUTTONUP, IntPtr.Zero, MakeLParam(e.X, e.Y));
            base.OnMouseUp(e);
        }

        // Keep Unity focused when switching active windows
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (gameRunning) FocusUnity();
        }

        // Resolves key codes (A, D, Space, Arrow keys, Numbers)
        private int GetVK(Keys key)
        {
            if (key == Keys.A || key == Keys.Left) return VK_A;
            if (key == Keys.D || key == Keys.Right) return VK_D;
            if (key == Keys.Space) return VK_SPACE;
            if (key == Keys.D1) return 0x31;
            if (key == Keys.D2) return 0x32;
            if (key == Keys.D3) return 0x33;
            if (key == Keys.D4) return 0x34;
            return -1;
        }

        // Intercepts key presses inside the C# Form and forwards them to Unity
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (gameRunning && unityHwnd != IntPtr.Zero)
            {
                int vk = GetVK(keyData);
                if (vk != -1)
                {
                    PostMessage(unityHwnd, WM_KEYDOWN, (IntPtr)vk, IntPtr.Zero);
                    return true; // Mark as handled so WinForms doesn't process it
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // Intercepts key releases inside the C# Form and forwards them to Unity
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (gameRunning && unityHwnd != IntPtr.Zero)
            {
                int vk = GetVK(e.KeyCode);
                if (vk != -1)
                {
                    PostMessage(unityHwnd, WM_KEYUP, (IntPtr)vk, IntPtr.Zero);
                    e.Handled = true;
                }
            }
            base.OnKeyUp(e);
        }

        // Safety check: Kill the Unity process when the container form is closed.
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (unityProcess != null && !unityProcess.HasExited)
                unityProcess.Kill();
            base.OnFormClosing(e);
        }

        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
    }
}