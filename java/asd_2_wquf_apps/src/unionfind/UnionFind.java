/*
 * File: UnionFind.java
 * Description: A base class implementing the QuickFind algorithm of
 * dynamic connectivity of elements of the set.
 * Authors:
 *   - Ilya Tsivilskiy
 * Copyright: (c) 2023 Ilya Tsivilskiy
 * License: This file is licensed under the MIT License.
 */

package unionfind;
import utils.Logger;

public class UnionFind implements IUnionFind {
    protected int[] id; // ID of a cluster that contains the connected elements
    protected boolean debug;

    /**
     * Create a UnionFind data structure
     * @param n     Total number of elements in collection
     * @param debug Print out the connectivity info after each operation
     */
    public UnionFind(int n, boolean debug) {
        // Initialize an array of indices
        id = new int[n];
        this.debug = debug;

        // Fill up the array:
        // At first, each element's id equals its actual index,
        // so all elements form an individual own cluster
        for (int i = 0; i < n; i++) {
            id[i] = i;
        }
    }

    @Override
    public void union(int p, int q) {
        if (debug) {
            Logger.writeLine(String.format("union(%d, %d):", p, q));
            Logger.write("before:\t");
            print();
        }

        // No need to connect the elements,
        // if they are already connected
        if (connected(p, q)) {
            return;
        }

        callUnion(p, q);

        if (debug) {
            Logger.write("after:\t");
            print();
            Logger.writeLine();
        }
    }

    @Override
    public boolean connected(int p, int q) {
        // They belong to the same cluster
        boolean result = callConnected(p, q);
        if (debug) {
            Logger.writeLine(String.format("connected(%d, %d) = %b", p, q, result));
        }
        return result;
    }

    /**
     * The "core" of the "union" method, connects two elements
     * @param p
     * @param q
     */
    protected void callUnion(int p, int q) {
        int pCluster = id[p]; // Original p-th id
        int qCluster = id[q]; // Original q-th id
        // Chain-style change of ids:
        // Recursively connect the elements
        for (int i = 0; i < id.length; i++) {
            // If i-th element belongs to p-th cluster
            if (id[i] == pCluster) {
                // Connect it to the q-th cluster
                id[i] = qCluster;
            }
        }
    }

    /**
     * The "core" of the "connected" method, checks whether two elements are connected
     * @param p
     * @param q
     * @return
     */
    protected boolean callConnected(int p, int q) {
        return id[p] == id[q];
    }

    @Override
    public String toString() {
        StringBuilder str = new StringBuilder();
        for (int anId : id) {
            str.append(anId).append(" ");
        }
        return str.toString();
    }

    @Override
    public void print() {
        Logger.writeLine(this.toString());
    }
}
