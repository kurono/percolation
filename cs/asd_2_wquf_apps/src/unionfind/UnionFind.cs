/*
 * File: UnionFind.cs
 * Description: A base class implementing the QuickFind algorithm of 
 * dynamic connectivity of elements of the set.
 * Authors:
 *   - Ilya Tsivilskiy
 * Copyright: (c) 2023 Ilya Tsivilskiy
 * License: This file is licensed under the MIT License.
 */

using asd_2_collunionfind.src.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace asd_2_collunionfind.src.unionfind
{
    internal class UnionFind : IUnionFind
    {
        protected int[] _id; // ID of a cluster that contains the connected elements
        protected bool _debug;

        /// <summary>
        /// Create a UnionFind data structure
        /// </summary>
        /// <param name="n">Total number of elements in collection</param>
        /// <param name="debug">Print out the connectivity info after each operation</param>
        public UnionFind(int n, bool debug = false)
        {
            // init an array of indices
            _id = new int[n];

            _debug = debug;

            // fill-up the array:
            // at first, each element's id equal its actual index,
            // so all elements form an individual own cluster
            for (int i = 0; i < n; i++)
            {
                _id[i] = i;
            }
        }

        // make the p-th element to belong to the cluster of the q-th element
        // union(4, 3) = connect 4 to the cluster of 3
        // NB: If a method in the base class is marked as "virtual",
        // it means that the method can be "overridden" in a derived class!
        virtual public void Union(int p, int q)
        {
            if (_debug)
            {
                Logger.WriteLine($"union({p}, {q}):");
                Logger.Write("before:\t");
                Print();
            }

            // no need to connect the elements,
            // if they are already connected
            if (_CallConnected(p, q))
            {
                return;
            }

            _CallUnion(p, q);

            if (_debug)
            {
                Logger.Write("after:\t");
                Print();
                Logger.WriteLine();
            }
        }

        // NB: If a method in the base class is marked as "virtual",
        // it means that the method can be "overridden" in a derived class!
        virtual public bool Connected(int p, int q)
        {
            // they belong to the same cluster
            bool result = _CallConnected(p, q);
            if (_debug)
            {
                Logger.WriteLine($"connected({p}, {q}) = {result}");
            }  
            return result;
        }

        /// <summary>
        /// The "core" of the "Union" method, 
        /// connects two elements
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        virtual protected void _CallUnion(int p, int q)
        {
            int pCluster = _id[p]; // original p-th id = 4
            int qCluster = _id[q]; // original q-th id = 3
            // chain-style change of ids:
            // recursively connect the elements
            for (int i = 0; i < _id.Length; i++)
            {
                // if i-th element belongs to p-th cluster
                if (_id[i] == pCluster) // if id[i] == 4
                {
                    // connect it to the q-th cluster
                    _id[i] = qCluster; // id[i] = 3
                }
            }
        }

        /// <summary>
        /// The "core" of the "Connected" method, 
        /// checks whether two elememts are connected
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        virtual protected bool _CallConnected(int p, int q)
        {
            return _id[p] == _id[q];
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            foreach (int id in _id)
            {
                str.Append(id).Append(" ");
            }
            return str.ToString();
        }

        public void Print()
        {
            Logger.WriteLine(this.ToString());
        }
    }
}
