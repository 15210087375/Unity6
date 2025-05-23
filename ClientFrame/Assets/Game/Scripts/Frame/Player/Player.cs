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
        
        DailyRefresh.Instance.OnLoginCheckAndRefresh();
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
        Bag = PlayerInterFace.ReadData<PlayerBag>(eCacheDataType.Item);
        Flag = PlayerInterFace.ReadData<BitFlag>(eCacheDataType.Flag);
        ExData = PlayerInterFace.ReadData<ExData>(eCacheDataType.Exdata);
        ExData64 = PlayerInterFace.ReadData<Exdata64>(eCacheDataType.Exdata64);
        Res = PlayerInterFace.ReadData<long[]>(eCacheDataType.Res);
    }

    
   
    private void Save()
    {
        PlayerInterFace.SaveData(Bag, eCacheDataType.Item);
        PlayerInterFace.SaveData(Flag, eCacheDataType.Flag);
        PlayerInterFace.SaveData(ExData, eCacheDataType.Exdata);
        PlayerInterFace.SaveData(ExData64, eCacheDataType.Exdata64);
        PlayerInterFace.SaveData(Res, eCacheDataType.Res);
        
        PlayerPrefs.Save();
    }
  
 
    public void Update()
    {
        if (!_isInit)
        {
            return;
        }
        if (FlagDirty)
        {
            FlagDirty = false;
            PlayerInterFace.SaveData(Flag, eCacheDataType.Flag);
        }

        if (ExDataDirty)
        {
            ExDataDirty = false;
            PlayerInterFace.SaveData(ExData, eCacheDataType.Exdata);
        }

        if (ExData64Dirty)
        {
            ExData64Dirty = false;
            PlayerInterFace.SaveData(ExData64, eCacheDataType.Exdata64);
        }

        if (ResDirty)
        {
            ResDirty = false;
            PlayerInterFace.SaveData(Res, eCacheDataType.Res);
        }

        if (BagDirty)
        {
            BagDirty = false;
            PlayerInterFace.SaveData(Bag, eCacheDataType.Item);
        }

    }


 

}