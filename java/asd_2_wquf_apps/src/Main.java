/*
 * File: Main.java
 * Description: Main entry point of the Percolation simulation program
 * Authors:
 *   - Ilya Tsivilskiy
 * Copyright: (c) 2023 Ilya Tsivilskiy
 * License: This file is licensed under the MIT License.
 */

import io.PPMExporter;
import percolation.Comparison;
import percolation.Grid;
import percolation.PercolationSolver;
import utils.Logger;
import utils.Stopwatch;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;

public class Main {
    public static void main(String[] args) {
        Logger.writeLine("Start!\n");
        Stopwatch timer = new Stopwatch();

        Logger.writeLine("Solves the percolation problem",
                "on a 2D square grid. The fluid",
                "flows from the top to the bottom side.",
                "The grid is considered percolated",
                "if there is a continuous path for the fluid",
                "from the top side to the bottom side.",
                "The algorithm utilizes a UnionFind-like data structure",
                "to implement dynamic connectivity",
                "between the 'opened' grid cells.",
                "A 'closed' cell is marked in dark grey,",
                "an 'opened' cell is marked in light grey,",
                "and cells filled with fluid are white.\n",
                "Command-line arguments:",
                "\tGrid resolution: -res N",
                "\tWrite cell data to console: -console",
                "\tWrite cell data to PPM file: -image",
                "\tRun in multiple threads: -ll\n");

        // allow the unicode characters support
        System.setProperty("file.encoding", "UTF-8");

        // make 'saves' folder
        String savesDirName = "saves";
        String currentDirectory = System.getProperty("user.dir");
        String savesFolderPath = Paths.get(currentDirectory, savesDirName).toString();
        try {
            // Check if the 'saves' folder exists, if not, create it
            if (!Files.exists(Paths.get(savesFolderPath))) {
                Files.createDirectory(Paths.get(savesFolderPath));
            }
        } catch (IOException e) {
            Logger.write("Error creating 'saves' folder", e.getMessage(), "\n");
        }

        // define the program settings
        boolean writeToConsole = false; // write grid data to console
        boolean writeToImage = false; // write grid data to image
        int res = 12; // cells count in each direction
        int imageMinRes = 300; // minimal resolution of the image to save to
        boolean ll = false; // run on multiple processors

        // parse command-line arguments
        if (args.length > 0) {
            for (int i = 0; i < args.length; i++) {
                if (args[i].equals("-res") && i + 1 < args.length) {
                    try {
                        res = Integer.parseInt(args[i + 1]);
                    } catch (NumberFormatException ex) {
                        Logger.writeLine("Invalid resolution value. Using default!");
                    }
                    i++; // Skip the next argument
                } else if (args[i].equals("-console")) {
                    writeToConsole = true;
                } else if (args[i].equals("-image")) {
                    writeToImage = true;
                } else if (args[i].equals("-ll")) {
                    ll = true;
                }
            }
        }

        // perform the simulation
        try {
            Grid grid = new Grid(res, res);
            PercolationSolver sol = new PercolationSolver(grid, ll, true);
            int maxIter = grid.cellsCount();
            for (int iter = 0; iter < maxIter; iter++) {
                if (writeToConsole && (iter == 0)) {
                    Logger.writeLine("Initial state of the cells:");
                    Logger.write(grid.toString());
                    Logger.writeLine("------------------------------------");
                }
                // open a cell
                sol.openRandom(true);
                //sol.open(ids[iter, 0], ids[iter, 1]);
                // check all opened cell if they are accessible to fluid flow from the top side
                sol.updateCellsFilledStatus();
                // cells, which status > than 'closed' are either opened or opened-and-filled
                int filledCellsCount = grid.countOfCellsWithValue(
                        Grid.is(Grid.Status.CLOSED), Comparison.Operator.GREATER_THAN);
                Logger.write("Iteration:", iter,
                        ", Opened cells = ", filledCellsCount,
                        ", Porosity = ", 100 * filledCellsCount / grid.cellsCount(), "%, ",
                        sol.percolatesTotally() ? "Percolates!" : "Does not percolate", "\n");
                if (writeToConsole) {
                    Logger.write(grid.toString());
                }
                Logger.writeLine("------------------------------------");
                // save to image
                if (writeToImage) {
                    String fileName = Paths.get(savesDirName, String.format("%06d.ppm", iter)).toString();
                    PPMExporter.writeFile(grid.rawData(),
                            grid.rowsCount(), grid.columnsCount(), fileName,
                            Grid.is(Grid.Status.CLOSED),
                            Grid.is(Grid.Status.OPENED_AND_FILLED),
                            (res < imageMinRes) ? imageMinRes / res : 1);
                }
            }
        } catch (Exception e) {
            Logger.writeLine(e.getMessage());
        }

        Logger.writeLine("Ok!");

        Logger.write("Elapsed time =", timer.getElapsedTime(), "[s]\n");

        // Prevent the console window from closing
        Logger.writeLine("Press any key to exit...");
        try {
            System.in.read();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}