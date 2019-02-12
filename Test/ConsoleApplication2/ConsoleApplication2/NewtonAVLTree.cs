using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication2
{
    public class NewtonAVLTree<T> : BinTree<T>
    {
        public NewtonAVLTree()
        {
            pRoot = null;
        }

        public NewtonTreeNode<T> AVLTree_Find(NewtonAVLTree<T> pTree, T pData, Comparison<T> CompareFunc)
        {
            return base.BinTree_Find(pTree.pRoot, pData, CompareFunc);
        }

        public NewtonTreeNode<T> AVLTree_Find(T pData, Comparison<T> CompareFunc)
        {
            return AVLTree_Find(this, pData, CompareFunc);
        }

        private void AVLTree_RotateLeftRight(ref NewtonTreeNode<T> ppRoot, NewtonTreeNode<T> pStartNode)
        {
            NewtonTreeNode<T> pANode, pBNode, pCNode;
            pANode = pStartNode;
            pBNode = pANode.pLeft;
            pCNode = pBNode.pRight;
            base.BinTree_RotateLeft(pBNode, ref ppRoot);
            base.BinTree_RotateRight(pANode, ref ppRoot);
            switch (pCNode.nMagic)
            {
                case 1:
                    pANode.nMagic = -1;
                    pBNode.nMagic = 0;
                    break;
                case -1:
                    pANode.nMagic = 0;
                    pBNode.nMagic = 1;
                    break;
                default:
                    pANode.nMagic = 0;
                    pBNode.nMagic = 0;
                    break;
            }
            pCNode.nMagic = 0;
        }

        private void AVLTree_RotateRightLeft(ref NewtonTreeNode<T> ppRoot, NewtonTreeNode<T> pStartNode)
        {
            NewtonTreeNode<T> pANode, pBNode, pCNode;
            pANode = pStartNode;
            pBNode = pANode.pRight;
            pCNode = pBNode.pLeft;
            base.BinTree_RotateRight(pBNode, ref ppRoot);
            base.BinTree_RotateLeft(pANode, ref ppRoot);
            switch (pCNode.nMagic)
            {
                case 1:
                    pANode.nMagic = 0;
                    pBNode.nMagic = -1;
                    break;
                case -1:
                    pANode.nMagic = 1;
                    pBNode.nMagic = 0;
                    break;
                default:
                    pANode.nMagic = 0;
                    pBNode.nMagic = 0;
                    break;
            }
            pCNode.nMagic = 0;
        }

        public static void AVLTree_FixBalance(NewtonTreeNode<T> pStartNode, T pData, System.Comparison<T> CompareFunc)
        {
            NewtonTreeNode<T> pNode, pSearchNode;
            int nResult;
            pNode = pStartNode;
            while (pNode != null)
            {
                pSearchNode = pNode;
                nResult = CompareFunc(pNode.pData, pData);
                if (nResult < 0)
                {
                    pNode = pNode.pRight;
                    pSearchNode.nMagic--;
                }
                else if (nResult > 0)
                {
                    pNode = pNode.pLeft;
                    pSearchNode.nMagic++;
                }
                else break;
            }
            return;
        }

        public NewtonTreeNode<T> AVLTree_GetMax()
        {
            return base.BinTree_FindMax(base.pRoot);
        }

        public bool AVLTree_Insert(NewtonAVLTree<T> pTree, T pData, System.Comparison<T> CompareFunc)
        {
            bool nRet = false;
            if (pTree == null || pData == null || CompareFunc == null) return false;
            nRet = AVLTreeNode_Insert(ref pTree.pRoot, pData, CompareFunc);
            return nRet;

        }

        public bool AVLTree_Insert(T pData, System.Comparison<T> CompareFunc)
        {
            return AVLTree_Insert(this, pData, CompareFunc);
        }

        bool AVLTreeNode_Insert(ref NewtonTreeNode<T> ppRootNode, T pData,
                       System.Comparison<T> CompareFunc)
        {
            NewtonTreeNode<T> pNode, pANode, pNewNode, pSearchNode, pBNode, pCNode;
            int nRet;

            nRet = 0;
            pANode = null;
            pSearchNode = null;

            /* 查找要插入的节点位置，并且在查找过程中要记录最后一个不平衡的节点 */
            pNode = ppRootNode;
            while (pNode != null)
            {
                pSearchNode = pNode;
                if (pSearchNode.nMagic == -1 || pSearchNode.nMagic == 1)
                {
                    pANode = pSearchNode;
                }
                nRet = CompareFunc(pNode.pData, pData);
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
                    /* 找到相同关键词的节点，插入失败 */
                    return false;
                }
            }

            pNewNode = new NewtonTreeNode<T>(pData, 0);
            if (pNewNode == null)
            {
                return false;
            }
            pNewNode.pParent = pSearchNode;

            if (pSearchNode == null)
            {
                ppRootNode = pNewNode;
                return true;
            }

            if (nRet < 0)
            {
                pSearchNode.pRight = pNewNode;
            }
            else
            {
                pSearchNode.pLeft = pNewNode;
            }



            /* 不存在不平衡的节点，直接插入后再修改平衡因子即可 */
            if (pANode == null)
            {
                /* 修改从根节点到插入节点的平衡因子 */
                AVLTree_FixBalance(ppRootNode, pData, CompareFunc);

                return true;
            }

            nRet = CompareFunc(pANode.pData, pData);

            /* 以下处理存在不平衡节点的情况 */
            if ((pANode.nMagic == 1 && nRet < 0)
                || (pANode.nMagic == -1 && nRet > 0))
            {
                /* A节点平衡因子为1且插入节点插入在右子树的情况
                 * 以及A节点平衡因子为-1且插入在左子树的情况
                 * 这两种情况插入后还是平衡树，只需修改相关节点平衡因子
                 */
                AVLTree_FixBalance(pANode, pData, CompareFunc);
            }
            else if (pANode.nMagic == 1 && nRet > 0)
            {
                if (CompareFunc(pANode.pLeft.pData, pData) > 0)
                {
                    /* LL型不平衡, 插入在A节点的左子树的左子树中 */
                    pBNode = pANode.pLeft;
                    BinTree_RotateRight(pANode, ref ppRootNode);
                    AVLTree_FixBalance(pBNode, pData, CompareFunc);
                    pANode.nMagic = 0;
                    pBNode.nMagic = 0;
                }
                else
                {
                    int nRetVal;

                    /* LR型不平衡, 插入在A节点的左子树的右子树中 */
                    pBNode = pANode.pLeft;
                    pCNode = pBNode.pRight;
                    nRetVal = CompareFunc(pCNode.pData, pData);
                    if (nRetVal > 0)
                    {
                        pCNode.nMagic++;
                    }
                    else if (nRetVal < 0)
                    {
                        pCNode.nMagic--;
                    }
                    AVLTree_RotateLeftRight(ref ppRootNode, pANode);
                    if (nRetVal > 0)
                    {
                        AVLTree_FixBalance(pBNode, pData, CompareFunc);
                    }
                    else if (nRetVal < 0)
                    {
                        AVLTree_FixBalance(pANode, pData, CompareFunc);
                    }
                }
            }
            else /* pANode.nMagic == -1 && nRet < 0 的情况*/
            {
                if (CompareFunc(pANode.pRight.pData, pData) > 0)
                {
                    int nRetVal;

                    /* RL型不平衡, 插入在A节点的右子树的左子树中 */
                    pBNode = pANode.pRight;
                    pCNode = pBNode.pLeft;
                    nRetVal = CompareFunc(pCNode.pData, pData);
                    if (nRetVal > 0)
                    {
                        pCNode.nMagic++;
                    }
                    else if (nRetVal < 0)
                    {
                        pCNode.nMagic--;
                    }
                    AVLTree_RotateRightLeft(ref ppRootNode, pANode);
                    if (nRetVal > 0)
                    {
                        AVLTree_FixBalance(pANode, pData, CompareFunc);
                    }
                    else if (nRetVal < 0)
                    {
                        AVLTree_FixBalance(pBNode, pData, CompareFunc);
                    }
                }
                else
                {
                    /* RR型不平衡, 插入在A节点的右子树的右子树中 */
                    pBNode = pANode.pRight;
                    BinTree_RotateLeft(pANode, ref ppRootNode);
                    AVLTree_FixBalance(pBNode, pData, CompareFunc);
                    pANode.nMagic = 0;
                    pBNode.nMagic = 0;
                }
            }
            return true;
        }

        /** AVL树的删除操作调整平衡函数，由删除函数调用

 @param AVLTREENODE **ppRoot - 指向AVL树的根节点指针的指针 
 @param AVLTREENODE *pNode - 要调整平衡的节点 
 @param void *pData - 删除的数据指针 
 @param COMPAREFUNC CompareFunc - 数据比较回调函数 
 @return void - 无 
*/
        void AVLTree_AdjustBalanceForDelete(ref NewtonTreeNode<T> ppRoot, NewtonTreeNode<T> pNode,
                                            T pData, System.Comparison<T> CompareFunc)
        {
            NewtonTreeNode<T> pANode;
            NewtonTreeNode<T> pParentNode;
            NewtonTreeNode<T> pBNode;
            NewtonTreeNode<T> pCNode;

            pANode = pNode;
            while (pANode != null)
            {
                switch (pANode.nMagic)
                {
                    case 0:
                        if (pANode == ppRoot)
                        {
                            /* pANode为根节点，高度减少了1，但左右子树高度相等，无需调整 */
                            break;
                        }
                        else
                        {
                            pParentNode = pANode.pParent;
                            if (pANode == pParentNode.pLeft)
                            {
                                pParentNode.nMagic -= 1;
                            }
                            else
                            {
                                pParentNode.nMagic += 1;
                            }
                            /* 将pANode 指向它的父节点，继续调整它的父节点的不平衡情况 */
                            pANode = pParentNode;
                            continue;
                        }
                    case -1:
                    case 1:
                        /* pANode原来的平衡因子为0，删除操作发生后，高度未改变，因此不需要
                         * 再调整，退出即可 
                         */
                        break;
                    case -2: /* L型不平衡情况 */
                        pBNode = pANode.pRight;
                        if (pBNode.nMagic == 0)
                        {
                            /* L0型不平衡情况 */
                            BinTree_RotateLeft(pANode, ref ppRoot);
                            pANode.nMagic = -1;
                            pBNode.nMagic = 1;

                            break;
                        }
                        else if (pBNode.nMagic == -1)
                        {
                            /* L-1型不平衡情况 */
                            pParentNode = pANode.pParent;
                            if (pParentNode != null)
                            {
                                if (pANode == pParentNode.pLeft)
                                {
                                    pParentNode.nMagic -= 1;
                                }
                                else
                                {
                                    pParentNode.nMagic += 1;
                                }
                            }

                            BinTree_RotateLeft(pANode, ref ppRoot);
                            pANode.nMagic = 0;
                            pBNode.nMagic = 0;

                            /* 将pANode 指向它的父节点，继续调整它的父节点的不平衡情况 */
                            pANode = pParentNode;
                        }
                        else /* pBNode.nMagic == 1的情况 */
                        {
                            /* L1型的情况 */
                            pParentNode = pANode.pParent;
                            if (pParentNode != null)
                            {
                                if (pANode == pParentNode.pLeft)
                                {
                                    pParentNode.nMagic -= 1;
                                }
                                else
                                {
                                    pParentNode.nMagic += 1;
                                }
                            }
                            pBNode = pANode.pRight;
                            pCNode = pBNode.pLeft;

                            AVLTree_RotateRightLeft(ref ppRoot, pANode);


                            /* 将pANode 指向它的父节点，继续调整它的父节点的不平衡情况 */
                            pANode = pParentNode;

                        }
                        continue; /* 继续while() 循环 */
                    case 2: /* R型不平衡情况 */
                        pBNode = pANode.pLeft;
                        if (pBNode.nMagic == 0)
                        {
                            /* R0型不平衡情况 */
                            BinTree_RotateRight(pANode, ref ppRoot);
                            pANode.nMagic = 1;
                            pBNode.nMagic = -1;
                            break;
                        }
                        else if (pBNode.nMagic == -1)
                        {
                            /* R-1型不平衡情况 */
                            pParentNode = pANode.pParent;
                            if (pParentNode != null)
                            {
                                if (pANode == pParentNode.pLeft)
                                {
                                    pParentNode.nMagic -= 1;
                                }
                                else
                                {
                                    pParentNode.nMagic += 1;
                                }
                            }
                            pBNode = pANode.pLeft;
                            pCNode = pBNode.pRight;
                            AVLTree_RotateLeftRight(ref ppRoot, pANode);
                            /* 将pANode 指向它的父节点，继续调整它的父节点的不平衡情况 */
                            pANode = pParentNode;
                        }
                        else /* pBNode.nMagic == 1的情况 */
                        {
                            /* R1型的情况 */
                            pParentNode = pANode.pParent;
                            if (pParentNode != null)
                            {
                                if (pANode == pParentNode.pLeft)
                                {
                                    pParentNode.nMagic -= 1;
                                }
                                else
                                {
                                    pParentNode.nMagic += 1;
                                }
                            }
                            BinTree_RotateRight(pANode, ref ppRoot);
                            pANode.nMagic = 0;
                            pBNode.nMagic = 0;
                            /* 将pANode 指向它的父节点，继续调整它的父节点的不平衡情况 */
                            pANode = pParentNode;
                        }
                        continue; /* 继续while() 循环 */
                    default:
                        break;
                } /* switch ( pANode.nMagic ) */
                break;
            }
        }


        /** AVL树的删除操作函数

            @param AVLTREE *pTree - AVL树指针 
            @param void *pData - 要删除的数据指针 
            @param COMPAREFUNC CompareFunc - 数据比较回调函数 
            @param DESTROYFUNC DestroyFunc - 数据释放回调函数 
            @return INT - CAPI_FAILED表示失败，CAPI_SUCCESS表示成功 
        */
        public bool AVLTree_Delete(NewtonAVLTree<T> pTree, T pData, System.Comparison<T> CompareFunc)
        {
            if (pTree.pRoot == null || pData == null
                || CompareFunc == null)
            {
                return false;
            }
            AVLTreeNode_Delete(ref pTree.pRoot, pData, CompareFunc);

            return true;
        }
        public bool AVLTree_Delete(T pData, System.Comparison<T> CompareFunc)
        {
            return AVLTree_Delete(this, pData, CompareFunc);
        }

        /** AVL树的删除节点函数

            @param AVLTREENODE **ppRoot - 指向AVL树根节点指针的指针 
            @param void *pData - 要删除的数据 
            @param COMPAREFUNC CompareFunc - 数据比较回调函数 
            @param DESTROYFUNC DestroyFunc - 数据释放回调函数 
            @return INT - CAPI_FAILED表示失败，CAPI_SUCCESS表示成功  
        */
        bool AVLTreeNode_Delete(ref NewtonTreeNode<T> ppRoot, T pData,
                               System.Comparison<T> CompareFunc)
        {

            NewtonTreeNode<T> pNode;
            NewtonTreeNode<T> pANode;
            NewtonTreeNode<T> pDelNode;
            T pDelData;
            int nRet = 0;

            pNode = ppRoot;

            while (pNode != null)
            {
                nRet = CompareFunc(pNode.pData, pData);
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

            if (pNode == null)
            {
                return false;
            }

            pDelData = pNode.pData;
            if (pNode.pLeft != null && pNode.pRight != null)
            {
                /* 处理查找到的pNode有两个子节点的情况 */
                pDelNode = pNode.pLeft;
                while (pDelNode.pRight != null)
                {
                    pDelNode = pDelNode.pRight;
                }
                pANode = pDelNode.pParent;

                pNode.pData = pDelNode.pData;

                if (pDelNode == pANode.pLeft)
                {
                    pANode.nMagic -= 1;
                }
                else
                {
                    pANode.nMagic += 1;
                }

                if (pDelNode != pNode.pLeft)
                {
                    pANode.pRight = pDelNode.pLeft;
                }
                else
                {
                    pANode.pLeft = pDelNode.pLeft;
                }

                if (pDelNode.pLeft != null)
                {
                    pDelNode.pLeft.pParent = pANode;
                }
            }
            else
            {
                pANode = pNode;
                /* 处理最多只有一个子节点的情况 */
                if (pNode.pLeft != null)
                {
                    /* 只有左节点的情况 */
                    pDelNode = pNode.pLeft;
                    pNode.pData = pDelNode.pData;
                    pNode.pLeft = null;
                    pANode.nMagic -= 1;
                }
                else if (pNode.pRight != null)
                {
                    /* 只有右节点的情况 */
                    pDelNode = pNode.pRight;
                    pNode.pData = pDelNode.pData;
                    pNode.pRight = null;
                    pANode.nMagic += 1;
                }
                else
                {
                    /* 处理删除节点的左右子节点都不存在的情况 */
                    pANode = pNode.pParent;
                    pDelNode = pNode;
                    if (pANode == null)
                    {
                        ppRoot = pANode;
                    }
                    else if (pANode.pLeft == pNode)
                    {
                        pANode.pLeft = null;
                        pANode.nMagic -= 1;
                    }
                    else
                    {
                        pANode.pRight = null;
                        pANode.nMagic += 1;
                    }
                }
            }

            /* 调整平衡 */
            if (pANode != null)
            {
                AVLTree_AdjustBalanceForDelete(ref ppRoot, pANode, pData, CompareFunc);
            }

            return true;
        }

        private void showTreeData(NewtonTreeNode<T> root, int depth)
        {
            if (root == null) return;
            showTreeData(root.pLeft, depth + 1);
            for (int i = 0; i < depth; i++)
                Console.Write(" ");
            Console.WriteLine(root.pData);
            showTreeData(root.pRight, depth + 1);
        }

        private void showTreeBalanceFactor(NewtonTreeNode<T> root, int depth)
        {
            if (root == null) return;
            showTreeBalanceFactor(root.pLeft, depth + 1);
            for (int i = 0; i < depth; i++)
                Console.Write(" ");
            Console.WriteLine(root.nMagic);
            showTreeBalanceFactor(root.pRight, depth + 1);
        }

        public void showTree()
        {
            Console.WriteLine("TreeData");
            showTreeData(this.pRoot, 0);
            Console.WriteLine("Tree nMagic");
            showTreeBalanceFactor(this.pRoot, 0);
        }
    }
}
