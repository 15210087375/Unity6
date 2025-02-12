using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public partial class GameUtils 
{
    public static class ServerTime
    {
            private static float startRunTime;
            private static long serverTime = 0;
            private static bool isInitServerTime = false;
            /// <summary>
            /// 1970-01-01 ms
            /// </summary>
            public static long GetTimestamp(DateTime dateTime)
            {
                DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return (dateTime.Ticks - dt1970.Ticks) / 10000;
            }
            public static long UtcServerTime
            {
                get
                {
                    if (!isInitServerTime)
                    {
                        return GetTimestamp(System.DateTime.UtcNow);
                    }
        
                    float truntime = Time.realtimeSinceStartup - startRunTime;
                    long ret = serverTime + (long)(truntime * 1000);
                    return ret;
                }
            }
            
            
            //同步时间回包
            public static void CorrectionTime(ulong serverNowTimeStamp)
            {
               
                if (serverTime == (long)serverNowTimeStamp)
                    return;
                try
                {
                    serverTime = (long)serverNowTimeStamp;
        
                    //var dif = serverTime - pTime.ClientTime;
                    //var dif = (DataUtile.GetTimestamp(System.DateTime.UtcNow) - pTime.ClientTime) /2 ;
                    //startRunTime = (Time.realtimeSinceStartup * 1000 - (dif)) / 1000f ;
                    
                    startRunTime = Time.realtimeSinceStartup;
                    Logger.Info($"设置服务器时间:{serverTime}  startRunTime:{startRunTime}");
                    isInitServerTime = true;
                }
                catch (Exception e)
                {
                    Logger.Error("CorrectionTime error:" + e);
                }
            }

            public static async UniTask<DateTime> GetWebTime()
            {
                // 获取时间地址
                string url = "https://www.baidu.com";

                Debug.Log($"开始获取服务器时间... 获取地址是: {url}");
                try
                {
                    var webTime = DateTime.Now;

                    // 发起请求
                    using UnityWebRequest webRequest = new UnityWebRequest(url);
                    // 发送请求
                    await webRequest.SendWebRequest();

                    // 检查是否出错
                    if (!webRequest.isNetworkError && !webRequest.isHttpError)
                    {
                        // 将返回值存为字典
                        Dictionary<string, string> resHeaders = webRequest.GetResponseHeaders();
                        string key = "DATE";
                        string value = null;

                        // 获取key为"DATE" 的 Value值
                        if (resHeaders != null && resHeaders.ContainsKey(key))
                        {
                            resHeaders.TryGetValue(key, out value);
                        }

                        if (value == null)
                        {
                            Debug.LogError("没有获取到key为DATE对应的Value值...");
                            return webTime;
                        }

                        // 取到了value，则进行转换为本地时间
                        webTime = FormattingGMT(value);
                        TimeSpan span = (webTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
                        ulong unixTime = (ulong) span.TotalSeconds;
                        CorrectionTime(unixTime);
                        Debug.Log($"{value}，转换后的网络时间：{webTime} 时间戳：{unixTime}");
                    }
                    else
                    {
                        Debug.LogError($"请求出错：{webRequest.error}");
                    }

                    return webTime;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Get web time error: {ex.Message}");
                    return DateTime.Now;
                }
            }
            private static DateTime FormattingGMT(string value)
            {
                DateTime time;
                try
                {
                     time = DateTime.ParseExact(value, "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                        System.Globalization.CultureInfo.InvariantCulture);
                   
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Formatting GMT error: {ex.Message}");
                    time = DateTime.Now;
                }

                return time;
            }
    }
}
