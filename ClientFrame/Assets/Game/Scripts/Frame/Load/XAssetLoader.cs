using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using xasset;
using Object = UnityEngine.Object;
public class XAssetLoader:ILoader
{
    private Dictionary<string,AssetRequest> _loadDic = new Dictionary<string, AssetRequest>();
    public IEnumerator Init(Action action)
    {
        var initializeAsync = Assets.InitializeAsync();
        yield return initializeAsync;
        action?.Invoke();
    }
    public T LoadAsset<T>(string path, Action<T> callback) where T : Object
    {
        var request = Asset.Load(path, typeof(T)); 
        var asset = request.asset as T;
        callback?.Invoke(asset);
        return asset;
    }

    public UniTask LoadAssetAsync<T>(string path, Action<T> callback) where T : Object
    {
        throw new NotImplementedException();
        
    }

    public void UnloadAsset(Object go)
    {
        throw new NotImplementedException();
    }

    public void Release(string path)
    {
        var check = _loadDic.TryGetValue(path,out var assetRequest);
        if (check)
        {
            assetRequest.Release();
        }
    }
}
