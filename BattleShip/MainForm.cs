using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShip
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control[,] userGrid = new Button[10, 10];
            Control[,] pcGrid=new Button[10,10];
            for(int i= 0;i<10;i++)
                for (int j = 0; j < 10; j++)
                {
                    pcGrid[i,j]=new Button();
                    this.Controls.Add(pcGrid[i, j]);
                    pcGrid[i, j].Size=new Size(25,25);
                    pcGrid[i, j].Location = new Point(j * 25 + 325, i * 25 + 25);
                    userGrid[i, j] = new Button();
                    this.Controls.Add(userGrid[i,j]);
                    userGrid[i,j].Size= new Size(25,25);
                    userGrid[i, j].Location = new Point(j*25+25, i*25+25);
                }
        }
    }
}
