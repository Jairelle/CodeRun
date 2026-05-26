namespace WinFormsApp1
{
    partial class HomeTab
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomeTab));
            button2 = new Button();
            Story = new Button();
            pictureBox1 = new PictureBox();
            PlayDiff = new Button();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Right;
            button2.BackColor = Color.Transparent;
            button2.BackgroundImage = (Image)resources.GetObject("button2.BackgroundImage");
            button2.BackgroundImageLayout = ImageLayout.Zoom;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button2.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button2.FlatStyle = FlatStyle.Flat;
            button2.ForeColor = Color.Transparent;
            button2.Location = new Point(840, 268);
            button2.Name = "button2";
            button2.Size = new Size(479, 264);
            button2.TabIndex = 2;
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // Story
            // 
            Story.Anchor = AnchorStyles.Left;
            Story.BackColor = Color.Transparent;
            Story.BackgroundImage = (Image)resources.GetObject("Story.BackgroundImage");
            Story.BackgroundImageLayout = ImageLayout.Zoom;
            Story.FlatAppearance.BorderSize = 0;
            Story.FlatAppearance.MouseDownBackColor = Color.Transparent;
            Story.FlatAppearance.MouseOverBackColor = Color.Transparent;
            Story.FlatStyle = FlatStyle.Flat;
            Story.ForeColor = Color.Transparent;
            Story.Location = new Point(95, 268);
            Story.Name = "Story";
            Story.Size = new Size(479, 264);
            Story.TabIndex = 3;
            Story.UseVisualStyleBackColor = false;
            Story.Click += button3_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1405, 753);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click_2;
            // 
            // PlayDiff
            // 
            PlayDiff.Anchor = AnchorStyles.None;
            PlayDiff.BackColor = Color.Transparent;
            PlayDiff.BackgroundImage = (Image)resources.GetObject("PlayDiff.BackgroundImage");
            PlayDiff.BackgroundImageLayout = ImageLayout.Zoom;
            PlayDiff.FlatAppearance.BorderSize = 0;
            PlayDiff.FlatAppearance.MouseDownBackColor = Color.Transparent;
            PlayDiff.FlatAppearance.MouseOverBackColor = Color.Transparent;
            PlayDiff.FlatStyle = FlatStyle.Flat;
            PlayDiff.ForeColor = Color.Transparent;
            PlayDiff.Location = new Point(480, 279);
            PlayDiff.Name = "PlayDiff";
            PlayDiff.Size = new Size(492, 243);
            PlayDiff.TabIndex = 5;
            PlayDiff.UseVisualStyleBackColor = false;
            PlayDiff.Click += PlayDiff_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Dock = DockStyle.Bottom;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(0, 665);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(1405, 88);
            pictureBox2.TabIndex = 6;
            pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Anchor = AnchorStyles.Top;
            pictureBox3.BackColor = Color.Transparent;
            pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
            pictureBox3.Location = new Point(146, 135);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(1152, 178);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 7;
            pictureBox3.TabStop = false;
            // 
            // HomeTab
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveBorder;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1405, 753);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox2);
            Controls.Add(PlayDiff);
            Controls.Add(Story);
            Controls.Add(button2);
            Controls.Add(pictureBox1);
            DoubleBuffered = true;
            ForeColor = SystemColors.ActiveCaption;
            Name = "HomeTab";
            Text = "HomeTab";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            Resize += Form1_Resize;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Button button2;
        private Button Story;
        private PictureBox pictureBox1;
        private Button PlayDiff;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
    }
}
