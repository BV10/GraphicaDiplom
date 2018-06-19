using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class FormSizeOfSquare : Form
    {
        public int SizeOfSideSquare { get; private set; }

        public FormSizeOfSquare()
        {
            InitializeComponent();
        }

        private void butChooseSize_Click(object sender, EventArgs e)
        {
            try
            {
                SizeOfSideSquare = int.Parse(tBoxSizeOfSquare.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Not valid size of sqaure!!!");
            }
            this.Close();
        }

        private void tBoxSizeOfSquare_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                butChooseSize.PerformClick();
        }

        private void tBoxSizeOfSquare_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
