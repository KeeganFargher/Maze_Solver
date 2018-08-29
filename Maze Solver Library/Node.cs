using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Maze_Solver_Library
{
    public class Node
    {
        /// <summary>
        /// The X location of the node
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// The Y location of the node
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// The row the node lives in
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// The column the node lives in
        /// </summary>
        public int Column { get; set; }

        public double FScore { get; set; }

        public int GScore { get; set; }

        public double Hscore { get; set; }

        /// <summary>
        /// If the node has been visited before
        /// </summary>
        public bool Visited { get; set; }

        /// <summary>
        /// Whether the node is a wall or not
        /// </summary>
        public bool IsWall { get; set; }

        /// <summary>
        /// Represents the top, right, bottom, left walls of the node, in that exact order.
        /// For example -- if Walls[0] is false then there will be no top wall
        /// </summary>
        public bool[] Walls { get; } = {true, true, true, true};

        /// <summary>
        /// The neighbors that surround this node
        /// </summary>
        public List<Node> Neighbors { get; set; }

        public Node(int x, int y, int row, int column)
        {
            X = x;
            Y = y;
            Row = row;
            Column = column;
        }

        public Node()
        {
            
        }

        public List<Point> Draw(int size)
        {
            List<Point> points = new List<Point>();
            if (Walls[0])
            {
                points.Add(new Point(X, Y));
                points.Add(new Point(X + size, Y));
            }
            if (Walls[1])
            {
                points.Add(new Point(X + size, Y));
                points.Add(new Point(X + size, Y + size));
            }
            if (Walls[2])
            {
                points.Add(new Point(X + size, Y + size));
                points.Add(new Point(X, Y + size));
            }
            if (Walls[3])
            {
                points.Add(new Point(X, Y + size));
                points.Add(new Point(X, Y));
            }
            return points;
        }

    }
}
