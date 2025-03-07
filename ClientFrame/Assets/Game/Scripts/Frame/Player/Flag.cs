using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 标记位集合
/// </summary>
[Serializable]
public class BitFlag
{
    private readonly List<int> _flag;
    private int _count;

    /// <summary>
    /// 初始化BitFlag
    /// </summary>
    /// <param name="maxCount">位数</param>
    public BitFlag(int maxCount)
    {
        _flag = new List<int>();
        Init(maxCount);
    }

    /// <summary>
    /// 初始化BitFlag
    /// </summary>
    /// <param name="maxCount">位数</param>
    /// <param name="defaultValue">默认值，如果0则所有位都是0，如果是个数字，则每个int都是数字</param>
    public BitFlag(int maxCount, int defaultValue)
    {
        _flag = new List<int>();
        Init(maxCount, defaultValue);
    }
    private void Init(int maxCount, int defaultValue = 0)
    {
        _count = maxCount;
        var intCount = maxCount / 32;
        if (maxCount % 32 > 0)
        {
            ++intCount;
        }
        for (int i = 0; i != intCount; ++i)
        {
            _flag.Add(defaultValue);
        }
    }
    
    public BitFlag(int maxCount, List<int> items)
    {
        _count = maxCount;
        _flag = items;
        var intCount = maxCount / 32;
        if (maxCount % 32 > 0)
        {
            ++intCount;
        }
        var nowCount = items.Count;
        if (nowCount >= intCount) return;
        for (var i = nowCount + 1; i <= intCount; ++i)
        {
            _flag.Add(0);
        }
    }
    public void Init(List<int> itmes)
    {//用于从数据库数据初始化
        _flag.Clear();
        _flag.AddRange(itmes);
    }

    public List<int> GetData()
    {
        return _flag;
    }

    

    // 获取第nIndex位的标记
    public int GetFlag(int nIndex)
    {
        if (nIndex < 0 || nIndex >= _count)
        {
            return -1;
        }
        return ((_flag[nIndex / 32] >> (nIndex % 32)) & 1);
    }

    // 设置第nIndex位的标记
    public void SetFlag(int nIndex)
    {
        if (nIndex < 0 || nIndex >= _count)
        {
            return;
        }
        _flag[nIndex / 32] |= 1 << (nIndex % 32);
        Player.Instance.FlagDirty = true;
    }

    // 清除第nIndex位的标记
    public void CleanFlag(int nIndex)
    {
        if (nIndex < 0 || nIndex >= _count)
        {
            return;
        }
        if(GetFlag(nIndex) == 0)
        {
            return;
        }
        _flag[nIndex / 32] &= ~(1 << (nIndex % 32));
        Player.Instance.FlagDirty = true;
    }

    /// <summary>
    /// 清除所有标记
    /// </summary>
    public void ReSetAllFlag(bool dirty = false)
    {
        for (int i = 0; i < _flag.Count; i++)
        {
            if (dirty)
            {
                _flag[i] = -1;
            }
            else
            {
                _flag[i] = 0;
            }
        }
        Player.Instance.FlagDirty = true;
    }

    public bool IsDirty()
    {
        foreach (int i in _flag)
        {
            if (i != 0)
            {
                return true;
            }
        }
        return false;
    }

    
    
    
    
    
    public  bool ReadFlagData()
    {
        var s = PlayerPrefs.GetString("Flag");
        if (string.IsNullOrEmpty(s))
        {
            return false;
        }
        var t = s.Split('|');
        List<int> list = new List<int>();
        foreach (var item in t)
        {
            list.Add( int.Parse(item));
        }
        Player.Instance.Flag.Init(list);
        return true;
    }
    
    public void SaveFlagData()
    {
        var s = new StringBuilder();
        var isfirst = true;
        foreach (var item in Player.Instance.Flag.GetData())
        {
            if (!isfirst)
            {
                s.Append("|");
            }
            s.Append(item);
            isfirst = false;
        }

        Logger.LogWarn(s.ToString());
        PlayerPrefs.SetString("Flag", s.ToString());
    }
    
    public void ClearFlag()
    {
        PlayerPrefs.DeleteKey("Flag");
    }
}
