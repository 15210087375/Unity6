using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ZhEditor
{
    public class ResEditorResBase
    {
       
        protected EditorResType ResType;
        public Type AstType;
        [Title("选择资源")]
        [HorizontalGroup("选择资源",width:0.78f)]
        [PropertyOrder(0),HideLabel]//SdfIconType.ArrowClockwise
        [OnValueChanged("Refresh")]
        [LabelText("选中的资源列表")]
        [ListDrawerSettings(NumberOfItemsPerPage = 8)]//OnBeginListElementGUI = "ListBeginElementGUI"
        public List<Object> SelectObject ;

        
        // 在列表元素开始处绘制自定义 GUI 元素。
        private void ListBeginElementGUI(int index)
        {
            // if (isSet)
            // {
            //     Logger.Error( SelectObject.Count.ToString());
            //     GUI.backgroundColor = (index < SelectObject.Count-1) ? (isLock? Color.red : Color.grey):Color.white;
            //     isSet = false;
            // }
            //
        }

        private bool _isLock;
        [PropertySpace(25)]
        [HorizontalGroup("选择资源", width: 0.1f)]
        [HideLabel]
        [ShowIf("@_isLock == false")]
        [Button("锁定", ButtonSizes.Large, ButtonStyle.Box, Icon = SdfIconType.Unlock), PropertyOrder(10)]
        public void LockSelect()
        {
            _isLock = true;
        }
        [PropertySpace(25)]
        [HorizontalGroup("选择资源", width: 0.1f)]
        [HideLabel]
        [ShowIf("@_isLock == true")]
        [Button("解锁", ButtonSizes.Large, ButtonStyle.Box, Icon = SdfIconType.Lock), PropertyOrder(10)]
        public void UnLockSelect()
        {
            _isLock = false;
        }
        [PropertySpace(25)]
        [HorizontalGroup("选择资源",width:0.1f)]
        [Button("",ButtonSizes.Large,ButtonStyle.Box,Icon = SdfIconType.ArrowClockwise),PropertyOrder(11)]
        public virtual void Refresh()
        {
           
        }
        
        [CustomValueDrawer("DrawLock")]
        [ShowIf("_isLock")]
        [HorizontalGroup("选择资源1",width:0.78f)]
        public string test;
        
        
        protected ResEditorResBase( EditorResType resType,Type astType)
        {
            ResType = resType;
            this.AstType = astType;
        }
        
        public virtual void InitSelectObject(Type t)
        {
            if(_isLock)
            {
                return;
            }
            if(Selection.objects.Length == 0)
            {
                return;
            }
            var list = new List<Object>();
            for (var i = 0; i < Selection.objects.Length; i++)
            {
                var obj = Selection.objects[i];
                var path = AssetDatabase.GetAssetPath(obj);
                if (AssetDatabase.IsValidFolder(path))
                {
                    list.Add(obj);
                }
                else
                {
                    var assetType = AssetDatabase.GetMainAssetTypeAtPath(path);
                    if (assetType != null && (assetType.IsSubclassOf(t) || assetType.IsAssignableFrom(t)))
                    {
                        list.Add(obj);
                    }
                }
            }
            SelectObject = list;
        }
       

        protected List<EditorResData> GetObjectByType<T>(List<Object> list) where T : Object
        {
            var result = new List<EditorResData>();
            if(list == null)
            {
                return result;
            }
            foreach (var selectObj in list)
            {
                var path = AssetDatabase.GetAssetPath(selectObj);
                if (AssetDatabase.IsValidFolder(path))
                {
                    var objs = AssetDatabase.FindAssets($"t:{ResType}", new[] { path });
                    foreach (var obj in objs)
                    {
                        var objPath = AssetDatabase.GUIDToAssetPath(obj);
                        var data = new EditorResData()
                        {
                            resType = typeof(T),
                            path = objPath
                        };

                        result.Add(data);
                    }
                }
                else
                {
                    var assetType = AssetDatabase.GetMainAssetTypeAtPath(path);
                    if (assetType!= null &&( assetType.IsSubclassOf(typeof(T))  || assetType.IsAssignableFrom(typeof(T))))
                    {
                        var data = new EditorResData()
                        {
                            resType = assetType,
                            path = path
                        };
                        result.Add(data);
                    }
                }
            }

            return result;
        }
        private void Clear()
        {
            SelectObject.Clear();
            
        }

        protected void DrawLock()
        {
            EditorGUILayout.HelpBox("已锁定", MessageType.Info);
        }
        protected void DrawNull()
        {
           
        }
        private void DrawSeparator(bool value)
        {
            UnityEditor.EditorGUILayout.Separator();
            SirenixEditorGUI.HorizontalLineSeparator();
        }

       
    }

}
