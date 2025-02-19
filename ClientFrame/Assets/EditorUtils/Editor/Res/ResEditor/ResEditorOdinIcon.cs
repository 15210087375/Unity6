using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using ObjectFieldAlignment = Sirenix.OdinInspector.ObjectFieldAlignment;

public class ResEditorOdinIcon : OdinEditorWindow
{

    [MenuItem("Assets/工具/查看OdinIcon")]
    public static void OpenResIcon()
    {
        var window = GetWindow<ResEditorOdinIcon>();
        window.titleContent = new GUIContent("OdinIcon");
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(300, 200);
        
        window.Show();
    } 
  
    
    [SerializeField][InlineButton("Copy",SdfIconType.Subtract,"复制枚举")]
    public SdfIconType IconType;


    public void Copy()
    {
        EditorGUIUtility.systemCopyBuffer = $"SdfIconType.{IconType}";
    }
  
    
    
}
