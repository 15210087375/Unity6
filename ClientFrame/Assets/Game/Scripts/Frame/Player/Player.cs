using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//玩家数据
public class Player
{
    private static Player mInstance;
    public static Player Instance => mInstance ??= new Player();
    
    private bool _isInit = false;
    //玩家ID
    public ulong PlayerId = 0;
    
    //资源
    public long[] Res = new long[(int)(ResType.Max - ResType.Coin)];
    public bool ResDirty = false;

    //物品
    public PlayerBag Bag = new PlayerBag();
    public bool BagDirty = false;

    //标记位
    public BitFlag Flag = new BitFlag(4096);
    public bool FlagDirty = false;
    
    //扩展计数
    public ExData ExData = new ExData();
    public bool ExDataDirty = false;
    public Exdata64 ExData64 = new Exdata64();
    public bool ExData64Dirty = false;


    //初始化
    public void Init()
    {
        InitByDB();
        
        _isInit = true;
    }

    //新数据
    private void InitByBase()
    {
        Debug.Log("init player data");
        Flag = new BitFlag(4096);
        ExData.InitByBase();
        ExData64.InitByBase();
        Bag = new PlayerBag();
        PlayerId = (ulong)DateTime.Now.ToBinary();

        Save();
    }

    //老数据 本地客户端 使用
    private void InitByDB()
    {
        if (!PlayerInterFace.ReadPlayerID())
        {
            InitByBase();
            return;
        }

        ExData.ReadExdataData();
        Flag.ReadFlagData();
        // PlayerInterFace.ReadExdata64Data();
        // PlayerInterFace.ReadResData();
        // PlayerInterFace.ReadItemData();
        // PlayerInterFace.ReadFlagData();
    }

    
   
    private void Save()
    {
        Flag.SaveFlagData();
        ExData.SaveExdataData();
        // PlayerInterFace.SaveExdata64Data();
        // PlayerInterFace.SaveResData();
        // PlayerInterFace.SaveItemData();
        // PlayerInterFace.SavePlayerID();
        
        PlayerPrefs.Save();
    }
  
 
    public void Update()
    {
        if (!_isInit)
        {
            return;
        }
        var isSave = false;
        
        if (FlagDirty)
        {
            FlagDirty = false;
            Flag.SaveFlagData();
            isSave = true;
        }

        if (ExDataDirty)
        {
            ExDataDirty = false;
            ExData.SaveExdataData();
            isSave = true;
        }

        if (ExData64Dirty)
        {
            ExData64Dirty = false;
            ExData64.SaveExdata64Data();
            isSave = true;
        }

        if (ResDirty)
        {
            ResDirty = false;
            PlayerInterFace.SaveResData();
            isSave = true;
        }

        if (BagDirty)
        {
            BagDirty = false;
            PlayerInterFace.SaveItemData();
            isSave = true;
        }

        if (isSave)
        {
            PlayerPrefs.Save();
        }
    }


 

}