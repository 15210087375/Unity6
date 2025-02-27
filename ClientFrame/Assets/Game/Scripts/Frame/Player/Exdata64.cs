using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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
    public void SaveExdata64Data()
    {
        StringBuilder s = new StringBuilder();
        bool isfirst = true;
        foreach (var item in Player.Instance.ExData64.Data)
        {
            if (!isfirst)
            {
                s.Append("|");
            }
            s.Append(item);
            isfirst = false;
        }

        Logger.LogWarn(s.ToString());
        PlayerPrefs.SetString("Exdata64", s.ToString());
    }
    public void ReadExdata64Data()
    {
        var s = PlayerPrefs.GetString("Exdata64");
        if (string.IsNullOrEmpty(s))
        {
            return;
        }
        var t = s.Split('|');
        foreach (var item in t)
        {
            Player.Instance.ExData64.Data.Add(long.Parse(item));
        }
    }
}