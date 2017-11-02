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
    public partial class Game_Finished : Form
    {
        public Game_Finished()
        {
            InitializeComponent();            
        }

        Profile[] profile = new Profile[5];
        int profileDimension;
        int profileAmount;
        int timeValue; 

        private void lblButtons_MouseEnter(object sender, EventArgs e)
        {
            Label label = new Label();
            label = (Label)sender;

            label.BackColor = Color.Gray;
        }

        private void lblButtons_MouseLeave(object sender, EventArgs e)
        {
            Label label = new Label();
            label = (Label)sender;

            label.BackColor = Color.DimGray;
        }

        private void lblMainMenu_Click(object sender, EventArgs e)
        {
            Main_Menu Main_Menu = new Main_Menu();
            Main_Menu.Show();
            Main_Menu.Activate();
            this.Hide();
        }

        private void pboxHome_Click(object sender, EventArgs e)
        {
            Main_Menu Main_Menu = new Main_Menu();
            Main_Menu.Show();
            Main_Menu.Activate();
            this.Hide();
        }

        private void Game_Finished_Load(object sender, EventArgs e)
        {
            // retrieve current score and time and set to respective Labels
            lblScore.Text = Question_1.GetQuestion().GetTempScore().ToString();
            lblTime.Text = Question_1.GetQuestion().GetTime().ToString();
            timeValue = Question_1.GetQuestion().GetTime();
            int tempScore = Question_1.GetQuestion().GetTempScore();

            // set Score
            Question_1.GetQuestion().SetScore(tempScore);

            // counter measure if time reaches -1
            if (timeValue == -1)
            {
                lblTime.Text = "0";               
            }
            
            if (timeValue < 1)
            {
                lblGameState.Text = "Y O U  R A N  O U T  O F  T I M E";                
            }
            else
            {
                lblGameState.Text = "G A M E  F I N I S H E D";
            }

            int width = this.Width;
            int textWidth = lblGameState.Width;
            int newValue = (width - textWidth) / 2;
            lblGameState.Location = new Point(newValue, 21);

            // reads in selected profile using StreamReader
            System.IO.StreamReader profileSelectedRead = new System.IO.StreamReader(Application.StartupPath + "\\ProfileSelected.txt");
            for (int i = 0; i < 6; i++)
            {
                profileSelectedRead.ReadLine();
            }
            // 7th line is profile dimension which is used to find which profile[i] is the selected profile
            profileDimension = Convert.ToInt32(profileSelectedRead.ReadLine());
            // closes StreamReader
            profileSelectedRead.Close();
            // DEBUG
            txtDebug.Text += "profile dimension " + profileDimension.ToString();

            // read in profiles from txt file using StreamReader
            System.IO.StreamReader profileInfo = new System.IO.StreamReader(Application.StartupPath + "\\ProfileInfo.txt");
            // check the number of saved profiles
            profileAmount = Convert.ToInt32(profileInfo.ReadLine());  
    
            for (int i = 0; i < profileAmount; i++)
            {
                // new instance of Profile class created
                // variables assigned to info read in using StreamReader
                profile[i] = new Profile();
                profile[i].Username = profileInfo.ReadLine();
                profile[i].Password = profileInfo.ReadLine();
                profile[i].Avatar = profileInfo.ReadLine();
                profile[i].GamesPlayed = Convert.ToInt32(profileInfo.ReadLine());
                profile[i].TotalPoints = Convert.ToInt32(profileInfo.ReadLine());
                profile[i].AveragePoints = Convert.ToInt32(profileInfo.ReadLine());

                // DEBUG: to check profile info
                txtDebug.Text += profile[i].Username + profile[i].Password + profile[i].Avatar + Convert.ToString(profile[i].GamesPlayed) + Convert.ToString(profile[i].TotalPoints) + Convert.ToString(profile[i].AveragePoints);
            }
            // closes StreamReader
            profileInfo.Close();

            // add 1 to GamesPlayed
            profile[profileDimension].GamesPlayed++;
            // add points achieved in game to total existing points as long as time is greater than 0
            if (timeValue > 0)
            {
                profile[profileDimension].TotalPoints = profile[profileDimension].TotalPoints + Convert.ToInt32(lblScore.Text);
            }            
            // divide new TotalPointsby new GamesPlayed
            profile[profileDimension].AveragePoints = profile[profileDimension].TotalPoints / profile[profileDimension].GamesPlayed;

            // write existing profiles with edited info back into ProfileInfo.txt using StreamWriter
            System.IO.StreamWriter profileWrite = new System.IO.StreamWriter(Application.StartupPath + "\\ProfileInfo.txt");
            // write profile amount
            profileWrite.WriteLine(Convert.ToString(profileAmount));
            // loop to write each in profile[i]
            for (int i = 0; i < profileAmount; i++)
            {
                profileWrite.WriteLine(profile[i].Username);
                profileWrite.WriteLine(profile[i].Password);
                profileWrite.WriteLine(profile[i].Avatar);
                profileWrite.WriteLine(profile[i].GamesPlayed);
                profileWrite.WriteLine(profile[i].TotalPoints);
                profileWrite.WriteLine(profile[i].AveragePoints);
            }
            // closes StreamWriter
            profileWrite.Close();
            // method to write selected profile
            ProfileSelectedWrite();
            // method to configure best completion times
            BestTimes();
                
        }

        private void lblPlayAgain_Click(object sender, EventArgs e)
        {
            // moves to Question_1 form
            Question_1 Question_1 = new Question_1();
            Question_1.Show();
            Question_1.Activate();
            this.Hide();
        }

        private void ProfileSelectedWrite()
        {
            // writes selected profile to ProfileSelected.txt using StreamWriter
            System.IO.StreamWriter profileSelected = new System.IO.StreamWriter(Application.StartupPath + "\\ProfileSelected.txt");
            // profileDimension is used to selected the correct class to write to the file
            profileSelected.WriteLine(profile[profileDimension].Username);
            profileSelected.WriteLine(profile[profileDimension].Password);
            profileSelected.WriteLine(profile[profileDimension].Avatar);
            profileSelected.WriteLine(profile[profileDimension].GamesPlayed);
            profileSelected.WriteLine(profile[profileDimension].TotalPoints);
            profileSelected.WriteLine(profile[profileDimension].AveragePoints);
            profileSelected.WriteLine(profileDimension);
            // closes StreamWriter
            profileSelected.Close();
            // DEBUG: ensure correct profile is written
            txtDebug.Text += "profile select write" + profile[profileDimension].Username;
        }

        private void BestTimes()
        {
            // DEBUG
            txtDebug.Text += "--------------";
            // read in best times using StreamReader
            System.IO.StreamReader bestTimeRead = new System.IO.StreamReader(Application.StartupPath + "\\BestTimes.txt");
            // array for storing read in info
            string[] bestLine = new string[10];
            // info read in and assigned to bestLine[i]
            for (int i = 0; i < 10; i++)
            {
                bestLine[i] = bestTimeRead.ReadLine();
            }
            // closes StreamReader
            bestTimeRead.Close();
            // arrays for storing best times and usernames. Each set of username/time has the same dimension in each array
            int[] bestTimes = new int[5];
            string[] bestUser = new string[5];
            // assignment of best times and usernames
            int c = 0;            
            for (int a = 0; a < 10; a = a + 2)
            {
                bestUser[c] = bestLine[a];
                bestTimes[c] = Convert.ToInt32(bestLine[a + 1]);
                txtDebug.Text += " user: " + bestUser[c] + " time: " + bestTimes[c].ToString();
                c++;
            }
            // 
            int scoreValue = Convert.ToInt32(lblScore.Text);
            txtDebug.Text += "SCORE=" + scoreValue.ToString();
            // if score is perfect
            if (scoreValue == 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    // if player's finish time is greater than a bestTime
                    if (timeValue > bestTimes[i])
                    {
                        // reassigns all the bestTimes and bestUser variables so that the new best time and user
                        // can be inputted into the file by choosing one of the if statements based upon which
                        // time the finish time is greater than

                        switch(i)
                        {
                            case 0:
                                bestUser[4] = bestUser[3];
                                bestTimes[4] = bestTimes[3];
                                bestUser[3] = bestUser[2];
                                bestTimes[3] = bestTimes[2];
                                bestUser[2] = bestUser[1];
                                bestTimes[2] = bestTimes[1];
                                bestUser[1] = bestUser[0];
                                bestTimes[1] = bestTimes[0];
                                bestUser[0] = profile[profileDimension].Username;
                                bestTimes[0] = timeValue;
                                break;

                            case 1:
                                bestUser[4] = bestUser[3];
                                bestTimes[4] = bestTimes[3];
                                bestUser[3] = bestUser[2];
                                bestTimes[3] = bestTimes[2];
                                bestUser[2] = bestUser[1];
                                bestTimes[2] = bestTimes[1];
                                bestUser[1] = profile[profileDimension].Username;
                                bestTimes[1] = timeValue;
                                break;

                            case 2:
                                bestUser[4] = bestUser[3];
                                bestTimes[4] = bestTimes[3];
                                bestUser[3] = bestUser[2];
                                bestTimes[3] = bestTimes[2];
                                bestUser[2] = profile[profileDimension].Username;
                                bestTimes[2] = timeValue;
                                break;

                            case 3:
                                bestUser[4] = bestUser[3];
                                bestTimes[4] = bestTimes[3];
                                bestUser[3] = profile[profileDimension].Username;
                                bestTimes[3] = timeValue;
                                break;

                            case 4:
                                bestUser[4] = profile[profileDimension].Username;
                                bestTimes[4] = timeValue;
                                break;

                            default:
                                break;
                        }
                        MessageBox.Show("Congratulations! You achieved a best time!", "Best Time Achievement");
                        break;
                    }
                }

                // write new bestUser and bestTimes back to BestTimes.txt using StreamWriter
                System.IO.StreamWriter bestTimeWrite = new System.IO.StreamWriter(Application.StartupPath + "\\BestTimes.txt");
                for (int i = 0; i < 5; i++)
                {
                    bestTimeWrite.WriteLine(bestUser[i]);
                    bestTimeWrite.WriteLine(bestTimes[i].ToString());

                }
                // closes StreamWriter
                bestTimeWrite.Close();
            }        
        }
    }
}
