using System;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class PlayDifficulties : Form
    {
        public PlayDifficulties()
        {
            InitializeComponent();
        }

        private void PlayDifficulties_Load(object sender, EventArgs e) { }

        // ── Back button ───────────────────────────────────────────────────
        private void BackButtonCredits_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ── Easy Mode button (button3) ────────────────────────────────────
        private void button3_Click(object sender, EventArgs e)
        {
            PlayerName playerNameForm = new PlayerName("Easy");
            playerNameForm.Show();
            this.Hide();
        }

        // ── Medium Mode button (button2) ──────────────────────────────────
        private void button2_Click(object sender, EventArgs e)
        {
            PlayerName playerNameForm = new PlayerName("Medium");
            playerNameForm.Show();
            this.Hide();
        }

        // ── Hard Mode button (button1) ────────────────────────────────────
        private void button1_Click(object sender, EventArgs e)
        {
            PlayerName playerNameForm = new PlayerName("Hard");
            playerNameForm.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void pictureBox3_Click(object sender, EventArgs e) { }
    }
}