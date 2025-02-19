
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using ZhEditor;
using Object = UnityEngine.Object;

public class ResEditorSpriteAtlas : ResEditorResBase
{

    [LabelText("SpriteAtlas 列表")]
    [ListDrawerSettings( HideAddButton = true, DraggableItems = false, HideRemoveButton = true,NumberOfItemsPerPage = 4)]
    public List<ResEditorSpriteAtlas.SpriteAtlasShowData> SpriteAtlasShowDataList = new List<ResEditorSpriteAtlas.SpriteAtlasShowData>();

    
    public ResEditorSpriteAtlas(EditorResType resType, Type astType) : base(resType, astType)
    {
        ResType = resType;
        AstType = astType;
    }
    
    
    public override  void Refresh()
    {
        base.Refresh();
        RefreshShow();
    }
    public override void InitSelectObject(Type t)
    {
        base.InitSelectObject(t);
        RefreshShow();
    }

    private void RefreshShow()
    {
        var list = GetObjectByType<SpriteAtlas>(SelectObject);
        SpriteAtlasShowDataList.Clear();
        for(var i = 0; i < list.Count; i++)
        {
            var path = list[i].path;
            
            var spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);
            var data = new ResEditorSpriteAtlas.SpriteAtlasShowData(spriteAtlas,list[i].path);
            SpriteAtlasShowDataList.Add(data);
        }
    }

        [Title("快捷配置")]
        [PropertyOrder(26)]
        [LabelText("Android Texture Format")]
        public MyTextureFormatType androidTextureImporterFormat = MyTextureFormatType.RGBA32;
        
        [PropertyOrder(26)]
        [LabelText("iOS Texture Format")]
        public MyTextureFormatType iOSTextureImporterFormat = MyTextureFormatType.RGBA32;
      
        [PropertyOrder(27),LabelText("Read|Write")]
        public bool ReadAndWrite = false;
        [PropertyOrder(28),LabelText("Mipmap")]
        public bool MipmapEnabled = false;
       

        private TextureImporterFormat GetFormat(MyTextureFormatType textureImporterFormat)
        {
            return textureImporterFormat switch
            {
                MyTextureFormatType.RGBA32 => TextureImporterFormat.RGBA32,
                MyTextureFormatType.RBGA16 => TextureImporterFormat.RGBA16,
                MyTextureFormatType.ASTC4x4 => TextureImporterFormat.ASTC_4x4,
                MyTextureFormatType.ASTC5x5 => TextureImporterFormat.ASTC_5x5,
                MyTextureFormatType.ASTC6x6 => TextureImporterFormat.ASTC_6x6,
                MyTextureFormatType.ASTC8x8 => TextureImporterFormat.ASTC_8x8,
                MyTextureFormatType.ASTC12x12 => TextureImporterFormat.ASTC_12x12,
                MyTextureFormatType.DXT1 => TextureImporterFormat.DXT1,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
       

        [PropertySpace(20)]
        [Button("快捷处理", ButtonSizes.Large,ButtonStyle.Box)]
        [PropertyOrder(60)]
        public void Execute()
        {
            for (var i = 0; i < SpriteAtlasShowDataList.Count; i++)
            {
                if (SpriteAtlasShowDataList[i].obj is SpriteAtlas)
                {
                    var atlas = SpriteAtlasShowDataList[i].obj as SpriteAtlas;
                    // atlas.
                    SpriteAtlasImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(atlas)) as SpriteAtlasImporter;
                    if (importer != null)
                    {
                        var setting =  new SpriteAtlasTextureSettings();
                        setting.readable = ReadAndWrite;
                        setting.generateMipMaps = MipmapEnabled;
                        setting.sRGB = true;
                        setting.filterMode = FilterMode.Bilinear;
                        importer.textureSettings = setting;
                        var platformAndroid = importer.GetPlatformSettings("Android");
                        platformAndroid.overridden = true;
                        platformAndroid.format = GetFormat(androidTextureImporterFormat);
                        importer.SetPlatformSettings(platformAndroid);
                        var platformIOS = importer.GetPlatformSettings("iPhone");
                        platformIOS.overridden = true;
                        platformIOS.format =GetFormat(iOSTextureImporterFormat);
                        importer.SetPlatformSettings(platformIOS);
                        // 应用更改
                        AssetDatabase.ImportAsset(importer.assetPath, ImportAssetOptions.ForceUpdate);
                    }
                

              
                }
               
            }
        }
        [Button("选中筛选的资源", ButtonSizes.Large, ButtonStyle.Box)]
        [PropertyOrder(61)]
        public void Execute2()
        {
            if (SpriteAtlasShowDataList.Count == 0)
            {
                return;
            }

            var objs = new List<Object>();
            for (var i = 0; i < SpriteAtlasShowDataList.Count; i++)
            {
                var path = AssetDatabase.GetAssetPath(SpriteAtlasShowDataList[i].texture);
                var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
                objs.Add(obj);
            }
            // ReSharper disable once CoVariantArrayConversion
            // var objs = TextureShowDataList.Select(x => (x.texture as Object)).ToArray();
            Selection.activeObject = objs[0];
            Selection.objects = objs.ToArray();
           
            AssetDatabase.Refresh();
        }
      
        [Serializable]
        public class SpriteAtlasShowData
        {
            [PreviewField(50)]
            [HideLabel]
            [HorizontalGroup("Img",Width = 50)]
            [ShowIf("@texture != null")]
            [ReadOnly]
            public Texture texture;
            
            
            [PropertySpace(5)]
            [ReadOnly,HideLabel]
            [BoxGroup("Img/desc",ShowLabel = false)]
            public Object obj;
            
            
            private string _path;
            [BoxGroup("Img/desc",ShowLabel = false)]
            [PropertyOrder(0), ShowInInspector, HideLabel, InlineButton("Copy", "复制路径"),InlineButton("GetDependencies","查看引用")]
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
            public SpriteAtlasShowData(SpriteAtlas sp, string path)
            {
                var spAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);
                var ary = new Sprite[spAtlas.spriteCount];
                spAtlas.GetSprites(ary);
                if (ary.Length > 0)
                {
                    texture = ary[0].texture;
                }
                
                this.obj = sp;
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
                window.titleContent = new GUIContent("Dependencies List");
                window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 500);
                window.Show();
            }
        }
 

}
