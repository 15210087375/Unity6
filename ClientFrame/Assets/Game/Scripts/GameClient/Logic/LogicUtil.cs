using UnityEngine;

public static class LogicUtil
{
   
    //检测二维数组B 是否可以放入二维数组A的startX，startY位置 如果可以放入则放入并返回true 否则返回false
    public static bool CheckDataInPlace(ref int[,] a, int[,] b,int startX,int startY,bool input = false)
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

        if (input)
        {
            for(var i = 0; i < rowsB; i++)
            {
                for(var j = 0; j < colsB; j++)
                {
                    if(b[i,j] == 1)
                    {
                        a[i + startX, j + startY] = 2;
                    }
                }
            }
        }
        return true;
    }

    public static int[,] GetRotateData(int[,] a,int rotate)
    {

        int[,] b=new int[3,3];
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                b[i, j] = rotate switch
                {
                    1 => a[2 - j, i],       //90° 右旋
                    2 => a[2 - i, 2 - j],   //180° 反转
                    3 => a[j, 2 - i],       //270° 左旋
                    _ => a[i, j]
                };
            }

        }
        return b;
    }
}
