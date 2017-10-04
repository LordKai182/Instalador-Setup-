using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xDialog;

namespace MessageBoxDemo
{
    public partial class frmDemo : Form
    {
        public frmDemo()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

            DialogResult result = MsgBox.Show("Are you sure you want to exit?", "Exit", MsgBox.Buttons.YesNo, MsgBox.Icon.Info, MsgBox.AnimateStyle.FadeIn);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Exiting now");
            }
        }

    }
}
