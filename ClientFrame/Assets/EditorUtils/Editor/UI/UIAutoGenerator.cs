using System;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector.Editor;
using Unity.VisualScripting;
using Object = UnityEngine.Object;

public class UIAutoGenerator : OdinEditorWindow
{
    private string tipDesc;
    private bool showTip;
    private const string MenuItemText = "Assets/Create/UI预设";
    [MenuItem(MenuItemText, false, -1000)]
    private static void OpenWindow()
    {
        
        GetWindow<UIAutoGenerator>().Show();
    }

    protected override void Initialize()
    {
        base.Initialize();
        if(Selection.activeObject != null)
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            var isFolder = AssetDatabase.IsValidFolder(path);
            if (!isFolder)
            {
                path = System.IO.Path.GetDirectoryName(path)?.Replace("\\", "/");
            }
            
            if(path == null)
            {
                Debug.LogError("路径错误");
                return;
            }
            if(path.StartsWith(EditorUtilsDefine.UIScriptPath))
            {
                scriptPath = path;
                prefabPath = path.Replace(EditorUtilsDefine.UIScriptPath, EditorUtilsDefine.UIPrefabPath);
            }else if(path.StartsWith(EditorUtilsDefine.UIPrefabPath))
            {
              
         
                prefabPath = path;
                scriptPath = path.Replace(EditorUtilsDefine.UIPrefabPath, EditorUtilsDefine.UIScriptPath);
            }
           
        }
        
    }

    [BoxGroup("Settings")]
    [LabelText("Class Name")]
    public string className = "LayerTemp";

    [BoxGroup("Settings")]
    [LabelText("脚本路径")]
    [FolderPath]
    public string scriptPath = "Assets/Game/Scripts/GameClient/UI/Home";

    [BoxGroup("Settings")]
    [LabelText("预设路径")]
    [FolderPath]
    public string prefabPath = "Assets/Res/UI/Prefabs/Home";

    [Button("生成脚本和预设")]
    private void GenerateUI()
    {
        if (string.IsNullOrEmpty(className))
        {
            Debug.LogError("Class name cannot be empty!");
            return;
        }
        //如果路径不存在则创建路径
        if (!Directory.Exists(scriptPath))
        {
            Directory.CreateDirectory(scriptPath);
        }
        if (!Directory.Exists(prefabPath))
        {
            Directory.CreateDirectory(prefabPath);
        }

        var uiType = EditorUtilsDefine.GetUIType(className);
        var prefabFilePath = Path.Combine(prefabPath, $"{className}.prefab");
        var scriptFilePath = Path.Combine(scriptPath, $"{className}.cs");

        var scriptSuc = GenerateScript(scriptFilePath);
        WriteUIPathDefine(prefabFilePath,uiType);
        var prefabSuc = GeneratePrefab(prefabFilePath);
    
        // Close();
        showTip = true;
        tipDesc = $"UI Script and Prefab generated successfully: {className}";
        // AssetDatabase.Refresh();
        if (!scriptSuc || !prefabSuc)
        {
            return;
        }
        Debug.Log($"UI Script and Prefab generated successfully: {className}");
    }
    
    //刷新UIPathDefine
    private void WriteUIPathDefine(string prefabFilePath,UIType uiType)
    {
        var loadPath = prefabFilePath.Replace($"{EditorUtilsDefine.UIPrefabPath}/", "").Replace("\\", "/");
        Debug.Log(prefabFilePath);
        Debug.Log(loadPath);
        var tempPath = EditorUtilsDefine.UIPathDefinePath;
        var scriptContent = File.ReadAllText(tempPath);
        string searchText = "//WindowPath Tag";
        string replaceText = $@"{{ WindowID.{className}, new UIPath(WindowID.ViewSetting, ""{loadPath}"", LayerIndex.{(uiType == UIType.Layer?"Layer":"View")}) }},
        //WindowPath Tag";
        
        var searchText1 = "//WindowID Tag";
        var replaceText1 = $@"{className},
    //WindowID Tag";
        scriptContent = Regex.Replace(scriptContent, searchText, replaceText);
        scriptContent = Regex.Replace(scriptContent, searchText1, replaceText1);
        
        File.WriteAllText(tempPath, scriptContent);
    }
    //生成脚本
    private bool GenerateScript(string scriptFilePath)
    {
        string scriptContent = $@"
using UnityEngine;
using UnityEngine.UI;

public class {className} : {EditorUtilsDefine.GetUITypeBase(className)}
{{
   
 

    void Start()
    {{
        // Initialize your UI elements here
    }}
}}";
        // 判断当前路径是否已存在
        if (File.Exists(scriptFilePath))
        {
            Debug.LogError($"Script already exists: {className}.cs",AssetDatabase.LoadMainAssetAtPath(scriptFilePath));
            return false;
        }
        File.WriteAllText(scriptFilePath, scriptContent);
       
        return true;
    }
    //生成预制体
    private bool GeneratePrefab(string prefabFilePath)
    {
        // 判断当前路径是否已存在
        if (File.Exists(prefabFilePath))
        {
            Debug.LogError($"Prefab already exists: {className}.prefab", AssetDatabase.LoadMainAssetAtPath(prefabFilePath));
            return false;
        }
        var uiType = EditorUtilsDefine.GetUIType(className);
        var tempPath = EditorUtilsDefine.GetUIPrefabTemplate(uiType);
        //根据tempPath加载模板预设，然后保存为新的预设
        var prefabTemplate = AssetDatabase.LoadAssetAtPath(tempPath, typeof(GameObject));
        var prefab = Object.Instantiate(prefabTemplate) as GameObject;
        PrefabUtility.SaveAsPrefabAsset(prefab, prefabFilePath);

        DestroyImmediate(prefab);
        return true;
    }


    [Button("挂载脚本")]
    private void AddUI()
    {
        var prefabFilePath = Path.Combine(prefabPath, $"{className}.prefab");
        var prefabTemplate = AssetDatabase.LoadAssetAtPath(prefabFilePath, typeof(GameObject));
        var prefab = Object.Instantiate(prefabTemplate) as GameObject;
        
        var type = GetTypeFromScriptPath();
        if(type == null)
        {
            Debug.LogError($"Cannot find class {className}");
            return;
        }

        if (prefab == null)
        {
            Debug.LogError($"Cannot find prefab {className}");
            return;
        }
        if(prefab.GetComponent(type) != null)
        {
            Debug.LogError($"Already added {className} to prefab");
            return;
        }
        prefab.AddComponent(type);
        PrefabUtility.SaveAsPrefabAsset(prefab, prefabFilePath);

        DestroyImmediate(prefab);
        showTip = true;
        tipDesc = $"UI Script added to prefab successfully: {className}";
        Debug.Log($"UI Script added to prefab successfully: {className}");
    }
   
    private Type GetTypeFromScriptPath()
    {
        // 遍历所有程序集，查找匹配的类型
        return AppDomain.CurrentDomain.GetAssemblies().Select(assembly => assembly.GetType(className)).FirstOrDefault(type => type != null);
    }


    protected override void OnGUI()
    {
        base.OnImGUI();
        if (showTip)
        {
            EditorGUILayout.HelpBox(tipDesc, MessageType.Info);
        }
    }
    
    public void ReplaceScript(string path,string searchText, string replaceText)
    {
        StreamReader reader = new StreamReader(path);
        string text = reader.ReadToEnd();
        reader.Close();
        // 替换文本中的指定内容
        text = Regex.Replace(text, searchText, replaceText);
        // 写回更改后的文本内容
        StreamWriter writer = new StreamWriter(path);
        writer.Write(text);
        writer.Flush();
        writer.Close();
        AssetDatabase.Refresh();
    }
}