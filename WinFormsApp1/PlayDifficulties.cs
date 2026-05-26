using System;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class PlayDifficulties : Form
    {
        private HomeTab parentHome;

        public PlayDifficulties(HomeTab home)
        {
            InitializeComponent();
            parentHome = home;
        }

        private void PlayDifficulties_Load(object sender, EventArgs e) { }

        // ── Back button ───────────────────────────────────────────────────
        private void BackButtonCredits_Click(object sender, EventArgs e)
        {
            parentHome.Show();
            this.Close();
        }

        // ── Easy Mode button (button3) ────────────────────────────────────
        private void button3_Click(object sender, EventArgs e)
        {
            PlayerName playerNameForm = new PlayerName(parentHome, "Easy");
            playerNameForm.Show();
            this.Close();
        }

        // ── Medium Mode button (button2) ──────────────────────────────────
        private void button2_Click(object sender, EventArgs e)
        {
            PlayerName playerNameForm = new PlayerName(parentHome, "Medium");
            playerNameForm.Show();
            this.Close();
        }

        // ── Hard Mode button (button1) ────────────────────────────────────
        private void button1_Click(object sender, EventArgs e)
        {
            PlayerName playerNameForm = new PlayerName(parentHome, "Hard");
            playerNameForm.Show();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void pictureBox3_Click(object sender, EventArgs e) { }
    }
}