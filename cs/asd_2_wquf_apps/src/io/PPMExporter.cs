/*
 * File: PPMExporter.cs
 * Description: Writes pseudo 2D array of integer data to PPM image.
 * Authors:
 *   - Ilya Tsivilskiy
 * Copyright: (c) 2023 Ilya Tsivilskiy
 * License: This file is licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace asd_2_wquf_apps.src.io
{
    /// <summary>
    /// A minimal graphics file format writer
    /// </summary>
    internal static class PPMExporter
    {
        /// <summary>
        /// Writes a pseudo 2D array into the file
        /// </summary>
        /// <param name="cellData">1D array of pseudo 2D data</param>
        /// <param name="rows">Rows count</param>
        /// <param name="cols">Columns count</param>
        /// <param name="fileName">File path and name with *.PPM extension</param>
        /// <param name="minValue">Minimal value in the data array</param>
        /// <param name="maxValue">Maximal value in the data array</param>
        /// <param name="upScaleFactor">Zoom factor</param>
        public static void WriteFile(int[] cellData, int rows, int cols, string fileName,
            int minValue = 0, int maxValue = 1, int upScaleFactor = 1)
        {
            // Upscale dimensions
            int upscaledRows = rows * upScaleFactor;
            int upscaledCols = cols * upScaleFactor;

            // create a FileStream to write to the PPM file
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                int maxColorValue = 255;

                // write the PPM header
                writer.WriteLine("P3"); // PPM magic number
                writer.WriteLine($"{upscaledCols} {upscaledRows}"); // Upscaled image dimensions
                writer.WriteLine(maxColorValue); // Maximum color value

                // per-pixel write the image data:
                // loop over upscaled cells
                for (int row = 0; row < upscaledRows; row++)
                {
                    for (int col = 0; col < upscaledCols; col++)
                    {
                        // map upscaled pixel coordinates back to original grid coordinates
                        int origRow = row / upScaleFactor;
                        int origCol = col / upScaleFactor;

                        // extract the cell data value
                        int value = cellData[origRow * cols + origCol];

                        // normalize it to be in [0 ... 255]
                        int color = maxColorValue * (value - minValue) / (maxValue - minValue);

                        writer.Write($"{color} {color} {color} ");
                    }
                    writer.WriteLine(); // add a new line after each row
                }
            }
        }
    }
}
