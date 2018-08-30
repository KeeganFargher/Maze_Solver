using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maze_Solver_Library
{
    public static class Maze
    {
        public delegate void EventDelegate();
        public static event EventDelegate ReportProgress;

        private static object synclock = new object();
        private static Random random = new Random();

        public static void GenerateMaze(Node[,] Nodes)
        {
            Node current = Nodes[0, 0];
            List<Node> stack = new List<Node>();
            current.Visited = true;

            while (!AreUnvisitedCells(Nodes))
            {
                //  STEP 1 - PICK A RANDOM NEIGHBOR

                Node next = GetRandomNeighbor( current, Nodes);
                if (next != null)
                {
                    next.Visited = true;

                    //  STEP 2
                    stack.Add(current);

                    //  STEP 3
                    int x = current.Row - next.Row;
                    if (x == 1)
                    {
                        current.Walls[3] = false;
                        next.Walls[1] = false;
                    }
                    else if (x == -1)
                    {
                        current.Walls[1] = false;
                        next.Walls[3] = false;
                    }

                    int y = current.Column - next.Column;
                    if (y == 1)
                    {
                        current.Walls[0] = false;
                        next.Walls[2] = false;
                    }
                    else if (y == -1)
                    {
                        current.Walls[2] = false;
                        next.Walls[0] = false;
                    }

                    //  STEP 4
                    current = next;
                }
                else if (stack.Count > 0)
                {
                    Node temp = stack[stack.Count - 1];
                    stack.Remove(stack[stack.Count - 1]);
                    current = temp;
                }

                ReportProgress?.Invoke();
            }
        }

        private static Node GetRandomNeighbor(Node node, Node[,] Nodes)
        {
            List<Node> neighbors = new List<Node>();

            //  TOP
            if (node.Column > 0)
            {
                Node top = Nodes[node.Row, node.Column - 1];
                if (!top.Visited)
                {
                    neighbors.Add(top);
                }
            }

            // RIGHT
            if (node.Row < Nodes.GetLength(0) - 1)
            {
                Node right = Nodes[node.Row + 1, node.Column];
                if (!right.Visited)
                {
                    neighbors.Add(right);
                }
            }

            //  BOTTOM
            if (node.Column < Nodes.GetLength(1) - 1)
            {
                Node bottom = Nodes[node.Row, node.Column + 1];
                if (!bottom.Visited)
                {
                    neighbors.Add(bottom);
                }
            }

            //  LEFT
            if (node.Row > 0)
            {
                Node left = Nodes[node.Row - 1, node.Column];
                if (!left.Visited)
                {
                    neighbors.Add(left);
                }
            }

            //  Returns a random neighbour
            if (neighbors.Count > 0)
            {
                int r = (int) Math.Floor(random.Next(0, neighbors.Count) * 1.0);
                return neighbors[r];
            }
            return null;
        }

        /// <summary>
        /// Checks if there are any unvisited nodes
        /// </summary>
        private static bool AreUnvisitedCells(Node[,] Nodes)
        {
            for (int r = 0; r < Nodes.GetLength(0); r++)
            {
                for (int c = 0; c < Nodes.GetLength(1); c++)
                {
                    if (!Nodes[r, c].Visited)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

}

