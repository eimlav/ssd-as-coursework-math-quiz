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
    public partial class Question_3 : Form
    {
        public Question_3()
        {
            InitializeComponent();
            // retrieve current score and time 
            playerScore = Question_1.GetQuestion().GetTempScore().ToString();
            timeLeft = Question_1.GetQuestion().GetTime();
        }

        string playerScore;
        int timeLeft;
        bool mouseDown;
        Point point = new Point();
        int mouseDownX = 0;
        int mouseDownY = 0;
        Panel panel = new Panel();
        Point temp = new Point();
        bool panelFirst;
        bool panelSecond;
        bool panelThird;
        int panelFirstValue;
        int panelSecondValue;
        int panelThirdValue;
        int answerCount = 0;
        Color color;
        int newPlayerScore;
        Point originalPoint = new Point();
        bool originalPointEnabled = true;

        private void lblSubmit_Click(object sender, EventArgs e)
        {
            newPlayerScore = Convert.ToInt32(playerScore);
            
            // each if statement adds 1 to answerCount if the statement is true
            if (panelFirstValue == 2)
            {
                answerCount++;
            }
            
            if (panelSecondValue == 3)
            {
                answerCount++;
            }

            if (panelThirdValue == 1)
            {
                answerCount++;
            }

            // correct answer if answerCount equals 3
            if (answerCount == 3)
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
            lblScore.BackColor = color;
            lblScore2.BackColor = color;
            lblScoreValue.BackColor = color;
            lblTimeLeft.BackColor = color;
            lblTime.BackColor = color;
            // method for saving score and time
            Question_1.GetQuestion().SetScore(newPlayerScore);
            Question_1.GetQuestion().SetTime(timeLeft);
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
            // label BackColor is set to DimGray
            label.BackColor = Color.DimGray;
        }

        private void timerNextQ_Tick(object sender, EventArgs e)
        {
            // timer disabled
            timerNextQ.Enabled = false;
            lblTime.ForeColor = Color.White;
            // moves to Question_4 form
            Question_4 Question_4 = new Question_4();
            Question_4.Show();
            Question_4.Activate();
            this.Hide();
        }

        private void Question_3_Load(object sender, EventArgs e)
        {           
            // assign current score to lblScoreValue Text 
            lblScoreValue.Text = playerScore;
            // set time Label
            lblTime.Text = Convert.ToString(timeLeft);
        }

        private void lblAns_MouseDown(object sender, MouseEventArgs e)
        {
            // triggered when mouse down is activated
            if (!mouseDown)
            {
                mouseDown = true;
                mouseDownX = e.X;
                mouseDownY = e.Y;
            }
            
            if (originalPointEnabled)
            {                
                Label label = new Label();
                label = ((Label)sender);
                // value denoting which panel the label is in
                int value = Convert.ToInt32(label.Tag);
                originalPoint.X = 385;
                // determine Y coordinate of label's panel
                switch(value)
                {
                    case 1:
                        originalPoint.Y = 82;
                        break;
                    case 2:
                        originalPoint.Y = 123;
                        break;
                    case 3:
                        originalPoint.Y = 164;
                        break;
                }
                // disable if statement until the mouse is lifted so that only the original position is retained
                originalPointEnabled = false;

            }

        }

        private void lblAns_MouseUp(object sender, MouseEventArgs e)
        {           
            string[] cover = new string[3];
            cover[0] = "7 Root 2";
            cover[1] = "6 Root 5";
            cover[2] = "5 Root 3";
            
            
            // sets mouseDown to false when left mouse button is lifted
            // disables movement of answer
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = false;
            }

            // if panel is dragged outside the boundaries of the answer boxes it is reseted back to it's original location
            if (panel.Location.Y < 75 || panel.Location.Y > 195 || panel.Location.X < 140)
            {
                panel.Location = originalPoint;
            }
            else if (panel.Location.Y > 75 && panel.Location.Y < 115 && panel.Location.X < 385) // if panel y coordinate is greater than 75 and less than 115 & x coordinate is less than 385 when mouse button is lifted
            {
                // if location is already taken, send panel back to original location
                if (panelFirst == true)
                {
                    panel.Location = originalPoint;
                }
                else // if location is not taken, send panel to specified location and set location to taken
                {
                    point = new Point(312, 82);
                    panel.Location = point;
                    panelFirst = true;
                    panel.Enabled = false;
                    panelFirstValue = Convert.ToInt32(panel.Tag);
                    pnlFirstCover.Visible = true;
                    lblFirstCover.Text = cover[panelFirstValue - 1];
                }              
            }
            else if (panel.Location.Y > 115 && panel.Location.Y < 155 && panel.Location.X < 385)  // if panel y coordinate is greater than 115 and less than 155 & x coordinate is less than 385when mouse button is lifted
            {
                // if location is already taken, send panel back to original location
                if (panelSecond == true)
                {
                    panel.Location = originalPoint;
                }
                else // if location is not taken, send panel to specified location and set location to taken
                {
                    point = new Point(313, 123);
                    panel.Location = point;
                    panelSecond = true;
                    panel.Enabled = false;
                    panelSecondValue = Convert.ToInt32(panel.Tag);
                    pnlSecondCover.Visible = true;
                    lblSecondCover.Text = cover[panelSecondValue - 1];
                }    
            }
            else if (panel.Location.Y > 155 && panel.Location.Y < 195 && panel.Location.X < 385) // if panel y coordinate is greater than 155 and less than 195 & x coordinate is less than 385 when mouse button is lifted
            {
                // if location is already taken, send panel back to original location
                if (panelThird == true)
                {
                    panel.Location = originalPoint;
                }
                else // if location is not taken, send panel to specified location and set location to taken
                {
                    point = new Point(313, 164);
                    panel.Location = point;
                    panelThird = true;
                    panel.Enabled = false;
                    panelThirdValue = Convert.ToInt32(panel.Tag);
                    pnlThirdCover.Visible = true;
                    lblThirdCover.Text = cover[panelThirdValue - 1];
                }    
            }

            originalPointEnabled = true;
        }

        private void lblAns_MouseMove(object sender, MouseEventArgs e)
        {
            string label = ((Label)sender).Name;            

            // assgins panel to the appropriate Panel containing the answer Label
            switch(label)
            {
                case "lblAns1":
                    panel = pnlAns1;
                    break;
                case "lblAns2":
                    panel = pnlAns2;
                    break;
                case "lblAns3":
                    panel = pnlAns3;
                    break;
            }
            
            // moves Panel panel as long as mouseDown is true
            if (mouseDown)
            {
                temp.X = panel.Location.X + (e.X - mouseDownX);
                temp.Y = panel.Location.Y + (e.Y - mouseDownY);
                panel.Location = temp;
            }
        }

        private void lblReset_Click(object sender, EventArgs e)
        {
            // resets location of Labels and variable values
            pnlAns1.Location = new Point(385, 82);
            pnlAns2.Location = new Point(385, 123);
            pnlAns3.Location = new Point(385, 164);

            panelFirst = false;
            panelSecond = false;
            panelThird = false;

            panelFirstValue = 0;
            panelSecondValue = 0;
            panelThirdValue = 0;

            pnlAns1.Enabled = true;
            pnlAns2.Enabled = true;
            pnlAns3.Enabled = true;

            pnlFirstCover.Visible = false;
            pnlSecondCover.Visible = false;
            pnlThirdCover.Visible = false;
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
