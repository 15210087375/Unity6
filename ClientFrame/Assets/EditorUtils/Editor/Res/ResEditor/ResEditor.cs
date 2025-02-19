using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace ZhEditor
{
    
    public enum EditorResType 
    {
        Prefab,
        Texture,
        SpriteAtlas,
        AudioClip,
        Shader
    }
    
    /// <summary>
    /// 图片资源筛选类型
    /// </summary>
    public enum TextureCheckType
    {
        None,
        BigTexture,
        Mipmap,
        NoCompress,
        ReadWrite,
    }
    /// <summary>
    /// 音效资源筛选类型
    /// </summary>
    public enum AudioCheckType
    {
        None,
        ForceToMono,
    }

    public enum AudioSortType
    {
        
    }
    public class ResEditor : OdinMenuEditorWindow
    {
        [HideInInspector]
        public List<EditorResData> objects = new List<EditorResData>();
        
        private OdinMenuTree _tree;
        
        // private List<Object> _cacheSelectObject;
        [MenuItem("Assets/工具/资源处理面板")]
        public static void OpenWindow()
        {
            var window = GetWindow<ResEditor>("资源处理面板");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
            window.Show();
        }
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(supportsMultiSelect: true);
            tree.Add("Res/预制体", new ResEditorPrefab(EditorResType.Prefab,typeof(GameObject)),EditorGUIUtility.FindTexture("d_PreMatCube"));
            tree.Add("Res/图片", new ResEditorTexture(EditorResType.Texture,typeof(Texture)),EditorGUIUtility.FindTexture("PreTextureArrayFirstSlice"));
            tree.Add("Res/图集", new ResEditorSpriteAtlas(EditorResType.SpriteAtlas,typeof(SpriteAtlas)),EditorGUIUtility.FindTexture("PreTextureMipMapLow"));
            tree.Add("Res/音频", new ResEditorAudio(EditorResType.AudioClip, typeof(AudioClip)),EditorGUIUtility.FindTexture("d_Profiler.Audio"));//SdfIconType.Disc);
            
            tree.AddAssetAtPath("自动导入配置", "Tools/Config/MyImportSettings.asset");
            _tree = tree;
            _tree.Selection.SelectionChanged += OnTreeSelectionChange;
            return tree;
        }
        private ResEditorResBase _curEditorResBase;
        private void OnTreeSelectionChange(SelectionChangedType selectChangeType)
        {
            switch (selectChangeType)
            {
                case SelectionChangedType.SelectionCleared:
                    RefreshCurPage();
                    break;
                case SelectionChangedType.ItemAdded:
                    RefreshCurPage();
                    break;
                
            }
        }
        //选中的资源发生变更
        private void OnSelectionChange()
        {
            // RefreshObjects();
            
            RefreshCurPage();
        }
        private void OnInspectorUpdate()
        {
            // You must call Repaint() to update Odin Inspector's inspector view when selection changes dynamically.
            Repaint();
        }
        private void RefreshCurPage()
        {
            _curEditorResBase = _tree.Selection.SelectedValue as ResEditorResBase;
            // Logger.Error($"当前选中的资源类型是 {_tree.Selection.SelectedValue.ToString()}");
            if (_curEditorResBase != null)
            {
                _curEditorResBase.InitSelectObject(_curEditorResBase.AstType);
            }
            
        }

        
        
    }
    
    [Serializable]
    public struct EditorResData
    {
       
        public Type resType;
        public string path;
   
      
    }
}