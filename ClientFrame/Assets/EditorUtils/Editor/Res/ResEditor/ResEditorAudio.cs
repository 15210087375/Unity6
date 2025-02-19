using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using ZhEditor;

public class ResEditorAudio : ResEditorResBase
{
    
    [Title("筛选")] [EnumToggleButtons, HideLabel] [OnValueChanged("CheckAudio")] [PropertyOrder(20)]
    public AudioCheckType CheckType = AudioCheckType.None;

    [CustomValueDrawer("DrawSeparator")]
    public bool showSeparator1;
        
    [LabelText("Audio 列表")][PropertyOrder(30)]
    [ListDrawerSettings( HideAddButton = true, DraggableItems = false, HideRemoveButton = true,NumberOfItemsPerPage = 4)]
    public List<AudioShowData> AudioShowDataList = new List<AudioShowData>();

    // protected EditorResType ResType = EditorResType.Prefab;
    private List<AudioShowData> _tempAllShowData = new List<AudioShowData>();

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
        var list = GetObjectByType<AudioClip>(SelectObject);
        AudioShowDataList.Clear();
        for(var i = 0; i < list.Count; i++)
        {
            var obj = AssetDatabase.LoadAssetAtPath<AudioClip>(list[i].path);
            var data = new AudioShowData(obj, list[i].path);
            AudioShowDataList.Add(data);
        }

        AudioShowDataList = AudioShowDataList.OrderByDescending(data => data.audio.length).ToList();
    }
    
    public ResEditorAudio(EditorResType resType,Type assetType) : base(resType,assetType)
    {
        ResType = resType;
        AstType = assetType;
    }

    private void CheckAudio()
    {
        var list = new List<AudioShowData>();
        foreach (var data in _tempAllShowData)
        {
            if (data.audio != null)
            {
                var path = AssetDatabase.GetAssetPath(data.audio);
                var importer = AssetImporter.GetAtPath(path) as AudioImporter;
                if (importer != null)
                {
                    switch (CheckType)
                    {
                        case AudioCheckType.None:
                            list.Add(data);
                            break;
                        case AudioCheckType.ForceToMono:
                            if (importer.forceToMono)
                            {
                                list.Add(data);
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }


    [Serializable]
    public class AudioShowData
    {
        [PreviewField(50)] [HideLabel] [HorizontalGroup("Audio", Width = 50)][ReadOnly]
        public AudioClip audio;
        
        private string path;
        [BoxGroup("Audio/desc", ShowLabel = false)]
        [PropertyOrder(0), ShowInInspector, HideLabel, InlineButton("Copy", "复制路径")]
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
        [PropertyOrder(1),HideLabel,ReadOnly]
        [BoxGroup("Audio/desc", ShowLabel = false)]
        public float length;
        // [PropertyOrder(1),HideLabel,ReadOnly]
        // [BoxGroup("Audio/desc", ShowLabel = false)]
        // public long size;
        public AudioShowData(AudioClip audio, string path)
        {
            this.audio = audio;
            this.path = path;
            this.length = audio.length;
            // size = Profiler.GetRuntimeMemorySizeLong(audio);
        }

        private void Copy()
        {
            EditorGUIUtility.systemCopyBuffer = Path;
        }

        private void GetDependencies()
        {
            var window = EditorWindow.GetWindow<ResEditorSearchDependencies>();
            window.InitObject(audio);
            window.Show();
        }
    }
}
