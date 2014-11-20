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
    public partial class FormForIP : Form
    {
        public FormForIP()
        {
            InitializeComponent();
        }

        private void IPHere_Enter(object sender, EventArgs e)
        {
            if (IPHere.Text == (String)IPHere.Tag)
            {
                IPHere.Text = "";
            }
        }

        private void IPHere_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(IPHere.Text))
            {
                IPHere.Text = (String)IPHere.Tag;
            }
        }

        private void Accept_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
