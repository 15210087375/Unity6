using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
public enum eCacheDataType
{
    Item,
    Res,
    Flag,
    Exdata,
    Exdata64
}
public partial class PlayerInterFace
{
   
    private static readonly List<string> _saveList = new List<string>()
    {
        "Item.bin",
        "Res.bin",
        "Flag.bin",
        "Exdata.bin",
        "Exdata64.bin"
    };
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
    

 

 
 
    #endregion

    
    public static void ClearData()
    {
       
        for (var i = 0; i < _saveList.Count; i++)
        {
            var filePath = $"{Define.AutoUpdateDownLoadPath}/{_saveList[i]}";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        
    }

    public static void SaveData<T>(T t,eCacheDataType type)
    {
        var desc = GameUtils.IO.Serialize<T>(t);
        GameUtils.IO.WriteFile($"{Define.AutoUpdateDownLoadPath}/{_saveList[(int)type]}", desc);
    }

    public static T ReadData<T>(eCacheDataType type)
    {
        var desc = GameUtils.IO.ReadFile($"{Define.AutoUpdateDownLoadPath}/{_saveList[(int)type]}");
        if (!string.IsNullOrEmpty(desc))
        {
            return GameUtils.IO.Deserialize<T>(desc);
        }
        return default(T);
    }



   
}