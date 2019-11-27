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
        Boolean mousePressed = false;
        Boolean mPressAndSelectedG = false;
        int radio = 30;

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
                rectangle = new Rectangle(node.X - radio, node.Y - radio, radio*2, radio*2);
                graphics.FillEllipse(brush, rectangle);
                pen = new Pen(node.Color, 5);
                graphics.DrawEllipse(pen, node.X - radio, node.Y - radio, radio*2, radio*2);
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mousePressed = true;
            
            int x, y;
            int find = 0;
            Node node = null;

            if(e.Button == System.Windows.Forms.MouseButtons.Left)// if mouse button pressed is left
            {
                foreach (Node oneNode in Nodes)
                {
                    x = oneNode.X;
                    y = oneNode.Y;
                    if (e.X > x - radio && e.X < x + radio && e.Y > y - radio && e.Y < y + radio)
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
                        node.Color = Color.Blue;
                        selectedNode = selected = node;                        
                    }
                    else
                    {
                        if (selected != node)
                        {
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
                    if (e.X > x - radio && e.X < x + radio && e.Y > y - radio && e.Y < y + radio)
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

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mousePressed = false;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if(mousePressed == true && e.Button == MouseButtons.Left && selected != null )
            {
                    selected.X = e.X;
                    selected.Y = e.Y;
                    Invalidate();
            }            
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
                    Node server = new Node();
                    Node client = new Node();
                    auxiliar = sr.ReadLine().Split(',');
                    int nodo1X = int.Parse(auxiliar[0]);
                    int nodo1Y = int.Parse(auxiliar[1]);
                    int nodo2X = int.Parse(auxiliar[2]);
                    int nodo2Y = int.Parse(auxiliar[3]);                    
               
                    foreach (Node node in Nodes)
                    {
                        if (node.X == nodo1X && node.Y == nodo1Y)//it just can be one of all
                        {
                            server = node;
                        }
                        else
                        {
                            if (node.X == nodo2X && node.Y == nodo2Y)//it also can be just one of all
                            {
                                client = node;
                            }
                        }                        
                    }
                    Line linea = new Line(server, client);
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
            foreach (Node node in Nodes)
            {
                sw.WriteLine(node.X + "," + node.Y);
            }
            sw.WriteLine("Lines");
            foreach (Line line in Lines)
            {
                sw.WriteLine(line.Node1.X + "," + line.Node1.Y + ","+ line.Node2.X + "," + line.Node2.Y);
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
            Node client = null;
            Node server = null;

            float m;
            float b;
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

      
    }//Form
}//namespace

