using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quiz_
{
    public partial class Question_5 : Form
    {
        public Question_5()
        {
            InitializeComponent();
            // retrieve current score and time 
            playerScore = Question_1.GetQuestion().GetTempScore().ToString();
            timeLeft = Question_1.GetQuestion().GetTime();
        }

        string playerScore;
        int timeLeft;
        int newPlayerScore;
        Color color;

        private void lblSubmit_Click(object sender, EventArgs e)
        {
            newPlayerScore = Convert.ToInt32(playerScore);
            // correct answer
            if (txtAns1.Text == "4X" && txtAns2.Text == "4")
            {
                color = Color.Green;

                newPlayerScore = Question_1.GetQuestion().IncreaseScore();
                lblScoreValue.Text = Convert.ToString(newPlayerScore);
            }
            else // incorrect answer
            {
                color = Color.Red;
            }
            // set color of objects
            pboxTitleMid.BackColor = color;
            lblQP1.BackColor = color;
            lblQP2.BackColor = color;
            lblQP3.BackColor = color;
            lblQP4.BackColor = color;
            lblScore.BackColor = color;
            lblScore2.BackColor = color;
            lblScoreValue.BackColor = color;
            lblTimeLeft.BackColor = color;
            lblTime.BackColor = color;
            // method for saving score and time
            timerNextQ.Enabled = true;
            Question_1.GetQuestion().SetScore(newPlayerScore);
            Question_1.GetQuestion().SetTime(timeLeft);
            timerTimeLeft.Enabled = false;
        }

        private void lblButtons_MouseEnter(object sender, EventArgs e)
        {
            // label assigned to active Label
            Label label = new Label();
            label = (Label)sender;
            // label BackColor is set to Gray
            label.BackColor = Color.Gray;
        }

        private void lblButtons_MouseLeave(object sender, EventArgs e)
        {
            // label assigned to active Label
            Label label = new Label();
            label = (Label)sender;
            // label BackColor is set to DimGray
            label.BackColor = Color.DimGray;
        }

        private void timerNextQ_Tick(object sender, EventArgs e)
        {
            // timer disabled  
            timerNextQ.Enabled = false;
            // moves to Game_Finished form
            Game_Finished Game_Finished = new Game_Finished();
            Game_Finished.Show();
            Game_Finished.Activate();
            this.Hide();
        }

        private void Question_5_Load(object sender, EventArgs e)
        {
            // assign current score to lblScoreValue Text 
            lblScoreValue.Text = playerScore;
            // set time Label
            lblTime.Text = Convert.ToString(timeLeft);
        }

        private void timerTimeLeft_Tick(object sender, EventArgs e)
        {
            // countdown timer; time is decremented then converted to a string for the time Label
            timeLeft = timeLeft - 1;
            lblTime.Text = Convert.ToString(timeLeft);

            if (timeLeft < 10) // if timer is nearly out, time Label turns red
            {
                lblTime.ForeColor = Color.Red;
            }

            if (timeLeft < 0) // if time runs out
            {
                lblTime.Text = "0";
                Question_1.GetQuestion().SetScore(newPlayerScore);
                Question_1.GetQuestion().SetTime(timeLeft);
                timerTimeLeft.Enabled = false;
                // moves to Game_Finished form
                Game_Finished Game_Finished = new Game_Finished();
                Game_Finished.Show();
                Game_Finished.Activate();
                this.Hide();
            }

        }

        private void pboxHome_Click(object sender, EventArgs e)
        {
            // warning message if player wants to exit current game
            if (MessageBox.Show("Are you sure you wish to exit the game?", "Exit game?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                timerTimeLeft.Enabled = false;
                
                // moves to Main_Menu
                Main_Menu Main_Menu = new Main_Menu();
                Main_Menu.Show();
                Main_Menu.Activate();
                this.Hide();
            }
        }
    }
}
