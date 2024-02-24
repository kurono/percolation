/*
 * File: Grid.cs
 * Description: A wrapper over 1D array of integers 
 * to mimic a 2D grid-like array of cells
 * which may be either closed, opened, or filled by the fluid. 
 * Authors:
 *   - Ilya Tsivilskiy
 * Copyright: (c) 2023 Ilya Tsivilskiy
 * License: This file is licensed under the MIT License.
 */

using asd_2_wquf_apps.src.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asd_2_wquf_apps.src.percolation
{
    /// <summary>
    /// A pseudo 2D grid of a cells
    /// </summary>
    internal class Grid // internal = accessible within the same assembly
    {
        private int _nrows;
        private int _ncols;
        private int[] _data; // flatted 1D array representing a pseudo-2D data structure

        public enum Status
        {
            CLOSED,             // 0
            OPENED,             // 1
            OPENED_AND_FILLED   // 2
        }

        /// <summary>
        /// Instantiates new Grid object
        /// </summary>
        /// <param name="nRows">Vertical resolution, a number of cells in each row</param>
        /// <param name="nCols">Horizontal resolution, a number of cells in each column</param>
        public Grid(int nRows, int nCols)
        {
            _nrows = nRows;
            _ncols = nCols;

            if ((_nrows <= 0) || (_ncols <= 0))
            {
                throw new ArgumentOutOfRangeException("Grid resolution should be positive!");
            }

            _data = new int[this.CellsCount];
        }

        #region Public methods
        /// <summary>
        /// Number of cells in a vertical direction
        /// </summary>
        public int RowsCount
        {
            get
            {
                return _nrows;
            }
        }

        /// <summary>
        /// Numbr of cells in a horizontal direction
        /// </summary>
        public int ColumnsCount
        {
            get
            {
                return _ncols;
            }
        }

        /// <summary>
        /// Total cells in the grid
        /// </summary>
        public int CellsCount
        {
            get
            {
                return this.RowsCount * this.ColumnsCount;
            }
        }

        /// <summary>
        /// A set of all 1D indices of cells belonging the current row
        /// </summary>
        /// <param name="row">The row to slice over</param>
        /// <returns>An array of global 1D column cell indices</returns>
        public int[] HorizontalSlice(int row)
        {
            int[] columns = new int[this.ColumnsCount];
            for (int ic = 0; ic < this.ColumnsCount; ic++)
            {
                columns[ic] = this.I1(row, ic);
            }
            return columns;
        }

        /// <summary>
        /// 1D array of pseudo-2D cell values
        /// </summary>
        public int[] RawData
        {
            get
            {
                return _data;
            }
        }

        /// <summary>
        /// Convert 2D index into the 1D of flatten array
        /// </summary>
        /// <param name="row">Cell index in a vertical direction</param>
        /// <param name="col">Cell index in a horizontal direction</param>
        /// <returns>1D index</returns>
        public int I1(int row, int col, bool constrain = false)
        {
            if (constrain)
            {
                // force constrain the requested cell indices to be in the available range
                row = Utils.Constrain<int>(row, 0, this.RowsCount - 1);
                col = Utils.Constrain<int>(col, 0, this.ColumnsCount - 1);
            }

            return this.ColumnsCount * row + col;
        }

        /// <summary>
        /// Convert 1D index of the flatten array to the 2D one 
        /// </summary>
        /// <param name="id1">1D index</param>
        /// <returns>A tuple of (row, col) indices</returns>
        public (int row, int col) I2(int id1, bool constrain = false)
        {
            int row = id1 / this.ColumnsCount; // integer division ! to find the row index
            int col = id1 - this.ColumnsCount * row; // the inverse transform of To1D

            if (constrain)
            {
                // force constrain the requested cell indices to be in the available range
                row = Utils.Constrain<int>(row, 0, this.RowsCount - 1);
                col = Utils.Constrain<int>(col, 0, this.ColumnsCount - 1);
            }

            return (row, col);
        }

        /// <summary>
        /// Checks whether requested cell is available in the data array
        /// </summary>
        /// <param name="row">Vertical cell index</param>
        /// <param name="col">Horizontal cell index</param>
        /// <returns></returns>
        public bool ContainsCell(int row, int col)
        {
            return Utils.InRange<int>(row, 0, this.RowsCount - 1) &&
                   Utils.InRange<int>(col, 0, this.ColumnsCount - 1);
        }

        /// <summary>
        /// Counts a number of cells of specific data value
        /// </summary>
        /// <param name="value">The data value to find to</param>
        /// <param name="operation">The comparison operator: cell[i] 'op' value</param>
        /// <returns></returns>
        public int CountOfCellsWithValue(int value, 
            Comparison.Operator operation = Comparison.Operator.EQUALS)
        {
            // uses the Language-Integrated Query (LINQ)
            // to aquire the data (from different sources) in a unified way
            switch (operation)
            {
                case Comparison.Operator.EQUALS:
                    return _data.Count(item => item == value);
                case Comparison.Operator.GREATER_THAN:
                    return _data.Count(item => item > value);
                case Comparison.Operator.LESS_THAN:
                    return _data.Count(item => item < value);
                case Comparison.Operator.GREATER_THAN_OR_EQUAL:
                    return _data.Count(item => item >= value);
                case Comparison.Operator.LESS_THAN_OR_EQUAL:
                    return _data.Count(item => item <= value);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Return all 1D indices of cells having the specific value
        /// </summary>
        /// <param name="value">The data value to find to</param>
        /// <param name="operation">The comparison operator: cell[i] 'op' value</param>
        /// <returns></returns>
        public int[] CellsWithValue(int value, 
            Comparison.Operator operation = Comparison.Operator.EQUALS)
        {
            // Use LINQ to return all cell IDs with the specific value
            return _data.Select((val, id) => new { Value = val, Index = id })
                  .Where(item =>
                  {
                      switch (operation)
                      {
                          case Comparison.Operator.EQUALS:
                              return item.Value == value;
                          case Comparison.Operator.GREATER_THAN:
                              return item.Value > value;
                          case Comparison.Operator.LESS_THAN:
                              return item.Value < value;
                          case Comparison.Operator.GREATER_THAN_OR_EQUAL:
                              return item.Value >= value;
                          case Comparison.Operator.LESS_THAN_OR_EQUAL:
                              return item.Value <= value;
                          default:
                              return false;
                      }
                  })
                  .Select(item => item.Index)
                  .ToArray();
        }

        /// <summary>
        /// Integer representation of a cell's filled status
        /// </summary>
        /// <param name="status">CLOSED, OPENED or </param>
        /// <returns></returns>
        public static int IS(Grid.Status status)
        {
            return Convert.ToInt32(status);
        }

        public override string ToString()
        {
            // define the palette
            //char[] pal = {'\u25A0', '\u25A1'}; // blank and filled square
            //char[] pal = {'\u2591', '\u2592', '\u2588'}; // vertical rectangle of full and medium shades 
            char[] pal = {'\u2591', '\u2592', '\u2588'}; // vertical rectangle of full and medium shades 

            StringBuilder sb = new StringBuilder();
            for (int ir = 0; ir < this.RowsCount; ir++)
            {
                for (int ic = 0; ic < this.ColumnsCount; ic++)
                {
                    // when using vertical symbols,
                    // its is needed to bouble them in a row to maintain aspect ratio
                    for (int repeat = 0; repeat < 2; repeat++)
                    {
                        sb.Append(pal[this[ir, ic]]);
                    }
                    //sb.Append(" ");
                }
                sb.AppendLine(); // Add a newline after each row
            }
            return sb.ToString();
        }
        #endregion

        #region Indexers
        /// <summary>
        /// 1D falttened indexer to acces the cell value defined by a single index
        /// </summary>
        /// <param name="id1">Cell index in a flatten array</param>
        /// <returns></returns>
        public int this[int id1]
        {
            // get accessor
            get
            {
                //return _data[Utils.Constrain<int>(id1, 0, this.CellsCount - 1)];
                return _data[id1];
            }
            // set accessor
            set
            {
                //_data[Utils.Constrain<int>(id1, 0, this.CellsCount - 1)] = value;
                _data[id1] = value;
            }
        }

        /// <summary>
        /// 2D indexer to acces the cell value defined by 2 indices
        /// </summary>
        /// <param name="row">Vertical index of a cell</param>
        /// <param name="col">Horizontal index of a cell</param>
        /// <returns></returns>
        public int this[int row, int col]
        {
            // get accessor
            get
            {
                return _data[I1(row, col)];
            }
            // set accessor
            set
            {
                _data[I1(row, col)] = value;
            }
        }
        #endregion

        #region Private methods

        #endregion
    }
}
