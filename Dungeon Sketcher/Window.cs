using Dungeon_Sketcher.engine;
using Dungeon_Sketcher.renderer;
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
    public partial class Window : Form
    {
        Plotter plotter;
        RenderingEngine renderer;
        Bitmap canvas;
        Graphics g;
        WindowTool wt;
        ToolPallet toolSelector;

        public Plotter Sketcher { get => plotter; }
        public Window()
        {
            InitializeComponent();
            drawingSurface.Size = this.ClientSize;
            canvas = new Bitmap(this.ClientRectangle.Width,
               this.ClientRectangle.Height,
               System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            drawingSurface.Image = canvas;
            tmrRender.Interval = 16;
            
        }

        protected override void OnResize(EventArgs e)
        {
            UpdateGraphicsSettings();
        }

        private void UpdateGraphicsSettings()
        {
            //Dispose();

            drawingSurface.Size = ClientSize;
            canvas = new Bitmap(this.ClientRectangle.Width,
               this.ClientRectangle.Height,
               System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            g = Graphics.FromImage(canvas);
            if (renderer != null)
            {
                renderer.UpdateGraphicsSettings(g);
            }
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
        }

        private void TmrRender_Tick(object sender, EventArgs e)
        {
            renderer.Paint();
            plotter.Tick();
            drawingSurface.Image = canvas;
            Invalidate();
        }

        private void Window_Load(object sender, EventArgs e)
        {
            plotter = new Plotter(this, drawingSurface);
            renderer = new RenderingEngine(this, drawingSurface, plotter, g);
            wt = new WindowTool(this);
            toolSelector = wt.Tools;

            UpdateGraphicsSettings();
            renderer.Paint();
            tmrRender.Start();
            wt.Show();
        }

        private void ToolPalletToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wt.Show();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers.Equals(Keys.Alt))
            {
                e.Handled = true;
            }
        }
    }
}
