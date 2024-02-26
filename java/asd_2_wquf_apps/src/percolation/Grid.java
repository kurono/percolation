package percolation;

import utils.Utils;

import java.util.stream.IntStream;

/**
 * A wrapper over 1D array of integers to mimic a 2D grid-like array of cells
 * which may be either closed, opened, or filled by the fluid.
 * Authors:
 *   - Ilya Tsivilskiy
 * Copyright: (c) 2023 Ilya Tsivilskiy
 * License: This file is licensed under the MIT License.
 */
public class Grid {
    private final int nrows;
    private final int ncols;
    private int[] data;

    /**
     * Enum representing the status of a cell.
     */
    public enum Status {
        CLOSED,
        OPENED,
        OPENED_AND_FILLED
    }

    /**
     * Constructor for Grid.
     * @param nRows Vertical resolution, a number of cells in each row
     * @param nCols Horizontal resolution, a number of cells in each column
     */
    public Grid(int nRows, int nCols) {
        nrows = nRows;
        ncols = nCols;

        if (nrows <= 0 || ncols <= 0) {
            throw new IllegalArgumentException("Grid resolution should be positive!");
        }

        data = new int[this.cellsCount()];
    }

    /**
     * Getter for the number of rows.
     */
    public int rowsCount() {
        return nrows;
    }

    /**
     * Getter for the number of columns.
     */
    public int columnsCount() {
        return ncols;
    }

    /**
     * Getter for the total number of cells.
     */
    public int cellsCount() {
        return rowsCount() * columnsCount();
    }

    /**
     * Retrieves a horizontal slice of the grid.
     * @param row The row to slice over
     * @return An array of global 1D column cell indices
     */
    public int[] horizontalSlice(int row) {
        int[] columns = new int[columnsCount()];
        for (int ic = 0; ic < columnsCount(); ic++) {
            columns[ic] = i1(row, ic);
        }
        return columns;
    }

    /**
     * Getter for the raw data array.
     */
    public int[] rawData() {
        return data;
    }

    /**
     * Converts 2D indices into the 1D index of the flatten array.
     * @param row Cell index in a vertical direction
     * @param col Cell index in a horizontal direction
     * @return 1D index
     */
    public int i1(int row, int col) {
        if (row < 0 || row >= rowsCount() || col < 0 || col >= columnsCount()) {
            throw new IllegalArgumentException("Cell indices out of range!");
        }
        return columnsCount() * row + col;
    }

    /**
     * Converts 1D index of the flatten array to the 2D index.
     * @param id1 1D index
     * @return A tuple of (row, col) indices
     */
    public int[] i2(int id1) {
        if (id1 < 0 || id1 >= cellsCount()) {
            throw new IllegalArgumentException("Index out of range!");
        }
        int row = id1 / columnsCount();
        int col = id1 - columnsCount() * row;
        return new int[] {row, col};
    }

    /**
     * Checks whether the requested cell is available in the data array.
     * @param row Vertical cell index
     * @param col Horizontal cell index
     * @return true if the cell is contained in the grid, false otherwise
     */
    public boolean containsCell(int row, int col) {
        return Utils.inRange(row, 0, rowsCount() - 1) &&
                Utils.inRange(col, 0, columnsCount() - 1);
    }

    /**
     * Counts the number of cells with a specific data value.
     * @param value The data value to find
     * @param operation The comparison operator: cell[i] 'op' value
     * @return The count of cells with the specified value
     */
    public int countOfCellsWithValue(int value, Comparison.Operator operation) {
        return (int) IntStream.of(data)
                .parallel()
                .filter(item -> {
                    return switch (operation) {
                        case EQUALS -> item == value;
                        case GREATER_THAN -> item > value;
                        case LESS_THAN -> item < value;
                        case GREATER_THAN_OR_EQUAL -> item >= value;
                        case LESS_THAN_OR_EQUAL -> item <= value;
                        default -> false;
                    };
                })
                .count();
    }

    /**
     * Returns all 1D indices of cells having the specific value.
     * @param value The data value to find
     * @param operation The comparison operator: cell[i] 'op' value
     * @return An array of indices of cells with the specified value
     */
    public int[] cellsWithValue(int value, Comparison.Operator operation) {
        return IntStream.range(0, data.length)
                .parallel()
                .filter(id -> {
                    return switch (operation) {
                        case EQUALS -> data[id] == value;
                        case GREATER_THAN -> data[id] > value;
                        case LESS_THAN -> data[id] < value;
                        case GREATER_THAN_OR_EQUAL -> data[id] >= value;
                        case LESS_THAN_OR_EQUAL -> data[id] <= value;
                        default -> false;
                    };
                })
                .toArray();
    }

    /**
     * Returns the integer representation of a cell's filled status.
     * @param status The status of the cell
     * @return The integer representation of the status
     */
    public static int is(Status status) {
        return status.ordinal();
    }

    /**
     * Overrides the toString method to represent the grid as a string.
     */
    @Override
    public String toString() {
        char[] pal = {'\u2591', '\u2592', '\u2588'};
        StringBuilder sb = new StringBuilder();
        for (int ir = 0; ir < rowsCount(); ir++) {
            for (int ic = 0; ic < columnsCount(); ic++) {
                for (int repeat = 0; repeat < 2; repeat++) {
                    sb.append(pal[this.data[ir * columnsCount() + ic]]);
                }
            }
            sb.append(System.lineSeparator());
        }
        return sb.toString();
    }

    /**
     * Getter for the cell value at the specified 1D index.
     * @param id1 The 1D index of the cell
     * @return The value of the cell at the specified index
     */
    public int get(int id1) {
        return data[id1];
    }

    /**
     * Setter for the cell value at the specified 1D index.
     * @param id1 The 1D index of the cell
     * @param value The value to set for the cell
     */
    public void set(int id1, int value) {
        data[id1] = value;
    }

    /**
     * Getter for the cell value at the specified 2D indices.
     * @param row The row index of the cell
     * @param col The column index of the cell
     * @return The value of the cell at the specified indices
     */
    public int get(int row, int col) {
        return data[i1(row, col)];
    }

    /**
     * Setter for the cell value at the specified 2D indices.
     * @param row The row index of the cell
     * @param col The column index of the cell
     * @param value The value to set for the cell
     */
    public void set(int row, int col, int value) {
        data[i1(row, col)] = value;
    }
}
