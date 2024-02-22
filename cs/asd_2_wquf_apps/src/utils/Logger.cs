/*
 * File: Stopwatch.cs
 * Description: Just a wrapper around the standard Stopwatch class, 
 * implemented for the sake of portablity.
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

namespace asd_2_collunionfind.src.utils
{
    /// <summary>
    /// A helper class to write an informtaion into system console
    /// </summary>
    internal static class Logger
    {
        public static readonly String DELIMITER = " ";
        /// <summary>
        /// Write all arguments sequentially in a single line
        /// </summary>
        /// <param name="args">A sequence of arguments</param>
        public static void Write(params object[] args)
        {
            for (int i=0; i < args.Length; i++)
            {
                Console.Write(args[i].ToString());
                if (i < args.Length - 1)
                {
                    Console.Write(DELIMITER);
                }
            }
        }

        /// <summary>
        /// Write all arguments starting from a new line
        /// </summary>
        /// <param name="args">A sequence of arguments</param>
        public static void WriteLine(params object[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine();
                return;
            }

            foreach (var arg in args)
            {
                Console.WriteLine(arg.ToString());
            }
        }
    }
}
