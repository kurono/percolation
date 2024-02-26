# percolation
The percolation problem solver in a 2D domain

**Description**

This code simulates the percolation of a fluid through a porous medium, where the porosity is defined on a 2D grid (refer to [Grid.cs](./cs/asd_2_wquf_apps/src/percolation/Grid.cs) class), and cell values may be either 0 ('closed' cell), 1 ('opened' pore), or 2 ('filled' by the fluid). The fluid penetrates the grid from its top side, and the system is considered percolated if one of the cells from the top side of the domain has a continuous path through the 'filled' grid's cells down to one of the cells of the bottom side of the domain.

The core class [PercolationSolver.cs](./cs/asd_2_wquf_apps/src/percolation/PercolationSolver.cs) randomly opens the remaining 'closed' cells of the grid and builds a dynamic connectivity structure between all opened cells connected to the top side of the domain. The connectivity is resolved using either [QuickFind](./cs/asd_2_wquf_apps/src/unionfind/UnionFind.cs), [QuickUnion](./cs/asd_2_wquf_apps/src/unionfind/UnionFindQU.cs) or [WeightedQuickUnion](./cs/asd_2_wquf_apps/src/unionfind/UnionFindQUWeighted.cs) algorithms implemented in the 'unionfind' package.

The source code is provided in both [C#](./cs/) and [Java](./java/) languages.
The compiled executables can be launched via Windows batch scripts: [run.bat for the C#-version](./cs/asd_2_wquf_apps/run.bat) or [runj.bat for the Java version](./java/asd_2_wquf_apps/runj.bat).

The code is partially parallelized using `Parallel.For` and `.AsParallel()` in C#, and via parallel streams in Java.

Although written in C#, the key concepts of this code are inspired by Robert Sedgewick's works.

**Demo**

If the program is run with the '-image' command line argument, it saves cell values at every iteration as PPM images into the 'saves/*.ppm' directory. 
Closed cells are black, opened cells are gray, and cells filled by the fluid are white.
When collected together, the frames look like this:

| 20 x 20: `-res 20` | 220 x 220: `-res 220` |
| :-------------: | :-------------:|
| ![Demo](./4readme/percolation_20.gif) | ![Demo](./4readme/percolation_220.gif) |

**Usage**

You can refer to the [run.bat](./cs/asd_2_wquf_apps/run.bat) batch file to launch the C# app from the Windows console terminal, or you can refer to the [runj.bat](./java/asd_2_wquf_apps/runj.bat) batch file to launch the Java JAR file, or you can launch the executable manually:

1) Windows:
| C# | Java |
| :-------------: | :-------------:|
| `asd_2_wquf_apps.exe -res 20 -console -image -ll` | `java -jar .\asd_2_wquf_apps.jar -res 20 -console -image -ll` |


2) Linux/Mac:
| C# | Java |
| :-------------: | :-------------:|
| `dotnet .\asd_2_wquf_apps.dll -res 20 -console -image -ll` | `java -jar .\asd_2_wquf_apps.jar -res 20 -console -image -ll` |

The command line arguments have the following meanings:
    `-res N` represents the grid resolution (its rows and columns count);
    
    `-console` specifies to write the grid data into the console in an ASCII-art manner;
    
    `-image` indicates to save the grid data into PPM files in the 'saves' directory;
	
    `-ll` try to launch this application on multiple processor threads.

So, if you want to print only the cell data to the console, type:
`asd_2_wquf_apps.exe -res 20 -console -ll`.
 If you want to save only the cell data as PPM images, type:
`asd_2_wquf_apps.exe -res 20 -image -ll`.

**License**

Published under the [MIT License](LICENSE).
