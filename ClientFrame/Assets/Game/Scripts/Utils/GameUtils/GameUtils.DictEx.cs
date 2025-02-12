using System.Collections;
using System.Collections.Generic;
using System.Reflection;

//字典扩展类
public static partial class GameExtensions
{
    public static ulong ModifyValue<T>(this Dictionary<T, ulong> dic, T key, ulong value)
    {
        ulong oldValue;
        if (dic.TryGetValue(key, out oldValue))
        {
            oldValue += value;
        }
        else
        {
            oldValue = value;
        }

        dic[key] = oldValue;
        return oldValue;
    }

    public static long ModifyValue<T>(this Dictionary<T, long> dic, T key, long value)
    {
        long oldValue;
        if (dic.TryGetValue(key, out oldValue))
        {
            oldValue += value;
        }
        else
        {
            oldValue = value;
        }

        dic[key] = oldValue;
        return oldValue;
    }

    public static ulong ModifyValue<T>(this SortedDictionary<T, ulong> dic, T key, ulong value)
    {
        ulong oldValue;
        if (dic.TryGetValue(key, out oldValue))
        {
            oldValue += value;
        }
        else
        {
            oldValue = value;
        }

        dic[key] = oldValue;
        return oldValue;
    }

    public static int ModifyValue<T>(this Dictionary<T, int> dic, T key, int value)
    {
        int oldValue;
        if (dic.TryGetValue(key, out oldValue))
        {
            oldValue += value;
        }
        else
        {
            oldValue = value;
        }

        dic[key] = oldValue;
        return oldValue;
    }


    public static int ModifyValue<T>(this SortedDictionary<T, int> dic, T key, int value)
    {
        int oldValue;
        if (dic.TryGetValue(key, out oldValue))
        {
            oldValue += value;
        }
        else
        {
            oldValue = value;
        }

        dic[key] = oldValue;
        return oldValue;
    }

    public static void ModifyValue<T>(this Dictionary<T, float> dic, T key, float value)
    {
        float oldValue;
        if (dic.TryGetValue(key, out oldValue))
        {
            dic[key] = oldValue + value;
        }
        else
        {
            dic[key] = value;
        }
    }

    public static void AddDirList<T, T1>(this Dictionary<T, List<T1>> dic, T key, T1 value)
    {
        List<T1> list;
        if (dic.TryGetValue(key, out list))
        {
            list.Add(value);
        }
        else
        {
            list = new List<T1>();
            list.Add(value);
            dic[key] = list;
        }
    }

    public static void AddDirList<T, T1>(this SortedDictionary<T, List<T1>> dic, T key, T1 value)
    {
        List<T1> list;
        if (dic.TryGetValue(key, out list))
        {
            list.Add(value);
        }
        else
        {
            list = new List<T1>();
            list.Add(value);
            dic[key] = list;
        }
    }

    //复制一个字典到目标字典，目标字典先清空
    public static void Copy<T, T1>(this SortedDictionary<T, List<T1>> dic, SortedDictionary<T, List<T1>> dic2)
    {
        dic.Clear();
        foreach (var pair in dic2)
        {
            var list = new List<T1>();
            list.AddRange(pair.Value);
            dic[pair.Key] = list;
        }
    }

    public static List<int> GetMaxKeys<T>(this Dictionary<int, T> dic, int count)
    {
        List<int> k = new List<int>();
        foreach (var item in dic)
        {
            k.Add(item.Key);
        }

        k.Sort();

        List<int> v = new List<int>();
        for (int i = 0; i < count; i++)
        {
            int maxIndex = k.Count - i - 1;
            if (maxIndex < 0)
            {
                return v;
            }

            v.Add(k[maxIndex]);
        }

        return v;
    }

    public static T GetMaxKeyValue<T>(this Dictionary<int, T> dic, int min, int max, ref int mi)
    {
        T m = default(T);
        for (int i = min; i <= max; i++)
        {
            T n;
            if (dic.TryGetValue(i, out n))
            {
                m = n;
                mi = i;
            }
        }

        return m;
    }

    public static T1 GetValue<T, T1>(this Dictionary<T, T1> dic, T key)
    {
        T1 m;
        if (dic.TryGetValue(key, out m))
        {
            return m;
        }

        return default(T1);
    }
}