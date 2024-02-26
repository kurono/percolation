/*
 * File: PPMExporter.java
 * Description: Writes pseudo 2D array of integer data to PPM image.
 * Authors:
 *   - Ilya Tsivilskiy
 * Copyright: (c) 2023 Ilya Tsivilskiy
 * License: This file is licensed under the MIT License.
 */

package io;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;

/**
 * A minimal graphics file format writer
 */
public class PPMExporter {

    /**
     * Writes a pseudo 2D array into the file
     *
     * @param cellData      1D array of pseudo 2D data
     * @param rows          Rows count
     * @param cols          Columns count
     * @param fileName      File path and name with *.PPM extension
     * @param minValue      Minimal value in the data array
     * @param maxValue      Maximal value in the data array
     * @param upScaleFactor Zoom factor
     */
    public static void writeFile(int[] cellData, int rows, int cols, String fileName,
                                 int minValue, int maxValue, int upScaleFactor) throws IOException {
        // Upscale dimensions
        int upscaledRows = rows * upScaleFactor;
        int upscaledCols = cols * upScaleFactor;

        // create a FileWriter to write to the PPM file
        try (BufferedWriter writer = new BufferedWriter(new FileWriter(fileName))) {
            int maxColorValue = 255;

            // write the PPM header
            writer.write("P3\n"); // PPM magic number
            writer.write(upscaledCols + " " + upscaledRows + "\n"); // Upscaled image dimensions
            writer.write(maxColorValue + "\n"); // Maximum color value

            // per-pixel write the image data:
            // loop over upscaled cells
            for (int row = 0; row < upscaledRows; row++) {
                for (int col = 0; col < upscaledCols; col++) {
                    // map upscaled pixel coordinates back to original grid coordinates
                    int origRow = row / upScaleFactor;
                    int origCol = col / upScaleFactor;

                    // extract the cell data value
                    int value = cellData[origRow * cols + origCol];

                    // normalize it to be in [0 ... 255]
                    int color = maxColorValue * (value - minValue) / (maxValue - minValue);

                    writer.write(color + " " + color + " " + color + " ");
                }
                writer.write("\n"); // add a new line after each row
            }
        }
    }
}
