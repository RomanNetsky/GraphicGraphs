using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace graphical_graph_editor
{
    public partial class Form1 : Form
    {

        List<Node> Nodes;
        List<Line> Lines;
        Node selected = null;
        public Form1()
        {
            InitializeComponent();
            Nodes = new List<Node>();
            Lines = new List<Line>();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Pen pen = new Pen(Color.Black, 5);
            Brush brush = new SolidBrush(BackColor);
            Rectangle rectangle;
            foreach (Line line in Lines)
            {
                graphics.DrawLine(pen, line.X, line.Y, line.X2, line.Y2);
            }
            foreach (Node node in Nodes)
            {
                rectangle = new Rectangle(node.X - 50, node.Y - 50, 100, 100);
                graphics.FillEllipse(brush, rectangle);
                graphics.DrawEllipse(pen, node.X - 50, node.Y - 50, 100, 100);
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            int x, y;
            int find = 0;
            Node node = null;
            foreach (Node oneNode in Nodes)
            {
                x = oneNode.X;
                y = oneNode.Y;
                if (e.X > x - 50 && e.X < x + 50 && e.Y > y - 50 && e.Y < y + 50)
                {
                    find = 1;
                    node = oneNode;
                }
            }

            if (find == 1)
            {
                if (selected== null)
                {
                    MessageBox.Show("Selecting");
                    selected = node;
                }
                else
                {
                    if (selected != node)
                    {
                        MessageBox.Show("conecting nodes");
                        Line linea = new Line(selected.X, selected.Y, node.X, node.Y, Nodes.IndexOf(selected), Nodes.IndexOf(node));
                        Lines.Add(linea);
                        selected = null;
                    }
                }
            }
            else
            {
                node = new Node(e.X, e.Y);
                Nodes.Add(node);
            }
            Invalidate();
        }

        public void openFile()
        {
            string[] auxiliar;
            StreamReader sr = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.DefaultExt = "txt";
            //openFileDialog.Filter = "Text files|*.txt* ";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                sr = new StreamReader(openFileDialog.FileName);
                auxiliar = sr.ReadLine().Split(',');
                while (sr != null && !sr.EndOfStream && auxiliar[0] != "Lines")
                {
                    int x = int.Parse(auxiliar[0]);
                    int y = int.Parse(auxiliar[1]);
                    Node nodo = new Node(x, y);
                    Nodes.Add(nodo);
                    auxiliar = sr.ReadLine().Split(',');
                }
                while (sr != null && !sr.EndOfStream)
                {
                    auxiliar = sr.ReadLine().Split(',');
                    int nodo1 = int.Parse(auxiliar[0]);
                    int nodo2 = int.Parse(auxiliar[1]);

                    Line linea = new Line(Nodes[nodo1].X, Nodes[nodo1].Y, Nodes[nodo2].X, Nodes[nodo2].Y, nodo1, nodo2);
                    Lines.Add(linea);
                }
                sr.Close();
            }

        }
        public void saveFile()
        {
            TextWriter sw = null;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save text Files";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                sw = new StreamWriter(saveFileDialog.FileName);
                MessageBox.Show(saveFileDialog.FileName);
            }
            foreach (Node nodo in Nodes)
            {
                sw.WriteLine(nodo.X + "," + nodo.Y);
            }
            sw.WriteLine("Lines");
            foreach (Line linea in Lines)
            {
                sw.WriteLine(linea.Node1 + "," + linea.Node2);
            }
            sw.Close();
        }

        public class Node
        {
            int x, y;
            public Node(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public int X { get { return x; } set { x = value; } }
            public int Y { get { return y; } set { y = value; } }
        }

        public class Line
        {
            int x1, y1, x2, y2, node1, node2;
            public Line(int x1, int y1, int x2, int y2, int node1, int node2)
            {
                this.x1 = x1;
                this.y1 = y1;
                this.x2 = x2;
                this.y2 = y2;
                this.node1 = node1;
                this.node2 = node2;
            }

            public int X { get { return x1; } set { x1 = value; } }
            public int Y { get { return y1; } set { y1 = value; } }
            public int X2 { get { return x2; } set { x2 = value; } }
            public int Y2 { get { return y2; } set { y2 = value; } }
            public int Node1 { get { return node1; } set { node1 = value; } }
            public int Node2 { get { return node2; } set { node2 = value; } }
        }


        private void ToolStripLabel1_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void ToolStripLabel2_Click(object sender, EventArgs e)
        {
            Nodes.Clear();
            Lines.Clear();
            openFile();
            Invalidate();
        }
    }//Form
}//namespace

