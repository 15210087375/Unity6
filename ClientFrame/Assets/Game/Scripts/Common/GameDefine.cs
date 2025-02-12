using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GameDefine
{
    public static bool IsEffectEnable = false;
    public static bool IsCameraShakeEnable = false;
    public static bool UseZoneManager = false;
    public static bool AutoPlay = false;

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