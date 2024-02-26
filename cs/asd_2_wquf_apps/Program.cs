/*
 * File: Program.cs
 * Description: Main entry point of the program
 * Authors:
 *   - Ilya Tsivilskiy
 * Copyright: (c) 2023 Ilya Tsivilskiy
 * License: This file is licensed under the MIT License.
 */

// See https://aka.ms/new-console-template for more information
using asd_2_collunionfind.src.unionfind;
using asd_2_collunionfind.src.utils;
using asd_2_wquf_apps.src.io;
using asd_2_wquf_apps.src.percolation;

Logger.WriteLine("Start!\n");
Logger.WriteLine("Solves the percolation problem",
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
                 "\tGrid resoluition: -res N",
                 "\tWrite cell data to console: -console",
                 "\tWrite cell data to PPM file: -image\n");

// allow the unicode characters support
Console.OutputEncoding = System.Text.Encoding.UTF8;

// make 'saves' folder
string savesDirName = "saves";
string currentDirectory = Directory.GetCurrentDirectory();
string savesFolderPath = Path.Combine(currentDirectory, savesDirName);
try
{
    // Check if the 'saves' folder exists, if not, create it
    if (!Directory.Exists(savesFolderPath))
    {
        Directory.CreateDirectory(savesFolderPath);
    }
}
catch (Exception e)
{
    Logger.Write("Error creating 'saves' folder", e.Message, "\n");
}

// define the program settings
bool writeToConsole = false; // write grid data to console
bool writeToImage = false; // write grid data to image
int res = 12; // cells count in each direction
int imageMinRes = 300; // minimal resolution of the image to save to
bool ll = false; // run on multiple processors

// parse command-line arguments
if (args.Length > 0)
{
    for (int i = 0; i < args.Length; i++)
    {
        if (args[i] == "-res" && i + 1 < args.Length)
        {
            if (!int.TryParse(args[i + 1], out res))
            {
                Logger.WriteLine("Invalid resolution value. Using default!");
            }
            i++; // Skip the next argument
        }
        else if (args[i] == "-console")
        {
            writeToConsole = true;
        }
        else if (args[i] == "-image")
        {
            writeToImage = true;
        }
        else if (args[i] == "-ll")
        {
            ll = true;            
        }
    }
}

// perform the simulation
try
{
    Grid grid = new Grid(res, res);
    // no need to explicitly specufy the 'ref', because
    // when passing an object as an argument to a method or constructor,
    // it's always passed by reference for reference data types,
    // so the 'sol' will change the actual cellStatusData
    PercolationSolver sol = new PercolationSolver(grid, ll, true);
    int maxIter = grid.CellsCount;
    //int[,] ids = { {1, 1}, {0, 2}, {1, 2}, {2, 3}, {2, 2} };
    //maxIter = ids.GetLength(0);
    for (int iter = 0; iter < maxIter; iter++)
    {
        if (writeToConsole && (iter == 0))
        {
            Logger.WriteLine("Initial state of the cells:");
            Logger.Write(grid.ToString());
            Logger.WriteLine("------------------------------------");
        }
        // open a cell
        sol.OpenRandom(true);
        //sol.Open(ids[iter, 0], ids[iter, 1]);
        // check all opened cell if they are accessible to fluid flow from the top side
        sol.UpdateCellsFilledStatus();
        // cells, which status > than 'closed' are either opened or opened-and-filled
        int filledCellsCount = grid.CountOfCellsWithValue(
            Grid.IS(Grid.Status.CLOSED), Comparison.Operator.GREATER_THAN);
        Logger.Write("Iteration:", iter,
                     ", Opened cells = ", filledCellsCount,
                     ", Porosity = ", 100 * filledCellsCount / grid.CellsCount, "%, ",
                     sol.PercolatesTotally ? "Percolates!" : "Does not percolate", "\n");
        if (writeToConsole)
        {
            Logger.Write(grid.ToString());
        }
        Logger.WriteLine("------------------------------------");
        // save to image
        if (writeToImage)
        {
            string fileName = $"{savesDirName}//{iter.ToString("D6")}.ppm";
            PPMExporter.WriteFile(grid.RawData, 
                grid.RowsCount, grid.ColumnsCount, fileName,
                Grid.IS(Grid.Status.CLOSED), 
                Grid.IS(Grid.Status.OPENED_AND_FILLED),
                (res < imageMinRes) ? imageMinRes / res : 1);
        }
    }
}
catch (Exception e)
{
    Logger.WriteLine(e.Message);
}

Logger.WriteLine("Ok!");

// Prevent the console window from closing
if (!writeToConsole)
{
    Logger.WriteLine("Press any key to exit...");
    Console.ReadKey();
}


