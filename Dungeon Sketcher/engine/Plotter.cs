using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dungeon_Sketcher.engine
{
    public class Plotter
    {
        Window parent;
        PictureBox drawingSurface;
        ViewFinder camera;

        ArrayList shapes;
        ArrayList selectedShape;
        AdvShape newShape;

        int cellSize =70;

        //Modes
        int mode = 0;
        static int modeSelect = 1;
        static int modeDrawRect = 2;
        static int modeDrawEllip = 4;

        //ComboKeys
        int key = 0;
        int keyAlt = 1;
        int keyShift = 2;
        int keyCtrl = 4;

        bool isPanning = false;
        Point panStart;
        bool isDrawing = false;
        Point shapeStart;
        bool isMoving = false;
        Point moveStart;
        Point cursorOffset;

        public ViewFinder Camera { get => camera; }
        private double XOffset { get => camera.XOffset; set => camera.XOffset= value; }
        private double YOffset { get => camera.YOffset; set => camera.YOffset = value; }
        private double ZoomLevel { get => camera.ZoomLevel; set => camera.ZoomLevel = value; }
        private int CellSize { get => camera.CellSize; set => camera.CellSize = value; }
        public bool SubCellMode { get => (key & keyAlt) == keyAlt; }
        public ArrayList Shapes { get => shapes; }
        public AdvShape NewShape { get => newShape; }
        public static int Select { get => modeSelect; }
        public static int DrawRectangle { get => modeDrawRect; }
        public static int DrawEllipse { get => modeDrawEllip; }

        public Plotter(Window parent, PictureBox drawingSurface)
        {
            this.parent = parent;
            this.drawingSurface = drawingSurface;
            shapes = new ArrayList();
            selectedShape = new ArrayList();
            camera = new ViewFinder(cellSize);

            //Event Listeners
            drawingSurface.MouseDown += new MouseEventHandler(this.OnMouseDown);
            drawingSurface.MouseUp += new MouseEventHandler(this.OnMouseUp);
            drawingSurface.MouseWheel += new MouseEventHandler(this.OnMouseScroll);
            parent.KeyDown += new KeyEventHandler(this.OnKeyPress);
            parent.KeyUp += new KeyEventHandler(this.OnKeyRelease);
        }

        public void OnMouseDown(Object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    if (isDrawing)
                    {
                        selectedShape.Clear();
                        isDrawing = false;
                    }else
                    {
                        //bring up context menu
                    }
                    break;
                case MouseButtons.Middle:
                    isPanning = true;
                    panStart = Stylus;
                    break;
                case MouseButtons.Left:
                    cursorOffset = new Point(Cursor.Position.X - e.Location.X, Cursor.Position.Y - e.Location.Y);

                    if ((mode & modeSelect) == modeSelect)
                    {
                        bool hasFound = false;
                        isMoving = true;
                        foreach (AdvShape s in shapes)
                        {
                            if (s.OnEdge(e.Location) && !hasFound)
                            {
                                if (selectedShape.Contains(s))
                                {
                                    selectedShape.Remove(s);
                                }
                                else
                                {
                                    selectedShape.Add(s);
                                    moveStart = new Point((int)((Camera.XOffset + Stylus.X) / Camera.ZoomLevel), (int)((Camera.YOffset + Stylus.Y) / Camera.ZoomLevel));

                                    if (s is AdvEllipse)
                                    {
                                        shapeStart = ((AdvEllipse)s).Center;
                                    }
                                    else
                                    {
                                        shapeStart = new Point(s.X, s.Y);
                                    }
                                }
                                hasFound = true;
                            }
                        }
                    }
                    if ((mode & modeDrawRect) == modeDrawRect)
                    {
                        isDrawing = true;
                        if (SubCellMode)
                        {
                            newShape = new AdvRectangle(camera.SnapToHalfGrid(e.Location.X), camera.SnapToHalfGrid(e.Location.Y), camera.CellSize, camera.CellSize, camera);
                            shapeStart = new Point((int)newShape.X, (int)newShape.Y);
                        }
                        else
                        {
                            newShape = new AdvRectangle(camera.SnapToGrid(e.Location.X), camera.SnapToGrid(e.Location.Y), camera.CellSize, camera.CellSize, camera);
                            shapeStart = new Point((int)newShape.X, (int)newShape.Y);
                        }
                        
                    }
                    if ((mode & modeDrawEllip) == modeDrawEllip)
                    {
                        isDrawing = true;
                        if (SubCellMode)
                        {
                            newShape = new AdvEllipse(camera.SnapToHalfGrid(e.Location), camera.CellSize/2, camera);
                        }
                        else
                        {
                            newShape = new AdvEllipse(camera.SnapToGrid(e.Location), camera.CellSize, camera);
                        }
                        
                    }
                    break;
            }
        }

        public void OnMouseUp (Object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    
                    break;
                case MouseButtons.Middle: //stop panning
                    isPanning = false;
                    break;
                case MouseButtons.Left:
                    isDrawing = false;
                    isMoving = false;
                    if ((mode & modeDrawRect) == modeDrawRect)//Finish Rectangle
                    {
                        if (!(newShape.Width == 0 || newShape.Height == 0))
                        {//If shape is valid; add to list
                            shapes.Add(newShape);
                        }
                        newShape = null;

                    }else if ((mode & modeDrawEllip) == modeDrawEllip)//Finish Ellipse
                    {
                        if (!(((AdvEllipse)newShape).RadiusX == 0 || ((AdvEllipse)newShape).RadiusY == 0))
                        {//If shape is valid; add to list
                            shapes.Add(newShape);
                        }
                        newShape = null;

                    }

                    break;
            }
        }

        public void OnMouseScroll (Object sender, MouseEventArgs e)
        {
            if ((key & keyAlt) == keyAlt)//Zoom
            {
                double oldZoom = ZoomLevel;
                ZoomLevel += e.Delta / 2400f;

                XOffset = XOffset + Stylus.X / oldZoom - Stylus.X / ZoomLevel;
                YOffset = YOffset + Stylus.Y / oldZoom - Stylus.Y / ZoomLevel;
            }
            else if ((key & keyCtrl)==keyCtrl)//Horizontal Pan
            {
                XOffset += e.Delta / Camera.ZoomLevel;
            }else//Vertical Pan
            {
                YOffset += e.Delta / Camera.ZoomLevel;
            }
            
        }

        public void OnKeyPress (Object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ControlKey://Change Scroll Direction
                    key |= keyCtrl;
                    break;
                case Keys.Menu://Activate SubCells
                    key |= keyAlt;
                    break;
                case Keys.ShiftKey://Activate Zoom
                    key |= keyShift;
                    break;
                case Keys.Delete://Delete a shape
                    if (selectedShape != null && (mode & modeSelect) == modeSelect)
                    {
                        shapes.Remove(selectedShape);
                        selectedShape = null;
                    }
                    break;
                case Keys.Left:
                    if (selectedShape.Count != 0 && (mode & modeSelect) == modeSelect)
                    {
                        foreach (AdvShape s in selectedShape)
                        { 
                            if (SubCellMode)
                            {
                                s.X = Camera.SnapToHalfGrid(s.X - Camera.CellSize / 2);
                            }
                            else
                            {
                                s.X = Camera.SnapToGrid(s.X - Camera.CellSize);
                            }
                        }
                    }
                        break;
                case Keys.Right:
                    if (selectedShape.Count != 0 && (mode & modeSelect) == modeSelect)
                    {
                        foreach (AdvShape s in selectedShape)
                        {
                            if (SubCellMode)
                            {
                                s.X = Camera.SnapToHalfGrid(s.X + Camera.CellSize / 2);
                            }
                            else
                            {
                                s.X = Camera.SnapToGrid(s.X + Camera.CellSize);
                            }
                        }
                    }
                    break;
                case Keys.Up:
                    if (selectedShape.Count != 0 && (mode & modeSelect) == modeSelect)
                    {
                        foreach (AdvShape s in selectedShape)
                        {
                            if (SubCellMode)
                            {
                                s.Y = Camera.SnapToHalfGrid(s.Y - Camera.CellSize / 2);
                            }
                            else
                            {
                                s.Y = Camera.SnapToGrid(s.Y - Camera.CellSize);
                            }
                        }
                    }
                    break;
                case Keys.Down:
                    if (selectedShape.Count != 0 && (mode & modeSelect) == modeSelect)
                    {
                        foreach (AdvShape s in selectedShape)
                        {
                            if (SubCellMode)
                            {
                                s.Y = Camera.SnapToHalfGrid(s.Y + Camera.CellSize / 2);
                            }
                            else
                            {
                                s.Y = Camera.SnapToGrid(s.Y + Camera.CellSize);
                            }
                        }
                    }
                    break;
            }
        }

        public void OnKeyRelease (Object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                    key ^= keyCtrl;
                    break;
                case Keys.Menu:
                    key ^= keyAlt;
                    break;
                case Keys.ShiftKey:
                    key ^= keyShift;
                    break;
            }
        }

        public void ChangeMode (int mode)
        {
            this.mode = 0;
            this.mode |= mode;
            selectedShape.Clear();
            parent.Focus();
        }

        public void Tick()
        {
            if (isPanning)
            {
                XOffset += panStart.X - Stylus.X;
                YOffset += panStart.Y - Stylus.Y;
                panStart = Stylus;
            }
            if (isMoving && selectedShape != null)
            {
                foreach (AdvShape s in selectedShape)
                { 
                    if (s is AdvEllipse)//Ellipse Move
                    {
                        if (SubCellMode)//Ellipse Move
                        {

                            ((AdvEllipse)s).Center = new Point(
                                camera.SnapToHalfGrid(shapeStart.X - (moveStart.X - (Camera.XOffset + Stylus.X) / ZoomLevel) + Camera.CellSize / 4),
                                camera.SnapToHalfGrid(shapeStart.Y - (moveStart.Y - (Camera.YOffset + Stylus.Y) / ZoomLevel) + Camera.CellSize / 4));
                        }
                        else
                        {
                            ((AdvEllipse)s).Center = new Point(
                                camera.SnapToGrid(shapeStart.X - (moveStart.X - (Camera.XOffset + Stylus.X) / ZoomLevel) + Camera.CellSize / 2),
                                camera.SnapToGrid(shapeStart.Y - (moveStart.Y - (Camera.YOffset + Stylus.Y) / ZoomLevel) + Camera.CellSize / 2));
                        }
                    }
                    else
                    {
                        if (SubCellMode)//Rectangle Move
                        {
                            s.X = camera.SnapToHalfGrid(shapeStart.X - (moveStart.X - (Camera.XOffset + Stylus.X) / ZoomLevel) + Camera.CellSize / 4);
                            s.Y = camera.SnapToHalfGrid(shapeStart.Y - (moveStart.Y - (Camera.YOffset + Stylus.Y) / ZoomLevel) + Camera.CellSize / 4);
                        } else
                        {
                            s.X = camera.SnapToGrid(shapeStart.X - (moveStart.X - (Camera.XOffset + Stylus.X) / ZoomLevel) + Camera.CellSize / 2);
                            s.Y = camera.SnapToGrid(shapeStart.Y - (moveStart.Y - (Camera.YOffset + Stylus.Y) / ZoomLevel) + Camera.CellSize / 2);
                        }
                    }
                }
            }
            if (isDrawing)
            {
                if (newShape is AdvRectangle)
                {
                    if ((Stylus.X) > shapeStart.X - camera.XOffset)
                    {
                        if (SubCellMode)
                        {
                            newShape.Width = camera.SnapToHalfGrid(Stylus.X - newShape.X + (int)(camera.CellSize * 1.25)) + (int)camera.XOffset;
                        }
                        else
                        {
                            newShape.Width = camera.SnapToGrid(Stylus.X - newShape.X + (int)(camera.CellSize * 1.25)) + (int)camera.XOffset;
                        }
                    }
                    else
                    {
                        if (SubCellMode)
                        {
                            newShape.X = camera.SnapToHalfGrid(Stylus.X);
                        }
                        else
                        {
                            newShape.X = camera.SnapToGrid(Stylus.X);
                        }
                        newShape.Width = camera.SnapToGrid(shapeStart.X - newShape.X) + (int)camera.XOffset;
                    }
                    if ((Stylus.Y) > shapeStart.Y - camera.YOffset)
                    {
                        if (SubCellMode)
                        {
                            newShape.Height = camera.SnapToHalfGrid(Stylus.Y - newShape.Y + (int)(camera.CellSize * 1.25)) + (int)camera.YOffset;
                        }
                        else
                        {
                            newShape.Height = camera.SnapToGrid(Stylus.Y - newShape.Y + (int)(camera.CellSize * 1.25)) + (int)camera.YOffset;
                        }
                    }
                    else
                    {
                        if (SubCellMode)
                        {
                            newShape.Y = camera.SnapToHalfGrid(Stylus.Y);
                        }
                        else
                        {
                            newShape.Y = camera.SnapToGrid(Stylus.Y);
                        }
                        newShape.Height = camera.SnapToGrid(shapeStart.Y - newShape.Y) + (int)camera.YOffset;
                    }
                }else if (newShape is AdvEllipse)
                {
                    if (SubCellMode)
                    {
                        ((AdvEllipse)newShape).RadiusX = camera.SnapToHalfGrid(
                                (int)Math.Sqrt(
                                    Math.Pow(
                                        ((AdvEllipse)newShape).Center.X - (Stylus.X) - camera.XOffset, 2
                                    ) +
                                    Math.Pow(
                                        ((AdvEllipse)newShape).Center.Y - (Stylus.Y) - camera.YOffset, 2
                                    )
                                )
                            );

                        ((AdvEllipse)newShape).RadiusY = camera.SnapToHalfGrid(
                        (int)Math.Sqrt(
                                    Math.Pow(
                                        ((AdvEllipse)newShape).Center.X - (Stylus.X) - camera.XOffset, 2
                                    ) +
                                    Math.Pow(
                                        ((AdvEllipse)newShape).Center.Y - (Stylus.Y) - camera.YOffset, 2
                                    )
                                )
                        );
                    }
                    else
                    {
                        ((AdvEllipse)newShape).RadiusX = camera.SnapToGrid(
                                (int)Math.Sqrt(
                                    Math.Pow(
                                        ((AdvEllipse)newShape).Center.X - (Stylus.X) - camera.XOffset, 2
                                    ) +
                                    Math.Pow(
                                        ((AdvEllipse)newShape).Center.Y - (Stylus.Y) - camera.YOffset, 2
                                    )
                                )
                            );

                        ((AdvEllipse)newShape).RadiusY = camera.SnapToGrid(
                        (int)Math.Sqrt(
                                    Math.Pow(
                                        ((AdvEllipse)newShape).Center.X - (Stylus.X) - camera.XOffset, 2
                                    ) +
                                    Math.Pow(
                                        ((AdvEllipse)newShape).Center.Y - (Stylus.Y) - camera.YOffset, 2
                                    )
                                )
                        );
                    }

                    
                }
            }
        }

        private Point Stylus { get => new Point((Cursor.Position.X - cursorOffset.X), (Cursor.Position.Y - cursorOffset.Y)); }
    }
}
