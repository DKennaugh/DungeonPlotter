using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Sketcher.engine
{
    public abstract class AdvShape
    {
        ViewFinder camera;

        int x;
        int y;
        int width;
        int height;
        int lineWidth = 11;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public ViewFinder Camera { get => camera; set => camera = value; }
        public Rectangle Shape {
            get
            {
                return new Rectangle((int)(X * camera.CellSize - camera.XOffset), (int)(Y *camera.CellSize - camera.YOffset), Width, Height) ;
            }
        }

        public abstract bool OnEdge(Point p);
        public Color ShapeColor { get => Color.Black; }
        public Color SelectedColor { get => Color.Blue; }
        public int LineWidth { get => lineWidth; set => lineWidth = value; }
        

        public AdvShape(int x, int y, int width, int height, ViewFinder camera)
        {
            Camera = camera;
            X = (int)(x + Camera.XOffset);
            Y = (int)(y + Camera.YOffset);
            Width = width;
            Height = height;
        }

        public abstract void Fill(Graphics g, SolidBrush b);
        public abstract void Draw(Graphics g, Pen p);
        public abstract bool Contains(Point p);
        public abstract HybridShape MergeShapes(AdvShape s);
        
    }

    class AdvRectangle : AdvShape
    {
        
        public AdvRectangle(int x, int y, int width, int height, ViewFinder camera) : base(x, y, width, height, camera) {}

        public override void Fill(Graphics g, SolidBrush b)
        {
            b.Color = ShapeColor;
            g.FillRectangle(b, (int)((X - Camera.XOffset) * Camera.ZoomLevel), (int)((Y - Camera.YOffset) * Camera.ZoomLevel), (int)(Width * Camera.ZoomLevel), (int)(Height * Camera.ZoomLevel));
        }

        public override void Draw(Graphics g, Pen p)
        {
            p.Color = ShapeColor;
            p.Width = LineWidth;
            g.DrawRectangle(p, (int)((X - Camera.XOffset) * Camera.ZoomLevel), (int)((Y - Camera.YOffset) * Camera.ZoomLevel), (int)(Width * Camera.ZoomLevel), (int)(Height * Camera.ZoomLevel));
        }

        public override bool Contains(Point p)
        {

            return (p.X / Camera.ZoomLevel >= X && p.X / Camera.ZoomLevel <= X + Width && p.Y / Camera.ZoomLevel >= Y && p.Y / Camera.ZoomLevel <= Y + Height);
        }

        public override bool OnEdge(Point p)
        {
            return 
                (p.X / Camera.ZoomLevel + Camera.XOffset >= X - LineWidth / 2 && 
                p.X / Camera.ZoomLevel + Camera.XOffset <= X + LineWidth / 2 &&
                p.Y /Camera.ZoomLevel + Camera.YOffset >= Y - LineWidth / 2 &&
                p.Y / Camera.ZoomLevel + Camera.YOffset <= Y + Height + LineWidth / 2) ||  //Left Line
                (p.X / Camera.ZoomLevel + Camera.XOffset >= X + Width - LineWidth / 2 &&
                p.X / Camera.ZoomLevel + Camera.XOffset <= X + Width + LineWidth / 2 &&
                p.Y / Camera.ZoomLevel + Camera.YOffset >= Y - LineWidth / 2 &&
                p.Y / Camera.ZoomLevel + Camera.YOffset <= Y + Height + LineWidth / 2) || // Right Line
                (p.Y / Camera.ZoomLevel + Camera.YOffset >= Y - LineWidth / 2 &&
                p.Y / Camera.ZoomLevel + Camera.YOffset <= Y + LineWidth / 2 &&
                p.X / Camera.ZoomLevel + Camera.XOffset >= X - LineWidth / 2 &&
                p.X / Camera.ZoomLevel + Camera.XOffset <= X + Width + LineWidth / 2) || //Top Line
                (p.Y / Camera.ZoomLevel + Camera.YOffset >= Y + Height - LineWidth / 2 &&
                p.Y / Camera.ZoomLevel + Camera.YOffset <= Y + Height + LineWidth / 2 &&
                p.X / Camera.ZoomLevel + Camera.XOffset >= X - LineWidth / 2 &&
                p.X / Camera.ZoomLevel + Camera.XOffset <= X + Width + LineWidth / 2); //BottomLine
        }

        public override HybridShape MergeShapes(AdvShape s)
        {
            throw new NotImplementedException();
        }
    }

    class AdvEllipse : AdvShape
    {
        public int RadiusX
        {
            get => Width/2;
            set
            {
                base.X = X + RadiusX - value;
                base.Width = value * 2;

            }
        }
        public int RadiusY
        {
            get => Height/2;
            set
            {

                base.Y = Y + RadiusY - value;
                base.Height = value * 2;
            }
        }
        public Point Center
        {
            get => new Point ((int)X + RadiusX,(int)Y + RadiusY);
            set
            {
                base.X = X + (value.X - Center.X);
                base.Y = Y + (value.Y - Center.Y);

            }
        }
        public AdvEllipse(int x, int y, int width, int height, ViewFinder camera) : base(x, y, width, height, camera) { }
        public AdvEllipse(Point center, int radius, ViewFinder camera) : base(center.X - radius, center.Y - radius, radius * 2, radius * 2, camera){}

        public AdvEllipse(Point center, int radiusX, int radiusY,  ViewFinder camera) : base(center.X - radiusX, center.Y - radiusY, radiusX * 2, radiusY * 2, camera){}

        public override void Fill(Graphics g, SolidBrush b)
        {
            b.Color = ShapeColor;
            g.FillEllipse(b, (int)((X - Camera.XOffset) * Camera.ZoomLevel), (int)((Y - Camera.YOffset) * Camera.ZoomLevel), (int)(Width * Camera.ZoomLevel), (int)(Height * Camera.ZoomLevel));
        }

        public override void Draw(Graphics g, Pen p)
        {
            p.Color = ShapeColor;
            p.Width = (int)(LineWidth*Camera.ZoomLevel);
            g.DrawEllipse(p, (int)((X - Camera.XOffset)*Camera.ZoomLevel), (int)((Y - Camera.YOffset)*Camera.ZoomLevel), (int)(Width*Camera.ZoomLevel), (int)(Height*Camera.ZoomLevel));
        }

        public override bool Contains(Point p)
        {
            return Math.Pow((p.X / Camera.ZoomLevel - Center.X + Camera.XOffset) / (float)RadiusX, 2)/Camera.ZoomLevel + Math.Pow((p.Y / Camera.ZoomLevel - Center.Y + Camera.YOffset) / (float)RadiusY, 2)/Camera.ZoomLevel <= 1;
        }

        public override bool OnEdge (Point p)
        {
            return (
                Math.Pow(
                    (p.X / Camera.ZoomLevel - Center.X + Camera.XOffset) / (RadiusX + (float)(LineWidth / 2))
                    , 2) +
                Math.Pow(
                    (p.Y / Camera.ZoomLevel - Center.Y + Camera.YOffset) / (RadiusY + (float)(LineWidth / 2))
                    , 2))
                <= 1
                && !(
                (Math.Pow(
                    (p.X / Camera.ZoomLevel - Center.X + Camera.XOffset) / (RadiusX - (float)(LineWidth / 2))
                    , 2) +
                Math.Pow(
                    (p.Y / Camera.ZoomLevel - Center.Y + Camera.YOffset) / (RadiusY - (float)(LineWidth / 2))
                    , 2))
                <= 1);
        }

        public override HybridShape MergeShapes(AdvShape s)
        {
            throw new NotImplementedException();
        }
    }

    public class HybridShape : AdvShape
    {
        ArrayList shapes;

        public HybridShape (ViewFinder camera, params AdvShape[] shapes) : base (0,0,0,0,camera)
        {
            this.shapes = new ArrayList(shapes);
            CalculateX();
            CalculateY();
            CalculateWidth();
            CalculateHeight();
        }

        private void CalculateX ()
        {
            if (shapes.Count == 0) { base.X = 0; }
            base.X = ((AdvShape)shapes[0]).X;
            foreach (AdvShape s in shapes)
            {
                if (s.X < base.X)
                {
                    base.X = s.X;
                }
            }
        }

        private void CalculateY()
        {
            if (shapes.Count == 0) { base.Y = 0; }
            base.Y = ((AdvShape)shapes[0]).Y;
            foreach (AdvShape s in shapes)
            {
                if (s.Y < base.Y)
                {
                    base.Y = s.Y;
                }
            }
        }

        private void CalculateWidth()
        {
            if (shapes.Count == 0) { base.Width = 0; }
            float furthestRight = ((AdvShape)shapes[0]).Width + ((AdvShape)shapes[0]).X;
            foreach (AdvShape s in shapes)
            {
                if (furthestRight < s.Width + s.X)
                {
                    furthestRight = s.Width + s.X;
                }
            }
            base.Width = (int)(furthestRight - X);
        }

        private void CalculateHeight()
        {
            if (shapes.Count == 0) { base.Width = 0; }
            float furthestDown = ((AdvShape)shapes[0]).Height + ((AdvShape)shapes[0]).Y;
            foreach (AdvShape s in shapes)
            {
                if (furthestDown < s.Height + s.Y)
                {
                    furthestDown = s.Height + s.Y;
                }
            }
            base.Height = (int)(furthestDown - Y);
        }

        public override bool Contains(Point p)
        {
            foreach (AdvShape s in shapes)
            {
                if (s.Contains(p))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool OnEdge (Point p)
        {
            foreach (AdvShape s in shapes)
            {
                if (s.OnEdge(p))
                {
                    return true;
                }
            }
            return false;
        }

        public override void Draw(Graphics g, Pen p)
        {
            foreach (AdvShape s in shapes)
            {
                s.Draw(g, p);
            }
        }

        public override void Fill(Graphics g, SolidBrush b)
        {
            foreach (AdvShape s in shapes)
            {
                s.Fill(g, b);
            }
        }

        public override HybridShape MergeShapes(AdvShape s)
        {
            shapes.Add(s);
            return this;
        }
    }

    public class SelectionBox : ArrayList
    {
        Rectangle bounds;

        public Rectangle BoundingBox { get => bounds; }
        public int X
        {
            get { return bounds.X; }
            set
            {
                int diff = value - X;
                foreach (AdvShape s in this)
                {
                    s.X += diff;
                }
            }

        }
        public int Y
        {
            get { return bounds.Y; }
            set
            {
                int diff = value - Y;
                foreach (AdvShape s in this)
                {
                    s.Y += diff;
                }
            }

        }
        public SelectionBox() : base()
        {
            bounds = new Rectangle(0, 0, 0, 0);
        }

        public void Add(AdvShape shape)
        {
            base.Add(shape);
            RecalculateBounds();
        }

        public void Remove (AdvShape shape)
        {
            base.Remove(shape);
            RecalculateBounds();
        }

        public void Add(ArrayList shapes)
        {
            foreach (AdvShape s in shapes)
            {
                shapes.Remove(s);
                this.Add(s);
            }
        }

        public void Add(SelectionBox shapes)
        {
            foreach (AdvShape s in shapes)
            {
                shapes.Remove(s);
                this.Add(s);
            }
        }

        public void Remove(ArrayList shapes)
        {
            foreach (AdvShape s in shapes)
            {
                this.Remove(s);
            }
        }

        public override void Clear()
        {
            base.Clear();
            bounds = new Rectangle(0, 0, 0, 0);
        }

        private void RecalculateBounds ()
        {
            if (this.Count == 0)
            {
                bounds = new Rectangle(0, 0, 0, 0);
            }else if (this.Count == 1)
            {
                bounds = ((AdvShape)this[0]).Shape;
            }else
            {
                int x = bounds.X;
                int y = bounds.Y;
                int width = bounds.Width;
                int height = bounds.Height;
                foreach (AdvShape s in this)
                {
                    if (s.X <  x)
                    {
                        x = s.X;
                    }
                    if (s.Y < y)
                    {
                        y = s.Y;
                    }
                    if (s.X + s.Width - x > width)
                    {
                        width = s.X + s.Width - x;
                    }
                    if (s.Y + s.Height - y > height)
                    {
                        height = s.Y + s.Height - y;
                    }
                }
                bounds = new Rectangle(x, y, width, height);
            }
            
        }
    }
}