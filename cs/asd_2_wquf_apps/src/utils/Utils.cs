/*
 * File: Utils.cs
 * Description: Some tiny useful utilities for the data processing
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

namespace asd_2_wquf_apps.src.utils
{
    internal class Utils
    {
        /// <summary>
        /// Checks whether vMin <= v <= vMax 
        /// </summary>
        /// <typeparam name="T">Explicit type of the variabes to compare to</typeparam>
        /// <param name="v">A variable to check its being in the range</param>
        /// <param name="vMin">Left-hand inclusive bound of the range</param>
        /// <param name="vMax">Right-hand inclusive bound of the range</param>
        /// <returns></returns>
        public static bool InRange<T>(T v, T vMin, T vMax) where T : IComparable<T>
        {
            return ((v.CompareTo(vMin) >= 0) && (v.CompareTo(vMax) <= 0));
        }

        /// <summary>
        /// Constrain a value to be in the range, bounds inclusive
        /// </summary>
        /// <typeparam name="T">Explicit type parameter</typeparam>
        /// <param name="v">A value to constrain</param>
        /// <param name="vMin">Left-hand inclusive bound of the range</param>
        /// <param name="vMax">Right-hand inclusive bound of the range</param>
        /// <returns></returns>
        public static T Constrain<T>(T v, T vMin, T vMax) where T : IComparable<T>
        {
            if (v.CompareTo(vMin) < 0)
            {
                return vMin;
            }
            else if (v.CompareTo(vMax) > 0)
            {
                return vMax;
            }
            else
            {
                return v;
            }
        }
    }
}
