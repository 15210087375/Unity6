using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public partial class GameDefine
{
   
    public static GameMode GameMode = GameMode.Dev;
    public static GameEnv GameEnv = GameEnv.Local;
    private static string _CacheResPath = string.Empty;

    public const int DesignWidth = 1080;
    public const int DesignHeight = 1920;

    //游戏可读写目录
    public static string CacheResPath
    {
        get
        {
            if (string.IsNullOrEmpty(_CacheResPath))
            {
                string _CacheResDir = "temporaryCache";
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || NO_UNITY
                _CacheResPath = GameUtils.String.PathFormat(Path.Combine(Environment.CurrentDirectory, _CacheResDir));
#else
                _CacheResPath = GameUtils.String.PathFormat(Path.Combine(UnityEngine.Application.persistentDataPath, _CacheResDir));
#endif

                if (Directory.Exists(_CacheResPath) == false)
                {
                    Directory.CreateDirectory(_CacheResPath);
                }
            }

            return _CacheResPath;
        }
    }
}

public enum GameMode
{
    Dev,
    Rel,
}

public enum GameEnv
{
    Local,
    Net,
}
