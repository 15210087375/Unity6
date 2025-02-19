using System;
using UnityEditor;
using UnityEngine;

 public class ImageFormatEditorExtension
    {
        
        
        [MenuItem("Assets/工具/批量设置图片格式/ETC2_RGBA8  & PVRTC_RGBA4")]
        private static void SetImageFormatETC2_RGBA8()
        {
            SetImageFormatAndRemoveMipmaps(TextureImporterFormat.RGBA32,TextureImporterFormat.RGBA32);
            SetImageFormatAndRemoveMipmaps(TextureImporterFormat.ETC2_RGBA8,TextureImporterFormat.PVRTC_RGBA4);
        }
        
        [MenuItem("Assets/工具/批量设置图片格式/RGBA32")]
        private static void SetImageFormat_RGBA32()
        {
            SetImageFormatAndRemoveMipmaps(TextureImporterFormat.RGBA32,TextureImporterFormat.RGBA32);
        }
        [MenuItem("Assets/工具/批量设置图片格式/ASTC_8X8")]
        private static void SetImageFormat_ASTC_8x8()
        {
            SetImageFormatAndRemoveMipmaps(TextureImporterFormat.RGBA32,TextureImporterFormat.RGBA32);
            SetImageFormatAndRemoveMipmaps(TextureImporterFormat.ASTC_8x8,TextureImporterFormat.ASTC_8x8);
        }
     
        [MenuItem("Assets/工具/批量设置图片格式/ASTC_6X6")]
        private static void SetImageFormat_ASTC_6x6()
        {
            SetImageFormatAndRemoveMipmaps(TextureImporterFormat.RGBA32,TextureImporterFormat.RGBA32);
            SetImageFormatAndRemoveMipmaps(TextureImporterFormat.ASTC_6x6,TextureImporterFormat.ASTC_6x6);
        }
        
        [MenuItem("Assets/工具/批量设置图片格式/ASTC_4X4")]
        private static void SetImageFormat_ASTC_4x4()
        {
            SetImageFormatAndRemoveMipmaps(TextureImporterFormat.RGBA32,TextureImporterFormat.RGBA32);
            SetImageFormatAndRemoveMipmaps(TextureImporterFormat.ASTC_4x4,TextureImporterFormat.ASTC_4x4);
        }
        private static void SetImageFormatAndRemoveMipmaps(TextureImporterFormat android,TextureImporterFormat ios)
        {
            foreach (var folderPath in Selection.GetFiltered(typeof(DefaultAsset), SelectionMode.Assets))
            {
                var assetPath = AssetDatabase.GetAssetPath(folderPath);
                var assets = AssetDatabase.FindAssets("t:Texture", new[] { assetPath });
                foreach (var asset in assets)
                {
                    var assetFilePath = AssetDatabase.GUIDToAssetPath(asset);
                    var importer = AssetImporter.GetAtPath(assetFilePath) as TextureImporter;

                    if (importer == null) continue;
                    
                    var standaloneSetting = importer.GetPlatformTextureSettings("Standalone");
                    standaloneSetting.format = TextureImporterFormat.RGBA32;
                    standaloneSetting.textureCompression = TextureImporterCompression.Compressed;
                    standaloneSetting.overridden = true;
                    standaloneSetting.compressionQuality = 50;
                    importer.SetPlatformTextureSettings(standaloneSetting);
                    
                    var androidSettings = importer.GetPlatformTextureSettings("Android");
                    androidSettings.overridden = true;
                    androidSettings.textureCompression = TextureImporterCompression.CompressedLQ;
                    androidSettings.format = android;
                    androidSettings.compressionQuality = 50;
                    importer.SetPlatformTextureSettings(androidSettings);
                    
                    var iOSSetting = importer.GetPlatformTextureSettings("iPhone");
                    iOSSetting.overridden = true;
                    iOSSetting.textureCompression = TextureImporterCompression.CompressedLQ;
                    iOSSetting.format = ios;
                    iOSSetting.compressionQuality = 50;
                    importer.SetPlatformTextureSettings(iOSSetting);

                    importer.textureType = TextureImporterType.Sprite;
                    importer.spriteImportMode = SpriteImportMode.Single;
                    importer.mipmapEnabled = false;
                    importer.isReadable = false;
                    importer.alphaIsTransparency = true;
                    importer.SaveAndReimport();
                }
            }
        }


      
       
    }