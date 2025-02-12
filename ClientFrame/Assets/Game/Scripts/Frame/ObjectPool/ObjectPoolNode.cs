using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

//缓存资源的类型
public enum ObjectPoolType
{
    //无效默认类型
    None = -1,
    //子弹类型
    Bullet = 0,
    //特效类型
    Effect = 1,
    //怪物类型
    Monster = 2,
    //格子Spine类型
    Spine = 3,
    //UI类型
    UI = 4,
}


//缓存节点
public class ObjectPoolNode
{
    //缓存资源的类型
    public ObjectPoolType PoolType;
    //缓存池数据
    public LeanGameObjectPool PoolObject;
    //缓存资源的名字
    public string ResName;
    //缓存资源的Obj
    public GameObject ResObject;

    public void Clean()
    {
        //删除所有缓存的节点
        PoolObject.Clean();
        LoadManager.Instance.UnloadAsset(PoolObject.gameObject);
    }
}
