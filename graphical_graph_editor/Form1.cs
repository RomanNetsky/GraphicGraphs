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
        Node selectedNode;

        Node selected = null;

        public Form1()
        {
            InitializeComponent();
            Nodes = new List<Node>();
            Lines = new List<Line>();
            selectedNode = new Node();

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
                pen = new Pen(node.Color, 5);
                graphics.DrawEllipse(pen, node.X - 50, node.Y - 50, 100, 100);
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            int x, y;
            int find = 0;
            Node node = null;

            if(e.Button == System.Windows.Forms.MouseButtons.Left)// if mouse button pressed is left
            {
                foreach (Node oneNode in Nodes)
                {
                    x = oneNode.X;
                    y = oneNode.Y;
                    if (e.X > x - 50 && e.X < x + 50 && e.Y > y - 50 && e.Y < y + 50)
                    {
                        find = 1;
                        node = oneNode;
                        selectedNode.Color = Color.Black;
                    }
                }
                if (find == 1)
                {                    
                    if (selected == null)
                    {
                        //MessageBox.Show("Selecting");
                        node.Color = Color.Blue;
                        selectedNode = selected = node;                        
                    }
                    else
                    {
                        if (selected != node)
                        {
                            //MessageBox.Show("conecting nodes");
                            Line linea = new Line(selected,node);
                            Lines.Add(linea);
                            selected = null;
                        }
                        else
                        {
                            selected = null;
                            node.Color = Color.Black;
                        }
                    }
                }
                else
                {
                    node = new Node(e.X, e.Y);
                    Nodes.Add(node);
                }
            }
            else // if mouse button pressed is right
            {
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
                    if (selected != null)
                    {
                        eliminateNexetEdges(node);
                        Nodes.Remove(node);
                        selected = null;
                    }
                }   
            }

            

            
            Invalidate();
        }


        public void eliminateNexetEdges(Node node)
        {
            List <Line> newEdges = new List<Line>();

           foreach( Line edge in Lines)
            {
                if(edge.Node1 != node && edge.Node2 != node)
                {
                    newEdges.Add(edge);
                }
            }
            Lines = newEdges;
        }

        public void openFile()
        {
            string[] auxiliar;
            StreamReader sr = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();

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

                    //Line linea = new Line(Nodes[nodo1].X, Nodes[nodo1].Y, Nodes[nodo2].X, Nodes[nodo2].Y, nodo1, nodo2);
                    //Lines.Add(linea);
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
            foreach (Node node in Nodes)
            {
                sw.WriteLine(node.X + "," + node.Y);
            }
            sw.WriteLine("Lines");
            foreach (Line line in Lines)
            {
                sw.WriteLine(line.Node1 + "," + line.Node2);
            }
            sw.Close();
        }

        public class Node
        {
            Color color;
            int x, y;

            int index;

            public Node(int x, int y,int index)
            {
                this.X = x;
                this.Y = y;
                this.index = index;
                color = Color.Black;
            }

            public Node(int x, int y)
            {
                this.X = x;
                this.Y = y;
                this.index = index;
                color = Color.Black;
            }

            public Node()
            {
            }

            public int X { get { return x; } set { x = value; } }
            public int Y { get { return y; } set { y = value; } }
            public Color Color { get { return color; } set { color = value; } }
        }

        public class Line
        {
            int x1, y1, x2, y2, node1, node2;

            Node client = null;
            Node server = null;
            int remainingInt;

            public Line(Node client, Node server)
            {
                this.client = client;
                this.server = server;
            }

            public int X { get { return client.X; } }
            public int Y { get { return client.Y; } }
            public int X2 { get { return server.X; } }
            public int Y2 { get { return server.Y; } }
            public Node Node1 { get { return client; } }
            public Node Node2 { get { return server; } }
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }//Form
}//namespace

