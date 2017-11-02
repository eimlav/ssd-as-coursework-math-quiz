using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.IO;

namespace Quiz_
{
    public partial class Main_Menu : Form
    {
        public Main_Menu()
        {
            InitializeComponent();
        }

        string username;
        string avatar;
        int helpCheckValue;

        private void lblButtons_MouseEnter(object sender, EventArgs e)
        {
            // label assigned to active Label
            Label label = new Label();
            label = (Label)sender;
            // label BackColor set to Gray
            label.BackColor = Color.Gray;
        }

        private void lblButtons_MouseLeave(object sender, EventArgs e)
        {
            // label assigned to active Label
            Label label = new Label();
            label = (Label)sender;
            // label BackColor set to DimGray
            label.BackColor = Color.DimGray;
        }

        private void lblButtons_Click(object sender, EventArgs e)
        {
            // labelName assigned to Name property of active Label
            string labelName = ((Label)sender).Name;
            // check which Label has been selected 
            switch(labelName)
            {
                case "lblStartGame":
                    // moves to Question_1 form
                    Question_1 Question_1 = new Question_1();
                    Question_1.Show();
                    Question_1.Activate();
                    this.Hide();
                    break;
                case "lblEditProfile":
                    // moves to Edit_Profile form
                    Edit_Profile Edit_Profile = new Edit_Profile();
                    Edit_Profile.Show();
                    Edit_Profile.Activate();
                    this.Hide();
                    break;
                case "lblPlayerStats":
                    // moves to Player_Stats form
                    Player_Stats Player_Stats = new Player_Stats();
                    Player_Stats.Show();
                    Player_Stats.Activate();
                    this.Hide();
                    break;
                    
            }
        }

