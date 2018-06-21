using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    class DescriptionWork
    {       
        public Label Algorithm { get; set; } // name algorithm
        public Label CurrentFigure { get; set; } // current draw figure
        public Label DesriptDotesPerSec { get; set; } // descript dotes per sec
        public Label DotesPerSec { get; set; } // dotes per sec
        public Label DescrCurrentDote { get; set; }
        public Label CurrentDote { get; set; } // current dote

        public void inVisibleLabels()
        {
            Algorithm.Visible = false;
            DesriptDotesPerSec.Visible = false;
            CurrentFigure.Visible = false;
            DotesPerSec.Visible = false;
            CurrentDote.Visible = false;
            DescrCurrentDote.Visible = false;
        }
    }
}
