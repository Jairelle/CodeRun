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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Story));
            BackButtonStory = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)BackButtonStory).BeginInit();
            SuspendLayout();
            // 
            // BackButtonStory
            // 
            BackButtonStory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BackButtonStory.BackColor = Color.Transparent;
            BackButtonStory.BackgroundImageLayout = ImageLayout.Center;
            BackButtonStory.Image = (Image)resources.GetObject("BackButtonStory.Image");
            BackButtonStory.Location = new Point(1262, 12);
            BackButtonStory.Name = "BackButtonStory";
            BackButtonStory.Size = new Size(94, 40);
            BackButtonStory.SizeMode = PictureBoxSizeMode.StretchImage;
            BackButtonStory.TabIndex = 7;
            BackButtonStory.TabStop = false;
            BackButtonStory.Click += BackButtonStory_Click_1;
            // 
            // Story
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1408, 763);
            Controls.Add(BackButtonStory);
            Name = "Story";
            Text = "Story";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)BackButtonStory).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox BackButtonStory;
    }
}