using UnityEngine;

public class CubeCellGroup 
{
    public CubeCell[,] cells;
   
    public CubeCellGroup(int[,] data,ColorType color)
    {
      
        cells = new CubeCell[data.GetLength(0), data.GetLength(1)];
        for (var i = 0; i < data.GetLength(0); i++)
        {
            for (var j = 0; j < data.GetLength(1); j++)
            {
                var cellData = new CubeCell(i, j,(CellType) data[i, j], (ColorType)color);
                cells[i, j] = cellData;
            }
        }
    }
}
