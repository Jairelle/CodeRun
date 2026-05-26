using CodeRun;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class HomeTab : Form
    {
        [DllImport("user32.dll")] static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll")] static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")] static extern IntPtr SetFocus(IntPtr hWnd);
        [DllImport("user32.dll")] static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")] static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);
        [DllImport("user32.dll")] static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
        [DllImport("kernel32.dll")] static extern uint GetCurrentThreadId();
        //Gem
        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        const int WM_ACTIVATE = 0x0006;
        const int WA_CLICKACTIVE = 2;
        //START
        private Process unityProcess;
        private bool gameRunning = false;

        public HomeTab()
        {
            InitializeComponent();

            // Make buttons children of the PictureBox so transparency works
            /*button1.Parent = pictureBox1;
            button2.Parent = pictureBox1;
            button3.Parent = pictureBox1;*/
        }

        private void FocusUnity()
        {
            if (unityProcess == null || unityProcess.HasExited) return;

            IntPtr unityHwnd = unityProcess.MainWindowHandle;
            if (unityHwnd == IntPtr.Zero) return;

            uint currentThread = GetCurrentThreadId();
            uint unityThread = GetWindowThreadProcessId(unityHwnd, IntPtr.Zero);

            AttachThreadInput(currentThread, unityThread, true);
            SetForegroundWindow(unityHwnd);
            SetFocus(unityHwnd);
            AttachThreadInput(currentThread, unityThread, false);
        }
        //Gem
        /*private void FocusUnity()
        {
            if (unityProcess == null || unityProcess.HasExited) return;
            IntPtr unityHwnd = unityProcess.MainWindowHandle;

            // Force Windows to treat the Unity window as "active" within the parent
            SendMessage(unityHwnd, WM_ACTIVATE, WA_CLICKACTIVE, 0);
            SetFocus(unityHwnd);
        }*/

        // Clicking anywhere on the form refocuses Unity
        protected override void OnMouseClick(MouseEventArgs e)
        {
            FocusUnity();
            base.OnMouseClick(e);
        }

        // Switching back to the app refocuses Unity
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (gameRunning) FocusUnity();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*string gamePath = @"C:\Users\oreto\source\repos\WinFormsApp1\GameScene2\My project (2).exe";

            if (!System.IO.File.Exists(gamePath))
            {
                MessageBox.Show("Game executable not found at: " + gamePath);
                return;
            }

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(gamePath);
                psi.Arguments = "-parentHWND " + this.Handle.ToInt32() +
                                " -screen-width " + this.ClientSize.Width +
                                " -screen-height " + this.ClientSize.Height;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;

                unityProcess = Process.Start(psi);
                unityProcess.WaitForInputIdle();
                Thread.Sleep(1000);

                SetParent(unityProcess.MainWindowHandle, this.Handle);
                MoveWindow(unityProcess.MainWindowHandle, 0, 0, this.ClientSize.Width, this.ClientSize.Height, true);

                button1.Visible = false;
                gameRunning = true;

                FocusUnity();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error launching game: " + ex.Message);
            }*/
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (unityProcess != null && !unityProcess.HasExited)
                MoveWindow(unityProcess.MainWindowHandle, 0, 0, this.ClientSize.Width, this.ClientSize.Height, true);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (unityProcess != null && !unityProcess.HasExited)
                unityProcess.Kill();
            base.OnFormClosing(e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Create an instance of your new form
            CreditsForm credits = new CreditsForm();

            // Show the form
            credits.ShowDialog();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void PlayDiff_Click(object sender, EventArgs e)
        {

            // 1. Create the form object
            PlayDifficulties difficulties = new PlayDifficulties(this);

            // 2. Optional: Make it look integrated (same as Credits)
            difficulties.FormBorderStyle = FormBorderStyle.None;
            difficulties.StartPosition = FormStartPosition.CenterParent;
            difficulties.Size = this.Size; // If you want it to be full size

            // 3. Show it
            difficulties.Show();//Updated
            this.Hide();
        }

        private void pictureBox1_Click_2(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 1. Create an instance of your 'Story' form
            Story storyScreen = new Story();

            // 2. Show the story screen
            storyScreen.Show();

            // 3. Hide the main menu
            this.Hide();
        }
    }
    }
//}