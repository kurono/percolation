/*
 * File: Comparison.java
 * Description: A set of math operators to compare cell data values in a Grid.
 * Authors:
 *   - Ilya Tsivilskiy
 * Copyright: (c) 2023 Ilya Tsivilskiy
 * License: This file is licensed under the MIT License.
 */

package percolation;

/**
 * A set of math operators to compare cell data values in a Grid.
 */
public class Comparison {

    /**
     * Enum representing different comparison operators.
     */
    public enum Operator {
        EQUALS,
        GREATER_THAN,
        LESS_THAN,
        GREATER_THAN_OR_EQUAL,
        LESS_THAN_OR_EQUAL
    }
}
