using TMPro;
using UnityEngine;

public partial class GameUtils
{
    
}

public static partial class GameExtensions
{
    public static void SetText(this TextMeshProUGUI text,int dictionaryId)
    {
        var dic = TableManager.Instance.Tables.TextRecord.GetOrDefault(dictionaryId);
        text.SetText(dic.Cn);
    }
    public static void SetText(this TextMeshProUGUI text, string content)
    {
        text.text = content;
    }
}