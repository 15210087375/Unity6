
using System.Collections.Generic;

public class ListPool<T>
{
    public static ListPool<T> Instance = new ListPool<T>();
    private List<List<T>> pool = new List<List<T>>();

    private ListPool() { }

    public List<T> Get(int defaultSize = 0)
    {
        if (pool.Count > 0) {
            var list = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
            return list;
        }
        return defaultSize > 0 ? new List<T>(defaultSize) : new List<T>();
    }

    public void Release(List<T> toRelease)
    {
        toRelease.Clear();
        pool.Add(toRelease);
    }
}
