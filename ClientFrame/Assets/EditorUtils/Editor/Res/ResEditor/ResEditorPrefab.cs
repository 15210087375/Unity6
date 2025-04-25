using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ExtendUI;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ZhEditor
{
    //todo 批量修改路径
    //todo 批量修改脚本
    //todo 批量修改tag
    public class ResEditorPrefab: ResEditorResBase
    {
        [CustomValueDrawer("DrawSeparator")]
        public bool showSeparator1;
        
        [LabelText("Prefab 列表")]
        [ListDrawerSettings( HideAddButton = true, DraggableItems = false, HideRemoveButton = true,NumberOfItemsPerPage = 4)]
        public List<PrefabShowData> PrefabShowDataList = new List<PrefabShowData>();

        // protected EditorResType ResType = EditorResType.Prefab;
        
        public override void Refresh()
        {
            base.Refresh();
            RefreshShow();
        }
        public override void InitSelectObject(Type t)
        {
            base.InitSelectObject(AstType);
            RefreshShow();
        }

        private void RefreshShow()
        {
            var list = GetObjectByType<GameObject>(SelectObject);
            PrefabShowDataList.Clear();
            for(var i = 0; i < list.Count; i++)
            {
                var obj = AssetDatabase.LoadAssetAtPath<GameObject>(list[i].path);
                var data = new PrefabShowData(obj, list[i].path);
                PrefabShowDataList.Add(data);
            }
        }
    
        public ResEditorPrefab(EditorResType resType,Type assetType) : base(resType,assetType)
        {
            ResType = resType;
            AstType = assetType;
        }
        
       

       
       
        [Title("批量处理")]
        [PropertySpace(20)]
        [HideLabel]
        [CustomValueDrawer("DrawNull")]
        public bool ShowSeparator2;
        
        [BoxGroup("基础控件")]
        [ToggleLeft]
        [LabelText(" button改为custom")]
        public bool OpTypeCustomBtn;
        
        
        [BoxGroup("基础控件")]
        [Button("替换基础控件",ButtonSizes.Large)]
        public void ChangeCustom()
        {
            var types = new List<OperationType>();
            if (OpTypeCustomBtn)
            {
                types.Add(OperationType.ChangeButtonToCustom);
            }
            ChangePrefabs(types);
        }
        
        
        
        
        private void ChangePrefabs(IReadOnlyList<OperationType> types)
        {
            foreach (var data in PrefabShowDataList)
            {
                var prefab = PrefabUtility.LoadPrefabContents(data.Path);
                var opCount = 0;
                for (var i = 0; i < types.Count; i++)
                {
                    var type = types[i];
                    switch (type)
                    {
                        case OperationType.ChangeButtonToCustom:
                            var btnAry = prefab.GetComponentsInChildren<Button>(true);
                            foreach (var btn in btnAry)
                            {
                                ChangeButtonToCustom(btn.gameObject);
                            }
                            if (btnAry.Length > 0)
                            {
                                opCount++;
                            }
                            break;
                        case OperationType.ChangeScript:
                            break;
                        case OperationType.ChangeTag:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(type), type, null);
                    }

                    if (opCount > 0)
                    {
                        
                        PrefabUtility.SaveAsPrefabAsset(prefab, data.Path);
                    }
                }
               
               
            }

            AssetDatabase.Refresh();
        }
        
        
        //替换 Button 组件为 CustomButton 组件
        private void ChangeButtonToCustom(GameObject go)
        {
            // //置换数据
            var custom = go.GetComponent<CustomButton>();
            if (custom == null)
            {
                var tempBtn = go.GetComponent<Button>();
                var transition = tempBtn.transition;
                var onClick = tempBtn.onClick;
                var targetGraphic = tempBtn.targetGraphic;
                var colors = tempBtn.colors;
                var spriteState = tempBtn.spriteState;
                var animationTriggers = tempBtn.animationTriggers;
                var navigation = tempBtn.navigation;
            
                UnityEngine.Object.DestroyImmediate(tempBtn);
            
                custom = go.AddComponent<CustomButton>();
                custom.transition = transition;
                custom.onClick = onClick;
                custom.targetGraphic = targetGraphic;
                custom.colors = colors;
                custom.spriteState = spriteState;
                custom.animationTriggers = animationTriggers;
                custom.navigation = navigation;
            }
            
      
        }
        [BoxGroup("自定义修改")]
        [Button("修改Canvas",ButtonSizes.Large)]
        public void ChangeCustomValue()
        {
            foreach (var data in PrefabShowDataList)
            {
                var prefab = PrefabUtility.LoadPrefabContents(data.Path);
                var canvass = prefab.GetComponentsInChildren<Canvas>(true);
                for (var i = 0; i < canvass.Length; i++)
                {
                    var canvas = canvass[i];
                    canvas.sortingOrder *= 100;
                }
                PrefabUtility.SaveAsPrefabAsset(prefab, data.Path);
               
            }

            AssetDatabase.Refresh();
        }

        public enum OperationType
        {
            [LabelText("Button=>CustomButton")]
            ChangeButtonToCustom,
            ChangeScript,
            ChangeTag
        }
        
        
        
        
        
        
        
        
        
        
        
        
        [Serializable]
        public class PrefabShowData
        {
            [PropertySpace(5)]
            [ReadOnly,HideLabel]
            public GameObject obj;
            private string _path;
            [PropertyOrder(0), ShowInInspector, HideLabel, InlineButton("Copy", "复制路径"), InlineButton("Open", "打开资源"),InlineButton("GetDependencies","查看引用")]
            public string Path
            {
                get=>_path;
                private set
                {
                    if (string.IsNullOrEmpty(_path))
                    {
                        _path = value;
                    }
                    
                }
            }
            public PrefabShowData(GameObject obj, string path)
            {
                this.obj = obj;
                this._path = path;
            }
            private void Copy()
            {
                EditorGUIUtility.systemCopyBuffer = Path;
            }
            private void Open()
            {
                AssetDatabase.OpenAsset(obj);
            }

            private void GetDependencies()
            {
                var window = EditorWindow.GetWindow<ResEditorSearchDependencies>();
                window.InitObject(obj);
                window.Show();
            }
        }
    }

}
