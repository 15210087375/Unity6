using System.Collections.Generic;
using UnityEngine;

public partial class CubsManager
{
     //检测二维数组B 是否可以放入二维数组A的startX，startY位置 如果可以放入返回true 否则返回false
    public bool CheckInputB2A(int[,] a, int[,] b,int startX,int startY)
    {
        var rowsA = a.GetLength(0);
        var colsA = a.GetLength(1);
        var rowsB = b.GetLength(0);
        var colsB = b.GetLength(1);
        if(rowsB+ startX > rowsA || colsB + startY > colsA)
        {
            return false;
        }
        for (var i = 0; i < rowsB; i++)
        {
            for (var j = 0; j < colsB; j++)
            {
                if (b[i, j] != 0 &&a[i + startX, j + startY] != 0)
                {
                    return false;
                }
            }
        }
        
        return true;
    }
    public bool CheckInputB2A(CubeCell[,] a, CubeCell[,] b,int startX,int startY)
    {
        var rowsA = a.GetLength(0);
        var colsA = a.GetLength(1);
        var rowsB = b.GetLength(0);
        var colsB = b.GetLength(1);
        if(rowsB+ startX > rowsA || colsB + startY > colsA)
        {
            return false;
        }
        for (var i = 0; i < rowsB; i++)
        {
            for (var j = 0; j < colsB; j++)
            {
                if (b[i, j].cellType != 0 &&a[i + startX, j + startY].cellType != 0)
                {
                    return false;
                }
            }
        }
        
        return true;
    }
    public List<CubeCell> GetInputGroupChangeData(CubeCellGroup group, int startX, int startY)
    {
        return GetInputB2AChangeData(Cells, group.cells, startX, startY);
    }
    //将B 放入A 时，哪些值会导致A发生变化
    private List<CubeCell> GetInputB2AChangeData(CubeCell[,] a, CubeCell[,] b,int startX,int startY)
    {
        if (startX < 0 || startY < 0)
        {
            return null;
        }
        var list = new List<CubeCell>();
        var rowsA = a.GetLength(0);
        var colsA = a.GetLength(1);
        var rowsB = b.GetLength(0);
        var colsB = b.GetLength(1);
        if(rowsB+ startX > rowsA || colsB + startY > colsA)
        {
            return null;
        }
        for (var i = 0; i < rowsB; i++)
        {
            for (var j = 0; j < colsB; j++)
            {
                if (b[i, j].cellType != 0 &&a[i + startX, j + startY].cellType != 0)
                {
                    return null;
                }

                if (b[i, j].cellType != 0)
                {
                    list.Add(b[i,j]);
                }
                
            }
        }
        
        return list;
    }

    public void InputGroup(CubeCellGroup group, int startX, int startY)
    {
        InputB2A(Cells, group.cells, startX, startY);
    }
    //使用前需确保检测过能放入
    private void InputB2A(CubeCell[,] a, CubeCell[,] b,int startX,int startY)
    {
        var rowsB = b.GetLength(0);
        var colsB = b.GetLength(1);
        for(var i = 0; i < rowsB; i++)
        {
            for(var j = 0; j < colsB; j++)
            {
                if(b[i,j].cellType == CellType.Normal)
                {
                    a[i + startX, j + startY].PasteData(b[i,j]);
                }
            }
        }
    }

    private int[,] GetRotateData(int[,] a,RotateAngle rotate)
    {
        var rows = a.GetLength(0);
        var cols = a.GetLength(1);
        var b = rotate switch
        {
            RotateAngle.Angle0 => new int[rows, cols],
            RotateAngle.Angle90 => new int[cols, rows],
            RotateAngle.Angle180 => new int[rows, cols],
            RotateAngle.Angle270 => new int[cols, rows],
            _ => new int[rows, cols]
        };
        Debug.LogError("偏转"+rotate);
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                switch (rotate)
                {
                    case RotateAngle.Angle0:
                        b[i, j] = a[i, j];
                        break;
                    case RotateAngle.Angle90:
                        b[j, rows - 1 - i] = a[i, j];
                        break;
                    case RotateAngle.Angle180:
                        b[rows - 1 - i, cols - 1 - j] = a[i, j];
                        break;
                    case RotateAngle.Angle270:
                        b[cols - 1 - j, i] = a[i, j];
                        break;
                    default:
                        b[i, j] = a[i, j];
                        break;
                }
            }
        }
        
        return b;
    }
    public List<CubeCell> ClearFullLine(int startX,int startY,int row,int col)
    {
        var list = CheckFullLine(startX, startY, row, col);
        foreach (var cell in list)
        {
            cell.ClearData();
        }
        return list;
    }
    private List<CubeCell> CheckFullLine(int startX,int startY,int width,int length)
    {
        Debug.Log($"检测消除 初始格子({startX},{startY}),行{width},列{length}   数据行{_rows},列{_cols}");
        var list = new List<CubeCell>();
        for (var i = startX; i < startX+width; i++)
        {
            var isFull = true;
            for (var j = 0; j < _cols; j++)
            {
                Debug.Log($"检测格子({i},{j})");
                if (!Cells[i, j].IsMergeType())
                {
                    isFull = false;
                }
            }

            if (!isFull) continue;
            for (var j = 0; j < _cols; j++)
            {
                list.Add(Cells[i, j]);
            }
        }
        
        for(var j = startY; j < startY+length; j++)
        {
            var isFull = true;
            for (var i = 0; i < _rows; i++)
            {
                Debug.Log($"检测格子({i},{j})");
                if (!Cells[i, j].IsMergeType())
                {
                    isFull = false;
                }
            }
            if (!isFull) continue;
            for (var i = 0; i < _rows; i++)
            {
                if(list.Contains(Cells[i, j]))
                {
                    continue;
                }
                list.Add(Cells[i, j]);
            }
        }
        
        return list;
    }

    
    public (int x,int y,bool over) CheckGameOver()
    {
        
        for (var i = 0; i < DragGroups.Count; i++)
        {
            var cells = DragGroups[i].cells;
            var rowB = cells.GetLength(0);
            var colB = cells.GetLength(1);
            
            for (var r = 0; r < _rows-rowB; r++)
            {
                for (var c = 0; c < _cols-colB; c++)
                {
                    if (CheckInputB2A(Cells, DragGroups[i].cells, r, c) == true)
                    {
                        return (r,c,false);
                    }
                }
            }
        }

        return (-1,-1,true);

    }
}
