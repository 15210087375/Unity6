using System;
using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;
using System.Threading;

public class DailyRefresh
{
    #region 字段与单例
    private const string LastRefreshTimeKey = "DailyRefresh_LastTime";
    private const string LastWeekRefreshTimeKey = "DailyRefresh_LastWeekTime";
    private DateTime lastRefreshTime;
    private DateTime lastWeekRefreshTime;

    // 刷新时间参数（可配置）
    public static int DailyRefreshHour = 16;      // 每日刷新小时
    public static int DailyRefreshMinute = 49;    // 每日刷新分钟
    public static int WeeklyRefreshHour = 4;     // 每周刷新小时
    public static int WeeklyRefreshMinute = 0;   // 每周刷新分钟
    public static DayOfWeek WeeklyRefreshDay = DayOfWeek.Monday; // 每周刷新星期几

    // 单例实例
    private static DailyRefresh _instance;
    public static DailyRefresh Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DailyRefresh();
            }
            return _instance;
        }
    }
    #endregion

    #region 定时器相关
    private CancellationTokenSource dailyCts;
    private CancellationTokenSource weeklyCts;
    #endregion

    #region 构造与初始化
    // 构造函数私有化，防止外部new
    private DailyRefresh()
    {
        LoadLastRefreshTime();
        LoadLastWeekRefreshTime();
    }
    #endregion

    #region 登录与刷新主逻辑
    /// <summary>
    /// 每次登录时调用，自动判断是否需要刷新，自动更新缓存，并启动定时器
    /// </summary>
    public bool OnLoginCheckAndRefresh()
    {
        LoadLastRefreshTime();
        LoadLastWeekRefreshTime();

        bool needDailyRefresh = CheckAndRefresh();
        bool needWeeklyRefresh = CheckAndWeekRefresh();

        if (needDailyRefresh)
        {
            SaveLastRefreshTime();
            // ==========================
            // 每日刷新逻辑
            // ==========================
            Logger.Info("每日刷新");
        }

        if (needWeeklyRefresh)
        {
            SaveLastWeekRefreshTime();
            // ==========================
            // 每周刷新逻辑
            // ==========================
            Logger.Info("每周刷新");
        }
        // 启动在线定时器
        StartOnlineRefreshTimer();
        // 返回是否有任何刷新
        return needDailyRefresh || needWeeklyRefresh;
    }
    #endregion

    #region 每日/每周刷新判定
    /// <summary>
    /// 检查是否需要每日刷新（每日指定时间），如果需要则更新时间
    /// </summary>
    private bool CheckAndRefresh()
    {
        DateTime now = DateTime.Now;
        DateTime todayRefreshTime = new DateTime(now.Year, now.Month, now.Day, DailyRefreshHour, DailyRefreshMinute, 0);
        if (now < todayRefreshTime)
        {
            todayRefreshTime = todayRefreshTime.AddDays(-1);
        }
        if (lastRefreshTime < todayRefreshTime)
        {
            lastRefreshTime = now;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检查是否需要每周刷新（每周指定星期几的指定时间），如果需要则更新时间
    /// </summary>
    private bool CheckAndWeekRefresh()
    {
        DateTime now = DateTime.Now;
        // 找到本周的刷新日
        int daysToTarget = ((int)now.DayOfWeek - (int)WeeklyRefreshDay + 7) % 7;
        DateTime thisWeekRefresh = new DateTime(now.Year, now.Month, now.Day, WeeklyRefreshHour, WeeklyRefreshMinute, 0).AddDays(-daysToTarget);
        if (now < thisWeekRefresh)
        {
            thisWeekRefresh = thisWeekRefresh.AddDays(-7);
        }
        if (lastWeekRefreshTime < thisWeekRefresh)
        {
            lastWeekRefreshTime = now;
            return true;
        }
        return false;
    }
    #endregion

    #region PlayerPrefs存取
    /// <summary>
    /// 保存上次每日刷新时间到PlayerPrefs
    /// </summary>
    private void SaveLastRefreshTime()
    {
        PlayerPrefs.SetString(LastRefreshTimeKey, lastRefreshTime.ToString("O")); // ISO 8601格式
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 保存上次每周刷新时间到PlayerPrefs
    /// </summary>
    private void SaveLastWeekRefreshTime()
    {
        PlayerPrefs.SetString(LastWeekRefreshTimeKey, lastWeekRefreshTime.ToString("O"));
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 从PlayerPrefs加载上次每日刷新时间
    /// </summary>
    private void LoadLastRefreshTime()
    {
        string timeStr = PlayerPrefs.GetString(LastRefreshTimeKey, "");
        if (!string.IsNullOrEmpty(timeStr))
        {
            DateTime.TryParse(timeStr, null, System.Globalization.DateTimeStyles.RoundtripKind, out lastRefreshTime);
        }
        else
        {
            lastRefreshTime = DateTime.MinValue;
        }
    }

    /// <summary>
    /// 从PlayerPrefs加载上次每周刷新时间
    /// </summary>
    private void LoadLastWeekRefreshTime()
    {
        string timeStr = PlayerPrefs.GetString(LastWeekRefreshTimeKey, "");
        if (!string.IsNullOrEmpty(timeStr))
        {
            DateTime.TryParse(timeStr, null, System.Globalization.DateTimeStyles.RoundtripKind, out lastWeekRefreshTime);
        }
        else
        {
            lastWeekRefreshTime = DateTime.MinValue;
        }
    }
    #endregion

    #region 获取下次刷新时间
    /// <summary>
    /// 获取下次每日刷新时间
    /// </summary>
    public DateTime GetNextRefreshTime()
    {
        DateTime now = DateTime.Now;
        DateTime nextRefresh = new DateTime(now.Year, now.Month, now.Day, DailyRefreshHour, DailyRefreshMinute, 0);
        if (now >= nextRefresh)
        {
            nextRefresh = nextRefresh.AddDays(1);
        }
        return nextRefresh;
    }

    /// <summary>
    /// 获取下次每周刷新时间（下次指定星期几的指定时间）
    /// </summary>
    public DateTime GetNextWeekRefreshTime()
    {
        DateTime now = DateTime.Now;
        int daysToNext = ((int)WeeklyRefreshDay - (int)now.DayOfWeek + 7) % 7;
        if (daysToNext == 0 && now >= new DateTime(now.Year, now.Month, now.Day, WeeklyRefreshHour, WeeklyRefreshMinute, 0))
        {
            daysToNext = 7;
        }
        DateTime nextWeekRefresh = new DateTime(now.Year, now.Month, now.Day, WeeklyRefreshHour, WeeklyRefreshMinute, 0).AddDays(daysToNext);
        return nextWeekRefresh;
    }
    #endregion

    #region 在线定时器（跨天/跨周在线刷新）
    /// <summary>
    /// 登录时调用，启动在线跨天/跨周定时器
    /// </summary>
    public void StartOnlineRefreshTimer()
    {
        StopOnlineRefreshTimer();
        dailyCts = new CancellationTokenSource();
        weeklyCts = new CancellationTokenSource();
        DailyTimerUniTask(dailyCts.Token).Forget();
        WeeklyTimerUniTask(weeklyCts.Token).Forget();
    }

    /// <summary>
    /// 停止定时器（如登出时调用）
    /// </summary>
    private void StopOnlineRefreshTimer()
    {
        if (dailyCts != null)
        {
            dailyCts.Cancel();
            dailyCts.Dispose();
            dailyCts = null;
        }
        if (weeklyCts != null)
        {
            weeklyCts.Cancel();
            weeklyCts.Dispose();
            weeklyCts = null;
        }
    }

    private async UniTaskVoid DailyTimerUniTask(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var now = DateTime.Now;
            var nextRefresh = GetNextRefreshTime();
            var waitMs = (int)(nextRefresh - now).TotalMilliseconds;
            Logger.Info($"等待 {waitMs / 1000} 秒后，刷新每日数据");
            Logger.Info($"当前时间：{now}, 下次刷新时间：{nextRefresh}");
            if (waitMs > 0)
                await UniTask.Delay(waitMs, cancellationToken: token);
            if (!token.IsCancellationRequested)
                OnLoginCheckAndRefresh();
        }
    }

    private async UniTaskVoid WeeklyTimerUniTask(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var now = DateTime.Now;
            var nextRefresh = GetNextWeekRefreshTime();
            var waitMs = (int)(nextRefresh - now).TotalMilliseconds;
            Logger.Info($"等待 {waitMs / 1000} 秒后，刷新每周数据");
            if (waitMs > 0)
                await UniTask.Delay(waitMs, cancellationToken: token);
            if (!token.IsCancellationRequested)
                OnLoginCheckAndRefresh();
        }
    }
    #endregion
}
