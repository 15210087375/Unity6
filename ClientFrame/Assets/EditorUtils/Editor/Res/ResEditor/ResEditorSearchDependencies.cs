
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ZhEditor
{
    public class ResEditorSearchDependencies:OdinEditorWindow
    {
        private const string MenuItemText = "Assets/工具/查找引用当前物体的资源";
        
        [LabelText("当前查询的资源")]
        [OnValueChanged("RefreshResult")]
        public Object _curSelectObject;
        
        [ListDrawerSettings(ShowFoldout = true,DraggableItems = false,HideAddButton = true,HideRemoveButton = true),ShowIf("@objs.Count > 0")]
        [LabelText("引用")]
        public List<Object> objs = new List<Object>();
        
        [Space]
        [ListDrawerSettings(ShowFoldout = true,DraggableItems = false,HideAddButton = true,HideRemoveButton = true),ShowIf("@dependencyList.Count > 0")]
        [LabelText("依赖")]
        public List<object> dependencyList = new List<object>();
        [Space]
        [Title("无引用", TitleAlignment = TitleAlignments.Centered,Bold = true)]
        [CustomValueDrawer("DrawNull")]
        [ShowIf("@objs.Count == 0 && dependencyList.Count != 0")]
        public string myText = "";
        
        [Space]
        [Title("无依赖", TitleAlignment = TitleAlignments.Centered,Bold = true)]
        [CustomValueDrawer("DrawNull")]
        [ShowIf("@objs.Count != 0 && dependencyList.Count == 0")]
        public string myText1 = "";
        
        [Space]
        [Title("无引用&无依赖", TitleAlignment = TitleAlignments.Centered,Bold = true)]
        [CustomValueDrawer("DrawNull")]
        [ShowIf("@objs.Count == 0 && dependencyList.Count == 0")]
        public string myText2 = "";
        
        private Dictionary<string, List<string>> _referenceCache = new Dictionary<string, List<string>>(1024);
        private void DrawNull()
        {
            
        }

       

        [MenuItem(MenuItemText, false, 25)]
        public static void Find()
        {
            var window =GetWindow<ResEditorSearchDependencies>();
            if (window != null)
            {
                window.RefreshAll();
                window.Search();
                return;
            }
      
            window.Show();
        
       
        }
        [MenuItem(MenuItemText, true)]
        public static bool Validate()
        {
            if (!Selection.activeObject) return false;
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            return !AssetDatabase.IsValidFolder(path);

        }

        
        protected override void OnEnable()
        {
            base.OnEnable();
            titleContent = new GUIContent("Dependencies List");
            position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 500);
        }
      
        
        [ButtonGroup("操作")]
        [Button("刷新结果")]
        public void RefreshResult()
        {
            Search();
        }
        
        [ButtonGroup("操作")]
        [Button("刷新搜索数据库")]
        public void RefreshAll()
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            foreach (var kv in _referenceCache)
            {
                kv.Value.Clear();
            }
            _referenceCache.Clear();
            objs = new List<Object>();
            var guids = AssetDatabase.FindAssets("");
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var dependencies = AssetDatabase.GetDependencies(assetPath, false);

                foreach (var dependency in dependencies)
                {
                    if (_referenceCache.ContainsKey(dependency))
                    {
                        if (!_referenceCache[dependency].Contains(assetPath))
                        {
                            _referenceCache[dependency].Add(assetPath);
                        }
                    }
                    else
                    {
                        _referenceCache[dependency] = new List<string>() { assetPath };
                    }
                }
            }
            
            Debug.Log($"Get All Assets takes {sw.ElapsedMilliseconds} milliseconds");
            sw.Stop();
        }
        public void InitObject(Object obj)
        {
            _curSelectObject = obj;
            if (_curSelectObject)
            {
                RefreshAll();
            }
            Search();
        }

        private  void Search()
        {

            _curSelectObject ??= Selection.activeObject;
            if(_curSelectObject == null) return;
            var path = AssetDatabase.GetAssetPath(_curSelectObject);
            Debug.Log($"Find: {path}" , _curSelectObject);
            
            objs.Clear();
            if (_curSelectObject == null)
            {
                Debug.LogWarning("当前未选中资源");
                return;
            }
            if (_referenceCache.ContainsKey(path))
            {
                foreach (var reference in _referenceCache[path])
                {
                    Debug.Log(reference, AssetDatabase.LoadMainAssetAtPath(reference));
                    var prefabObj = AssetDatabase.LoadAssetAtPath<Object>(reference);
                    if (!objs.Contains(prefabObj))
                    {
                        objs.Add(prefabObj);
                    }
                }
            }
            else
            {
                Debug.LogWarning("No references");
            }
            
            dependencyList.Clear();
            var assetPath = AssetDatabase.GetAssetPath(_curSelectObject);
            var dependencies = AssetDatabase.GetDependencies(assetPath, false);
            foreach (var dependency in dependencies)
            {
                var obj = AssetDatabase.LoadAssetAtPath<Object>(dependency);
                dependencyList.Add(obj);
            }
        }

      

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Debug.Log("关闭搜索引用界面");
            _referenceCache.Clear();
            objs.Clear();
        }
   
    }
}

