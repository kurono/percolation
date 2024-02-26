/*
 * File: Stopwatch.java
 * Description: Just a wrapper around the standard Stopwatch class,
 * implemented for the sake of portablity.
 * Authors:
 *   - Ilya Tsivilskiy
 * Copyright: (c) 2023 Ilya Tsivilskiy
 * License: This file is licensed under the MIT License.
 */

package utils;

public class Stopwatch {
    private long start;

    /**
     * Initializes a new stopwatch
     */
    public Stopwatch() {
        reset();
    }

    /**
     * Resets the time stamp
     */
    public void reset() {
        this.start = Stopwatch.now();
    }

    /**
     * Returns the elapsed time [s] since the stopwatch was reset
     * @return Elapsed time [s]
     */
    public double getElapsedTime() {
        return (Stopwatch.now() - start) / 1000.0;
    }

    private static long now() {
        return System.currentTimeMillis();
    }
}
