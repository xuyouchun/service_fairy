using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication2
{
    public class BinTree<T>
    {
        public NewtonTreeNode<T> pRoot;
        public BinTree()
        {
            pRoot = null;
        }

        protected NewtonTreeNode<T> BinTree_Find(NewtonTreeNode<T> pRoot, T pData, System.Comparison<T> CompareFunc)
        {

            NewtonTreeNode<T> pNode = pRoot;
            while (pNode != null)
            {

                int nRet = CompareFunc(pNode.pData, pData);
                if (nRet < 0)
                {
                    pNode = pNode.pRight;
                }
                else if (nRet > 0)
                {
                    pNode = pNode.pLeft;
                }
                else
                {
                    return pNode;
                }
            }
            return null;
        }

        protected NewtonTreeNode<T> BinTree_FindMax(NewtonTreeNode<T> pRoot)
        {
            if (pRoot == null) return null;
            NewtonTreeNode<T> pMaxNode = pRoot;
            while (pMaxNode.pRight != null)
            {
                pMaxNode = pMaxNode.pRight;
            }
            return pMaxNode;
        }

        protected void BinTree_Insert(ref NewtonTreeNode<T> pRoot, T pData, System.Comparison<T> CompareFunc, int nMagic)
        {
            NewtonTreeNode<T> pNode, pNewNode;
            int nRet = 0;
            pNode = pRoot;
            while (pNode != null)
            {
                nRet = CompareFunc(pNode.pData, pData);
                if (nRet < 0)
                {
                    if (pNode.pRight != null)
                    {
                        pNode = pNode.pRight;
                        continue;
                    }
                }
                else
                {
                    if (pNode.pLeft != null)
                    {
                        pNode = pNode.pLeft;
                        continue;
                    }
                }
                break;
            }
            pNewNode = new NewtonTreeNode<T>(pData, nMagic);
            if (pNode == null)
            {
                pRoot = pNewNode;
                pNewNode.pParent = null;
            }
            else
            {
                if (nRet < 0)
                {
                    pNode.pRight = pNewNode;
                }
                else
                {
                    pNode.pLeft = pNewNode;
                }
                pNewNode.pParent = pNode;
            }
        }

        protected bool BinTree_Delete(BinTree<T> pBinTree, T pData, System.Comparison<T> CompareFunc)
        {
            NewtonTreeNode<T> pNode, pParentNode, pMaxNode, pParentMaxNode;
            if (pBinTree == null || pBinTree.pRoot == null || pData == null || CompareFunc == null)
                return false;
            pNode = pBinTree.pRoot;
            //pNode = this.BinTree_Find(pNode, pData, CompareFunc,out pParentNode);
            pParentNode = null;
            while (pNode != null)
            {
                pParentNode = pNode;
                int nRet = CompareFunc(pNode.pData, pData);
                if (nRet < 0)
                {
                    pNode = pNode.pRight;
                }
                else if (nRet > 0)
                {
                    pNode = pNode.pLeft;
                }
                else
                {
                    break;
                }
            }

            if (pNode == null) return false;
            if (pNode.pLeft != null && pNode.pRight != null)
            {
                pMaxNode = pNode.pLeft;
                pParentMaxNode = pNode;
                while (pMaxNode.pRight != null)
                {
                    pParentMaxNode = pMaxNode;
                    pMaxNode = pMaxNode.pRight;
                }
                pNode.pData = pMaxNode.pData;
                if (pMaxNode == pNode.pLeft)
                {
                    pNode.pLeft = pMaxNode.pLeft;
                }
                else
                {
                    pParentMaxNode.pRight = pMaxNode.pLeft;
                }
                return true;
            }
            if (pNode.pLeft != null)
            {
                pMaxNode = pNode.pLeft;
            }
            else
            {
                pMaxNode = pNode.pRight;
            }
            if (pNode == pBinTree.pRoot)
            {
                pBinTree.pRoot = pMaxNode;
            }
            else
            {
                if (pParentNode.pLeft == pNode)
                {
                    pParentNode.pLeft = pMaxNode;
                }
                else
                {
                    pParentNode.pRight = pMaxNode;
                }
            }
            return true;
        }

        protected void BinTree_RotateLeft(NewtonTreeNode<T> pANode, ref NewtonTreeNode<T> ppRoot)
        {
            NewtonTreeNode<T> pBNode;
            pBNode = pANode.pRight;
            pANode.pRight = pBNode.pLeft;
            if (pBNode.pLeft != null)
            {
                pBNode.pLeft.pParent = pANode;
            }
            pBNode.pParent = pANode.pParent;
            if (pANode == ppRoot)
            {
                ppRoot = pBNode;
            }
            else if (pANode == pANode.pParent.pLeft)
            {
                pANode.pParent.pLeft = pBNode;
            }
            else
            {
                pANode.pParent.pRight = pBNode;
            }
            pBNode.pLeft = pANode;
            pANode.pParent = pBNode;
        }

        protected void BinTree_RotateRight(NewtonTreeNode<T> pANode, ref NewtonTreeNode<T> ppRoot)
        {
            NewtonTreeNode<T> pBNode;
            pBNode = pANode.pLeft;
            pANode.pLeft = pBNode.pRight;
            if (pBNode.pRight != null)
            {
                pBNode.pRight.pParent = pANode;
            }
            pBNode.pParent = pANode.pParent;
            if (pANode == ppRoot)
            {
                ppRoot = pBNode;
            }
            else if (pANode == pANode.pParent.pRight)
            {
                pANode.pParent.pRight = pBNode;
            }
            else
            {
                pANode.pParent.pLeft = pBNode;
            }
            pBNode.pRight = pANode;
            pANode.pParent = pBNode;
        }
    }
}
