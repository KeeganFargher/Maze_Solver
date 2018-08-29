using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maze_Solver_Library
{

    public class AStar
    {
        public delegate void EventDelegate();

        public event EventDelegate ReportProgress;

        public int MazeSize { get; set; } = 20;

        public Node[,] Nodes { get; private set; }

        public List<Node> OpenSet { get; set; } = new List<Node>();
        public List<Node> ClosedSet { get; set; } = new List<Node>();
        public List<Node> Path { get; set; } = new List<Node>();

        private Node _startNode = new Node();
        private Node _finishNode = new Node();

        private int _rows;
        private int _columns;

        public void PopulateNodes(int formWidth, int formHeight, int sidebarWidth)
        {
            //  Set the amount of rows and columns for the maze
            _rows = (int) Math.Floor((formWidth - sidebarWidth * 1.0) / MazeSize);
            _columns = (int) Math.Floor(formHeight / (double) MazeSize);

            Nodes = new Node[_rows - 1, _columns - 2];

            //  Initialize the array
            for (int row = 0; row < Nodes.GetLength(0); row++)
            {
                for (int col = 0; col < Nodes.GetLength(1); col++)
                {
                    Nodes[row, col] = new Node(row * MazeSize, col * MazeSize, row, col);
                }
            }

            //  Create a start and end point for the maze
            _startNode = Nodes[0, 0];
            _finishNode = Nodes[Nodes.GetLength(0) - 1, Nodes.GetLength(1) - 1];
            OpenSet.Add(_startNode);
        }

        public void SolveMaze()
        {

        }
    }
}
