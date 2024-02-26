/*
 * File: Utils.java
 * Description: Some tiny useful utilities for the data processing
 * Authors:
 *   - Ilya Tsivilskiy
 * Copyright: (c) 2023 Ilya Tsivilskiy
 * License: This file is licensed under the MIT License.
 */

package utils;

public class Utils {
    /**
     * Checks whether vMin <= v <= vMax
     * @param v    A variable to check its being in the range
     * @param vMin Left-hand inclusive bound of the range
     * @param vMax Right-hand inclusive bound of the range
     * @param <T>  The type of the variables to compare
     * @return true if v is in the range [vMin, vMax], false otherwise
     */
    public static <T extends Comparable<T>> boolean inRange(T v, T vMin, T vMax) {
        return v.compareTo(vMin) >= 0 && v.compareTo(vMax) <= 0;
    }

    /**
     * Constrains a value to be in the range [vMin, vMax], bounds inclusive
     * @param v    A value to constrain
     * @param vMin Left-hand inclusive bound of the range
     * @param vMax Right-hand inclusive bound of the range
     * @param <T>  The type of the value to constrain
     * @return The constrained value
     */
    public static <T extends Comparable<T>> T constrain(T v, T vMin, T vMax) {
        if (v.compareTo(vMin) < 0) {
            return vMin;
        } else if (v.compareTo(vMax) > 0) {
            return vMax;
        } else {
            return v;
        }
    }
}