        private void pboxHome_Click(object sender, EventArgs e)
        {
            // warning message when home icon is hit
            if(MessageBox.Show("Are you sure you wish to log off?", "Log Off", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                // moves to Start_Menu form
                Start_Menu Start_Menu = new Start_Menu();
                Start_Menu.Show();
                Start_Menu.Activate();
                this.Hide();
            }                        
        }

        private void timDateTime_Tick(object sender, EventArgs e)
        {
            // method to update time activated
            Time();           
        }

        private void Time()
        {
            // time related variables assigned
            DateTime date = DateTime.Now;
            int day = date.Day;
            int month = date.Month;
            int year = date.Year;
            int minute = date.Minute;
            string minuteZero;
            // check is currect minute is less than ten. if so 0 is assigned to minuteZero 
            if (minute < 10)
            {
                minuteZero = "0";
            }
            else // if current minute is greater than ten, minuteZero is empty
            {
                minuteZero = "";
            }
            int hour = date.Hour;
            // date and time labels written
            lblDate.Text = Convert.ToString(day) + "/" + Convert.ToString(month) + "/" + Convert.ToString(year);
            lblTime.Text = Convert.ToString(hour) + ":" + minuteZero + Convert.ToString(minute);
        }

        private void Main_Menu_Load(object sender, EventArgs e)
        {
            // method to update time
            Time();
            // reads in selected profile using StreamReader
            System.IO.StreamReader profileSelected = new System.IO.StreamReader(Application.StartupPath + "\\ProfileSelected.txt");
            // required variables assigned from ProfileSelected.txt 
            username = profileSelected.ReadLine();
            string skipLine = profileSelected.ReadLine();
            avatar = profileSelected.ReadLine();
            int gamesPlayed = Convert.ToInt32(profileSelected.ReadLine());            
            // convert hexadecimal avatar string to a Color
            int red = int.Parse(avatar.Substring(1, 2), NumberStyles.HexNumber);
            int green = int.Parse(avatar.Substring(3, 2), NumberStyles.HexNumber);
            int blue = int.Parse(avatar.Substring(5, 2), NumberStyles.HexNumber);
            // RGB Color generated from variables created from converted hexadecimal 
            Color color = Color.FromArgb(red, green, blue);
            // avatar PictureBox BackColor set to color Color
            pboxAvatar.BackColor = color;
            // username Label set to username read in earlier
            lblUsername.Text = username;
            // closes StreamReader
            profileSelected.Close();
            // read in helpCheckValue using BinaryWriter
            try
            {
                // read in helpCheckValue using BinaryReader
                BinaryReader helpCheckRead = new BinaryReader(File.Open("helpCheck", FileMode.Open));
                helpCheckValue = helpCheckRead.Read();
                // closes BinaryWriter
                helpCheckRead.Close();
            }
            catch (Exception ex)
            {
                // if error occurs, write error to console
                Console.WriteLine("Exception: " + ex);                    
            }
            
                                              
            // check if helpCheckValue = 0
            if(helpCheckValue == 0)
            {
                // welcome message is displayed
                if (MessageBox.Show("Welcome to Quiz+. If you are a new player and wish to learn more about the application then click the 'Yes' button. If you do not wish for any assistance then select the 'No' button.", "Welcome to Quiz+", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Help();
                }
                // open BinaryWriter
                BinaryWriter helpCheck = new BinaryWriter(File.Open("helpCheck", FileMode.Open));
                //write help check value to file
                helpCheck.Write(1);
                // close BinaryWriter
                helpCheck.Close(); 
            }
            // method to set up best times
            BestTimes();
        }

        private void Main_Menu_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            // method to display help menu
            Help();
        }

        private void Help()
        {
            // help menu becomes visible
            pnlHelp.Visible = true;
            tstripHelp.Focus();
        }

        private void tsbQuiz_Click(object sender, EventArgs e)
        {
            // quiz related information for help menu
            txtHelp.Text = "QUIZ\r\n==========================\r\n";
            txtHelp.Text += "To start a new game select the 'Start Game' option on the main menu. This will start a new game in which you must answer questions in order to complete the game.\r\n\r\n";
            txtHelp.Text += "To select an answer, click it with the left mouse button. If the answer contains a textbox, it means that it requires keyboard input. You must also remember that some questions involve selecting more than one answer.\r\n\r\n";
            txtHelp.Text += "Once you have made your choice, select the 'Submit' button to check your chosen answer(s) and to confirm whether they are right or not. If the answer(s) is right, the banner will turn green. IF the answer(s) is wrong, the banner will turn red.";
            txtHelp.Text += "A correct answer will add 1 point to your score. An incorrect answer will neither add nor remove any points.\r\n\r\n";
            txtHelp.Text += "Another key aspect that must be remembered is the timer. You are given 150 seconds to complete the game. Failure to complete all the questions in this time will result in a game over in which you will lose any accumalated points.\r\n\r\n";
            txtHelp.Text += "If you earn a perfect score then you are eligible to go onto the Best Time board in the Main Menu, but only if you have a better time that that of a time on the Best TIme board. This shows the players with the top completion times.";
        }

        private void tsbProfiles_Click(object sender, EventArgs e)
        {
            // profile related information for help menu
            txtHelp.Text = "CREATING A PROFILE\r\n==========================\r\n";
            txtHelp.Text += "In order to create a new profile, you must return to the start menu. This can be done by selecting the 'Home' icon on the main menu. Once on the start menu, select the 'Create New Profile' option.\r\n";
            txtHelp.Text += "A dialog will appear in which you are prompted to input a username and choose an avatar. When choosing a username, ensure that it is unique as if any existing user has the same name, you will be prompted to choose a different name. In addition, usernames have a maximum length of 15 characters so remember to take this into consideration when choosing as well.\r\n\r\n";
            txtHelp.Text += "For the avatar, you are presented with 5 preset options. To choose one, simply left click the avatar. If however you would like a custom avatar then double click the '...' box.\r\n\r\n";
            txtHelp.Text += "A dialog with 3 numeric displays corresponding to the red, green and blue values of the chosen avatar will appear in which you enter a number between 0 and 255. A preview of the chosen colour appears above. Once you are happy with your choice, select the 'Add Avatar' button to select the chosen colour.\r\n\r\n";
            txtHelp.Text += "==========================\r\n";
            txtHelp.Text += "EDITING A PROFILE\r\n==========================\r\n";
            txtHelp.Text += "Each player is provided with their own profile, which allows them to choose their own unique username and avatar as well as an optional password to protect their profile. In addition, the number of games played, total points and average points per game are stored within the profile.\r\n\r\n";
            txtHelp.Text += "To view the games played, total points or average points per game, select the 'Player Statistics' option on the main menu. By selecting the 'Edit Profile' option on the main menu, the player is able to change their username, avatar or password.\r\n\r\n";
            txtHelp.Text += "In the 'Edit Profile' screen, to change your username simply type in the new username in the textbox within the 'Username' section.\r\n\r\n ";
            txtHelp.Text += "To change the avatar, the player is provided with 5 preset options or a sixth 'custom' option in which they can choose any colour they want. To choose one of the preset options, left click the avatar. If you wish to pick a custom avatar, double click the bottom right avatar that is highlighted initially upon entering the 'Edit Profile' screen\r\n\r\n";
            txtHelp.Text += "A dialog with 3 numeric displays corresponding to the red, green and blue values of the chosen avatar will appear in which you enter a number between 0 and 255. A preview of the chosen colour appears above. Once you are happy with your choice, select the 'Add Avatar' button to select the chosen colour.\r\n\r\n";
            txtHelp.Text += "To add a password first ensure the 'Enable Password' checkbox has been checked. If the checkbox is not checked, the designated textbox will disable and a password will not be added/password functionality will be disabled. Once enabled type in the new password in the textbox.\r\n\r\n";
            txtHelp.Text += "Once you have completed making all desired changes, select the 'Save Changes' button to save any changes made. If no changes were made to any of the aspects, they will remain the same.\r\n\r\n";          
        }

        private void tsbExit_Click(object sender, EventArgs e)
        {
            // help menu turns invisible
            pnlHelp.Visible = false;
        }        
        
        private void BestTimes()
        {
            // reads in best times and corresponding usernames using StreamReader
            System.IO.StreamReader bestTimeRead = new System.IO.StreamReader(Application.StartupPath + "\\BestTimes.txt");
            string[] bestLine = new string[10];
            for (int i = 0; i < 10; i++)
            {
                bestLine[i] = bestTimeRead.ReadLine();
            }
            // close Streameader
            bestTimeRead.Close();
            int[] bestTimes = new int[5];
            string[] bestUser = new string[5];
            int c = 0;
            // the times and usernames read in are assigned to respective variables in which each set is assigned to same dimension i.e. bestUser[2]'s time is bestTimes[2]
            for (int a = 0; a < 10; a = a + 2)
            {
                bestUser[c] = bestLine[a];
                bestTimes[c] = Convert.ToInt32(bestLine[a + 1]);       
                c++;
            }

            // usernames written to Labels
            lblBTUsername1.Text = bestUser[0];
            lblBTUsername2.Text = bestUser[1];
            lblBTUsername3.Text = bestUser[2];
            lblBTUsername4.Text = bestUser[3];
            lblBTUsername5.Text = bestUser[4];
            // times written to Labels
            lblBTTime1.Text = bestTimes[0].ToString();
            lblBTTime2.Text = bestTimes[1].ToString();
            lblBTTime3.Text = bestTimes[2].ToString();
            lblBTTime4.Text = bestTimes[3].ToString();
            lblBTTime5.Text = bestTimes[4].ToString();

        }

    }
}
