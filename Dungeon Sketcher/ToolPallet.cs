using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dungeon_Sketcher.engine;

namespace Dungeon_Sketcher
{
    public partial class ToolPallet : UserControl
    {
        Plotter plotter;

        public ToolPallet()
        {
            InitializeComponent();
        }

        public void SetPlotter (Plotter plotter)
        {
            this.plotter = plotter;
        }

        private void BtnSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (btnSelect.Checked)
            {
                plotter.ChangeMode(Plotter.Select);
            }
        }

        private void BtnSquare_CheckedChanged(object sender, EventArgs e)
        {
            if (btnSquare.Checked)
            {
                plotter.ChangeMode(Plotter.DrawRectangle);
            }
        }

        private void BtnCircle_CheckedChanged(object sender, EventArgs e)
        {
            if (btnCircle.Checked)
            {
                plotter.ChangeMode(Plotter.DrawEllipse);
            }
        }
    }
}
