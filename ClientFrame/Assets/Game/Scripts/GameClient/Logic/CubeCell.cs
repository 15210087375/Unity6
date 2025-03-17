using UnityEngine;

public enum CellType
{
    Empty = 0,      //空格子
    Normal = 1,     //可消除格子
    Normal2 = 2,    //可消除格子2
    
    Wooden = 3,     //木头格子
    
    Sealed = 10,    //封闭不可用的格子
}
public enum  ColorType
{
    Empty = 0,
    Red = 1,
    Green = 2,
    Blue = 3,
    Yellow = 4,
    Count 
}
public class CubeCell 
{
    public int X;
    public int Y;
    public CellType cellType;
    public ColorType colorType;

    public CubeCell(int x, int y, CellType value,ColorType color)
    {
        X = x;
        Y = y;
        cellType = value;
        colorType = color;
    }
    public CubeCell(int x,int y,int value,int color)
    {
        X = x;
        Y = y;
        cellType = (CellType)value;
        colorType = (ColorType)color;
    }
    
    public void PasteData(CubeCell cell)
    {
        cellType = cell.cellType;
        colorType = cell.colorType;
    }

    public void ClearData()
    {
        cellType = CellType.Empty;
        colorType = ColorType.Empty;
    }

    public bool IsMergeType()
    {
        return cellType == CellType.Normal || cellType == CellType.Normal2;
    }
}
