using System.Text;

//字符传工具类型
public static partial class GameUtils
{
    public static class String
    {
        static StringBuilder builder = new StringBuilder(2048);
        public static string CombineString(params string[] texts)
        {
            builder.Clear();
            for (int i = 0; i < texts.Length; i++)
            {
                string text = texts[i];
                builder.Append(text);
            }

            return builder.ToString();
        }

        public static string PathFormat(string strPath)
        {
            return strPath.Replace("\\", "/");
        }
    }
}

public static partial class GameExtensions
{
    public static int ToInt(this string str)
    {
        bool isNumeric = int.TryParse(str, out var value);

        
        if (!isNumeric)
        {
            Logger.Error($"字符串 \" {str}\"  不能转成int");
        }
        return value;
    }
    public static ulong ToUlong(this string str)
    {
        bool isNumeric = ulong.TryParse(str, out var value);

        
        if (!isNumeric)
        {
            Logger.Error($"字符串 \" {str}\"  不能转成int");
        }
        return value;
    }
}