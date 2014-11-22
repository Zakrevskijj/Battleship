using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using System.Threading;

namespace BattleShip
{
    public partial class MainForm : Form
    {
        public Logic objBattle = new Logic();
        private bool _net;
        private bool host;
        public static int X, Y;
        Network nw;
        public MainForm()
        {
            nw = new Network(this);
            InitializeComponent();
            System.Reflection.PropertyInfo aProp =
                typeof (Control).GetProperty(
                    "DoubleBuffered",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);

            aProp.SetValue(panel1, true, null);
            aProp.SetValue(panel2, true, null);
        }

        #region Paints
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
                        e.Graphics.FillRectangle(Brushes.White, j * w + 1, i * h + 1, w - 1, h - 1);
                    if (objBattle[i, j] == 2 || objBattle[i, j] == 1)
                        e.Graphics.FillRectangle(Brushes.Navy, j * w + 1, i * h + 1, w - 1, h - 1);
                    if (objBattle[i, j] == 4)
                        e.Graphics.FillRectangle(Brushes.LightCoral, j * w + 1, i * h + 1, w - 1, h - 1);
                    if (objBattle[i, j] == 3)
                        e.Graphics.FillRectangle(Brushes.Red, j * w + 1, i * h + 1, w - 1, h - 1);
                   
                }
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            int w = panel1.Width / objBattle.ReturnMasSize();
            int h = panel1.Height / objBattle.ReturnMasSize();
            ControlPaint.DrawGrid(e.Graphics, new Rectangle(Point.Empty, panel1.Size), new Size(w, 1), Color.White);
            ControlPaint.DrawGrid(e.Graphics, new Rectangle(Point.Empty, panel1.Size), new Size(1, h), Color.White);
            for (int i = 0; i < objBattle.ReturnMasSize(); i++)
                for (int j = 0; j < objBattle.ReturnMasSize(); j++)
                {
                    if (objBattle.GetEnemyValue(i, j) < 3)
                        e.Graphics.FillRectangle(Brushes.White, j * w + 1, i * h + 1, w - 1, h - 1);
                    //if (objBattle.GetEnemyValue(i, j) == 2)
                    //    e.Graphics.FillRectangle(Brushes.Red, j * w + 1, i * h + 1, w - 1, h - 1);
                    if (objBattle.GetEnemyValue(i, j) == 3)
                        e.Graphics.FillRectangle(Brushes.Red, j * w + 1, i * h + 1, w - 1, h - 1);
                    if (objBattle.GetEnemyValue(i, j) == 4)
                        e.Graphics.FillRectangle(Brushes.LightCoral, j * w + 1, i * h + 1, w - 1, h - 1);
                }
        }
        #endregion

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
                //wcf
                int size;
                if (radioButton1.Checked) size = 1;
                else if (radioButton2.Checked) size = 2;
                else if (radioButton3.Checked) size = 3;
                else size = 4;
                int x = e.X/w;
                int y = e.Y/h;
                if (x == 9 && size != 1 && !Logic.Vertical)
                    x -= size - 1;
                if (y == 9 && size != 1 && Logic.Vertical)
                    y -= size - 1;
                objBattle.NullMas();

                if (objBattle.CheckSq(x, y, size - 1, objBattle.GetLink()))
                {
                    objBattle.CreateShip(x, y, 2, size - 1, objBattle.GetLink());
                    if (radioButton1.Checked) objBattle.Count1X++;
                    else if (radioButton2.Checked) objBattle.Count2X++;
                    else if (radioButton3.Checked) objBattle.Count3X++;
                    else objBattle.Count4X++;
                    if (objBattle.Count1X > 3 && radioButton1.Enabled)
                    {
                        radioButton1.Enabled = false;
                        objBattle.FindNextRb(radioButton1, radioButton2, radioButton3, radioButton4);
                    }
                    else if (objBattle.Count2X > 2 && radioButton2.Enabled)
                    {
                        radioButton2.Enabled = false;
                        objBattle.FindNextRb(radioButton1, radioButton2, radioButton3, radioButton4);
                    }
                    else if (objBattle.Count3X > 1 && radioButton3.Enabled)
                    {
                        radioButton3.Enabled = false;
                        objBattle.FindNextRb(radioButton1, radioButton2, radioButton3, radioButton4);
                    }
                    else if (objBattle.Count4X > 0 && radioButton4.Enabled)
                    {
                        radioButton4.Enabled = false;
                        objBattle.FindNextRb(radioButton1, radioButton2, radioButton3, radioButton4);
                    }
                    if (objBattle.Count4X > 0 && objBattle.Count3X > 1 && objBattle.Count2X > 2 && objBattle.Count1X > 3)
                    {
                        buttonReady.Enabled = true;
                    }
                }
                panel1.Invalidate();
            }
        }

        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !buttonAutoGen.Enabled)
            {
                int w = panel1.Width / objBattle.ReturnMasSize();
                int h = panel1.Height / objBattle.ReturnMasSize();
                int x = e.X / w;
                int y = e.Y / h;
                if (!_net)
                {
                    if (objBattle.GetEnemyValue(y, x) == 2)
                    {
                        objBattle.SetEnemyValue(y, x, 3);
                        objBattle.EnemyShip--;
                        objBattle.Explosion(x, y, objBattle.GetEnemyLink());
                        panel2.Invalidate();
                        if (objBattle.EnemyShip == 0)
                            Victory("Победил человек");
                    }
                    else if (objBattle.GetEnemyValue(y, x) == 0)
                    {
                        EnemyTurn();
                        objBattle.SetEnemyValue(y, x, 4);
                        panel2.Invalidate();
                    }
                }
                else
                {
                    string boom = x + y.ToString();
                    new Thread(nw.ThreadSend).Start(boom);
                    new Thread(nw.Receiver).Start();
                }
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
            buttonReady.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var r = new Random();
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    objBattle[i, j] = 0;
            int count = 9, size=0;
            while (count >= 0)
            {
                if (count < 4) size = 1;
                else if (count < 7) size = 2;
                else if (count < 9) size = 3;
                else if (count == 9) size = 4;
                int y = r.Next(0, 10);
                int x = r.Next(0, 10);
                if (r.Next(0, 2) == 1)
                    objBattle.ChangeVerctical();
                objBattle.NullMas();

                if (objBattle.CheckSq(x, y, size - 1, objBattle.GetLink()))
                {
                    objBattle.CreateShip(x, y, 2, size - 1, objBattle.GetLink());
                    count--;
                }
            }
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            radioButton3.Enabled = false;
            radioButton4.Enabled = false;
            panel1.Invalidate();
            buttonReady.Enabled = true;
        }

        public void EnemyTurn()
        {
            Random r = new Random();
            bool flag = true;
            while (flag)
            {
                if (!_net)
                {
                    X = r.Next(0, 10);
                    Y = r.Next(0, 10);
                }
                if (objBattle[X, Y] == 0 || objBattle[X, Y] == 2)
                    flag = false;
                if (objBattle[X, Y] == 0)
                {
                    objBattle[X, Y] = 4;
                    if (_net)
                    nw.ThreadSend(4);
                    
                    panel1.Invalidate();
                }
                else
                {
                    objBattle[X, Y] = 3;
                    if (_net)
                    nw.ThreadSend(3);
                    objBattle.MyShip--;
                    objBattle.Explosion(X, Y, objBattle.GetLink());
                    if (objBattle.MyShip == 0 && !_net)
                        Victory("Победил компьютер");
                }
                
            }
            if(_net)
                new Thread(nw.Receiver).Start();
        }

        private void Victory(string str)
        {
            MessageBox.Show(str);
        }

        private void buttonReady_Click(object sender, EventArgs e)
        {
            panel1.Enabled = false;
            label1.Location = new Point(label1.Location.X - 75, label1.Location.Y);
            buttonReady.Enabled = false;
            buttonReset.Enabled = false;
            buttonAutoGen.Enabled = false;
            if (_net)
            {
                if (!host)
                {
                    new Thread(nw.Receiver).Start();
                    panel2.Enabled = false;
                }
            }
            else
            {
                objBattle.Count1X = 0;
                objBattle.Count2X = 0;
                objBattle.Count3X = 0;
                objBattle.Count4X = 0;
                int y, x, count = 10, size = 3;
                while (count > 0)
                {
                    Random r = new Random();
                    if (count < 10) size = 2;
                    if (count < 8) size = 1;
                    if (count < 5) size = 0;
                    y = r.Next(0, 10);
                    x = r.Next(0, 10);
                    if (r.Next(0, 2) == 1)
                        objBattle.ChangeVerctical();
                    if (objBattle.CheckSq(x, y, size, objBattle.GetEnemyLink()))
                    {
                        objBattle.CreateShip(x, y, 2, size, objBattle.GetEnemyLink());
                        count--;
                    }
                }              
            }
            panel2.Invalidate();
        }

        private void сКомпьютеромToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _net = false;
            label1.Text = @"Играем против компьютера";
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _net = true;
            host = true;
            new Thread(nw.Receiver).Start();
            label1.Text = @"Играем по сети";
        }

        private void присоединитсяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormForIP ffip=new FormForIP();
            if (ffip.ShowDialog()==DialogResult.OK)
            {
                _net = true;
                label1.Text = @"Играем по сети";
                nw.Ip = IPAddress.Parse(ffip.IPHere.Text);
                nw.Connect();
            }
            else 
            {
                MessageBox.Show("Пуффф");
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nw.Disconnect();
            Dispose(true);
            Close();
            Application.Exit();
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            panel2.Invalidate();
        }

    }
}
