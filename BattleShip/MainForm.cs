using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleShip
{
    public partial class MainForm : Form
    {
        //Control[,] userGrid = new Button[10, 10];
        //Control[,] enemyGrid = new Button[10, 10];
        //private State[,] userStates = new State[10, 10];
        //private State[,] enStates = new State[10, 10];
        //private int[] userShips=new int[4]{4,3,2,1};
        //private int[] enemyShips = new int[4] { 4, 3, 2, 1 };
        //private int IMaxLength = 4;
        //private int JMaxLength = 4;

        private Logic objBattle = new Logic();

        public MainForm()
        {
            InitializeComponent();
            System.Reflection.PropertyInfo aProp =
                typeof (Control).GetProperty(
                    "DoubleBuffered",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);

            aProp.SetValue(panel1, true, null);
        }

        private enum State
        {
            Clear,
            Ship,
            Miss,
            Hit
        }

        //private void Form1_Load(object sender, EventArgs e)
        //{
        //    for(int i= 0;i<10;i++)
        //        for (int j = 0; j < 10; j++)
        //        {
        //            enemyGrid[i,j]=new Button();
        //            enemyGrid[i, j].Tag = i.ToString() + j.ToString();
        //            enemyGrid[i,j].Click+=EnemyButtonClick;
        //            Controls.Add(enemyGrid[i, j]);
        //            enemyGrid[i, j].Size=new Size(25,25);
        //            enemyGrid[i, j].Location = new Point(j * 25 + 325, i * 25 + 25);
        //            enemyGrid[i, j].Enabled = false;

        //            userGrid[i, j] = new Button();
        //            userGrid[i, j].Tag = i.ToString() + j.ToString();
        //            userGrid[i, j].Click += UserButtonClick;
        //            Controls.Add(userGrid[i,j]);
        //            userGrid[i,j].Size= new Size(25,25);
        //            userGrid[i, j].Location = new Point(j*25+25, i*25+25);

        //        }
        //}

        //private void UserButtonClick(object sender, EventArgs e)
        //{
        //    int i = Convert.ToInt32((sender as Button).Tag.ToString()[0].ToString());
        //    int j = Convert.ToInt32((sender as Button).Tag.ToString()[1].ToString());
        //    if (userStates[i, j] == State.Clear)
        //    {
        //        if(CheckBut(i,j))
        //        userStates[i, j] = State.Ship;
        //        (sender as Button).Text = "X";
        //    }
        //    else
        //    {
        //        userStates[i, j] = State.Clear;
        //        (sender as Button).Text = "";
        //    }
        //}

        //private bool CheckBut(int i, int j)
        //{

        //}

        //private void EnemyButtonClick(object sender, EventArgs e)
        //{

        //}

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

            int w = panel1.Width/objBattle.ReturnMasSize();
            int h = panel1.Height/objBattle.ReturnMasSize();
            ControlPaint.DrawGrid(e.Graphics, new Rectangle(Point.Empty, panel1.Size), new Size(w, 1), Color.White);
            ControlPaint.DrawGrid(e.Graphics, new Rectangle(Point.Empty, panel1.Size), new Size(1, h), Color.White);
            for (int i = 0; i < objBattle.ReturnMasSize(); i++)
                for (int j = 0; j < objBattle.ReturnMasSize(); j++)
                {
                    if (objBattle[i, j] == 0)
                        e.Graphics.FillRectangle(Brushes.White, j*w + 1, i*h + 1, w - 1, h - 1);
                    if (objBattle[i, j] == 1 || objBattle[i, j] == 2)
                        e.Graphics.FillRectangle(Brushes.Navy, j*w + 1, i*h + 1, w - 1, h - 1);
                }
        }

        private void Game_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!(radioButton1.Enabled == false && radioButton2.Enabled == false && radioButton3.Enabled == false
                  && radioButton4.Enabled == false))
            {
                int w = panel1.Width/objBattle.ReturnMasSize();
                int h = panel1.Height/objBattle.ReturnMasSize();
                int x = e.X/w;
                int y = e.Y/h;
                int size;
                if (radioButton1.Checked) size = 1;
                else if (radioButton2.Checked) size = 2;
                else if (radioButton3.Checked) size = 3;
                else size = 4;
                objBattle.WatchShip(y, x, w, h, size, panel1);
            }
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            objBattle.NullMas();
            panel1.Invalidate();
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                objBattle.ChangeVerctical();
                objBattle.NullMas();
                panel1.Invalidate();
            }
            else
            {
                int w = panel1.Width/objBattle.ReturnMasSize();
                int h = panel1.Height/objBattle.ReturnMasSize();
                int x = e.X/w;
                int y = e.Y/h;
                int size;
                if (radioButton1.Checked) size = 1;
                else if (radioButton2.Checked) size = 2;
                else if (radioButton3.Checked) size = 3;
                else size = 4;
                objBattle.NullMas();
                if (objBattle.CheckSq(x, y, size - 1, objBattle.GetLink()))
                {
                    objBattle.CreateShip(x, y, 2, size - 1, objBattle.GetLink());
                    if (radioButton1.Checked) objBattle.Count1X++;
                    else if (radioButton2.Checked) objBattle.Count2X++;
                    else if (radioButton3.Checked) objBattle.Count3X++;
                    else objBattle.Count4X++;
                    if (objBattle.Count1X > 3) radioButton1.Enabled = false;
                    if (objBattle.Count2X > 2) radioButton2.Enabled = false;
                    if (objBattle.Count3X > 1) radioButton3.Enabled = false;
                    if (objBattle.Count4X > 0) radioButton4.Enabled = false;
                    objBattle.FindNextRb(radioButton1, radioButton2, radioButton3, radioButton4);
                }
                panel1.Invalidate();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            objBattle.NullMas();
            panel1.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            objBattle.Reset();
            radioButton1.Enabled = true;
            radioButton1.Checked = true;
            radioButton2.Enabled = true;
            radioButton3.Enabled = true;
            radioButton4.Enabled = true;
            panel1.Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    objBattle[i, j] = 0;
            int y, x, count = 9, size=0;
            while (count >=0)
            {
                Random r = new Random();
                if(count<4) size = 0;
                else if (count < 7) size = 1;
                else if (count < 9) size = 2;
                else if (count  == 9) size = 3;
                y = r.Next(0, 10);
                x = r.Next(0, 10);
                if (r.Next(0, 2) == 1)
                    objBattle.ChangeVerctical();
                if (objBattle.CheckSq(x, y, size, objBattle.GetLink()))
                {
                    objBattle.CreateShip(x, y, 2, size, objBattle.GetLink());
                    count--;
                }
            }
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            radioButton3.Enabled = false;
            radioButton4.Enabled = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            panel1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

    }
}
