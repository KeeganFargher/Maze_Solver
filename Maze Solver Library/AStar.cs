﻿using System;
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
            _finishNode = Nodes[Nodes.GetLength(0) - 1, Nodes.GetLength(1) - 1];
            OpenSet.Add(_startNode);
        }

        public void AddNeighbors()
        {
            //  Add neighbors to each node
            for (int row = 0; row < Nodes.GetLength(0); row++)
            {
                for (int col = 0; col < Nodes.GetLength(1); col++)
                {
                    Nodes[row, col].AddNeighbors(Nodes);
                }
            }
        }

        public void SolveMaze()
        {

            while (OpenSet.Count != 0)
            {
                int winnerIndex = 0;
                Node current = new Node();

                for (int i = 0; i < OpenSet.Count; i++)
                {
                    if (OpenSet[i].FScore < OpenSet[winnerIndex].FScore)
                    {
                        winnerIndex = i;
                    }
                }

                current = OpenSet[winnerIndex];

                if (current == _finishNode)
                {
                    return;
                }

                OpenSet.Remove(current);
                ClosedSet.Add(current);

                List<Node> neighbors = current.Neighbors;
                for (int i = 0; i < neighbors.Count; i++)
                {
                    Node neighbor = neighbors[i];
                    if (!ClosedSet.Contains(neighbor) && !neighbor.IsWall)
                    {
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

                Paths = new List<Node>();
                Node temp = current;
                Paths.Add(temp);
                while (temp.Previous != null)
                {
                    Paths.Add(temp.Previous);
                    temp = temp.Previous;
                }
                ReportProgress();
                Thread.Sleep(1);
            }
        }

        private double Heuristic(Node a, Node b)
        {
            double dist = Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
            return dist;
        }
    }
}
