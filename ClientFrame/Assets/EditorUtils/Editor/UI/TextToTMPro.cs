using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TextToTMPro : Editor
{
    
    [MenuItem("Assets/工具/Text转TMPro(同时修改脚本)")]
    public static void SetSelectedMonsterPrefabsValue1()
    {
        Debug.Log($"数量{Selection.gameObjects.Length}");
        if (Selection.gameObjects.Length == 0)
        {
            return;
        }

        foreach (var obj in Selection.gameObjects)
        {
            var path = AssetDatabase.GetAssetPath(obj);
            Text2TextMeshPro(path,true);
        }
    }
    [MenuItem("Assets/工具/Text转TMPro(prefab)")]
    public static void SetSelectedMonsterPrefabsValue2()
    {
        Debug.Log($"数量{Selection.gameObjects.Length}");
        if (Selection.gameObjects.Length == 0)
        {
            return;
        }

        foreach (var obj in Selection.gameObjects)
        {
            var path = AssetDatabase.GetAssetPath(obj);
            Text2TextMeshPro(path,false);
        }
    }
    private static void Text2TextMeshPro(string path,bool changeScript )
    {
        var prefab1 = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        var prefab = PrefabUtility.InstantiatePrefab(prefab1) as GameObject;
        // var prefab = PrefabUtility.LoadPrefabContents(path);
        if (prefab)
        {
            if (prefab != null)
            {
                SerializedObject serializedObject = new SerializedObject(prefab);
                var scripts = prefab.GetComponents<MonoBehaviour>().Where(s=>!IsUIComponent(s)).ToList();
                
                foreach (var script in scripts)
                {
                    
                    var components = script.GetComponents<Component>().Where(s=>!IsUIComponent(s)).ToList();
                    foreach (var component in components)
                    {
                        
                        // var componentType = component.GetType();
                        var fields = component.GetAllFields().Where(f => f.FieldType == typeof(UnityEngine.UI.Text)).ToList();
                     
                        foreach (var field in fields)
                        {
                            if (field.FieldType == typeof(UnityEngine.UI.Text))
                            {
                                if (field.GetValue(component) == null)
                                {
                                    continue;
                                }
                                var textComponent = field.GetValue(component) as UnityEngine.UI.Text;
                                var identifier = textComponent.GetOrAddComponent<MyIdentifier>();
                                // 设置标识符为子对象的名称
                                identifier.SetIdentifier(field.Name);
                            }
                        }
                    }
                    if(changeScript)
                    {
                        ReplaceScript(MonoScript.FromMonoBehaviour(script));
                    }
                }
            }


            Text[] list = prefab.GetComponentsInChildren<Text>(true);
            for (int i = 0; i < list.Length; i++)
            {
                Text text = list[i];
                ChangeText2TMP(text);
            }

            Debug.LogError("保存预制体");
        }

        PrefabUtility.SaveAsPrefabAsset(prefab, path, out bool success);
        DestroyImmediate(prefab);
        if (!success)
        {
            Debug.LogError($"预制体：{path} 保存失败!");
        }
    }
    
    private static bool IsUIComponent(Component component)
    {
        return component.GetType().IsSubclassOf(typeof(UnityEngine.UI.Graphic));
    }

    private static void ChangeText2TMP(Text text)
    {
        Transform target = text.transform;
        Vector2 size = text.rectTransform.sizeDelta;
        string strContent = text.text;
        Color color = text.color;
        int fontSize = text.fontSize;
        FontStyle fontStyle = text.fontStyle;
        TextAnchor textAnchor = text.alignment;
        bool richText = text.supportRichText;
        HorizontalWrapMode horizontalWrapMode = text.horizontalOverflow;
        VerticalWrapMode verticalWrapMode = text.verticalOverflow;
        bool raycastTarget = text.raycastTarget;
        GameObject.DestroyImmediate(text);

        TextMeshProUGUI textMeshPro = target.gameObject.AddComponent<TextMeshProUGUI>();

        textMeshPro.rectTransform.sizeDelta = size;
        textMeshPro.text = strContent;
        textMeshPro.color = color;
        textMeshPro.fontSize = fontSize;
        textMeshPro.fontStyle = fontStyle == FontStyle.BoldAndItalic ? FontStyles.Bold : (FontStyles)fontStyle;
        switch (textAnchor)
        {
            case TextAnchor.UpperLeft:
                textMeshPro.alignment = TextAlignmentOptions.TopLeft;
                break;
            case TextAnchor.UpperCenter:
                textMeshPro.alignment = TextAlignmentOptions.Top;
                break;
            case TextAnchor.UpperRight:
                textMeshPro.alignment = TextAlignmentOptions.TopRight;
                break;
            case TextAnchor.MiddleLeft:
                textMeshPro.alignment = TextAlignmentOptions.MidlineLeft;
                break;
            case TextAnchor.MiddleCenter:
                textMeshPro.alignment = TextAlignmentOptions.Midline;
                break;
            case TextAnchor.MiddleRight:
                textMeshPro.alignment = TextAlignmentOptions.MidlineRight;
                break;
            case TextAnchor.LowerLeft:
                textMeshPro.alignment = TextAlignmentOptions.BottomLeft;
                break;
            case TextAnchor.LowerCenter:
                textMeshPro.alignment = TextAlignmentOptions.Bottom;
                break;
            case TextAnchor.LowerRight:
                textMeshPro.alignment = TextAlignmentOptions.BottomRight;
                break;
        }

        textMeshPro.richText = richText;
        if (verticalWrapMode == VerticalWrapMode.Overflow)
        {
            textMeshPro.enableWordWrapping = true;
            textMeshPro.overflowMode = TextOverflowModes.Overflow;
        }
        else
        {
            textMeshPro.enableWordWrapping = horizontalWrapMode == HorizontalWrapMode.Overflow ? false : true;
        }

        textMeshPro.raycastTarget = raycastTarget;
        var font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Res/Font/AlibabaPuHuiTi-3500-Heavy SDF.asset");
        textMeshPro.font = font;
    }
    [MenuItem("Assets/工具/脚本Text转TMPro")]
    public static void ReplaceAllScriptTxt2MeshPro()
    {
        for(var i = 0;i<Selection.objects.Length;i++)
        {
            var obj = Selection.objects[i];
            var path = AssetDatabase.GetAssetPath(obj);
            if (path.EndsWith(".cs"))
            {
                var script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
                ReplaceScript(script);
            }
        }
    }
    private static void ReplaceScript(MonoScript script)
    {
        var listPattern = new List<string>()
        {
            " Text ","<Text>","Text "
        };
        var listPatternReplace = new List<string>()
        {
            " TextMeshProUGUI ","<TextMeshProUGUI>","TextMeshProUGUI "
        };
        var text = "";
        var path = AssetDatabase.GetAssetPath(script);
        text = File.ReadAllText(path);
        if (!text.Contains("using TMPro;"))
        {
            text = $@"using TMPro;
{text}
";
        }
        for (var i = 0; i < listPattern.Count; i++)
        {
            var matches = new List<Match>();
            var pattern = listPattern[i];
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Multiline);
            
            // 找到所有匹配的Button
            var tempMatches = regex.Matches(text);
            matches.AddRange(tempMatches.Cast<Match>());
            foreach (var match in matches)
            {
                var value = match.Value;
                var newValue = listPatternReplace[i];
                text = text.Replace(value, newValue);
            }
            
        }
        
        File.WriteAllText(path, text);

        AssetDatabase.Refresh();
    }
   
    

    [MenuItem("Assets/工具/Text转TMPro(重新赋值)")]
    static void GetSelectedScript()
    {
        Debug.Log($"数量{Selection.gameObjects.Length}");
        if (Selection.gameObjects.Length == 0)
        {
            return;
        }

        foreach (var obj in Selection.gameObjects)
        {
            var path = AssetDatabase.GetAssetPath(obj);
            var prefab1 = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            var prefab = PrefabUtility.InstantiatePrefab(prefab1) as GameObject;
            // var prefab = PrefabUtility.LoadPrefabContents(path);
            if (prefab != null)
            {
                var idList = prefab.GetComponentsInChildren<MyIdentifier>(true).ToList();
                SerializedObject serializedObject = new SerializedObject(prefab);
                var scripts = prefab.GetComponents<MonoBehaviour>().Where(s=>!IsUIComponent(s)).ToList();
                foreach (var script in scripts)
                {
                    var components = script.GetComponents<Component>().Where(s=>!IsUIComponent(s)).ToList();
                    foreach (var component in components)
                    {
                        var fields = component.GetAllFields();   
                        foreach (var field in fields)
                        {
                            if (field.FieldType == typeof(TextMeshProUGUI))
                            {
                                for (var i = 0; i < idList.Count; i++)
                                {
                                    if (idList[i].identifier == field.Name)
                                    {
                                        field.SetValue(script, idList[i].GetComponent<TextMeshProUGUI>());
                                        var identifier = idList[i];
                                        idList.Remove(idList[i]);
                                        DestroyImmediate(identifier);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            PrefabUtility.SaveAsPrefabAsset(prefab, path, out bool success);
            DestroyImmediate(prefab);
            if (!success)
            {
                Debug.LogError($"预制体：{path} 保存失败!");
            }
        }
    }
    
}

public static class UGUITools
{
    public static FieldInfo[] GetAllFields(this Component component)
    {
        return component.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
    }
}