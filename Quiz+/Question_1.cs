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
    public partial class Question_1 : Form
    {    
        public Question_1()
        {
            InitializeComponent();
        }

        // create Question class for passing time and score
        private static Question question;

        public static Question GetQuestion()
        {
            return question;
        }

        int timeLeft;
        int newPlayerScore;

        private void lblSubmit_Click(object sender, EventArgs e)
        {
            int playerAnswer = 0;
            Color color;
            newPlayerScore = 0;

            // check which RadioButton is checked
            if (rdoAns1.Checked)
            {
                playerAnswer = 1;
            }
            else if (rdoAns2.Checked)
            {
                playerAnswer = 2;
            }
            else if (rdoAns3.Checked)
            {
                playerAnswer = 3;
            }
            else if (rdoAns4.Checked)
            {
                playerAnswer = 4;
            }
            // correct answer
            if (playerAnswer == 2)
            {
                color = Color.Green;
                
                newPlayerScore = question.IncreaseScore();
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
            lblScore.BackColor = color;
            lblScore2.BackColor = color;
            lblScoreValue.BackColor = color;
            lblTimeLeft.BackColor = color;
            lblTime.BackColor = color;
            // method for saving score and time
            question.SetScore(newPlayerScore);
            question.SetTime(timeLeft);
            timerTimeLeft.Enabled = false;
            timerNextQ.Enabled = true;
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
            // label BackColor is set to DImGray
            label.BackColor = Color.DimGray;
        }

        private void timerNextQ_Tick(object sender, EventArgs e)
        {        
            // timer disabled 
            timerNextQ.Enabled = false;         
            lblTime.ForeColor = Color.White;
            // moves to Question_2 form
            Question_2 Question_2 = new Question_2();
            Question_2.Show();
            Question_2.Activate();
            this.Hide();
        }

        private void Question_1_Load(object sender, EventArgs e)
        {
            question = new Question();
            // assign current score to lblScoreValue Text 
            lblScoreValue.Text = question.GetTempScore().ToString();
            timeLeft = question.GetTime();
            timerTimeLeft.Enabled = true;
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
                timerTimeLeft.Enabled = false;
                question.SetScore(newPlayerScore);
                question.SetTime(timeLeft);
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
            if(MessageBox.Show("Are you sure you wish to exit the game?", "Exit game?", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
