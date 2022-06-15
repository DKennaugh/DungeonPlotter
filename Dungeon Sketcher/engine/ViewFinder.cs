using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Sketcher.engine
{
    public class ViewFinder
    {
        double xOffset = 0;
        double yOffset = 0;
        double zoomLevel = 1;

        float zoomMin = 0.25f;
        float zoomMax = 5;

        int cellSize;

        public double XOffset { get => xOffset; set => xOffset = value; }
        public double YOffset { get => yOffset; set => yOffset = value; }
        public double ZoomLevel
        {
            get => zoomLevel;
            set {
                if (value < zoomMin)
                {
                    zoomLevel = zoomMin;
                }
                else if (value > zoomMax)
                {
                    zoomLevel = zoomMax;
                }
                else
                {
                    zoomLevel = value;
                }
            }
        }
        public int CellSize { get => cellSize; set => cellSize = value; }
        public ViewFinder (int cellSize)
        {
            this.cellSize = cellSize;
        }

        public int SnapToGrid(double value)
        {
            return (int)(value / cellSize) * cellSize;
        }

        public int SnapToHalfGrid(double value)
        {
            return (int)(value / (cellSize / 2)) * cellSize / 2;
        }

        public Point SnapToGrid(Point point)
        {
            return new Point(SnapToGrid(point.X), SnapToGrid(point.Y));
        }

        public Point SnapToHalfGrid(Point point)
        {
            return new Point(SnapToHalfGrid(point.X), SnapToHalfGrid(point.Y));
        }


    }
}
