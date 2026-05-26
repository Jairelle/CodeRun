using CodeRun;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace WinFormsApp1
{
    // The main home screen/launcher of our application.
    public partial class HomeTab : Form
    {
        // ── Win32 API Imports ────────────────────────────────────────────────
        // We use native Windows DLLs (user32 and kernel32) to do things that standard 
        // WinForms code can't do—specifically, stealing the Unity game window and 
        // stitching it directly into our Form's interface.
        
        // SetParent lets us force the Unity window to become a child control of our form.
        [DllImport("user32.dll")] static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        
        // MoveWindow helps us resize and reposition the Unity window to perfectly fit inside our form.
        [DllImport("user32.dll")] static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        
        // SetFocus forces Windows to focus the Unity control so it starts receiving inputs.
        [DllImport("user32.dll")] static extern IntPtr SetFocus(IntPtr hWnd);
        
        // SetForegroundWindow brings the Unity window to the front of the drawing stack.
        [DllImport("user32.dll")] static extern bool SetForegroundWindow(IntPtr hWnd);
        
        // AttachThreadInput connects the input processing of our main form thread to the Unity game thread.
        // Without this, keyboard and mouse inputs wouldn't register inside the embedded game.
        [DllImport("user32.dll")] static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);
        
        // GetWindowThreadProcessId helps us figure out which thread belongs to the Unity window.
        [DllImport("user32.dll")] static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
        
        // GetCurrentThreadId gets the ID of our C# Windows Forms main thread.
        [DllImport("kernel32.dll")] static extern uint GetCurrentThreadId();
        
        // SendMessage lets us send low-level window messages to control the Unity window state.
        [DllImport("user32.dll")] static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        const int WM_ACTIVATE = 0x0006;
        const int WA_CLICKACTIVE = 2;
        
        // ── State Variables ──────────────────────────────────────────────────
        private Process unityProcess; // Holds the reference to the running Unity game process.
        private bool gameRunning = false; // Flag to keep track of whether the game is currently active.

        public HomeTab()
        {
            InitializeComponent();
        }

        // ── Unity Input and Focus Management ──────────────────────────────────
        // This is a helper method to force focus onto the Unity game.
        // In embedded applications, focus gets lost easily when clicking around.
        // We link the threads and force the OS to direct input focus to the Unity window.
        private void FocusUnity()
        {
            if (unityProcess == null || unityProcess.HasExited) return;

            IntPtr unityHwnd = unityProcess.MainWindowHandle;
            if (unityHwnd == IntPtr.Zero) return;

            // Find our main thread ID and the Unity thread ID
            uint currentThread = GetCurrentThreadId();
            uint unityThread = GetWindowThreadProcessId(unityHwnd, IntPtr.Zero);

            // Connect input queues, set focus, and then disconnect them cleanly
            AttachThreadInput(currentThread, unityThread, true);
            SetForegroundWindow(unityHwnd);
            SetFocus(unityHwnd);
            AttachThreadInput(currentThread, unityThread, false);
        }

        // If the user clicks anywhere on our Form background, we redirect focus back to Unity
        // so they don't lose control of the player.
        protected override void OnMouseClick(MouseEventArgs e)
        {
            FocusUnity();
            base.OnMouseClick(e);
        }

        // When the user alt-tabs away and then switches back to our application,
        // automatically refocus the Unity window if a game session is currently active.
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (gameRunning) FocusUnity();
        }

        // Leftover handler from design changes (unused)
        private void button1_Click(object sender, EventArgs e)
        {
        }

        // Keeps the Unity game window filling the form even if the user resizes the main window.
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (unityProcess != null && !unityProcess.HasExited)
                MoveWindow(unityProcess.MainWindowHandle, 0, 0, this.ClientSize.Width, this.ClientSize.Height, true);
        }

        // Critical safety cleanup: If the user closes the WinForms launcher, we MUST 
        // make sure the embedded Unity process is killed as well, otherwise it will
        // run in the background forever as a zombie process.
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (unityProcess != null && !unityProcess.HasExited)
                unityProcess.Kill();
            base.OnFormClosing(e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        // Open the Credits dialog
        private void button2_Click(object sender, EventArgs e)
        {
            CreditsForm credits = new CreditsForm();
            credits.ShowDialog(); // ShowDialog makes it a modal pop-up (prevents interacting with menu behind it)
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
        }

        // Runs when the Home Tab form is loaded. We hide the bottom gray bar control
        // here to ensure the pixel art background extends all the way down.
        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox2.Visible = false;
        }

        // Handles the Play button click: goes to the difficulty selection screen
        private void PlayDiff_Click(object sender, EventArgs e)
        {
            // Create the difficulty screen and pass 'this' (the current HomeTab) 
            // so it can show us again when the user backs out.
            PlayDifficulties difficulties = new PlayDifficulties(this);

            // Make it match our window style and fill the space
            difficulties.FormBorderStyle = FormBorderStyle.None;
            difficulties.StartPosition = FormStartPosition.CenterParent;
            difficulties.Size = this.Size;

            difficulties.Show();
            this.Hide(); // Hide ourselves so only the difficulty selection is visible
        }

        private void pictureBox1_Click_2(object sender, EventArgs e)
        {
        }

        // Handles the Story button click: opens the animated/media intro screen
        private void button3_Click(object sender, EventArgs e)
        {
            // Create the Story screen and pass 'this' home screen so we can return here
            Story storyScreen = new Story(this);
            storyScreen.Show();
            this.Hide(); // Hide the main menu
        }
    }
}