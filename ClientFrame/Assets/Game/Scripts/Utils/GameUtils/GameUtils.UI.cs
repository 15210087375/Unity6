using System.Text.RegularExpressions;
using UnityEngine;

public static partial class GameUtils
{
    public static class UI
    {
        //设置分辨率
        public static int DesignWidth = 1080;
        public static int DesignHeight = 1920;
        
        
        public static int ScreenWidth = Screen.width;
        public static int ScreenHeight = Screen.height;
        public static float ScreenRatio = (ScreenHeight + 0f) / ScreenWidth;
        public static int Width = Screen.width;
        public static int Height = Screen.height;
        public static float WidthScale = Width / (DesignWidth + 0f);
        public static float HeightScale = Height / (DesignHeight + 0f);
        public static Vector2 ScreenSize;
        public static float WidthScaleAll = WidthScale < HeightScale ? WidthScale / HeightScale : 1f;
        public static Vector2 WindowSize = new Vector2(-1, -1);
        
        
        //刘海屏幕适配 相关
        // public static float ThinScale = 0f;
        // private static float FringeHeight = -55f;
        // private static  float SmartIslandHeight = -70f;
        // private static bool ifSmartIsland = false;
        
        //初始化屏幕相关数据
        public static void Init()
        {
            Width = DesignWidth;
            Height = (int)(Screen.height * (Width + 0f) / Screen.width);
            
            WidthScale = Width / (DesignWidth + 0f);
            HeightScale = Height / (DesignHeight + 0f);
            
            ScreenSize = new Vector2(Screen.width * DesignHeight / (Screen.height + 0f), DesignHeight);
            
            WidthScaleAll = WidthScale < HeightScale ? WidthScale / HeightScale : 1f;
            
            //设置窗口大小
            WindowSize.x = DesignWidth;
            WindowSize.y = Height / (Width + 0f) * DesignWidth;
            WindowSize.y = Mathf.Clamp(WindowSize.y, DesignHeight, WindowSize.y);
        }
        
        
        //十六进制转颜色
        public static Color GetColor(string hex)
        {
            var tempHex = hex;
            if (!tempHex.StartsWith("#"))
            {
                tempHex = $"#{tempHex}";
            }

            tempHex = tempHex.Replace("[", "");
            tempHex = tempHex.Replace("]", "");
            string pattern = "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$";

            // 使用Regex.IsMatch方法判断字符串是否匹配正则表达式
            if (Regex.IsMatch(tempHex, pattern))
            {
                ColorUtility.TryParseHtmlString(tempHex, out var color);
                return color;
            }
            else
            {
                Logger.Error($"传入的颜色值不正确，颜色值为：{tempHex}");
                return Color.white;
            }
        }
        
   
        // /// <summary>
        // /// 获取微信胶囊按钮左侧剩余宽度
        // /// </summary>
        // /// <returns></returns>
        // public static float GetWXButtonResidueWidth()
        // {
        //     #if UNITY_WECHAT && !UNITY_EDITOR
        //     var info = WeChatWASM.WX.GetSystemInfoSync();
        //     var wxMenuInfo = WeChatWASM.WX.GetMenuButtonBoundingClientRect();
        //     var width = (float)(wxMenuInfo.left * DesignWidth / info.windowWidth);
        //     return width;
        //     #else
        //     return DesignWidth;
        //     #endif
        // }
        /// <summary>
        /// 获取顶部异形屏幕区域高度
        /// </summary>
        /// <returns></returns>
        public static float GetSafeOffsetTop()
        {
      
            #if UNITY_WECHAT && UNITY_WEBGL && !UNITY_EDITOR
                var info = WeChatWASM.WX.GetSystemInfoSync();
                var wxMenuInfo = WeChatWASM.WX.GetMenuButtonBoundingClientRect();
                var top = (float)(wxMenuInfo.top * DesignWidth / info.windowWidth);
                
                return top;
            #else
                if(Screen.cutouts.Length > 0)//留海等裁切区域
                {
                    return Screen.cutouts[0].height;
                }
                else
                {
                   
                    return Screen.height - Screen.safeArea.yMax;
                }
            #endif
           
        }
     
        public static bool CheckOverlap(RectTransform rectTransform1,RectTransform rectTransform2)
        {

            Rect rect1 = GetScreenRect(rectTransform1);
            Rect rect2 = GetScreenRect(rectTransform2);

            bool isOverlap = rect1.Overlaps(rect2);

            return isOverlap;
        }
        private static Rect GetScreenRect(RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            Vector2 size = new Vector2(Mathf.Abs(corners[0].x - corners[2].x),
                Mathf.Abs(corners[0].y - corners[2].y));
            Rect rect = new Rect(corners[0], size);
            return rect;

            
        }
    }
}