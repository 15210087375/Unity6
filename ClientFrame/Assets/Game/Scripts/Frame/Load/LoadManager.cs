using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

public  class LoadManager :Singleton<LoadManager>,ILoader
{

    
    private readonly ILoader _loader;
    public LoadManager()
    {
        _loader = new XAssetLoader();
    }


    public IEnumerator Init(Action action)
    {
        yield return _loader.Init(action);
    }

    public T LoadAsset<T>(string path, System.Action<T> callback = null) where T : UnityEngine.Object
    {
        return _loader.LoadAsset(path, callback);
    }
    
    public async UniTask LoadAssetAsync<T>(string path, System.Action<T> callback= null) where T : UnityEngine.Object
    {
       await _loader.LoadAssetAsync(path, callback);
    }
    
    public void UnloadAsset(Object go)
    {
        _loader.UnloadAsset(go);
    }

    public void Release(string path)
    {
        throw new NotImplementedException();
    }
}
