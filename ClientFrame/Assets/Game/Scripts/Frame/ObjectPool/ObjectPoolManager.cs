using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Lean.Pool;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ObjectPoolManager : MonoBehaviour
{
    public void Awake()
    {
        Instance = this;
    }

    public static ObjectPoolManager Instance;

    //所有的缓存池节点
    private static Dictionary<string, ObjectPoolNode> ObjectPools = new Dictionary<string, ObjectPoolNode>();

    //从缓存池中创建一个节点
    public static void NewObject(string resName, ObjectPoolType eType, Transform pt, Vector3 lp, Vector3 ls,
        Vector3 la,int max = 0,string effectTag = "",Action<GameObject> callBack = null)
    {
        GameObject ret = null;

        //从容器中找到缓存池
        if (ObjectPools.TryGetValue(resName, out ObjectPoolNode node) == false)
        {
            CreateNode(resName, eType, (node2) =>
            {
                if (node2 == null)
                {
                    callBack?.Invoke(null);
                    return;
                }

                if (ObjectPools.TryGetValue(resName, out ObjectPoolNode node) == false)
                {
                    if (max > 0)
                    {
                        node2.PoolObject.Capacity = max;    
                    }
                    ObjectPools.Add(resName, node2);
                
                    //从缓存池生成一个资源
                    if (node2.PoolObject.TrySpawn(ref ret, lp,la, ls,  pt, false) == false)
                    {
                        callBack?.Invoke(null);
                        return;
                    }
                    callBack?.Invoke(ret);
                }
                else
                {
                    //从缓存池生成一个资源
                    if (node.PoolObject.TrySpawn(ref ret, lp,la, ls,  pt, false) == false)
                    {
                        callBack?.Invoke(null);
                        return;
                    }

                    callBack?.Invoke(ret);   
                }
                
            });
        }
        else
        {
            //从缓存池生成一个资源
            if (node.PoolObject.TrySpawn(ref ret, lp,la, ls,  pt, false) == false)
            {
                callBack?.Invoke(null);
                return;
            }

            callBack?.Invoke(ret);
        }
   
    }


    //删除一个节点
    public static void Release(GameObject obj,float delay = 0.0f)
    {
        LeanPool.Despawn(obj,delay);
    }

    //生成一个节点
    private static async void CreateNode(string resName, ObjectPoolType type,Action<ObjectPoolNode> callBack )
    {
        LoadManager.Instance.LoadAsset<GameObject>(resName,(res) =>
        {
            if (res == null)
            {
                Logger.LogWarn("----创建缓存节点，加载失败----检查路径-:{0}", resName);
                callBack?.Invoke(null);
                return ;
            }

            //ObjectPoolNode node;
            if (ObjectPools.TryGetValue(resName, out ObjectPoolNode node))
            {
                callBack?.Invoke(node);
                return;
            }
            node = new ObjectPoolNode();
            node.PoolType = type;
            node.ResName = resName;
            node.ResObject = res as GameObject;

            var gameObject = new GameObject(resName);

            gameObject.transform.parent = Instance.transform;
            var pool = gameObject.AddComponent<LeanGameObjectPool>();

            if (type == ObjectPoolType.UI)
            {
                pool.Notification = LeanGameObjectPool.NotificationType.BroadcastIPoolable;
            }
            else
            {
                pool.Notification = LeanGameObjectPool.NotificationType.IPoolable;
            }
            pool.Prefab = node.ResObject;
            pool.Strategy = LeanGameObjectPool.StrategyType.ActivateAndDeactivate;
            //预生成多少
            pool.Preload = 0;
            //最大容量 0 无限制
            pool.Capacity = 0;
            //是否回收 
            pool.Recycle = true;
            //是否保存，换场景被销毁
            pool.Persist = false;
            //是否让对象以索引来命名
            pool.Stamp = true;
            node.PoolObject = pool;
            callBack?.Invoke(node);
        });
    }
    
    //清楚所有的战斗缓存
    public static void CleanAllObject()
    {
        // RemoveList.Clear();
        // foreach (var pool in ObjectPools)
        // {
        //     
        // }
        // RemoveList.Clear();
        foreach (var pool in ObjectPools)
        {
            pool.Value.Clean();
        }

        ObjectPools.Clear();
    }
    
    public static List<string> RemoveList = new List<string>();
    

    //根据类型清理
    public static void CleanObject(ObjectPoolType objType)
    {
        RemoveList.Clear();
        foreach (var pool in ObjectPools)
        {
            if (pool.Value.PoolType == objType)
            {
                RemoveList.Add(pool.Key);
            }
        }

        foreach (var str in RemoveList)
        {
            ObjectPools[str].Clean();
            ObjectPools.Remove(str);
        }
        RemoveList.Clear();
        
    }
}