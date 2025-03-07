
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;


public static class Define
{
    //自动更新下载目录
    private static string _AutoUpdateDownLoadPath = string.Empty;

    public static string AutoUpdateDownLoadPath
    {
        get
        {
            if (string.IsNullOrEmpty(_AutoUpdateDownLoadPath))
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || NO_UNITY
                _AutoUpdateDownLoadPath =
                    GameUtils.String.PathFormat(Path.Combine(Environment.CurrentDirectory, Define.CacheResDir));
#else
                _AutoUpdateDownLoadPath =
 GameUtils.String.PathFormat(Path.Combine(Application.persistentDataPath, Define.CacheResDir));
#endif

                if (Directory.Exists(_AutoUpdateDownLoadPath) == false)
                {
                    Directory.CreateDirectory(_AutoUpdateDownLoadPath);
                }
            }

            return _AutoUpdateDownLoadPath;
        }
    }

    //Application.temporaryCachePat 目录下，里存放资源的目录
    public static string CacheResDir = "temporaryCacheRes";
    

}

