using System.Collections.Generic;
using UnityEngine;

public partial class CubsManager : Singleton<CubsManager>
{
   

    private List<int[,]> _dataTempList = new List<int[,]>();
    public CubeCell[,] Cells;
    public List<CubeCellGroup> DragGroups;
    
    private int _rows;
    private int _cols;
    public void NewGame(int rows, int cols)
    {
       
        _dataTempList = new List<int[,]>
        {
            T,
            Cube,
            Z,
            L,
            Line
        };
        
        _rows = rows;
        _cols = cols;
        GeneratorCells(rows, cols);
        GeneratorDragCells();

    }

    private void GeneratorCells(int rows, int cols)
    {
        var data = new int[rows, cols];
        Cells = new CubeCell[rows, cols];
        for(var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                var cellData = new CubeCell(i,j,data[i,j],0);
                Cells[i, j] = cellData;
            }
        }

    }
    public void GeneratorDragCells()
    {
        DragGroups=new List<CubeCellGroup>();
        for (var i = 0; i < 3; i++)
        {
            DragGroups.Add(GeneratorGroup());
        }
    }


    private readonly int[,] T = new int[,]
    {
        {1,1,1},
        {0,1,0},
      
    };
    private readonly int[,] Cube = new int[,]
    {
        {1,1},
        {1,1},
    };
    private readonly int[,] Z = new int[,]
    {
        {1,1,0},
        {0,1,1},
    };
    private readonly int[,] L = new int[,]
    {
        {1,1,1},
        {1,0,0},
    };
    private readonly int[,] Line = new int[,]
    {
        {1},
        {1},
        {1},
        {1},
    };
}
