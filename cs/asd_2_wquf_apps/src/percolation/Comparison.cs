/*
 * File: Comparison.cs
 * Description: A set of math operators to compare cell data values in a Grid.
 * Authors:
 *   - Ilya Tsivilskiy
 * Copyright: (c) 2023 Ilya Tsivilskiy
 * License: This file is licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asd_2_wquf_apps.src.percolation
{
    internal static class Comparison
    {
        public enum Operator
        {
            EQUALS,
            GREATER_THAN,
            LESS_THAN,
            GREATER_THAN_OR_EQUAL,
            LESS_THAN_OR_EQUAL
        }
    }
}
