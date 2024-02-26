package percolation;

import unionfind.IUnionFind;
import unionfind.UnionFindQUWeighted;
import utils.Logger;

import java.util.Random;
import java.util.concurrent.ThreadLocalRandom;
import java.util.function.Function;
import java.util.stream.IntStream;

/**
 * A solver that uses the UnionFind-like data structures
 * to implement the dynamic connectivity between the cells of
 * a pseudo 2D grid representing a porous medium that may or may not
 * percolate a fluid.
 * Authors:
 *   - Ilya Tsivilskiy
 * Copyright: (c) 2023 Ilya Tsivilskiy
 * License: This file is licensed under the MIT License.
 */
public class PercolationSolver {
    private final Grid grid;
    private final IUnionFind connectivity;
    private final int vTopID;
    private final int vBottomID;
    private final Random rnd;
    private final boolean ll;
    private final boolean debug;

    public PercolationSolver(Grid grid, boolean ll, boolean debug) {
        this.grid = grid;
        int totalCells = grid.cellsCount();
        this.vTopID = totalCells;
        this.vBottomID = totalCells + 1;
        this.connectivity = new UnionFindQUWeighted(totalCells + 2, false);
        this.rnd = ThreadLocalRandom.current();
        this.ll = ll;
        this.debug = debug;
    }

    /**
     * Open a specific cell of the 2D grid for the potential percolation
     * @param row Vertical index of a cell in a 2D grid
     * @param col Horizontal index of a cell in a 2D grid
     */
    public void open(int row, int col) {
        if (isOpened(row, col)) {
            return;
        }

        grid.set(row, col, Grid.is(Grid.Status.OPENED));

        int currentCellID = grid.i1(row, col);

        if (row == 0) {
            connectivity.union(currentCellID, vTopID);
        } else if (row == grid.rowsCount() - 1) {
            connectivity.union(currentCellID, vBottomID);
        }

        int[][] neighbours = { {-1, 0}, {1, 0}, {0, -1}, {0, 1} };
        for (int[] neighbour : neighbours) {
            int neighborRow = row + neighbour[0];
            int neighborCol = col + neighbour[1];
            if (grid.containsCell(neighborRow, neighborCol)) {
                int neighborCellID = grid.i1(neighborRow, neighborCol);
                if (isOpened(neighborCellID)) {
                    connectivity.union(currentCellID, neighborCellID);
                }
            }
        }
    }

    /**
     * Open a randomly selected cell in a 2D grid
     * @param selectFromClosed If true, randomly select from the cells that are not opened yet
     */
    public void openRandom(boolean selectFromClosed) {
        int randomRow, randomCol;
        if (selectFromClosed) {
            int[] closedCells = grid.cellsWithValue(
                    Grid.is(Grid.Status.CLOSED), Comparison.Operator.EQUALS);
            int randomSelect = rnd.nextInt(closedCells.length);
            int selectedCellID = closedCells[randomSelect];
            int[] indices = grid.i2(selectedCellID);
            randomRow = indices[0];
            randomCol = indices[1];
        } else {
            randomRow = rnd.nextInt(grid.rowsCount());
            randomCol = rnd.nextInt(grid.columnsCount());
        }

        if (debug) {
            Logger.write("Open a cell [", randomRow, ",", randomCol, "]\n");
        }

        open(randomRow, randomCol);
    }

    /**
     * Checks whether the grid percolates from any cell of the top layer to the current cell
     * @param id1 1D index of a cell in a 2D grid
     * @return True if the grid percolates from any cell of the top layer to the current cell, false otherwise
     */
    public boolean percolatesUpToCell(int id1) {
        return connectivity.connected(vTopID, id1);
    }

    /**
     * Checks whether the grid percolates from any cell of the top layer to the current cell
     * @param row Vertical index of a cell in a 2D grid
     * @param col Horizontal index of a cell in a 2D grid
     * @return True if the grid percolates from any cell of the top layer to the current cell, false otherwise
     */
    public boolean percolateUpToCell(int row, int col) {
        return percolatesUpToCell(grid.i1(row, col));
    }

    /**
     * Checks whether the grid percolates from any cell of the top layer to any cell of the bottom layer
     * @return True if the grid percolates from any cell of the top layer to any cell of the bottom layer, false otherwise
     */
    public boolean percolatesTotally() {
        return connectivity.connected(vTopID, vBottomID);
    }

    /**
     * Check all opened cells if they are connected to the top side of the grid,
     * and updates the cell value accordingly to be equal to Grid.Status.OPENED_AND_FILLED
     */
    public void updateCellsFilledStatus() {
        /*int[] openedCellIDs = grid.cellsWithValue(
                Grid.is(Grid.Status.OPENED), Comparison.Operator.GREATER_THAN);
        for (int id : openedCellIDs) {
            grid.set(id,
                    percolatesUpToCell(id)
                            ? Grid.is(Grid.Status.OPENED_AND_FILLED)
                            : Grid.is(Grid.Status.OPENED));
        }*/

        Function<Integer, Integer> cellStatus = i -> isOpened(i)
                ? (percolatesUpToCell(i)
                        ? Grid.is(Grid.Status.OPENED_AND_FILLED)
                        : Grid.is(Grid.Status.OPENED))
                : Grid.is(Grid.Status.CLOSED);

        if (this.ll) {
            IntStream.range(0, grid.cellsCount()).parallel().forEach(i -> {
                grid.set(i, cellStatus.apply(i));
            });
        } else {
            for (int i = 0; i < grid.cellsCount(); i++) {
                grid.set(i, cellStatus.apply(i));
            }
        }
    }

    /**
     * Checks if the 1D-way-indiced cell is potentially opened to fluid
     * @param id1 1D index of a cell in a flatten array
     * @return True if the cell is opened, false otherwise
     */
    private boolean isOpened(int id1) {
        return grid.get(id1) > Grid.is(Grid.Status.CLOSED);
    }

    /**
     * Checks if the 2D-way-indiced cell is opened
     * @param row Vertical index of a cell in a 2D grid
     * @param col Horizontal index of a cell in a 2D grid
     * @return True if the cell is opened, false otherwise
     */
    private boolean isOpened(int row, int col) {
        return isOpened(grid.i1(row, col));
    }
}
