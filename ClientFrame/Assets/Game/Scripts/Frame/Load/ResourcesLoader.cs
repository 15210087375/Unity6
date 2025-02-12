using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourcesLoader : ILoader
{
    public IEnumerator Init(Action action)
    {
        action?.Invoke();
        yield return null;
    }

    public T LoadAsset<T>(string path, Action<T> callback) where T : Object
    {
        var asset = Resources.Load<T>(path);
        callback?.Invoke(asset);
        return asset;
    }

    public async UniTask LoadAssetAsync<T>(string path, Action<T> callback) where T : Object
    {
        var asset = await Resources.LoadAsync<T>(path);
        callback?.Invoke(asset as T);
       
    }
    
    public void UnloadAsset(Object go)
    {
        Resources.UnloadAsset(go);
    }

    public void Release(string path)
    {
        
    }
}
