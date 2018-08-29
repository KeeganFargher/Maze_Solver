using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Maze_Solver_Library;

namespace Maze_Solver
{
    public partial class MazeSolverForm : Form
    {
        private AStar _aStar = new AStar();

        private bool paint = false;
            
        public MazeSolverForm()
        {
            InitializeComponent();
        }

        private void MazeSolverForm_Paint(object sender, PaintEventArgs e)
        {
            if (!paint) return;
            Brush brushColor = new SolidBrush(Color.White);
            Pen pen = new Pen(brushColor, 1);

            for (int row = 0; row < _aStar.Nodes.GetLength(0); row++)
            {
                for (int col = 0; col < _aStar.Nodes.GetLength(1); col++)
                {
                    List<Point> points = _aStar.Nodes[row, col].Draw(_aStar.MazeSize);
                    for (int i = 0; i < points.Count - 1; i+=2)
                    {
                        e.Graphics.DrawLine(pen, points[i], points[i + 1]);
                    }
                }
            }
        }

        private void ReportProgressHandler()
        {
            Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            paint = true;
            _aStar.ReportProgress += ReportProgressHandler;
            _aStar.PopulateNodes(Width, Height, panelSidebar.Width);

            Maze.ReportProgress += ReportProgressHandler;
            Task.Run(() => Maze.GenerateMaze(_aStar.Nodes))
                .ContinueWith(t => Invalidate());

            Invalidate();
        }
    }
}
