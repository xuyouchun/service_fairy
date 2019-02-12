using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication2
{
    public class NewtonTreeNode<T>
    {
        public NewtonTreeNode<T> pLeft, pRight, pParent;
        public int nMagic;
        public T pData;

        public NewtonTreeNode(T pData)
        {
            this.pData = pData;
            pLeft = null;
            pRight = null;
            pParent = null;
            nMagic = 0;
        }

        public NewtonTreeNode(T pData, int nMagic)
        {
            this.pData = pData;
            pLeft = null;
            pRight = null;
            pParent = null;
            this.nMagic = nMagic;
        }
    }
}

