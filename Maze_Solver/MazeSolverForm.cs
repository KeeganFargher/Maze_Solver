using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Maze_Solver_Library;

namespace Maze_Solver
{
    public partial class MazeSolverForm : Form
    {
        private AStar _aStar = new AStar();

        private bool paint;
            
        public MazeSolverForm()
        {
            InitializeComponent();
        }

        private void MazeSolverForm_Paint(object sender, PaintEventArgs e)
        {

        }


        private void PanelMain_Paint(object sender, PaintEventArgs e)
        {
            if (!paint) return;
            Brush brushColor = new SolidBrush(Color.White);
            Pen pen = new Pen(brushColor, 1);

            //  Draw maze
            DrawMaze(e, pen, 2);

            brushColor = new SolidBrush(Color.FromArgb(95, 39, 205));
            pen = new Pen(brushColor, 3);

            DrawPath(e, pen);
        }

        private void DrawMaze(PaintEventArgs e, Pen pen, int penSize)
        {
            for (int row = 0; row < _aStar.Nodes.GetLength(0); row++)
            {
                for (int col = 0; col < _aStar.Nodes.GetLength(1); col++)
                {
                    List<Point> points = _aStar.Nodes[row, col].Draw(_aStar.MazeSize);
                    for (int i = 0; i < points.Count - 1; i += 2)
                    {
                        e.Graphics.DrawLine(pen, points[i], points[i + 1]);
                    }

                    //  Fill color to see that this is the starting node
                    if (_aStar.Nodes[row, col].IsStartNode)
                    {
                        Brush startBrush = new SolidBrush(Color.Green);
                        e.Graphics.FillRectangle(startBrush, GetRectangleF(row, col, penSize));
                    }

                    if (_aStar.Nodes[row, col].IsFinishNode)
                    {
                        Brush startBrush = new SolidBrush(Color.Red);
                        e.Graphics.FillRectangle(startBrush, GetRectangleF(row, col, penSize));
                    }
                }
            }
        }

        private RectangleF GetRectangleF(int row, int col, int penSize)
        {
            return new RectangleF(
                _aStar.Nodes[row, col].X + penSize,
                _aStar.Nodes[row, col].Y + penSize,
                _aStar.MazeSize - penSize,
                _aStar.MazeSize - penSize);
        }

        private void DrawPath(PaintEventArgs e, Pen pen)
        {
            List<Node> paths = _aStar.Paths;
            int size = _aStar.MazeSize;
            for (int i = 0; i < paths.Count - 1; i++)
            {
                Point point1 = new Point(paths[i].X + size / 2, paths[i].Y + size / 2);
                Point point2 = new Point(paths[i + 1].X + size / 2, paths[i + 1].Y + size / 2);
                e.Graphics.DrawLine(pen, point1, point2);
            }
        }

        private void ReportProgressHandler()
        {
            panelMain.Invalidate();
        }

        private void ButtonGenerator_Click(object sender, EventArgs e)
        {
            paint = true;
            _aStar.MazeSize = Convert.ToInt32(trackBarSize.Value);
            _aStar.VisualizeMaze = checkBoxAnimate.Checked;
            _aStar.ReportProgress += ReportProgressHandler;
            _aStar.PopulateNodes(panelMain.Width - 5, panelMain.Height - 5);

            Maze.ReportProgress += ReportProgressHandler;

            Task.Run(() => Maze.GenerateMaze(_aStar.Nodes, _aStar.VisualizeMaze))
                .ContinueWith(t => _aStar.AddNeighbors());
        }

        private void ButtonSolve_Click(object sender, EventArgs e)
        {
            Task.Run(() => _aStar.SolveMaze(checkBoxSolver.Checked))
                .ContinueWith(t => Invalidate());
            panelMain.Invalidate();
        }
    }
}
