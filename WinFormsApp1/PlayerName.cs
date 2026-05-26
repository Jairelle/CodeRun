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
    public partial class PlayerName : Form
    {
        [DllImport("user32.dll")] static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll")] static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")] static extern IntPtr SetFocus(IntPtr hWnd);
        [DllImport("user32.dll")] static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")] static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);
        [DllImport("user32.dll")] static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
        [DllImport("kernel32.dll")] static extern uint GetCurrentThreadId();
        [DllImport("user32.dll")] static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        const uint WM_KEYDOWN = 0x0100;
        const uint WM_KEYUP = 0x0101;
        const uint WM_MOUSEMOVE = 0x0200;
        const uint WM_LBUTTONDOWN = 0x0201;
        const uint WM_LBUTTONUP = 0x0202;

        const int VK_A = 0x41;
        const int VK_D = 0x44;
        const int VK_SPACE = 0x20;
        const int VK_LEFT = 0x25;
        const int VK_RIGHT = 0x27;

        public static readonly string PlayerDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "BridgeGame", "playerdata.txt"
        );

        private Process unityProcess;
        private IntPtr unityHwnd = IntPtr.Zero;
        private bool gameRunning = false;
        private string selectedDifficulty;

        // ── NEW: signal watcher ───────────────────────────────────────────
        private System.Windows.Forms.Timer unityWatchTimer;

        private static readonly string SignalPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "BridgeGame", "signal.txt"
        );
        // ─────────────────────────────────────────────────────────────────

        public PlayerName(string difficulty = "Easy")
        {
            InitializeComponent();
            selectedDifficulty = difficulty;
            this.KeyPreview = true;
            this.Load += PlayerName_Load;
        }

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

        private void PlayerName_Load(object sender, EventArgs e)
        {
            this.ActiveControl = pictureBox1;

            pictureBox2.BackColor = Color.Transparent;
            if (pictureBox2.Image != null)
            {
                Bitmap bmp = new Bitmap(pictureBox2.Image);
                bmp.MakeTransparent(Color.White);
                pictureBox2.Image = bmp;
            }

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

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (gameRunning) return;

            string playerName = textBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(playerName) || playerName == "Enter Name")
            {
                MessageBox.Show("Please enter your name before playing!",
                                "Name Required",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }

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

                unityProcess = Process.Start(psi);
                unityProcess.WaitForInputIdle();
                Thread.Sleep(1000);

                SetParent(unityProcess.MainWindowHandle, this.Handle);
                MoveWindow(unityProcess.MainWindowHandle, 0, 0,
                           this.ClientSize.Width, this.ClientSize.Height, true);

                unityHwnd = unityProcess.MainWindowHandle;
                gameRunning = true;

                textBox1.Visible = false;
                pictureBox1.Visible = false;
                pictureBox2.Visible = false;
                pictureBox3.Visible = false;

                FocusUnity();

                // ── NEW: start watching for Unity signals ─────────────────
                unityWatchTimer = new System.Windows.Forms.Timer();
                unityWatchTimer.Interval = 1000;
                unityWatchTimer.Tick += CheckUnitySignal;
                unityWatchTimer.Start();
                // ─────────────────────────────────────────────────────────
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error launching game: " + ex.Message,
                                "Launch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── NEW: detect BACK_TO_HOME → open Form1 ────────────────────────
        private void CheckUnitySignal(object sender, EventArgs e)
        {
            if (File.Exists(SignalPath))
            {
                string signal = File.ReadAllText(SignalPath).Trim();

                if (signal == "BACK_TO_HOME")
                {
                    File.Delete(SignalPath);
                    unityWatchTimer.Stop();
                    gameRunning = false;

                    if (unityProcess != null && !unityProcess.HasExited)
                        unityProcess.Kill();

                    HomeTab mainMenu = new HomeTab();  // ← opens THIS Form1 screen
                    mainMenu.Show();
                    this.Close();
                    return;
                }
            }

            if (unityProcess != null && unityProcess.HasExited && gameRunning)
            {
                unityWatchTimer.Stop();
                gameRunning = false;

                HomeTab mainMenu = new HomeTab();
                mainMenu.Show();
                this.Close();
            }
        }
        // ─────────────────────────────────────────────────────────────────

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

        private IntPtr MakeLParam(int x, int y) =>
            (IntPtr)(((y & 0xFFFF) << 16) | (x & 0xFFFF));

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (gameRunning && unityHwnd != IntPtr.Zero)
                PostMessage(unityHwnd, WM_MOUSEMOVE, IntPtr.Zero, MakeLParam(e.X, e.Y));
            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (gameRunning && unityHwnd != IntPtr.Zero && e.Button == MouseButtons.Left)
            {
                FocusUnity();

                float xPct = (float)e.X / ClientSize.Width;
                float yPct = (float)e.Y / ClientSize.Height;

                float quizTop = 0.55f;
                float quizBot = 0.92f;
                float quizLeft = 0.18f;
                float quizRight = 0.82f;
                float quizMidH = (quizTop + quizBot) / 2f;
                float quizMidW = (quizLeft + quizRight) / 2f;

                if (xPct >= quizLeft && xPct <= quizRight &&
                    yPct >= quizTop && yPct <= quizBot)
                {
                    bool leftCol = xPct < quizMidW;
                    bool topRow = yPct < quizMidH;

                    int vk;
                    if (topRow && leftCol) vk = 0x31;
                    else if (topRow && !leftCol) vk = 0x32;
                    else if (!topRow && leftCol) vk = 0x33;
                    else vk = 0x34;

                    PostMessage(unityHwnd, WM_KEYDOWN, (IntPtr)vk, IntPtr.Zero);
                    Thread.Sleep(50);
                    PostMessage(unityHwnd, WM_KEYUP, (IntPtr)vk, IntPtr.Zero);
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (gameRunning && unityHwnd != IntPtr.Zero)
                if (e.Button == MouseButtons.Left)
                    PostMessage(unityHwnd, WM_LBUTTONUP, IntPtr.Zero, MakeLParam(e.X, e.Y));
            base.OnMouseUp(e);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (gameRunning) FocusUnity();
        }

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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (gameRunning && unityHwnd != IntPtr.Zero)
            {
                int vk = GetVK(keyData);
                if (vk != -1)
                {
                    PostMessage(unityHwnd, WM_KEYDOWN, (IntPtr)vk, IntPtr.Zero);
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

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