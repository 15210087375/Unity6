using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;

public static class Logger
{
    [Conditional("DEBUG_BUILD"), Conditional("UNITY_EDITOR")]
    public static void Debug(string format, params object[] args)
    {
        var str = format;
        //上传网络
        if (args != null && args.Length > 0)
        {
            str = string.Format(format, args);   
        }
#if UNITY_EDITOR || DEBUG_BUILD
        UnityEngine.Debug.Log(str);
#endif
    }

    [Conditional("DEBUG_BUILD"), Conditional("UNITY_EDITOR")]
    public static void Info(string format, params object[] args)
    {
        var str = format;
        //上传网络
        if (args != null && args.Length > 0)
        {
            str = string.Format(format, args);   
        }
#if UNITY_EDITOR || DEBUG_BUILD
        UnityEngine.Debug.Log(str);
#endif
    }

    [Conditional("DEBUG_BUILD"), Conditional("UNITY_EDITOR")]
    public static void LogWarn(string format, params object[] args)
    {
        var str = format;
        //上传网络
        if (args != null && args.Length > 0)
        {
            str = string.Format(format, args);   
        }
        //电脑和debug模式才打印警告
#if UNITY_EDITOR || DEBUG_BUILD
        UnityEngine.Debug.LogWarning(str);
#endif
    }

    public static Dictionary<string, object> logParams = new Dictionary<string, object>();

    public static void Error(string format, params object[] args)
    {
        var str = format;
        //上传网络
        if (args != null && args.Length > 0)
        {
            str = string.Format(format, args);   
        }
         
        UnityEngine.Debug.LogError(str);

//真机情况下发到tga后台        
// #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE)
//          SendServer(str);
// #endif
    }
    
    /// <summary>
    /// 直接把字符串写到指定文件名的文件中
    /// </summary>
    /// <param name="fileNane">文件名</param>
    /// <param name="format">字符串</param>
    /// <param name="args">格式化参数</param>
    [Conditional("DEBUG_BUILD"), Conditional("UNITY_EDITOR")]
    public static void File(string fileNane,string format, params object[] args)
    {
        var str = format;
        //上传网络
        if (args != null && args.Length > 0)
        {
            str = string.Format(format, args);   
        }
    }
    
    //文件写到指定文件中
    public static System.IO.StreamWriter FileWriter;
    [Conditional("DEBUG_BUILD"), Conditional("UNITY_EDITOR")]
    public static void Write(string format, params object[] args)
    {
        var str = format;
        //上传网络
        if (args != null && args.Length > 0)
        {
            str = string.Format(format, args);   
        }
        //UnityEngine.Debug.Log(str);
        if (FileWriter == null)
        {
            return;
        }
        FileWriter.WriteLine(str);
        FileWriter.Flush();
    }
    
    [Conditional("DEBUG_BUILD"), Conditional("UNITY_EDITOR")]
    public static void InitWrite(string name)
    {
        var now = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff");
        var fileName = Path.Combine(GameDefine.CacheResPath, $"{name}_{now}.txt");
        if (!Directory.Exists(GameDefine.CacheResPath))
        {
            Directory.CreateDirectory(GameDefine.CacheResPath);
        }
        if (!System.IO.File.Exists(fileName))
        {
            // 文件不存在，创建文件
            System.IO.File.Create(fileName).Dispose();
        }
        //先关闭旧的文件
        CloseWrite();
        FileWriter = new System.IO.StreamWriter(fileName, false, Encoding.UTF8);
    }
    
    [Conditional("DEBUG_BUILD"), Conditional("UNITY_EDITOR")]
    public static void CloseWrite()
    {
        if (FileWriter == null)
        {
            return;
        }
        FileWriter.Flush();
        FileWriter.Close();
        FileWriter = null;
    }
}