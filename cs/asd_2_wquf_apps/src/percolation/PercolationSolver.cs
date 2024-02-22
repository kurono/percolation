/*
 * File: PercolationSover.cs
 * Description: A solver that uses the UnionFind-like data structures
 * to impement the dynamic connectivity between the cells of 
 * a pseudo 2D grid representing a porous medium that may or may not
 * percolate a fluid.
 * Authors:
 *   - Ilya Tsivilskiy
 * Copyright: (c) 2023 Ilya Tsivilskiy
 * License: This file is licensed under the MIT License.
 */

using asd_2_collunionfind.src.unionfind;
using asd_2_collunionfind.src.utils;
using asd_2_wquf_apps.src.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asd_2_wquf_apps.src.percolation
{
    internal class PercolationSolver
    {
        private Grid _grid; // pseudo 2D grid, where the cell values are: 0 - closed, 1 - opened
        private IUnionFind _connectivity; // checks, if the top side is connected to the bottom one
        private int _vTopID; // 1D index of a virtual cell connecting all top cells  
        private int _vBottomID; // 1D index of a virtual cell connecting all bottom cells
        private Random _rnd; // pseudo-random numbers generator
        private bool _debug;

        public PercolationSolver(Grid grid, bool debug = false)
        {
            // The grid will hold an 1D array of cell values,
            // all of them are closed ( = 0) at the first stage
            _grid = grid;
            int totalCells = _grid.CellsCount;

            // append two virtual nodes connecting the top and bottom layers of the grid
            _vTopID = totalCells;
            _vBottomID = totalCells + 1;

            // init Weighted QuickUnion data structures
            // to resolve the dynamic connectivity problem:
            // all cells of the grid and two more virtual ones
            //_connectivity = new UnionFind(totalCells + 2);
            _connectivity = new UnionFindQUWeighted(totalCells + 2);

            // init the randomizer at a class level
            // instead of within a method
            // to ensure variety in the generate
            _rnd = new Random();

            _debug = debug;
        }

        /// <summary>
        /// Open a specific cell of the 2D grid for the potential percolation
        /// </summary>
        /// <param name="row">Vertical index of a cell in a 2D grid</param>
        /// <param name="col">Horizontal index of a cell in a 2D grid</param>
        public void Open(int row, int col)
        {
            // do nothing, if a cell is opened already
            if (_IsOpened(row, col))
            {
                return;
            }

            // mark the selected cell as opened
            _grid[row, col] = Grid.StatusToInt(Grid.Status.OPENED);

            int currentCellID = _grid.To1D(row, col);

            // check the special cases,
            // if a cell is located in the top or the bottom side of a grid, 
            // then connect it to the one of the virtual nodes:
            if (row == 0)
            {
                // connect to the virtual top node
                _connectivity.Union(currentCellID, _vTopID);
            }
            else if (row == _grid.RowsCount - 1)
            {
                // connect to the virtual bottom node
                _connectivity.Union(currentCellID, _vBottomID);
            }

            /*
            // check if a fluid can percolate to the curent cell,
            // and update the cell status, if yes:
            if (this.PercolateUpToCell(currentCellID))
            {
                _grid[currentCellID] = Grid.StatusToInt(Grid.Status.OPENED_AND_FILLED);
            }
            */

            // connect current cell to the neighboring cells,
            // if they are opened:

            // init an array of relative 2D IDs of neighbor cells
            int[,] neighbours = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };
            // enchanced for loop over neighbor cells
            for (int inc = 0; inc < neighbours.GetLength(0); inc++)
            {
                int neighborRow = row + neighbours[inc, 0];
                int neighborCol = col + neighbours[inc, 1];

                // check whether the probing neighbor cell 
                // is within the grid row-col range
                if (Utils.InRange<int>(neighborRow, 0, _grid.RowsCount - 1) &&
                    Utils.InRange<int>(neighborCol, 0, _grid.ColumnsCount - 1))
                {
                    // convert it to 1D index 
                    int neighborCellID = _grid.To1D(neighborRow, neighborCol);
                    // connect the current cell to its opened neighbor cell
                    if (_IsOpened(neighborCellID))
                    {
                        _connectivity.Union(currentCellID, neighborCellID);
                    }
                }
            }
        }

        /// <summary>
        /// Open a randomly selected cell in a 2D grid
        /// <param name="considerAlreadyOpened">If true, randomly select from the cells that are not opened yet</param>
        /// </summary>
        public void OpenRandom(bool considerAlreadyOpened = false)
        {
            int randomRow = 0, randomCol = 0;
            if (considerAlreadyOpened)
            {
                // extract global IDs of the closed cells
                int[] closedCellIDs = _grid.CellsWithValue(
                    Grid.StatusToInt(Grid.Status.CLOSED), Comparison.Operator.EQUALS);

                // randomly select a number
                int randomSelect = _rnd.Next(0, closedCellIDs.Length);

                // marke the cell as 'opened'
                int selectedCellID = closedCellIDs[randomSelect];
                _grid[selectedCellID] = Grid.StatusToInt(Grid.Status.OPENED);

                // convert 1D index to the 2D ones
                (randomRow, randomCol) = _grid.To2D(selectedCellID);
            }
            else
            {
                randomRow = _rnd.Next(0, _grid.RowsCount); // exclusive the rhs value
                randomCol = _rnd.Next(0, _grid.ColumnsCount); // exclusive the rhs value 
            }

            if (_debug)
            {
                Logger.Write("Open a cell [", randomRow, ",", randomCol, "]\n");
            }
            this.Open(randomRow, randomCol);
        }

        /// <summary>
        /// Checks whether the grid percolates from any cell of the top layer to the current cell
        /// </summary>
        /// <param name="id1">1D index of a cell in a 2D grid</param>
        /// <returns></returns>
        public bool PercolateUpToCell(int id1)
        {
            return _connectivity.Connected(_vTopID, id1);
        }

        /// <summary>
        /// Checks whether the grid percolates from any cell of the top layer to the current cell
        /// </summary>
        /// <param name="row">Vertical index of a cell in a 2D grid</param>
        /// <param name="col">Horizontal index of a cell in a 2D grid</param>
        /// <returns></returns>
        public bool PercolateUpToCell(int row, int col)
        {
            return this.PercolateUpToCell(_grid.To1D(row, col));
        }

        /// <summary>
        /// Checks whether the grid percolates from any cell of the top layer 
        /// to the any cell of the bottom layer
        /// </summary>
        public bool PercolatesTotally
        {
            get
            {
                return _connectivity.Connected(_vTopID, _vBottomID);
            }
        }

        /// <summary>
        /// Check all opened cells if they are connected to the top side of the grid,
        /// and updates the cell value accordingly to be equal to Grid.Status.OPENED_AND_FILLED
        /// </summary>
        public void UpdateCellsFilledStatus()
        {
            int[] openedCellIDs = _grid.CellsWithValue(
                Grid.StatusToInt(Grid.Status.CLOSED), Comparison.Operator.GREATER_THAN);
            foreach (int id in openedCellIDs)
            {
                _grid[id] = Grid.StatusToInt(
                    this.PercolateUpToCell(id) ? Grid.Status.OPENED_AND_FILLED : Grid.Status.OPENED);
            }
        }

        /// <summary>
        /// Checks if the 1D-way-indiced cell is opened
        /// </summary>
        /// <param name="id1">1D index of a cell in a flatten array</param>
        /// <returns></returns>
        private bool _IsOpened(int id1)
        {
            // a cell is opened if its status > that closed
            return _grid[id1] > Grid.StatusToInt(Grid.Status.CLOSED);
        }

        /// <summary>
        /// Checks if the 2D-way-indiced cell is opened
        /// </summary>
        /// <param name="row">Vertical index of a cell in a 2D grid</param>
        /// <param name="col">Horizontal index of a cell in a 2D grid</param>
        /// <returns></returns>
        private bool _IsOpened(int row, int col)
        {
            return _IsOpened(_grid[row, col]);
        }
    }
}
