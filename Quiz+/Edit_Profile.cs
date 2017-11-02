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

namespace Quiz_
{
    public partial class Edit_Profile : Form
    {
        public Edit_Profile()
        {
            InitializeComponent();
        }

        PictureBox avatar;
        bool SaveCheckValue;
        string hexString;
        Profile[] profile = new Profile[5];
        int profileDimension;
        Color color;
        int profileAmount;

        private void pboxHome_Click(object sender, EventArgs e)
        {
            // method to check changes are saved
            SaveCheck();

            // if user confirms changes are saved
            if (SaveCheckValue == true)
            {
                // moves to Main_Menu form
                Main_Menu Main_Menu = new Main_Menu();
                Main_Menu.Show();
                Main_Menu.Activate();
                this.Hide();  
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

        private void chkEnablePassword_CheckedChanged(object sender, EventArgs e)
        {
            // password textbox enabled/disabled according to the chkEnablePassword checkbox
            if(chkEnablePassword.Checked == true)
            {
                txtPassword.Enabled = true;
            }
            else
            {
                txtPassword.Enabled = false;
            }
        }

        private void Edit_Profile_FormClosing(object sender, FormClosingEventArgs e)
        {
            // method to check changes are saved
            SaveCheck();
        }

        private void SaveCheck()
        {
            // message to check changes are saved
            if(MessageBox.Show("Have you saved your changes?", "Edit Profile", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SaveCheckValue = true;
            }
            else
            {
                SaveCheckValue = false;
            }
        }


        private void Edit_Profile_Load(object sender, EventArgs e)
        {
            // read in profiles from txt file using StreamReader
            System.IO.StreamReader profileInfo = new System.IO.StreamReader(Application.StartupPath + "\\ProfileInfo.txt");
            // check the number of saved profiles
            profileAmount = Convert.ToInt32(profileInfo.ReadLine());
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
                    profile[i].AveragePoints = Convert.ToInt32(profileInfo.ReadLine());

                    // DEBUG: to check profile info
                    txtDebug.Text += profile[i].Username + profile[i].Password + profile[i].Avatar + Convert.ToString(profile[i].GamesPlayed) + Convert.ToString(profile[i].TotalPoints) + Convert.ToString(profile[i].AveragePoints);
                }
            }
            // closes StreamReader
            profileInfo.Close();

            // reads in selected profile using StreamReader
            System.IO.StreamReader profileSelectedRead = new System.IO.StreamReader(Application.StartupPath + "\\ProfileSelected.txt");
            // 6 lines skipped
            for (int i = 0; i < 6; i++)
            {
                profileSelectedRead.ReadLine();
            }
            // 7th line is profile dimension which is used to find which profile[i] is the selected profile
            profileDimension = Convert.ToInt32(profileSelectedRead.ReadLine());
            // DEBUG
            txtDebug.Text += "profile dimension " + profileDimension.ToString();
            
            hexString = profile[profileDimension].Avatar;
            // hexadecimal string convertde to a Color
            int red = int.Parse(hexString.Substring(1, 2), NumberStyles.HexNumber);
            int green = int.Parse(hexString.Substring(3, 2), NumberStyles.HexNumber);
            int blue = int.Parse(hexString.Substring(5, 2), NumberStyles.HexNumber);
            color = Color.FromArgb(red, green, blue);
            // custom avatar PictureBox BackColor set to color
            pboxAvatarCustom.BackColor = color;
            // custom avatar Preview BackColor set to color
            nudRed.Value = red;
            nudGreen.Value = green;
            nudBlue.Value = blue;
            // check if password exists
            if(profile[profileDimension].Password != "")
            {
                chkEnablePassword.Checked = true;
                txtPassword.Enabled = true;
                txtPassword.Text = profile[profileDimension].Password;
            }
            else
            {
                chkEnablePassword.Checked = false;
                txtPassword.Text = "";
                txtPassword.Enabled = false;               
            }
            // closes StreamReader
            profileSelectedRead.Close();
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            // username and password assigned to Text property from respective TextBoxes
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            
            bool checkUsername = true;
            // if a new username has been entered
            if (username != "")
            {
                // check availability of username
                for (int i = 0; i < profileAmount; i++)
                {
                    // if username is taken sets checkUsername to false to indicate not to assign the new username 
                    if (username == profile[i].Username)
                    {
                        checkUsername = false;
                        break;
                    }                    
                }  
                // if username is not taken
                if(checkUsername)
                {
                    profile[profileDimension].Username = username;
                }
                else // if username is taken display message
                {                    
                    MessageBox.Show("The username you have chosen is already taken. Please try another.", "Edit Profile", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsername.Focus();
                    return;
                }
            }
            // DEBUG: new username is OK
            txtDebug.Text += "passed username";

            // if the profile has no password and the enable password checkbox is checked
            if (profile[profileDimension].Password == "" && chkEnablePassword.Checked == true)
            {
                // if no password is entered message appears
                if (password == "")
                {
                    MessageBox.Show("Enter a password with at least 1 character or disable password", "Edit Profile", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else // if password is entered, it is assigned as the profiles password
                {
                    profile[profileDimension].Password = password;
                }                              
            }
            else if (chkEnablePassword.Checked == false) // if enable password checkbox is not checked, profile password set to ""; password disabled
            {
                profile[profileDimension].Password = "";
            }
            // DEBUG: got past password stage
            txtDebug.Text += "passed password";
            // profile avatar set equal to hexString
            profile[profileDimension].Avatar = hexString;
            // write existing profiles with edited info back into ProfileInfo.txt using StreamWriter
            System.IO.StreamWriter profileWrite = new System.IO.StreamWriter(Application.StartupPath + "\\ProfileInfo.txt");
            // write in number of profiles
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
            MessageBox.Show("Changes saved", "Edit Profile", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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

        private void AvatarSelector()
        {
            hexString = ColorTranslator.ToHtml(avatar.BackColor);
            int red = int.Parse(hexString.Substring(1, 2), NumberStyles.HexNumber);
            int green = int.Parse(hexString.Substring(3, 2), NumberStyles.HexNumber);
            int blue = int.Parse(hexString.Substring(5, 2), NumberStyles.HexNumber);
            color = Color.FromArgb(red, green, blue);
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
            // custom avatar panel turns invisble
            pnlAvatarCustom.Visible = false;
        }

        private void pboxAvatarCustom_DoubleClick(object sender, EventArgs e)
        {
            // custom avatar panel turns visible
            pnlAvatarCustom.Visible = true;
            pnlRGBView.BackColor = pboxAvatarCustom.BackColor;
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
            txtDebug.Text += profile[profileDimension].Username;
        }

        private void btnDeleteProfile_Click(object sender, EventArgs e)
        {
            // warning message if user wants to delete profile
            if (MessageBox.Show("Are you sure you wish to delete your profile?", "Delete Profile", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                // final warning message
                if(MessageBox.Show("Once deleted the account cannot be recovered. Do you still wish to delete your profile?", "Delete Profile", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    // method to delete profile
                    DeleteProfile();
                }
            }
        }

        private void DeleteProfile()
        {
            // writes profile info to ProfileInfo.txt using StreamWriter
            System.IO.StreamWriter profileWrite = new System.IO.StreamWriter(Application.StartupPath + "\\ProfileInfo.txt");
            // number of profiles - 1 to account for deleted profile written
            profileWrite.WriteLine(Convert.ToString(profileAmount - 1));
            // profiles written
            for (int i = 0; i < profileAmount; i++)
            {
                // will write the current profile class as long as it is not the active profile
                if(i != profileDimension)
                {
                    profileWrite.WriteLine(profile[i].Username);
                    profileWrite.WriteLine(profile[i].Password);
                    profileWrite.WriteLine(profile[i].Avatar);
                    profileWrite.WriteLine(profile[i].GamesPlayed);
                    profileWrite.WriteLine(profile[i].TotalPoints);
                    profileWrite.WriteLine(profile[i].AveragePoints);
                }                
            }
            // closes StreamWriter
            profileWrite.Close();

            // confirmation message
            if(MessageBox.Show("Profile deletion complete. Returning to Start Menu", "Delete Profile", MessageBoxButtons.OK,MessageBoxIcon.Information) == DialogResult.OK)
            {
                // moves to Start_Menu form
                Start_Menu Start_Menu = new Start_Menu();
                Start_Menu.Show();
                Start_Menu.Activate();
                this.Hide();  
            }
        }
    }
}
