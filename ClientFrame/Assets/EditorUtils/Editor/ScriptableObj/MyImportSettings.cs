using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using Unity.VisualScripting;
using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "MyImportSettings", menuName = "ScriptableObjects/资源导入配置", order = 1)]
public class MyImportSettings : ScriptableObject
{
    [Title("图片导入配置列表")]
    [ListDrawerSettings]
    public List<TextureImportData> textureData;
    
    [Space(20)]
    [Title("音效导入配置列表")]
    [ListDrawerSettings]
    public List<AudioImportData> audioData;
}



[Serializable]
public class TextureImportData
{
    
    [FolderPath(RequireExistingPath = true)]
    public string path;
    
    [EnumPaging]
    public TextureImporterType textureImporterType = TextureImporterType.Sprite;
    [LabelText("Read/Write Enabled")]
    public bool isReadable = false;
    [LabelText("MipMap")]
    public bool mipmapEnabled = false;
   
    [ValueDropdown("sizes")]
    public int maxSize = 2048;
    private List<int> sizes = new List<int>(){2048,1024,512,256,128,64,32,16};
    
    
    public MyTextureFormatType androidFormat = MyTextureFormatType.RGBA32;
    
    public MyTextureFormatType iOSFormat = MyTextureFormatType.RGBA32;
  
}

[Serializable]
public class AudioImportData
{
    [FolderPath(RequireExistingPath = true)]
    public string path;
    [ProgressBar(0,300)]
    public int minTime = 0;
    [ProgressBar(0,300)]
    public int maxTime = 0;
}

public enum MyTextureFormatType
{
    RGBA32 = TextureImporterFormat.RGBA32,
    RBGA16 = TextureImporterFormat.RGBA16,
    ASTC4x4 = TextureImporterFormat.ASTC_5x5,
    ASTC5x5 = TextureImporterFormat.ASTC_4x4,
    ASTC6x6 = TextureImporterFormat.ASTC_6x6,
    ASTC8x8 = TextureImporterFormat.ASTC_8x8,
    ASTC12x12 = TextureImporterFormat.ASTC_12x12,
    // ETC_2 = TextureImporterFormat.ETC2_RGBA8,
    // PVRTC_RGB2 = TextureImporterFormat.PVRTC_RGB2,
    // PVRTC_RGB4 = TextureImporterFormat.PVRTC_RGB4,
    // PVRTC_RGBA2 = TextureImporterFormat.PVRTC_RGBA2,
    // PVRTC_RGBA4 = TextureImporterFormat.PVRTC_RGBA4,
    DXT1 = TextureImporterFormat.DXT1,
        
}

public enum MyImportType
{
    Texture,
    Audio
}