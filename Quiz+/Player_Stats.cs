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
    public partial class Player_Stats : Form
    {
        public Player_Stats()
        {
            InitializeComponent();
        }    

        private void pboxHome_Click(object sender, EventArgs e)
        {
            // moves to Main_Menu form
            Main_Menu Main_Menu = new Main_Menu();
            Main_Menu.Show();
            Main_Menu.Activate();
            this.Hide();
        }

        private void Player_Stats_Load(object sender, EventArgs e)
        {
            // reads in selected profile using StreamReader
            System.IO.StreamReader profileSelected = new System.IO.StreamReader(Application.StartupPath + "\\ProfileSelected.txt");
            string[] profileInfo = new string[6];
            // info read in is assigned to a string array
            profileInfo[0] = profileSelected.ReadLine();
            profileInfo[1] = profileSelected.ReadLine();
            profileInfo[2] = profileSelected.ReadLine();
            profileInfo[3] = profileSelected.ReadLine();
            profileInfo[4] = profileSelected.ReadLine();
            profileInfo[5] = profileSelected.ReadLine();
            // info from array assigned to respective Labels
            lblUsername.Text = profileInfo[0];
            lblGamesPlayed.Text = profileInfo[3];
            lblTotalPoints.Text = profileInfo[4];
            lblAvPoints.Text = profileInfo[5];
            // closes StreamReader
            profileSelected.Close();
            // align stats
            Label[] stats = new Label[4];
            stats[0] = lblUsername;
            stats[1] = lblGamesPlayed;
            stats[2] = lblTotalPoints;
            stats[3] = lblAvPoints;
            for(int i = 0; i < 4; i++)
            {
                int divisor = pnlStats.Width - stats[i].Width;
                int x = divisor / 2;
                int y = 6 + (20*i);
                stats[i].Location = new Point(x, y);
            }
            
            
        }


    }
}
