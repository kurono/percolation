/*
 * File: UnionFindQUWeighted.java
 * Description: An extension of the UnionFindQU
 * to achieve the weighted way of merging trees and their sub-branches,
 * to prevent non-controllable grwth of the tree structure.
 * Authors:
 *   - Ilya Tsivilskiy
 * Copyright: (c) 2023 Ilya Tsivilskiy
 * License: This file is licensed under the MIT License.
 */

package unionfind;

public class UnionFindQUWeighted extends UnionFindQU {

    protected int[] size; // size[i] in the number of elements in a subtree rooted at i
    protected int componentsCount;  // total number of clusters

    public UnionFindQUWeighted(int n, boolean debug) {
        super(n, debug);
        // initially each element's index is its own root:
        // _id is the ID of the root (parent) element

        componentsCount = n;
        size = new int[n];
        for (int i = 0; i < n; i++) {
            size[i] = 1;
        }
    }

    /**
     * Make the p-th element to be a child of q-th element's parent
     * @param p p-th element's ID
     * @param q q-th element's ID
     */
    @Override
    protected void callUnion(int p, int q) {
        // get the root elements of q-th and q-th elements
        int rootP = root(p);
        int rootQ = root(q);
        if (rootP == rootQ) return;

        // make smaller root point to larger one:
        // if p is smaller than q
        if (size[rootP] < size[rootQ]) {
            // then parent(p) = q
            setParent(rootP, rootQ);
            size[rootQ] += size[rootP];
        }
        // if p is greater than q
        else {
            // then parent(q) = p
            setParent(rootQ, rootP);
            size[rootP] += size[rootQ];
        }
        // decrease total number of trees/clusters/components
        componentsCount--;
    }
}
