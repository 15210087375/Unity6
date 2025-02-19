using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class ForeachEditorTools : EditorWindow
{
    private const string MenuItemText = "Assets/工具/查找引用当前物体的资源";
    private static List<Object> _objs = new List<Object>();
    private Vector2 _pos = Vector2.zero;
    [MenuItem(MenuItemText, false, 25)]
    public static void Find()
    {
        var window = (ForeachEditorTools)EditorWindow.GetWindow(typeof(ForeachEditorTools));
        window.titleContent = new GUIContent("Object List");
        window.Show();
        
       
    }

    private void Search()
    {
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        var referenceCache = new Dictionary<string, List<string>>();
        _objs = new List<Object>();
        var guids = AssetDatabase.FindAssets("");
        foreach (var guid in guids)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var dependencies = AssetDatabase.GetDependencies(assetPath, false);

            foreach (var dependency in dependencies)
            {
                if (referenceCache.ContainsKey(dependency))
                {
                    if (!referenceCache[dependency].Contains(assetPath))
                    {
                        referenceCache[dependency].Add(assetPath);
                    }
                }
                else
                {
                    referenceCache[dependency] = new List<string>() { assetPath };
                }
            }
        }
        Debug.Log("Build index takes " + sw.ElapsedMilliseconds + " milliseconds");
        sw.Stop();
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        Debug.Log("Find: " + path, Selection.activeObject);
        if (referenceCache.ContainsKey(path))
        {
            foreach (var reference in referenceCache[path])
            {
                Debug.Log(reference, AssetDatabase.LoadMainAssetAtPath(reference));
                var prefabObj = AssetDatabase.LoadAssetAtPath<Object>(reference);
                if (!_objs.Contains(prefabObj))
                {
                    _objs.Add(prefabObj);
                }
            }
        }
        else
        {
            Debug.LogWarning("No references");
        }

        referenceCache.Clear();
    }

    [MenuItem(MenuItemText, true)]
    public static bool Validate()
    {
        if (!Selection.activeObject) return false;
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        return !AssetDatabase.IsValidFolder(path);

    }

    private void OnBecameVisible()
    {
        Search();
    }
    private void OnEnable()
    {
        
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
       
    }

    private void OnHierarchyChanged()
    {
        Repaint();
    }
    
    private void OnGUI()
    {
        // 在 EditorWindow 中使用 GUILayout 绘制 UI 元素
        GUILayout.Label("引用了当前资源的Asset列表", EditorStyles.boldLabel);

        Rect rect = new Rect(10, 20, position.width-20, position.height-30);
        GUI.Box(rect, "My Border");

        // 添加一些文本
        GUILayout.BeginArea(rect);
        if (_objs is { Count: > 0 })
        {
            _pos = GUILayout.BeginScrollView(_pos);
            foreach (var obj in _objs)
            {
                EditorGUILayout.ObjectField(obj, typeof(Object), false);
            }

            GUILayout.EndScrollView();
        }
        GUILayout.EndArea();
        

    }

    
    [MenuItem("Assets/工具/批量移除指定组件")]
    public static void RemoveAllRigidbody()
    {

        // 获取目录下所有的预制体资源
        foreach (var obj in Selection.objects)
        {
            if (!PrefabUtility.IsPartOfPrefabAsset(obj)) continue;
            var go = Instantiate(obj) as GameObject;
            //这里是你要移除的组件
            if (go != null)
            {
                var uiComps = go.GetComponentsInChildren<Image>(true);
                foreach (var uiComp in uiComps)
                {
                    // DestroyImmediate(uiComp, true);
                }
            }
            // TraversePrefab(go.transform, 0); // 从 prefab 的根节点开始遍历
            PrefabUtility.SaveAsPrefabAsset(go, PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(obj)); 
            AssetDatabase.SaveAssets();
            DestroyImmediate(go);
        }
        
    }
  
  

    // [MenuItem("Assets/工具/批量移动特效的依赖")]
    public static void MoveAllEffectDependences()
    {
        string selectedFolderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!selectedFolderPath.Contains("Assets/Res/Effects"))
        {
            Debug.LogError("请选择Effects目录下的文件夹");
            return;
        }

        foreach (var folderPath in Selection.GetFiltered(typeof(DefaultAsset), SelectionMode.Assets))
        {
            string assetPath = AssetDatabase.GetAssetPath(folderPath);
            // 遍历所有的预制体资源
            var prefabPaths = AssetDatabase.FindAssets("t:Prefab", new[] { assetPath });
            // 获取目录下所有的预制体资源
            foreach (var prefabPath in prefabPaths)
            {
                string szPrefabPath = AssetDatabase.GUIDToAssetPath(prefabPath);
                string[] deps = AssetDatabase.GetDependencies(szPrefabPath);
                foreach (var dep in deps)
                {
                    if (dep.EndsWith(".mat"))
                    {
                        // 创建Materials文件夹
                        string szSelectedMatPath = selectedFolderPath + "/Materials/";
                        if (!Directory.Exists(szSelectedMatPath))
                        {
                            Directory.CreateDirectory(szSelectedMatPath);
                        }

                        int pos = _CheckDepDir(dep, selectedFolderPath);
                        // 0: 不在Effects目录下，移动到选定目录下
                        if (pos == 0)
                        {
                            if (File.Exists(dep))
                            {
                                string szDstFilePath = szSelectedMatPath + "/" + Path.GetFileName(dep);
                                string szDstMetaFilePath = szDstFilePath + ".meta";
                                if (!File.Exists(szDstFilePath))
                                {
                                    File.Move(dep, szDstFilePath);
                                    File.Move(dep + ".meta", szDstMetaFilePath);
                                }
                            }
                        }
                        // 1: 在选定目录下，什么都不做
                        else if (pos == 1)
                        {
                        }
                        // 3: 在EffectCommons目录下，什么都不做
                        else if (pos == 3)
                        {
                        }
                        // 2: 在其他Effects目录下，移动到EffectCommons目录下
                        else if (pos == 2)
                        {
                            if (File.Exists(dep))
                            {
                                string szDstFilePath = "Assets/Res/EffectCommons/Materials/" + Path.GetFileName(dep);
                                string szDstMetaFilePath = szDstFilePath + ".meta";
                                if (!File.Exists(szDstFilePath))
                                {
                                    File.Move(dep, szDstFilePath);
                                    File.Move(dep + ".meta", szDstMetaFilePath);
                                }
                            }
                        }
                    }
                    else if (dep.EndsWith(".png"))
                    {
                        // 创建Textures文件夹
                        string szSelectedMatPath = selectedFolderPath + "/Textures/";
                        if (!Directory.Exists(szSelectedMatPath))
                        {
                            Directory.CreateDirectory(szSelectedMatPath);
                        }

                        int pos = _CheckDepDir(dep, selectedFolderPath);
                        // 0: 不在Effects目录下，移动到选定目录下
                        if (pos == 0)
                        {
                            if (File.Exists(dep))
                            {
                                string szDstFilePath = szSelectedMatPath + "/" + Path.GetFileName(dep);
                                string szDstMetaFilePath = szDstFilePath + ".meta";
                                if (!File.Exists(szDstFilePath))
                                {
                                    File.Move(dep, szDstFilePath);
                                    File.Move(dep + ".meta", szDstMetaFilePath);
                                }
                            }
                        }
                        // 1: 在选定目录下，什么都不做
                        else if (pos == 1)
                        {
                        }
                        // 3: 在EffectCommons目录下，什么都不做
                        else if (pos == 3)
                        {
                        }
                        // 2: 在其他Effects目录下，移动到EffectCommons目录下
                        else if (pos == 2)
                        {
                            if (File.Exists(dep))
                            {
                                string szDstFilePath = "Assets/Res/EffectCommons/Textures/" + Path.GetFileName(dep);
                                string szDstMetaFilePath = szDstFilePath + ".meta";
                                if (!File.Exists(szDstFilePath))
                                {
                                    File.Move(dep, szDstFilePath);
                                    File.Move(dep + ".meta", szDstMetaFilePath);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    
    /// <summary>
    /// 0: 不在Effects目录下，1: 在选定目录下，2: 在其他Effects目录下，3: 在EffectCommons目录下
    /// </summary>
    /// <param name="dep"></param>
    /// <param name="selectedPath"></param>
    /// <returns></returns>
    private static int _CheckDepDir(string dep, string selectedPath)
    {
        bool bInSelectedPath = dep.Contains(selectedPath);
        bool bInEffectsPath = dep.Contains("Assets/Res/Effects");
        bool bInEffectCommonsPath = dep.Contains("Assets/Res/EffectCommons");
        bool bInSpriteRendererTexture = dep.Contains("Assets/Res/SpriteRendererTextures");
        bool bInAltas = dep.Contains("Assets/Res/Atlas");
        if (bInEffectCommonsPath || bInSpriteRendererTexture || bInAltas)
        {
            return 3;
        }
        else if (bInEffectsPath && bInSelectedPath)
        {
            return 1;
        }
        else if (bInEffectsPath && !bInSelectedPath)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }
}