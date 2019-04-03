using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeApplication
{
    class Stack
    {
        private Space[] StackArray;
        private int size;

        public Stack()
        {
            StackArray = new Space[1256];
            size = 0;
        }

        public bool isEmpty()
        {
            if (size <= 0)
                return true;
            else return false;
        }

        public void push(Space element)
        {
            StackArray[size] = element;
            size++;
        }

        public Space pop()
        {
            if (isEmpty())
            {
                Space error = new Space();
                return error;
            }
            else
            {
                size--;
                return StackArray[size];
            }
        }
    }
}
