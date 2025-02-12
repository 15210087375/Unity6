using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

public interface ILoader
{
    public IEnumerator Init(Action action);
    public T LoadAsset<T>(string path, System.Action<T> callback) where T : UnityEngine.Object;
    public UniTask LoadAssetAsync<T>(string path, System.Action<T> callback)where T : UnityEngine.Object;
    public void UnloadAsset(Object go);
    public void Release(string path);
}
