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
    public partial class Start_Menu : Form
    {
        public Start_Menu()
        {
            InitializeComponent();
        }

        // variable declarations
        Main_Menu Main_Menu = new Main_Menu();    
        PictureBox avatar;   
        Profile[] profile = new Profile[5];
        int profileAmount;
        string newUsername;
        string hexString = "#FF0000";
        int profileDimension;
        bool newProfileYes;
        int helpCheckValue;

        private void Start_Menu_Load(object sender, EventArgs e)
        {
            // read in profiles from txt file using StreamReader
            System.IO.StreamReader profileInfo = new System.IO.StreamReader(Application.StartupPath + "\\ProfileInfo.txt");

            // check the number of saved profiles
            profileAmount = Convert.ToInt32(profileInfo.ReadLine());           
            // DEBUG
            txtDebug.Text = Convert.ToString(profileAmount);
            // if there are any existing profiles, they are be read in
            if (profileAmount > 0)
            {
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
                    // if any games have been played, the average points is found
                    if(profile[i].GamesPlayed != 0)
                    {
                        profile[i].AveragePoints = profile[i].TotalPoints / profile[i].GamesPlayed;
                    }
                    else // if not, the average points is set to 0
                    {
                        profile[i].AveragePoints = 0;
                    }
                    
                    string x = profileInfo.ReadLine();
                    // DEBUG: to check profile info
                    txtDebug.Text += profile[i].Username + profile[i].Password + profile[i].Avatar + Convert.ToString(profile[i].GamesPlayed) + Convert.ToString(profile[i].TotalPoints) + Convert.ToString(profile[i].AveragePoints);
                }                
            } // if there are no existing profiles, Log In Label is set invisible and a welcome message is shown
            else
            {
                lblLogIn.Visible = false;
                pboxLogInBottom.Visible = false;
                pboxLogInTop.Visible = false;
                pboxLogInBack.Visible = false;

                pnlWelcome.Visible = true;
            }
            // closes StreamReader
            profileInfo.Close();
        }

        private void btnNewProfSubmit_Click(object sender, EventArgs e)
        {
            // text in username TextBox assigned to newUsername
            newUsername = txtNewProfileUser.Text;
            bool[] checkNewUsername = new bool[5];
            bool checkNewUsernames = true;

            // check if nothing has been entered into txtNewProfileUser
            if (newUsername != "")
            {
                // check if username is taken
                for (int i = 0; i < profileAmount; i++)
                {
                    if (newUsername == profile[i].Username)
                    {
                        checkNewUsernames = false;
                        break;
                    }
                }
                
                // error MessageBox shown
                if (checkNewUsernames == false)
                {
                    MessageBox.Show("The username you have chosen is already taken. Please try another.", "Create New Profile", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }               
                else // if username isn't taken
                {
                    // write in existing profiles and new profile using StreamWriter
                    System.IO.StreamWriter profileWrite = new System.IO.StreamWriter(Application.StartupPath + "\\ProfileInfo.txt");
                    // profie amount incremented to account for the new profile and written to file
                    profileWrite.WriteLine(Convert.ToString(profileAmount + 1));
                    // existing profiles written to file
                    for (int i = 0; i < profileAmount; i++)
                    {
                        profileWrite.WriteLine(profile[i].Username);
                        profileWrite.WriteLine(profile[i].Password);
                        profileWrite.WriteLine(profile[i].Avatar);
                        profileWrite.WriteLine(profile[i].GamesPlayed);
                        profileWrite.WriteLine(profile[i].TotalPoints);
                        profileWrite.WriteLine(profile[i].AveragePoints);                  
                    }
                    // new profile written to file
                    profileWrite.WriteLine(newUsername);
                    profileWrite.WriteLine("");
                    profileWrite.WriteLine(hexString);
                    profileWrite.WriteLine("0");
                    profileWrite.WriteLine("0");
                    profileWrite.WriteLine("0");
                    // closes StreamWriter
                    profileWrite.Close();

                    newProfileYes = true; 
                    // method to write profile to ProfileSelected.txt
                    ProfileSelectedWrite();
                    // write helpCheckValue to binary file
                    helpCheckValue = 0;
                    HelpCheckWrite();
                    // moves to Main_Menu form
                    Main_Menu.Show();
                    Main_Menu.Activate();
                    this.Hide();
                }
            }
        }

        private void pboxAvatar_Click(object sender, EventArgs e)
        {
            // x and y coordinates set to location of selected PictureBox minus 4 from each coordinate
            int x = ((PictureBox)sender).Location.X - 4;
            int y = ((PictureBox)sender).Location.Y - 4;
            // pboxAvatarBack set to location using the new x and y points
            pboxAvatarBack.Location = new Point(x, y);
            // avatar set to the selected PictureBox
            avatar = (PictureBox)sender;
            // method to select avatar
            AvatarSelector();       
        }

        private void AvatarSelector()
        {
            // avatar BackColor converted to hexadecimal string
            hexString = ColorTranslator.ToHtml(avatar.BackColor);
            // hexadecimal string converted to a Color 
            int red = int.Parse(hexString.Substring(1, 2), NumberStyles.HexNumber);
            int green = int.Parse(hexString.Substring(3, 2), NumberStyles.HexNumber);
            int blue = int.Parse(hexString.Substring(5, 2), NumberStyles.HexNumber);
            Color color = Color.FromArgb(red, green, blue);
            // DEBUG CODE
            label11.BackColor = color;
            txtDebug.Text += hexString;          
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            bool checkLogIn = false;
            // check if nothing has been entered into txtUsername
            if (txtUsername.Text != "")
            {
                // loop for checking each profile
                for (int i = 0; i < profileAmount; i++)
                {
                    // check if txtUsername.Text equals any existing username
                    if (txtUsername.Text == profile[i].Username)
                    {
                        // check if txtPassword.Text equals the chosen profile's password
                        if (txtPassword.Text == profile[i].Password)
                        {                           
                            // log in was a success
                            checkLogIn = true;
                            // new profile was not created
                            newProfileYes = false;
                        }
                        else
                        {
                            // txtPassword.Text does not equal the chosen profile's password
                            checkLogIn = false;
                        }
                    }
                    else
                    {
                        // txtUsername.Text does not equal any existing username
                        checkLogIn = false;
                    }
                    // username and password were correct; log in was a success
                    if (checkLogIn)
                    {
                        // TextBoxes reseted
                        txtUsername.Text = "";
                        txtPassword.Text = "";
                        // selected profile class = i so the value is assigned to profileDimension
                        profileDimension = i;
                        // method to write chosen profile to ProfileSelected.txt
                        ProfileSelectedWrite();
                        // write helpCheckValue to binary file
                        helpCheckValue = 1;
                        HelpCheckWrite();
                        // moves to Main_Menu form
                        Main_Menu.Show();
                        Main_Menu.Activate();
                        this.Hide();

                        break;
                    }
                }
                if (checkLogIn == false) // username or password was incorrect so error message appears
                {
                    MessageBox.Show("Incorrect username or password. Please try again.", "Profile Log In", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsername.Focus();
                }               
            }
            else // username or password was incorrect so error message appears; unsuccessful log in
            {
                MessageBox.Show("Please enter a valid username and/or password", "Profile Log In", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblMouseEnter(object sender, EventArgs e)
        {
            // label assigned to active Label
            Label label = new Label();
            label = (Label)sender;
            // label BackColor is set to Gray
            label.BackColor = Color.Gray;
        }

        private void lblMouseLeave(object sender, EventArgs e)
        {
            // label assigned to active Label
            Label label = new Label();
            label = (Label)sender;
            // label BackColor is set to DimGray
            label.BackColor = Color.DimGray;
        }

        private void lblLogIn_Click(object sender, EventArgs e)
        {
            // log in panel set to visible. new profile panel set to invisible
            pnlLogIn.Visible = true;
            pnlNewProfile.Visible = false;
        }

        private void timAvatarCustom_Tick(object sender, EventArgs e)
        {
            int red;
            int green;
            int blue;

            red = 0;
            green = 0;
            blue = 0;
            // variables assigned to respective numericUpDown objects
            red = Convert.ToInt32(nudRed.Value);
            green = Convert.ToInt32(nudGreen.Value);
            blue = Convert.ToInt32(nudBlue.Value);
            // BackColor of preview panel set to the converted RGB value generated from the 3 numericUpDown objects
            pnlRGBView.BackColor = Color.FromArgb(red, green, blue);
        }

        private void btnAvatarCustom_Click(object sender, EventArgs e)
        {
            // avatar BackColor set to preview panel BackColor
            pboxAvatarCustom.BackColor = pnlRGBView.BackColor;
            // avatar custom image is removed
            pboxAvatarCustom.Image = null;
            // avatar set to custom avatar PictureBox
            avatar = pboxAvatarCustom;
            // method to select avatar
            AvatarSelector();
            // custom avatar panel turns invisble. new profile panel turns visible
            pnlAvatarCustom.Visible = false;
            pnlNewProfile.Visible = true;
        }

        private void pboxAvatarCustom_DoubleClick(object sender, EventArgs e)
        {
            // custom avatar panel turns visible. new profile panel turns invisible
            pnlAvatarCustom.Visible = true;
            pnlNewProfile.Visible = false;
        }

        private void lblCreateAProfile_Click(object sender, EventArgs e)
        {
            // check if max profiles has been reached
            if (profileAmount == 5)
            {
                // error message appears to alert user
                MessageBox.Show("Max number of profiles reached. Please delete an existing profile to create a new profile", "Create New Profile", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else // max profiles not reached so new profile panel shows as expected
            {                
                pnlLogIn.Visible = false;
                pnlWelcome.Visible = false;
                pnlNewProfile.Visible = true;
            }           
        }

        private void ProfileSelectedWrite()
        {
            // writes selected profile to ProfileSelected.txt using StreamWriter
            System.IO.StreamWriter profileSelected = new System.IO.StreamWriter(Application.StartupPath + "\\ProfileSelected.txt");
            // check if selected profile is a new profile
            // if so, variables assigned in btnNewProfSubmit_Click method called
            if (newProfileYes)
            {
                profileSelected.WriteLine(newUsername);
                profileSelected.WriteLine("");
                profileSelected.WriteLine(hexString);
                profileSelected.WriteLine("0");
                profileSelected.WriteLine("0");
                profileSelected.WriteLine("0");
                profileSelected.WriteLine(profileAmount);
            }
            else // if an existing profile, profileDimension is used to selected the correct class to write to the file
            {
                profileSelected.WriteLine(profile[profileDimension].Username);
                profileSelected.WriteLine(profile[profileDimension].Password);
                profileSelected.WriteLine(profile[profileDimension].Avatar);
                profileSelected.WriteLine(profile[profileDimension].GamesPlayed);
                profileSelected.WriteLine(profile[profileDimension].TotalPoints);
                profileSelected.WriteLine(profile[profileDimension].AveragePoints);
                profileSelected.WriteLine(profileDimension);
            }
            // closes StreamWriter
            profileSelected.Close();
        }

        private void Start_Menu_Activated(object sender, EventArgs e)
        {
            // reloads form to ensure that any changes made to other parts of the application will be accounted for i.e. changes to profiles
            this.Refresh();
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            // if return key is hit while in username or password textbox, btnLogIn_Click method is activated
            string key = e.KeyCode.ToString();
            if(key == "Return")
            {
                btnLogIn.PerformClick();
            }
        }

        private void WelcomeMessage_Click(object sender, EventArgs e)
        {
            // welcome message turns invisible when clicked
            pnlWelcome.Visible = false;
            pnlNewProfile.Visible = true;
        }

        private void HelpCheckWrite()
        {
            try
            {
                // open BinaryWriter
                BinaryWriter helpCheck = new BinaryWriter(File.Open("helpCheck", FileMode.Open));
                //write help check value to file
                helpCheck.Write(helpCheckValue);
                // close BinaryWriter
                helpCheck.Close();                
            }
            catch (Exception ex)
            {
                // if exception occurs, write exception to console
                Console.WriteLine("Exception: " + ex);
            }
        }
    }
}
