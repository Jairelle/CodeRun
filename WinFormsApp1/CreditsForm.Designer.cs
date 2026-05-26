namespace WinFormsApp1
{
    partial class CreditsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreditsForm));
            pictureBox1 = new PictureBox();
            BackButtonCredits = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BackButtonCredits).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.DarkViolet;
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1360, 626);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // BackButtonCredits
            // 
            BackButtonCredits.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BackButtonCredits.BackColor = Color.Transparent;
            BackButtonCredits.BackgroundImageLayout = ImageLayout.Center;
            BackButtonCredits.Image = (Image)resources.GetObject("BackButtonCredits.Image");
            BackButtonCredits.Location = new Point(1254, 12);
            BackButtonCredits.Name = "BackButtonCredits";
            BackButtonCredits.Size = new Size(94, 40);
            BackButtonCredits.SizeMode = PictureBoxSizeMode.StretchImage;
            BackButtonCredits.TabIndex = 5;
            BackButtonCredits.TabStop = false;
            BackButtonCredits.Click += BackButtonCredits_Click;
            // 
            // CreditsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1360, 626);
            Controls.Add(BackButtonCredits);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "CreditsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CreditsForm";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)BackButtonCredits).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private PictureBox BackButtonCredits;
    }
}