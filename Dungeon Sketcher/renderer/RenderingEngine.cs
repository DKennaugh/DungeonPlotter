using Dungeon_Sketcher.engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dungeon_Sketcher.renderer
{
    class RenderingEngine
    {
        Window parent;
        PictureBox drawingSurface;
        Plotter plotter;
        ViewFinder camera;
        Graphics g;

        //Brushes
        SolidBrush brush;
        Pen pen;

        public RenderingEngine (Window parent, PictureBox drawingSurface, Plotter plotter, Graphics g)
        {
            this.parent = parent;
            this.drawingSurface = drawingSurface;
            this.plotter = plotter;
            camera = plotter.Camera;
            this.g = g;

            //Brushes
            brush = new SolidBrush(Color.White);
            pen = new Pen(Color.DarkGray);
        }

        public void Paint ()
        {
            //background
            brush.Color = Color.White;
            g.FillRectangle(brush, 0, 0, drawingSurface.Width, drawingSurface.Height);

            //Subgrid
            if (plotter.SubCellMode)
            {
                brush.Color = Color.LightGray;
                for (int y = 0; y <= (float)drawingSurface.Height / camera.CellSize / camera.ZoomLevel + 1; y++)
                {
                    g.FillRectangle(brush, 0, (int)((camera.CellSize * (y + 0.5f) - (float)camera.YOffset % camera.CellSize) * camera.ZoomLevel), drawingSurface.Width, 1);
                }
                for (int x = 0; x <= (float)drawingSurface.Width / camera.CellSize / camera.ZoomLevel + 1; x++)
                {
                    g.FillRectangle(brush, (int)((camera.CellSize * (x + 0.5f) - (float)camera.XOffset % camera.CellSize) * camera.ZoomLevel), 0, 1, drawingSurface.Height);
                }
            }

            //Grid
            brush.Color = Color.Gray;
            for (int y = 0; y <= (float)drawingSurface.Height / camera.CellSize / camera.ZoomLevel + 1; y++)
            {
                g.FillRectangle(brush, 0, (int)((camera.CellSize * y - (float)camera.YOffset % camera.CellSize) * camera.ZoomLevel), drawingSurface.Width, 1);
            }
            for (int x = 0; x <= (float)drawingSurface.Width / camera.CellSize / camera.ZoomLevel + 1; x++)
            {
                g.FillRectangle(brush, (int)((camera.CellSize * x - (float)camera.XOffset % camera.CellSize) * camera.ZoomLevel), 0, 1, drawingSurface.Height);
            }

            //10x10 Cells
            brush.Color = Color.DarkGray;
            for (int y = 0; y <= (float)drawingSurface.Height / camera.CellSize / 10 / camera.ZoomLevel; y++)
            {
                g.FillRectangle(brush, 0, (int)((camera.CellSize * y - (float)camera.YOffset / 10 % camera.CellSize) * 10 * camera.ZoomLevel) - 1, drawingSurface.Width, 3);
            }
            for (int x = 0; x <= (float)drawingSurface.Width / camera.CellSize / 10 / camera.ZoomLevel; x++)
            {
                g.FillRectangle(brush, (int)((camera.CellSize * x - (float)camera.XOffset / 10 % camera.CellSize) * 10 * camera.ZoomLevel) - 1, 0, 3, drawingSurface.Height);
            }


            //Shapes
            pen.Color = Color.Black;
            foreach (AdvShape s in plotter.Shapes)
            {
                s.Draw(g, pen);
            }
            if (plotter.NewShape != null)
            {
                plotter.NewShape.Draw(g, pen);
            }
        }

        public void UpdateGraphicsSettings(Graphics g)
        {
            this.g = g;
        }
    }
}
