using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
/// <summary>
/// 为脚本添加partial修饰符的菜单项
/// </summary>
public class ScriptPartialMenu
{
    [MenuItem("Assets/Create/Add Partial", false, 80)]
    private static void AddPartialModifier()
    {
        // 获取选中的脚本文件
        Object[] selectedObjects = Selection.GetFiltered<Object>(SelectionMode.Assets);
        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("请选择一个脚本文件！");
            return;
        }

        var path = AssetDatabase.GetAssetPath(Selection.objects[0]);
         if (!path.EndsWith(".cs"))
         {
             Debug.LogWarning($"文件 {path} 不是C#脚本文件！");
             return;
         }

         // 读取文件内容
         string content = File.ReadAllText(path);

         // 检查是否已经有partial修饰符
         if (!content.Contains(" partial class "))
         {
             // 添加partial修饰符
             content = Regex.Replace(content, @"(public|private|protected|internal)?\s+class\s+(\w+)",
                 match => match.Value.Replace("class", "partial class"));

             // 写回文件
             File.WriteAllText(path, content);
         }
         CreatePartialScript();
         // 刷新资源数据库
         AssetDatabase.Refresh();

         Debug.Log($"已为脚本 {path} 添加partial修饰符！");
    }

    [MenuItem("Assets/Create/Add Partial", true)]
    private static bool ValidateAddPartialModifier()
    {
        // 验证是否选中了脚本文件
        Object[] selectedObjects = Selection.GetFiltered<Object>(SelectionMode.Assets);
        if (selectedObjects.Length == 0)
            return false;
        if (selectedObjects.Length != 1)
            return false;
        foreach (Object obj in selectedObjects)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            if (!path.EndsWith(".cs"))
                return false;
        }

        return true;
    }

    private static void CreatePartialScript()
    {
        // 获取选中的脚本文件
        Object[] selectedObjects = Selection.GetFiltered<Object>(SelectionMode.Assets);
     
        Object obj = selectedObjects[0];
        string path = AssetDatabase.GetAssetPath(obj);
        if (!path.EndsWith(".cs"))
        {
            Debug.LogWarning($"文件 {path} 不是C#脚本文件！");
            return;
        }

        // 获取原始脚本的类名
        string content = File.ReadAllText(path);
        Match match = Regex.Match(content, @"(public|private|protected|internal)?\s+(?:partial\s+)?class\s+(\w+)");
        if (!match.Success)
        {
            Debug.LogWarning($"无法在脚本 {path} 中找到类定义！");
            return;
        }

        string className = match.Groups[2].Value;
        string accessModifier = match.Groups[1].Value;
        if (string.IsNullOrEmpty(accessModifier))
        {
            accessModifier = "public";
        }

        // 创建新的脚本路径
        string directory = Path.GetDirectoryName(path);
        string fileName = Path.GetFileNameWithoutExtension(path);
        string newFileName = $"{fileName}.Partial.cs";
        string newPath = Path.Combine(directory, newFileName);

        // 检查文件是否已存在
        if (File.Exists(newPath))
        {
            Debug.LogWarning($"文件 {newPath} 已存在！");
            return;
        }

        // 创建新的partial脚本内容
        string newContent = $@"using UnityEngine;

{accessModifier} partial class {className}
{{
    // TODO: 在这里添加partial类的实现
}}";

        // 写入新文件
        File.WriteAllText(newPath, newContent);
        
        // 刷新资源数据库
        AssetDatabase.Refresh();
        
        Debug.Log($"已创建partial脚本：{newPath}");
        // 选中创建的脚本
        Object newScript = AssetDatabase.LoadAssetAtPath<Object>(newPath);
        if (newScript != null)
        {
            // Selection.objects = new Object[] { newScript };
            Selection.activeObject = newScript;
            // //将选中脚本设置为改名状态
            // EditorUtility.FocusProjectWindow();
            // EditorGUIUtility.PingObject(newScript);
            // EditorApplication.ExecuteMenuItem("Assets/Rename");
            
            
        }
        else
        {
            Debug.LogWarning($"无法选中创建的脚本：{newPath}");
        }
    }

   
} 