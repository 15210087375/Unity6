using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace ZhEditor
{
    public class ResEditorTexture : ResEditorResBase
    {
        [Title("筛选")] [EnumToggleButtons, HideLabel] [OnValueChanged("CheckTexture")] [PropertyOrder(20)]
        public TextureCheckType CheckType = TextureCheckType.None;

        [Space]
        [LabelText("Texture 列表")]
        [ListDrawerSettings(HideAddButton = true, DraggableItems = false, HideRemoveButton = true,
            NumberOfItemsPerPage = 4)]
        [PropertyOrder(21)]
        public List<TextureShowData> TextureShowDataList = new List<TextureShowData>();


        private List<TextureShowData> _tempAllShowData = new List<TextureShowData>();

        public override void Refresh()
        {
            base.Refresh();
            CheckType = TextureCheckType.None;
            RefreshShow();
        }

        public override void InitSelectObject(Type t)
        {
            base.InitSelectObject(t);
            RefreshShow();
        }

        private void RefreshShow()
        {
            var list = GetObjectByType<Texture>(SelectObject);
            TextureShowDataList.Clear();
            for (var i = 0; i < list.Count; i++)
            {
                var path = list[i].path;
                if (!IsNormalImage(path))
                {
                    continue;
                }

                var texture = AssetDatabase.LoadAssetAtPath<Texture>(path);
                var data = new TextureShowData(texture, list[i].path,MoveToLast);
                TextureShowDataList.Add(data);
            }

            _tempAllShowData = new List<TextureShowData>(TextureShowDataList);
            CheckTexture();
        }

        private void MoveToLast(string path)
        {
            var data = _tempAllShowData.Find(x => x.Path == path);
            if (data != null)
            {
                _tempAllShowData.Remove(data);
                _tempAllShowData.Add(data);
            }
            TextureShowDataList = new List<TextureShowData>(_tempAllShowData);
        }

        public ResEditorTexture(EditorResType resType, Type astType) : base(resType, astType)
        {
            ResType = resType;
        }

        private bool IsNormalImage(string path)
        {
            return path.EndsWith(".png") || path.EndsWith(".jpg") || path.EndsWith(".jpeg");
        }


        private void CheckTexture()
        {
            var list = new List<TextureShowData>();
            foreach (var data in _tempAllShowData)
            {
                if (data.texture != null)
                {
                    var path = AssetDatabase.GetAssetPath(data.texture);
                    var importer = AssetImporter.GetAtPath(path) as TextureImporter;

                    if (importer != null)
                    {
                        switch (CheckType)
                        {
                            case TextureCheckType.None:
                                list.Add(data);
                                break;
                            case TextureCheckType.BigTexture:
                                importer.GetSourceTextureWidthAndHeight(out var width, out var height);
                                if (width > 1024 || height > 1024)
                                {
                                    list.Add(data);
                                }

                                break;
                            case TextureCheckType.Mipmap:
                                if (importer.mipmapEnabled == true)
                                    list.Add(data);
                                break;
                            case TextureCheckType.NoCompress:
                                if (importer.textureCompression == TextureImporterCompression.Uncompressed)
                                    list.Add(data);
                                break;
                            case TextureCheckType.ReadWrite:
                                if (importer.isReadable == true)
                                    list.Add(data);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }

            TextureShowDataList = list;
        }
      
        
     

     
        [Button("选中筛选的资源", ButtonSizes.Large, ButtonStyle.Box)]
        [PropertyOrder(61)]
        public void Execute()
        {
            if (TextureShowDataList.Count == 0)
            {
                return;
            }

            var objs = new List<Object>();
            for (var i = 0; i < TextureShowDataList.Count; i++)
            {
                var path = AssetDatabase.GetAssetPath(TextureShowDataList[i].texture);
                var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
                objs.Add(obj);
            }
            // ReSharper disable once CoVariantArrayConversion
            // var objs = TextureShowDataList.Select(x => (x.texture as Object)).ToArray();
            Selection.activeObject = objs[0];
            Selection.objects = objs.ToArray();
           
            AssetDatabase.Refresh();
            AssetDatabase.RefreshSettings();
        }
       
        [Serializable]
        public class TextureShowData
        {
            [PreviewField(40)] [HideLabel][ReadOnly] [HorizontalGroup("Img", Width = 40)] [ShowIf("@texture != null")]
            public Texture texture;


            private string path;

            [BoxGroup("Img/desc", ShowLabel = false)]
            [PropertyOrder(0), ShowInInspector, HideLabel, InlineButton("Copy", "复制路径"),
             InlineButton("GetDependencies", "查看引用"),InlineButton("SetDirty", "设置修改")]
            public string Path
            {
                get => path;
                private set
                {
                    if (string.IsNullOrEmpty(path))
                    {
                        path = value;
                    }
                }
            }


            [ReadOnly]
            [BoxGroup("Img/desc", ShowLabel = false)]
            [HideLabel, PropertyOrder(1)]
            [ProgressBar(0, 100, DrawValueLabel = false)]
            public int value;
            
            private Action<string> action;
            public TextureShowData(Texture tex, string path,Action<string> action)
            {
                this.texture = tex;
                this.path = path;
                this.action = action;
                value = 0;
            }

            private void SetDirty()
            {
                value = value == 0 ? 100 : 0;
                if (value == 100 )
                {
                    action?.Invoke(Path);
                }
            }
            private void Copy()
            {
                EditorGUIUtility.systemCopyBuffer = Path;
            }

            private void GetDependencies()
            {
                var window = EditorWindow.GetWindow<ResEditorSearchDependencies>();
                window.InitObject(texture);
                window.Show();
            }
        }
    }
}