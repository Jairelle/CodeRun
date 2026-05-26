namespace CodeRun
{
    partial class Story
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Story));
            BackButtonStory = new PictureBox();
            pageSetupDialog1 = new PageSetupDialog();
            axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            ((System.ComponentModel.ISupportInitialize)axWindowsMediaPlayer1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BackButtonStory).BeginInit();
            SuspendLayout();
            // 
            // axWindowsMediaPlayer1
            // 
            axWindowsMediaPlayer1.Dock = DockStyle.Fill;
            axWindowsMediaPlayer1.Enabled = true;
            axWindowsMediaPlayer1.Location = new Point(0, 0);
            axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            axWindowsMediaPlayer1.OcxState = (AxHost.State)resources.GetObject("axWindowsMediaPlayer1.OcxState");
            axWindowsMediaPlayer1.Size = new Size(1373, 669);
            axWindowsMediaPlayer1.TabIndex = 8;
            axWindowsMediaPlayer1.Enter += axWindowsMediaPlayer1_Enter;
            // 
            // BackButtonStory
            // 
            BackButtonStory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BackButtonStory.BackColor = Color.Transparent;
            BackButtonStory.BackgroundImageLayout = ImageLayout.Center;
            BackButtonStory.Image = (Image)resources.GetObject("BackButtonStory.Image");
            BackButtonStory.Location = new Point(1267, 12);
            BackButtonStory.Name = "BackButtonStory";
            BackButtonStory.Size = new Size(94, 38);
            BackButtonStory.SizeMode = PictureBoxSizeMode.StretchImage;
            BackButtonStory.TabIndex = 7;
            BackButtonStory.TabStop = false;
            BackButtonStory.Click += BackButtonStory_Click_1;
            // 
            // Story
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1373, 669);
            Controls.Add(BackButtonStory);
            Controls.Add(axWindowsMediaPlayer1);
            Name = "Story";
            Text = "Story";
            WindowState = FormWindowState.Maximized;
            FormClosed += Story_FormClosed;
            Load += Story_Load;
            ((System.ComponentModel.ISupportInitialize)axWindowsMediaPlayer1).EndInit();
            ((System.ComponentModel.ISupportInitialize)BackButtonStory).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox BackButtonStory;
        private PageSetupDialog pageSetupDialog1;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
    }
}