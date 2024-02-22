# percolation
The percolation problem solver in a 2D domain

**Description**

This code simulates the percolation of a fluid through a porous medium, where the porosity is defined on a 2D grid (refer to ![Grid.cs](./cs/asd_2_wquf_apps/src/percolation/Grid.cs) class), and cell values may be either 0 ('closed' cell), 1 ('opened' pore), or 2 ('filled' by the fluid). The fluid penetrates the grid from its top side, and the system is considered percolated if one of the cells from the top side of the domain has a continuous path through the 'filled' grid's cells down to one of the cells of the bottom side of the domain.

The core class ![PercolationSolver.cs](./cs/asd_2_wquf_apps/src/percolation/PercolationSolver.cs) randomly opens the remaining 'closed' cells of the grid and builds a dynamic connectivity structure between all opened cells connected to the top side of the domain. The connectivity is resolved using either ![QuickFind](./cs/asd_2_wquf_apps/src/unionfind/UnionFind.cs), ![QuickUnion](./cs/asd_2_wquf_apps/src/unionfind/UnionFindQU.cs) or ![WeightedQuickUnion](./cs/asd_2_wquf_apps/src/unionfind/UnionFindQUWeighted.cs) algorithms implemented in the 'unionfind' package.

Although written in C#, the key concepts of this code are inspired by Robert Sedgewick's works.

**Demo**

If the program is run with the '-image' command line argument, it saves cell values at every iteration as PPM images into the 'saves/*.ppm' directory. When collected together, the frames look like this:

![Demo](./4readme/percolation_20.gif)

Closed cells are black, opened cells are gray, and cells filled by the fluid are white.

**Usage**

You can refer to ![run.bat](./cs/asd_2_wquf_apps/run.bat) batch file to launch the app from the Windows console terminal, or launch the executable manually:

1) Windows:
`asd_2_wquf_apps.exe -res 20 -console -image`

2) Linux/Mac:
`dotnet asd_2_wquf_apps.dll -res 20 -console -image`

The command line arguments have the following meanings:
    `-res N` represents the grid resolution (its rows and columns count);
    `-console` specifies to write the grid data into the console in an ASCII-art manner;
    `-image` indicates to save the grid data into PPM files in the 'saves' directory.

So, if you want to print only the cell data to the console, type:
`asd_2_wquf_apps.exe -res 20 -console`.
 If you want to save only the cell data as PPM images, type:
`asd_2_wquf_apps.exe -res 20 -image`.

**License**

Published under the [MIT License](LICENSE).
