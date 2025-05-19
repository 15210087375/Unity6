using UnityEngine;

public class EditorUtilsDefine
{
    //UI模板路径
    private const string LayerPrefabTemplate = "Assets/EditorUtils/Res/PrefabTemplate/LayerTemplate.prefab";
    private const string ViewPrefabTemplate = "Assets/EditorUtils/Res/PrefabTemplate/ViewTemplate.prefab";
    private const string CellPrefabTemplate = "Assets/EditorUtils/Res/PrefabTemplate/CellTemplate.prefab";
    
    //UI路径定义文件路径
    public const string UIPathDefinePath = "Assets/Game/Scripts/Frame/UI/UIPathDefine.cs";
    //UI脚本路径
    public const string UIScriptPath = "Assets/Game/Scripts/GameClient/UI";
    //UI预设路径
    public const string UIPrefabPath = "Assets/Res/UI/Prefabs";
    
    public static string GetUIPrefabTemplate(UIType uiType)
    {
        return uiType switch
        {
            UIType.Layer => LayerPrefabTemplate,
            UIType.View => ViewPrefabTemplate,
            UIType.Cell => CellPrefabTemplate,
            _ => ViewPrefabTemplate
        };
    }
    public static string GetUITypeBase(string className)
    {
        if (className.StartsWith("Layer"))
        {
            return "LayerBase";
        }
        else if (className.StartsWith("View"))
        {
            return className.EndsWith("Cell")? "MonoBehaviour" : "ViewBase";
        }
        else
        {
            return "MonoBehaviour";
        }
    }
    
    public static UIType GetUIType(string className)
    {
        if (className.StartsWith("Layer"))
        {
            return UIType.Layer;
        }
        else if (className.StartsWith("View"))
        {
            return className.EndsWith("Cell") ? UIType.Cell : UIType.View;
        }
        else
        {
            return UIType.None;
        }
    }
    
    public static LayerIndex GetLayerIndex(UIType uiType)
    {
        return uiType switch
        {
            UIType.Layer => LayerIndex.Layer,
            UIType.View => LayerIndex.View,
            UIType.Cell => LayerIndex.View,
            _ => LayerIndex.Low
        };
    }
}
