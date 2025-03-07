using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class Exdata64
{
    public List<long> Data = new List<long>();

    public void InitByBase()
    {
        for (var i = 0; i != 4; ++i)
        {
            Data.Add(0);
        }
    }
    private void SetMoreIndex(int nIndex)
    {
        if (Data.Count > nIndex)
        {
            return;
        }
        for (var i = Data.Count; i <= nIndex; i++)
        {
            Data.Add(0);
        }
        Player.Instance.ExData64Dirty = true;
    }

    public long GetExData(int nIndex)
    {
        if (Data.Count <= nIndex)
        {
            SetMoreIndex(nIndex);
        }
        return Data[nIndex];
    }
    public void SetExData(int nIndex, long nValue)
    {
        if (Data.Count <= nIndex)
        {
            // Logger.Error("SetExdata64 not find exdata {0}", nIndex);
            SetMoreIndex(nIndex);
        }
        Data[nIndex] = nValue;
        Player.Instance.ExData64Dirty = true;
    }
 
}