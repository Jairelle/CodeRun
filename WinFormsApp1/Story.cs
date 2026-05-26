using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinFormsApp1;

namespace CodeRun
{
    // The screen that displays the introduction video/story for our game.
    public partial class Story : Form
    {
        private HomeTab parentHome; // Saved reference to our Home screen so we can go back to it
        private bool returningToHome = false; // State flag to distinguish between closing the app vs. going back to the menu

        // Parameterless constructor. Required by the Visual Studio Form Designer so it can load the preview.
        public Story()
        {
            InitializeComponent();
        }

        // Main constructor that receives the Home screen reference.
        public Story(HomeTab home) : this()
        {
            parentHome = home;
        }

        // ── Back button click ─────────────────────────────────────────────
        // When the user clicks the "Back" button:
        // 1. We set the flag to true so we know this isn't an app exit event.
        // 2. We show the main home screen.
        // 3. We close this Story screen.
        private void BackButtonStory_Click_1(object sender, EventArgs e)
        {
            returningToHome = true;
            if (parentHome != null)
            {
                parentHome.Show();
            }
            this.Close();
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {
        }

        // ── Form Load Event ───────────────────────────────────────────────
        // Called when the Story window is loaded. We configure the video player here.
        private void Story_Load(object sender, EventArgs e)
        {
            // CRITICAL WORKAROUND:
            // Visual Studio's WinForms designer has a bug where it accidentally redeclares
            // 'axWindowsMediaPlayer1' as a local variable inside InitializeComponent().
            // This leaves our class-level 'this.axWindowsMediaPlayer1' field set to null, 
            // causing a NullReferenceException when we try to play the video.
            // 
            // To make this bulletproof, if the field is null, we scan the form's child
            // controls at runtime, find the Media Player control, and assign it back to our field.
            if (this.axWindowsMediaPlayer1 == null)
            {
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is AxWMPLib.AxWindowsMediaPlayer player)
                    {
                        this.axWindowsMediaPlayer1 = player;
                        break;
                    }
                }
            }

            // Once we have a valid media player reference, set the video file path and play it!
            if (this.axWindowsMediaPlayer1 != null)
            {
                this.axWindowsMediaPlayer1.URL = @"C:\Users\oreto\OneDrive\Desktop\CodeRun\Video.mp4";
                this.axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }

        // ── Form Closed Event ─────────────────────────────────────────────
        // Handles what happens when this screen is closed.
        // If the user clicked the window's top-right "X" button (meaning returningToHome is false),
        // we want to exit the entire application completely.
        // Otherwise, if they just clicked the "Back" button, we let the Close() complete normally.
        private void Story_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!returningToHome)
            {
                Application.Exit();
            }
        }
    }
}
