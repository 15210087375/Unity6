// using System.Collections.Generic;
// using UnityEngine;
// public enum RotateAngle
// {
//     Angle0 = 0,
//     Angle90 = 90,
//     Angle180 = 180,
//     Angle270 = 270,
// }
// public static class LogicUtil
// {
//    
//     //检测二维数组B 是否可以放入二维数组A的startX，startY位置 如果可以放入返回true 否则返回false
//     public static bool CheckInputB2A(int[,] a, int[,] b,int startX,int startY)
//     {
//         var rowsA = a.GetLength(0);
//         var colsA = a.GetLength(1);
//         var rowsB = b.GetLength(0);
//         var colsB = b.GetLength(1);
//         if(rowsB+ startX > rowsA || colsB + startY > colsA)
//         {
//             return false;
//         }
//         for (var i = 0; i < rowsB; i++)
//         {
//             for (var j = 0; j < colsB; j++)
//             {
//                 if (b[i, j] != 0 &&a[i + startX, j + startY] != 0)
//                 {
//                     return false;
//                 }
//             }
//         }
//         
//         return true;
//     }
//     //将B 放入A 时，A的哪些值会发生变化
//     public static List<CubeCell> GetInputB2AChangeData(CubeCell[,] a, CubeCell[,] b,int startX,int startY)
//     {
//         if (startX < 0 || startY < 0)
//         {
//             return null;
//         }
//         var list = new List<CubeCell>();
//         var rowsA = a.GetLength(0);
//         var colsA = a.GetLength(1);
//         var rowsB = b.GetLength(0);
//         var colsB = b.GetLength(1);
//         if(rowsB+ startX > rowsA || colsB + startY > colsA)
//         {
//             return null;
//         }
//         for (var i = 0; i < rowsB; i++)
//         {
//             for (var j = 0; j < colsB; j++)
//             {
//                 if (b[i, j].cellType != 0 &&a[i + startX, j + startY].cellType != 0)
//                 {
//                     return null;
//                 }
//
//                 if (b[i, j].cellType != 0)
//                 {
//                     list.Add(a[i + startX, j + startY]);
//                 }
//                 
//             }
//         }
//         
//         return list;
//     }
//     //使用前需确保检测过能放入
//     public static void InputB2A(int[,] a, int[,] b,int startX,int startY)
//     {
//         var rowsB = b.GetLength(0);
//         var colsB = b.GetLength(1);
//         for(var i = 0; i < rowsB; i++)
//         {
//             for(var j = 0; j < colsB; j++)
//             {
//                 if(b[i,j] == 1)
//                 {
//                     a[i + startX, j + startY] = 1;
//                 }
//             }
//         }
//     }
//
//     public static int[,] GetRotateData(int[,] a,RotateAngle rotate)
//     {
//         var rows = a.GetLength(0);
//         var cols = a.GetLength(1);
//         var b = rotate switch
//         {
//             RotateAngle.Angle0 => new int[rows, cols],
//             RotateAngle.Angle90 => new int[cols, rows],
//             RotateAngle.Angle180 => new int[rows, cols],
//             RotateAngle.Angle270 => new int[cols, rows],
//             _ => new int[rows, cols]
//         };
//
//         for (var i = 0; i < rows; i++)
//         {
//             for (var j = 0; j < cols; j++)
//             {
//                 switch (rotate)
//                 {
//                     case RotateAngle.Angle0:
//                         b[i, j] = a[i, j];
//                         break;
//                     case RotateAngle.Angle90:
//                         b[j, rows - 1 - i] = a[i, j];
//                         break;
//                     case RotateAngle.Angle180:
//                         b[rows - 1 - i, cols - 1 - j] = a[i, j];
//                         break;
//                     case RotateAngle.Angle270:
//                         b[cols - 1 - j, i] = a[i, j];
//                         break;
//                     default:
//                         b[i, j] = a[i, j];
//                         break;
//                 }
//             }
//         }
//         
//         return b;
//     }
// }
