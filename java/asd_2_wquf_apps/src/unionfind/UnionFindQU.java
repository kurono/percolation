/*
 * File: UnionFindQU.java
 * Description: An extension of the UnionFind
 * insted, implementing a QuickUnion algorithm of
 * dynamic connectivity of elements of the set.
 * It uses the 'path compression' approach in the 'Root' method
 * to shorten the pedigree.
 * Authors:
 *   - Ilya Tsivilskiy
 * Copyright: (c) 2023 Ilya Tsivilskiy
 * License: This file is licensed under the MIT License.
 */

package unionfind;

/**
 * An extension of the UnionFind, implementing a QuickUnion algorithm
 * of dynamic connectivity of elements of the set. It uses the 'path compression'
 * approach in the 'root' method to shorten the pedigree.
 */
public class UnionFindQU extends UnionFind {

    public UnionFindQU(int n, boolean debug) {
        super(n, debug);
        // initially each element's index is its own root:
        // _id is the ID of the root (parent) element
    }

    /**
     * Get the index of the i-th element's parent element
     * @param i A element to get its parent
     * @return Element's parent index
     */
    public int getParent(int i) {
        return id[i];
    }

    /**
     * Set the i-th element's parent
     * @param i An element to update
     * @param parentIndex Element's parent index
     */
    public void setParent(int i, int parentIndex) {
        id[i] = parentIndex;
    }

    /**
     * Changes element's parent pointers until reach the root
     * @param i Element's index
     * @return Index of the canonical root element
     */
    protected int root(int i) {
        // chain-like changing the roots
        // while the i-th element index is not an index of its root
        while (i != getParent(i)) {
            // to point every other node to its super-parent (the very main parent)
            setParent(i, getParent(getParent(i)));
            // jump to the root of i-th element
            i = getParent(i);
        }
        return i;
    }

    // make the p-th element to be a child of q-th element's parent
    // union(4, 3) = 4 is a child of 3's root (parent) element
    @Override
    protected void callUnion(int p, int q) {
        // get the root elements of q-th and q-th elements
        int rootP = root(p);
        int rootQ = root(q);
        if (rootP == rootQ) return;

        // make the root of p-th element to be a child of the q-ths parent
        setParent(rootP, rootQ);
    }

    @Override
    protected boolean callConnected(int p, int q) {
        // they have the same root (parent) element
        return root(p) == root(q);
    }
}
