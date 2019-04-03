using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeApplication
{
    class Space
    {
        private int x, y;
        private char character;
        private bool explored;
        private int directions;

        public Space()
        {
            x = -1;
            y = -1;
            character = 'E';
            explored = false;
            directions = -1;
        }

        public Space(int a, int b, char c, bool e)
        {
            x = a;
            y = b;
            character = c;
            explored = e;
            directions = 15;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public char getC()
        {
            return character;
        }

        public void explore(char c)
        {
            character = c;
            explored = true;
        }

        public bool isExplored()
        {
            return explored;
        }

        public void setDirections(int k)
        {
            directions = k;
        }

        public int getDirections()
        {
            return directions;
        }

        public void unexplore()
        {
            explored = false;
        }
    }
}
