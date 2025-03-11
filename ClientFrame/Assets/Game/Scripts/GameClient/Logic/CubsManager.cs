using System.Collections.Generic;
using UnityEngine;

public class CubsManager : Singleton<CubsManager>
{
   
    public int[,] Data;
    public List<int[,]> dataTempList = new List<int[,]>();

    public void NewGame(int rows, int cols)
    {
        Data = new int[rows, cols];
        dataTempList = new List<int[,]>
        {
            T,
            Cube,
            Z,
            L,
            Line
        };
    }

    public int[,] RandomData()
    {
        var data = dataTempList[Random.Range(0, dataTempList.Count)];
        var randomRotate = Random.Range(0, 4);
        var tempData = LogicUtil.GetRotateData(data, randomRotate);
        return tempData;
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
