/*
 * File: IUnionFind.cs
 * Description: An interface 
 * for various UnionFind-like data structures 
 * that describe connectivity within a set of elements.
 * Authors:
 *   - Ilya Tsivilskiy
 * Copyright: (c) 2023 Ilya Tsivilskiy
 * License: This file is licensed under the MIT License.
 */

using asd_2_collunionfind.src.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asd_2_collunionfind.src.unionfind
{
    internal interface IUnionFind
    {
        /// <summary>
        /// Connects p-th element to q-th element
        /// </summary>
        /// <param name="p">Index of an element to be connected</param>
        /// <param name="q">Index of an element to connect to</param>
        void Union(int p, int q);

        /// <summary>
        /// Checks whether two elements are connected
        /// </summary>
        /// <param name="p">Index of the element</param>
        /// <param name="q">Index of another element</param>
        /// <returns>The status of connection</returns>
        bool Connected(int p, int q);

        /// <summary>
        /// Makes a string of current connectivity pattern
        /// </summary>
        /// <returns>A string representation of _id</returns>
        string ToString();

        /// <summary>
        /// Print out the data structure to console
        /// </summary>
        void Print();
    }
}
