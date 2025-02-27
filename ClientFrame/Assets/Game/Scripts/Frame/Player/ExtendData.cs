using System;
using System.Collections.Generic;
using System.Text;
using cfg.Common;
using UnityEngine;

public class ExData
{
    public List<int> Data = new List<int>();
    private ExtendData _table;

    private ExtendData Record
    {
        get => _table = TableManager.Instance.Tables.ExtendData;
        set => _table = value;
    }

    public void InitByBase()
    {
        for (var i = 0; i < Record.DataList.Count; ++i)
        {
            
            var tbExData = Record.GetOrDefault(i);
            if (tbExData != null)
            {
                Data.Add(tbExData.InitValue);
                continue;
            }
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
            var tbExData = Record.Get(i);
            if (tbExData != null)
            {
                Data.Add(tbExData.InitValue);
                continue;
            }
            Data.Add(0);
        }
        Player.Instance.ExDataDirty = true;
    }

    public int GetExData(int nIndex)
    {
        if (Data.Count <= nIndex)
        {
            SetMoreIndex(nIndex);
        }
        return Data[nIndex];
    }
    public void SetExData(int nIndex, int nValue)
    {
        if (Data.Count <= nIndex)
        {
            SetMoreIndex(nIndex);
        }
        EventCenter.Broadcast(EventCetnerType.ExDataValueChange, nIndex, nValue);
        Data[nIndex] = nValue;
        Player.Instance.ExDataDirty = true;
    }
    public void ResetExData(int nIndex)
    {
        var tbExData = Record.Get(nIndex);
        SetExData(nIndex, tbExData.InitValue);
    }
    
    
    public void SaveExdataData()
    {
        var s = new StringBuilder();
        var isfirst = true;
        foreach (var item in Player.Instance.ExData.Data)
        {
            if (!isfirst)
            {
                s.Append("|");
            }
            s.Append(item);
            isfirst = false;
        }

        Logger.LogWarn(s.ToString());
        PlayerPrefs.SetString("Exdata", s.ToString());
    }
    public void ReadExdataData()
    {
        var s = PlayerPrefs.GetString("Exdata");
        if(string.IsNullOrEmpty(s))
        {
            return;
        }
        Player.Instance.ExData.Data.Clear();
        var t = s.Split('|');
        foreach (var item in t)
        {
            Player.Instance.ExData.Data.Add(int.Parse(item));
        }
    }

    public static void ClearExData()
    {
        PlayerPrefs.DeleteKey("Exdata");
    }
}