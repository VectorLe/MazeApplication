using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MazeApplication
{
    public partial class MazeApplication : Form
    {
        Space[,] maze;
        int locX = 1, locY = 0,
            lenWidth = 0, lenHeight = 0,
            size = 10;
            
        Bitmap drawingSurface;
        Graphics gfx;

        public MazeApplication()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            this.AutoScroll = true;

            //prepare bitmap picturebox
            drawingSurface = new Bitmap(ClientRectangle.Width, ClientRectangle.Height); //1112 470
            gfx = Graphics.FromImage(drawingSurface);

            //key listener
            this.KeyPreview = true;
            this.KeyPress += new KeyPressEventHandler(Form1_KeyPress);
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            bool solved = false;
            int x = 1, y = 0;
            Stack stack = new Stack();
            maze[x, y].explore('$');

            while (!solved)
            {
                if (x == lenWidth - 1 && y == lenHeight - 2)
                {
                    solved = true;
                }
                //use 't' to define solution path
                //left
                else if (maze[x - 1, y].getC() == ' ')
                {
                    maze[x, y].explore('t');
                    stack.push(maze[x, y]);
                    x--;
                    maze[x, y].explore('t');
                }
                //up
                else if (maze[x, y + 1].getC() == ' ')
                {
                    maze[x, y].explore('t');
                    stack.push(maze[x, y]);
                    y++;
                    maze[x, y].explore('t');
                }
                //right
                else if (maze[x + 1, y].getC() == ' ')
                {
                    maze[x, y].explore('t');
                    stack.push(maze[x, y]);
                    x++;
                    maze[x, y].explore('t');
                }
                //down
                else if (maze[x, y - 1].getC() == ' ')
                {
                    maze[x, y].explore('t');
                    stack.push(maze[x, y]);
                    y--;
                    maze[x, y].explore('t');
                }
                //backtrack, use 'f' to not include in solution path
                else
                {
                    maze[x, y].explore('f');
                    Space back = stack.pop();
                    x = back.getX();
                    y = back.getY();
                    maze[x, y].explore('f');
                }
            }//end while

            for (int i = 0; i < lenWidth; i++)
            {
                for (int j = 0; j < lenHeight; j++)
                {
                    if (maze[i, j].getC() == 't')
                    {
                        gfx.FillRectangle(Brushes.Purple, i * size, (lenHeight - 1 - j) * size, size, size);
                    }
                    else if (maze[i, j].getC() != '#')
                    {
                        gfx.FillRectangle(Brushes.White, i * size, (lenHeight - 1 - j) * size, size, size);
                    }
                }
            }
            pictureBox.Image = drawingSurface;
        }//end btnSolve

        private void btnReset_Click(object sender, EventArgs e)
        {
            gfx.FillRectangle(Brushes.Gray, locX * size, locY * size, size, size);
            locX = 1;
            locY = lenHeight - 1;
            gfx.FillRectangle(Brushes.Red, locX * size, locY * size, size, size);
            pictureBox.Image = drawingSurface;
        }

        void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lenWidth > 0 && lenHeight > 0)
            {
                switch (e.KeyChar) //input e.KeyChar comes in as ASCII
                {
                    //w
                    case (char)119:
                        if (maze[locX, lenHeight - locY].getC() == ' ')
                        {
                            gfx.FillRectangle(Brushes.Gray, locX * size, locY * size, size, size);
                            locY--;
                            gfx.FillRectangle(Brushes.Red, locX * size, locY * size, size, size);
                        }
                        break;
                    //a
                    case (char)97:
                        if (maze[locX - 1, lenHeight - 1 - locY].getC() == ' ')
                        {
                            gfx.FillRectangle(Brushes.Gray, locX * size, locY * size, size, size);
                            locX--;
                            gfx.FillRectangle(Brushes.Red, locX * size, locY * size, size, size);
                        }
                        break;
                    //s
                    case (char)115:
                        if (lenHeight - 1 - locY > 0)
                        {
                            if (maze[locX, lenHeight - 2 - locY].getC() == ' ')
                            {
                                gfx.FillRectangle(Brushes.Gray, locX * size, locY * size, size, size);
                                locY++;
                                gfx.FillRectangle(Brushes.Red, locX * size, locY * size, size, size);
                            }
                        }
                        break;
                    //d
                    case (char)100:
                        if (locX < lenWidth - 1)
                        {
                            if (maze[locX + 1, lenHeight - 1 - locY].getC() == ' ')
                            {
                                gfx.FillRectangle(Brushes.Gray, locX * size, locY * size, size, size);
                                locX++;
                                gfx.FillRectangle(Brushes.Red, locX * size, locY * size, size, size);
                            }
                        }
                        else
                        {
                            //Game can only be won using d, since exit is always on top right of maze
                            MessageBox.Show("Congratulations! You Won!!");
                        }
                        break;
                }//end switch
            }//end if
            pictureBox.Image = drawingSurface;
        }//end keypress

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            gfx.Clear(pictureBox.BackColor);

            //error message
            string message = "ERROR: You must enter an odd integer for the width and height.";
            message += " The width must be below " + ClientRectangle.Width / 10 + " and the height must be below " + ClientRectangle.Height / 10;

            //declare variables
            int width, height;
            Stack stack = new Stack();
            bool isWidthNum, isHeightNum;

            //get input from textboxes
            string strWidth = txtWidth.Text;
            isWidthNum = int.TryParse(strWidth, out width);
            string strHeight = txtHeight.Text;
            isHeightNum = int.TryParse(strHeight, out height);

            //if input is valid execute maze gen code
            if (isWidthNum && isHeightNum && width % 2 != 0 && height % 2 != 0 && width < ClientRectangle.Width / 10 && height < ClientRectangle.Height / 10)
            {
                //set global variables
                maze = new Space[width, height];
                locX = 1;
                locY = height - 1;
                lenHeight = height;
                lenWidth = width;

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        // # is wall, ' ' is space
                        if (i * j % 2 == 0)
                        {
                            maze[i, j] = new Space(i, j, '#', true);
                            maze[i, j].setDirections(0);
                        }
                        else
                        {
                            maze[i, j] = new Space(i, j, ' ', false);
                            if (i == 1)
                                maze[i, j].setDirections(maze[i, j].getDirections() - 8);
                            if (i == width - 2)
                                maze[i, j].setDirections(maze[i, j].getDirections() - 2);
                            if (j == 1)
                                maze[i, j].setDirections(maze[i, j].getDirections() - 1);
                            if (j == height - 2)
                                maze[i, j].setDirections(maze[i, j].getDirections() - 4);
                        }
                    }
                }

                //random starting point
                Random start = new Random();
                int x = start.Next(1, width - 1);
                int y = start.Next(1, height - 1);
                while (maze[x, y].isExplored())
                {
                    x = start.Next(1, width - 1);
                    y = start.Next(1, height - 1);
                }
                maze[x, y].explore(' ');

                Random dir = new Random();

                do
                {
                    int direction;

                    while (maze[x, y].getDirections() > 0)
                    {
                        direction = dir.Next(0, 4);

                        int left = maze[x, y].getDirections() & 8;
                        int up = maze[x, y].getDirections() & 4;
                        int right = maze[x, y].getDirections() & 2;
                        int down = maze[x, y].getDirections() & 1;

                        switch (direction)
                        {
                            //left
                            case 0:
                                if (left > 0)
                                {
                                    maze[x, y].setDirections(maze[x, y].getDirections() - 8);
                                    if (!maze[x - 2, y].isExplored())
                                    {
                                        stack.push(maze[x, y]);
                                        maze[x - 1, y].explore(' ');
                                        maze[x - 2, y].explore(' ');
                                        x -= 2;
                                        break;
                                    }
                                }
                                break;
                            //up
                            case 1:
                                if (up > 0)
                                {
                                    maze[x, y].setDirections(maze[x, y].getDirections() - 4);
                                    if (!maze[x, y + 2].isExplored())
                                    {
                                        stack.push(maze[x, y]);
                                        maze[x, y + 1].explore(' ');
                                        maze[x, y + 2].explore(' ');
                                        y += 2;
                                        break;
                                    }
                                }
                                break;
                            //right
                            case 2:
                                if (right > 0)
                                {
                                    maze[x, y].setDirections(maze[x, y].getDirections() - 2);
                                    if (!maze[x + 2, y].isExplored())
                                    {
                                        stack.push(maze[x, y]);
                                        maze[x + 1, y].explore(' ');
                                        maze[x + 2, y].explore(' ');
                                        x += 2;
                                        break;
                                    }
                                }
                                break;
                            //down
                            case 3:
                                if (down > 0)
                                {
                                    maze[x, y].setDirections(maze[x, y].getDirections() - 1);
                                    if (!maze[x, y - 2].isExplored())
                                    {
                                        stack.push(maze[x, y]);
                                        maze[x, y - 1].explore(' ');
                                        maze[x, y - 2].explore(' ');
                                        y -= 2;
                                        break;
                                    }
                                }
                                break;
                        }//end switch
                    }//end while

                    //backtrack
                    if (!stack.isEmpty())
                    {
                        Space back = stack.pop();
                        x = back.getX();
                        y = back.getY();
                    }
                } while (!stack.isEmpty());

                maze[1, 0].explore(' ');
                maze[width - 1, height - 2].explore(' ');

                //display maze
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (maze[i, height - 1 - j].getC() == '#')
                        {
                            gfx.FillRectangle(Brushes.Black, i * size, j * size, size, size);
                        }
                        else if (maze[i, height - 1 - j].getC() == ' ')
                        {
                            gfx.FillRectangle(Brushes.White, i * size, j * size, size, size);
                        }

                    }//end for 
                }//end for

                gfx.FillRectangle(Brushes.Red, locX * size, locY * size, size, size);
                pictureBox.Image = drawingSurface;
                //end display
            }//end if
            else
            {
                MessageBox.Show(message);
            }
        }
    }
}
