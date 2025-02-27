using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public partial class PlayerInterFace
{
   
   
    // public static bool CheckCondition(int conId)
    // {
    //     if(conId == -1)
    //     {
    //         return true;
    //     }
    //     var tbCondition = Table.GetConditionTable(conId);
    //     if(tbCondition == null)
    //     {
    //         Logger.Error("CheckCondition not find Id={0}", conId);
    //         return false;
    //     }
    //     if(tbCondition.FlagType == 0)
    //     {//必须条件均满足
    //         if (tbCondition.TrueFlag[0] != -1)
    //         {
    //             if (Player.Instance.mFlag.GetFlag(tbCondition.TrueFlag[0]) != 1)
    //             {
    //                 return false;
    //             }
    //         }
    //         if (tbCondition.TrueFlag[1] != -1)
    //         {
    //             if (Player.Instance.mFlag.GetFlag(tbCondition.TrueFlag[1]) != 1)
    //             {
    //                 return false;
    //             }
    //         }
    //         if (tbCondition.TrueFlag[2] != -1)
    //         {
    //             if (Player.Instance.mFlag.GetFlag(tbCondition.TrueFlag[2]) != 1)
    //             {
    //                 return false;
    //             }
    //         }
    //     }
    //     else
    //     {//有一个条件满足即可
    //         if(tbCondition.TrueFlag[0]!= -1)
    //         {
    //             bool isFlag = false;
    //             for (int i = 0; i < 3; i++)
    //             {
    //                 if (tbCondition.TrueFlag[i] != -1)
    //                 {
    //                     if (Player.Instance.mFlag.GetFlag(tbCondition.TrueFlag[i]) == 1)
    //                     {
    //                         isFlag = true;
    //                     }
    //                 }
    //             }
    //             if (!isFlag)
    //             {
    //                 return false;
    //             }
    //         }
    //         
    //     }
    //     if (tbCondition.FalseFlag[0] != -1)
    //     {
    //         if (Player.Instance.mFlag.GetFlag(tbCondition.FalseFlag[0]) == 1)
    //         {
    //             return false;
    //         }
    //     }
    //     for (int i = 0; i < 4; i++)
    //     {
    //         int exId = tbCondition.ExdataId[i];
    //         if(exId == -1)
    //         {
    //             break;
    //         }
    //         int nowValue = Player.Instance.mExdata.GetExdata(exId);
    //         if (nowValue < tbCondition.ExdataMin[i])
    //         {
    //             return false;
    //         }
    //         if (nowValue > tbCondition.ExdataMax[i])
    //         {
    //             return false;
    //         }
    //     }
    //     return true;
    // }
    public static long GetRes(ResType res)
    {
        var resId = (int)res;
      
        return GetRes(resId);
    }
    public static long GetRes(int resId)
    {
        if(resId < 0 || resId >=(int)ResType.Max)
        {
            Logger.Error("GetRes not find Res Id={0}", resId);
            return -1;
        }
        return Player.Instance.Res[resId];
    }
    public static  void SetRes(ResType res, long value)
    {
        var resId = (int)res;
        SetRes(resId, value);
    }
    public static void SetRes(int resId, long value)
    {
        if (resId < 0 || resId >= (int)ResType.Max)
        {
            Logger.Error("SetRes not find Res Id={0}", resId);
            return;
        }
        //抛出事件
        var e = (ResType)resId;
        //修改值
        Player.Instance.Res[resId] = value;
        Player.Instance.ResDirty = true;

    }
    public static  void AddRes(ResType res, long addvalue)
    {
        var resId = (int)res;
        AddRes(resId, addvalue);
    }
    public static void AddRes(int resId, long addvalue)
    {
        if (resId < 0 || resId >= (int)ResType.Max)
        {
            Logger.Error("AddRes not find Res Id={0}", resId);
            return;
        }
        long nowValue = GetRes(resId);
        var newValue = nowValue + addvalue;
        SetRes(resId, newValue);
    }

    #region  存储与加载
    
    private static void SavePlayerID()
    {
        PlayerPrefs.SetString("PlayerID", Player.Instance.PlayerId.ToString());
        PlayerPrefs.Save();
    }
    public static bool ReadPlayerID()
    {
        var s = PlayerPrefs.GetString("PlayerID");
        if (string.IsNullOrEmpty(s))
        {
            Player.Instance.PlayerId = (ulong)DateTime.Now.ToBinary();
            PlayerInterFace.SavePlayerID();
            return false;
        }
        Player.Instance.PlayerId = s.ToUlong();
        return true;
    }
    
    public static void SaveResData()
    {
        StringBuilder s = new StringBuilder();
        bool isfirst = true;
        foreach (var item in Player.Instance.Res)
        {
            if(!isfirst)
            {
                s.Append("|");
            }
            s.Append(item);
            isfirst = false;
        }

        Logger.LogWarn(s.ToString());
        PlayerPrefs.SetString("Res", s.ToString());
    }
    public static void ReadResData()
    {
        var s = PlayerPrefs.GetString("Res");
        if (string.IsNullOrEmpty(s))
        {
            Logger.Info("没有资源数据");
            return;
        }
        var t = s.Split('|');
        int index = 0;
        foreach (var item in t)
        {
            Player.Instance.Res[index] = long.Parse(item);
            index++;
        }
    }

 
   
    //1:i|id|count|exdata0|exdata1|i|id|count|exdata0|exdata1:2:i|id|count
    public static void SaveItemData()
    {
        StringBuilder s = new StringBuilder();
        bool isfirst = true;
        foreach (var bag in Player.Instance.Bag.Bags)
        {
            if (!isfirst)
            {
                s.Append(":");
            }
            s.Append(bag.Key);
            s.Append(":");
            bool isItemFirst = true;
            foreach (var item in bag.Value.Items)
            {
                if (!isItemFirst)
                {
                    s.Append("|");
                }
                s.Append("i");
                s.Append("|");
                s.Append(item.ItemId);
                s.Append("|");
                s.Append(item.Count);
                for (int i = 0; i < item.DataList.Count; i++)
                {
                    s.Append("|");
                    s.Append(item.DataList[i]);
                }
                isItemFirst = false;
            }
            isfirst = false;
        }
        Logger.LogWarn(s.ToString());
        PlayerPrefs.SetString("Item", s.ToString());
    }
    public static bool ReadItemData()
    {
        var s = PlayerPrefs.GetString("Item");
        if(string.IsNullOrEmpty(s))
        {            
            return false;
        }
        var t = s.Split(':');
        bool waitId = true;
        BagOne one = null;
        foreach (var bag in t)
        {
            if (waitId)
            {
                one = Player.Instance.Bag.AddBag(int.Parse(bag));
                waitId = false;
            }
            else
            {
                var t2 = bag.Split('|');
                ItemBase item = null;
                int itemid = 0;
                int count = 0;
                int index = -1;
                foreach (var i in t2)
                {
                    if(i == "i")
                    {
                        if(item!= null)
                        {
                            one.Items.Add(item);
                            item = null;
                        }
                        index = 0;
                    }
                    else
                    {
                        switch (index)
                        {
                            case 0:
                                {
                                    itemid = int.Parse(i);
                                    index++;
                                    break;
                                }
                            case 1:
                                {
                                    count = int.Parse(i);
                                    item = new ItemBase(itemid, count);
                                    index++;
                                    break;
                                }
                            case 2:
                                {
                                    item.DataList.Add(int.Parse(i));
                                    break;
                                }
                            default:
                                Logger.Error("Item is Null");
                                break;
                        }
                    }
                }
                if(item != null)
                {
                    one.Items.Add(item);
                }
                waitId = true;
            }
        }
        return true;
    }
    #endregion


   
}