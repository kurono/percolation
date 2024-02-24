/*
 * File: UnionFindQU.cs
 * Description: An extension of the UnionFind
 * insted, implementing a QuickUnion algorithm of 
 * dynamic connectivity of elements of the set.
 * It uses the 'path compression' approach in the 'Root' method
 * to shorten the pedigree.
 * Authors:
 *   - Ilya Tsivilskiy
 * Copyright: (c) 2023 Ilya Tsivilskiy
 * License: This file is licensed under the MIT License.
 */

using asd_2_collunionfind.src.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asd_2_collunionfind.src.unionfind
{
    internal class UnionFindQU : UnionFind
    {
        public UnionFindQU(int n) : base(n)
        {
            // initially each element's index is its own root:
            // _id is the ID of the root (parent) element
        }

        /// <summary>
        /// Parent[i] = parent of i-th element
        /// </summary>
        public int[] Parent
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        /**
         * Changes element's parent pointers until reach the root
         * @param i Element's index
         * @return Index of the canonical root element
         */
        protected int Root(int i)
        {
            // chain-like changing the roots
            // while the i-th element index is not an index of its root
            while (i != Parent[i])
            {
                // to point every other node to its super-parent (the very main parent)
                Parent[i] = Parent[Parent[i]];
                // jump to the root of i-th element
                i = Parent[i];
            }
            return i;
        }

        // make the p-th element to be a child of q-th element's parent
        // union(4, 3) = 4 is a child of 3's root (parent) element
        override protected void _CallUnion(int p, int q)
        {
            // get the root elements of q-th and q-th elements
            int rootP = Root(p);
            int rootQ = Root(q);
            if (rootP == rootQ) return;

            // make the root of p-the element a child of q-ths parent
            Parent[rootP] = rootQ;
        }

        override protected bool _CallConnected(int p, int q)
        {
            // they have the same root (parent) element
            return Root(p) == Root(q);
        }
    }
}
