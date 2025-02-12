using UnityEngine;

public class EditorUtilsDefine
{
    //UI模板路径
    private const string LayerPrefabTemplate = "Assets/EditorUtils/Res/PrefabTemplate/LayerTemplate.prefab";
    private const string ViewPrefabTemplate = "Assets/EditorUtils/Res/PrefabTemplate/ViewTemplate.prefab";
    
    //UI路径定义文件路径
    public const string UIPathDefinePath = "Assets/Game/Scripts/Frame/UI/UIPathDefine.cs";
    //UI脚本路径
    public const string UIScriptPath = "Assets/Game/Scripts/GameClient/UI";
    //UI预设路径
    public const string UIPrefabPath = "Assets/Res/UI/Prefabs";
    
    public static string GetUIPrefabTemplate(UIType uiType)
    {
        return uiType == UIType.Layer ? LayerPrefabTemplate : ViewPrefabTemplate;
    }
    public static string GetUITypeBase(string className)
    {
        if (className.Contains("Layer"))
        {
            return "LayerBase";
        }
        else if (className.Contains("View"))
        {
            return "ViewBase";
        }
        else
        {
            return "MonoBehaviour";
        }
    }
    
    public static UIType GetUIType(string className)
    {
        if (className.Contains("Layer"))
        {
            return UIType.Layer;
        }
        else if (className.Contains("View"))
        {
            return UIType.View;
        }
        else
        {
            return UIType.None;
        }
    }
}
