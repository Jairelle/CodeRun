using System;
using System.Windows.Forms;

namespace WinFormsApp1
{
    // The screen where the player selects their game difficulty (Easy, Medium, Hard).
    public partial class PlayDifficulties : Form
    {
        // Reference to our main menu form, so we can show it again when going back.
        private HomeTab parentHome;

        // Constructor that accepts the parent HomeTab so we can maintain reference to it.
        public PlayDifficulties(HomeTab home)
        {
            InitializeComponent();
            parentHome = home;
        }

        private void PlayDifficulties_Load(object sender, EventArgs e) { }

        // ── Back button ───────────────────────────────────────────────────
        // When the user clicks the "Back" button, we show the main menu again and close
        // this screen to keep the UI clean.
        private void BackButtonCredits_Click(object sender, EventArgs e)
        {
            parentHome.Show();
            this.Close();
        }

        // ── Easy Mode button ──────────────────────────────────────────────
        // Opens the player name entry screen configured for "Easy" difficulty,
        // passes the HomeTab reference down the line, and closes this dialog.
        private void button3_Click(object sender, EventArgs e)
        {
            PlayerName playerNameForm = new PlayerName(parentHome, "Easy");
            playerNameForm.Show();
            this.Close();
        }

        // ── Medium Mode button ────────────────────────────────────────────
        // Opens the player name entry screen configured for "Medium" difficulty,
        // passes the HomeTab reference down the line, and closes this dialog.
        private void button2_Click(object sender, EventArgs e)
        {
            PlayerName playerNameForm = new PlayerName(parentHome, "Medium");
            playerNameForm.Show();
            this.Close();
        }

        // ── Hard Mode button ──────────────────────────────────────────────
        // Opens the player name entry screen configured for "Hard" difficulty,
        // passes the HomeTab reference down the line, and closes this dialog.
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