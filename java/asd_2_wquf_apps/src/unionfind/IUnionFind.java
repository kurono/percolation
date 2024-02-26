/*
 * File: IUnionFind.java
 * Description: An interface
 * for various UnionFind-like data structures
 * that describe connectivity within a set of elements.
 * Authors:
 *   - Ilya Tsivilskiy
 * Copyright: (c) 2023 Ilya Tsivilskiy
 * License: This file is licensed under the MIT License.
 */

package unionfind;

public interface IUnionFind {
    /**
     * Connects p-th element to q-th element
     * @param p Index of an element to be connected
     * @param q Index of an element to connect to
     */
    void union(int p, int q);

    /**
     * Checks whether two elements are connected
     * @param p Index of the element
     * @param q Index of another element
     * @return The status of connection
     */
    boolean connected(int p, int q);

    /**
     * Makes a string of current connectivity pattern
     * @return A string representation of the connectivity pattern
     */
    String toString();

    /**
     * Print out the data structure to console
     */
    void print();
}
