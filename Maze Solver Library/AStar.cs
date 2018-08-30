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

        public int MazeSize { get; set; } = 10;
        public bool VisualizeMaze { get; set; } = true;

        public Node[,] Nodes { get; private set; }

        private List<Node> OpenSet { get; set; } = new List<Node>();
        private List<Node> ClosedSet { get; set; } = new List<Node>();
        public List<Node> Paths { get; private set; } = new List<Node>();

        private Node _startNode = new Node();
        private Node _finishNode = new Node();

        private int _rows;
        private int _columns;

        public void PopulateNodes(int formWidth, int formHeight, int sidebarWidth)
        {
            OpenSet = new List<Node>();
            ClosedSet = new List<Node>();
            Paths = new List<Node>();

            //  Set the amount of rows and columns for the maze
            _rows = (int) Math.Floor((formWidth  - sidebarWidth * 1.0) / MazeSize);
            _columns = (int) Math.Floor(formHeight  / (double) MazeSize);

            Nodes = new Node[_rows - 3, _columns - 3];

            //  Initialize the array
            for (int row = 0; row < Nodes.GetLength(0); row++)
            {
                for (int col = 0; col < Nodes.GetLength(1); col++)
                {
                    Nodes[row, col] = new Node((row + 1) * MazeSize, (col + 1) * MazeSize, row, col);
                }
            }

            //  Create a start and end point for the maze
            _startNode = Nodes[0, 0];
            _startNode.IsStartNode = true;

            _finishNode = Nodes[Nodes.GetLength(0) - 1, Nodes.GetLength(1) - 1];
            _finishNode.IsFinishNode = true;

            OpenSet.Add(_startNode);
        }

        //  Add neighbors to each node
        public void AddNeighbors()
        {
            for (int row = 0; row < Nodes.GetLength(0); row++)
            {
                for (int col = 0; col < Nodes.GetLength(1); col++)
                {
                    Nodes[row, col].AddNeighbors(Nodes);
                }
            }
        }

        public void SolveMaze(bool visualizeSolver)
        {
            while (OpenSet.Count != 0)
            {
                int winnerIndex = CalculateWinnerIndex();
                Node current = OpenSet[winnerIndex];

                OpenSet.Remove(current);
                ClosedSet.Add(current);

                var neighbors = current.Neighbors;
                CalculateNeighbors(current, neighbors);

                //  Calculates the current path
                CalculatePath(current);

                //  We reached the end if this evaluates to true
                if (current == _finishNode)
                {
                    return;
                }

                ReportProgress();
                if (visualizeSolver)
                {
                    Thread.Sleep(10);
                }
            }
        }

        private void CalculateNeighbors(Node current, IReadOnlyList<Node> neighbors)
        {
            for (int i = 0; i < neighbors.Count; i++)
            {
                Node neighbor = neighbors[i];

                if (ClosedSet.Contains(neighbor) || neighbor.IsWall) continue;

                int tempGScore = current.GScore + 1;

                bool newPath = false;
                if (OpenSet.Contains(neighbor))
                {
                    if (tempGScore < neighbor.GScore)
                    {
                        neighbor.GScore = tempGScore;
                        newPath = true;
                    }
                }
                else
                {
                    neighbor.GScore = tempGScore;
                    OpenSet.Add(neighbor);
                    newPath = true;
                }

                if (newPath)
                {
                    neighbor.Hscore = Heuristic(neighbor, _finishNode);
                    neighbor.FScore = neighbor.GScore + neighbor.Hscore;
                    neighbor.Previous = current;
                }
            }
        }

        private int CalculateWinnerIndex()
        {
            int winnerIndex = 0;
            for (int i = 0; i < OpenSet.Count; i++)
            {
                if (OpenSet[i].FScore < OpenSet[winnerIndex].FScore)
                {
                    winnerIndex = i;
                }
            }

            return winnerIndex;
        }

        private void CalculatePath(Node current)
        {
            Paths = new List<Node>();
            Node temp = current;
            Paths.Add(temp);
            while (temp.Previous != null)
            {
                Paths.Add(temp.Previous);
                temp = temp.Previous;
            }
        }

        private static double Heuristic(Node a, Node b)
        {
            double dist = Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
            return dist;
        }
    }
}
