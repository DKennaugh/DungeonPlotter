using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dungeon_Sketcher
{
    public partial class WindowTool : Form
    {
        Window parent;
        public WindowTool(Window parent)
        {
            this.parent = parent;
            InitializeComponent();
            toolPallet.SetPlotter(parent.Sketcher);
            
        }
        public ToolPallet Tools { get => toolPallet; }

        private void WindowTool_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
