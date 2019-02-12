using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication2
{
    class Program
    {
        static int _IntCompareFunc(int x, int y)
        {
            return x.CompareTo(y);
        }

        static void Main(string[] args)
        {
            //调用例子
            NewtonAVLTree<int> nat = new NewtonAVLTree<int>();
            nat.AVLTree_Insert(nat, 10, _IntCompareFunc);
            nat.showTree();

            //           nat.AVLTree_Insert(nat, 15, CompareFunc);
            //           nat.showTree();
            //           nat.AVLTree_Insert(nat, 1, CompareFunc);
            //           nat.showTree();
            //           int nMax = nat.AVLTree_GetMax().pData;
            //           nat.AVLTree_Insert(nat, 21, CompareFunc);
            //           nat.showTree();

            //           nat.AVLTree_Insert(nat, 2, CompareFunc);
            //           nat.showTree();

            //           nat.AVLTree_Insert(nat, 16, CompareFunc);
            //           nat.showTree();

            //           nat.AVLTree_Insert(nat, 31, CompareFunc);
            //           nat.showTree();

            //           nat.AVLTree_Insert(nat, -5, CompareFunc);
            //           nat.showTree();


            //           NewtonTreeNode<int> pNode = nat.AVLTree_Find(-5, CompareFunc);
            //           nat.AVLTree_Delete(10, CompareFunc);
            //           nat.showTree();
            //            nat.AVLTree_Delete(16, CompareFunc);
            //           nat.showTree();

        }
    }
}
