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
    public partial class FormRadiusOfCircle : Form
    {
        public int RadiusOfCircle { get; private set; }

        public FormRadiusOfCircle()
        {
            InitializeComponent();
        }

        private void butChooseRadius_Click(object sender, EventArgs e)
        {
            try
            {
                RadiusOfCircle = int.Parse(tBoxRadius.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Not valid size of sqaure!!!");
            }
            this.Close();
        }

        private void tBoxRadius_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                butChooseRadius.PerformClick();
        }
    }    
}
