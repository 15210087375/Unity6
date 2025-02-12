using System.Collections;
using System.Collections.Generic;



public static partial class GameExtensions
{
    //获取List索引
    public static T GetListIndex<T>(this List<T> list,int index, bool isLog = true)
    {        
        int LC = list.Count;
        if (index < 0)
        {
            if(isLog)
            {
                Logger.Error("GetIndex LC = {0}, index = {1}", LC, index);
            }
            if (LC < 1)
            {
                return default(T);
            }
            return list[0];
        }
        if(index >= LC)
        {
            if (isLog)
            {
                Logger.LogWarn("GetIndex LC = {0}, index = {1}", LC, index);
            }
            if (LC < 1)
            {
                return default(T);
            }
            return list[LC - 1];
        }
        return list[index];
    }
}