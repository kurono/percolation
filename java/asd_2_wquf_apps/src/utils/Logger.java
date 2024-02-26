/*
 * File: Logger.java
 * Description: Just a wrapper around the standard Stopwatch class,
 * implemented for the sake of portablity.
 * Authors:
 *   - Ilya Tsivilskiy
 * Copyright: (c) 2023 Ilya Tsivilskiy
 * License: This file is licensed under the MIT License.
 */

package utils;

/**
 * A helper class to write information to the system console
 */
public class Logger {

    public static final String DELIMITER = " ";

    /**
     * Write all arguments sequentially in a single line
     *
     * @param args A sequence of arguments
     */
    public static void write(Object... args) {
        for (int i = 0; i < args.length; i++) {
            System.out.print(args[i]);
            if (i < args.length - 1) {
                System.out.print(DELIMITER);
            }
        }
    }

    /**
     * Write all arguments starting from a new line
     *
     * @param args A sequence of arguments
     */
    public static void writeLine(Object... args) {
        for (Object arg : args) {
            System.out.println(arg);
        }
    }
}