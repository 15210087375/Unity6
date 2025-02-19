// using System.Collections.Generic;
// using System.IO;
// using Newtonsoft.Json;
// using TMPro;
// using UnityEditor;
// using UnityEngine;
// public class TmpCustomEditors 
// {
//     private const string MenuItemText = "Assets/工具/Tmp保存偏移数据";
//     private const string MenuItemText1 = "Assets/工具/Tmp导入偏移数据";
//         
//  
//     [MenuItem(MenuItemText, false, 25)]
//     public static void SaveData()
//     {
//         //判断路径Assets/Tools 是否有对应json文件
//         var path = AssetDatabase.GetAssetPath(Selection.activeObject);
//         var jsonPath = $"Assets/Tools/{Selection.activeObject.name}.json";
//         if (!File.Exists(jsonPath))
//         {
//             File.Create(jsonPath).Dispose();
//         }
//         var tmpSpriteAsset = AssetDatabase.LoadAssetAtPath<TMP_SpriteAsset>(path);
//         var list = tmpSpriteAsset.spriteGlyphTable;
//         var dic = new Dictionary<string, List<float>>();
//         for (var i = 0; i < list.Count; i++)
//         {
//             var data = new List<float>();
//             var spriteGlyph = list[i];
//             var metrics = spriteGlyph.metrics;
//             data.Add(metrics.horizontalBearingX);
//             data.Add(metrics.horizontalBearingY);
//             var name = tmpSpriteAsset.spriteCharacterTable[(int)spriteGlyph.index].name ;
//             dic.Add(name, data);
//         }
//         var str = JsonConvert.SerializeObject(dic);
//         File.WriteAllText(jsonPath,str);
//         AssetDatabase.Refresh();
//     }
//     
//     [MenuItem(MenuItemText, true)]
//     public static bool Validate()
//     {
//         if (!Selection.activeObject) return false;
//         var path = AssetDatabase.GetAssetPath(Selection.activeObject);
//         return IsTmpSpriteFile(path);
//
//     }
//     [MenuItem(MenuItemText1, false, 25)]
//     public static void ReadData()
//     {
//         var path = AssetDatabase.GetAssetPath(Selection.activeObject);
//         //这里填自己缓存json文件的路径
//         var jsonPath = $"Assets/Tools/{Selection.activeObject.name}.json";
//         if (!File.Exists(jsonPath))
//         {
//             Debug.LogError("没有对应的预存json文件");
//             return;
//         }
//         var json = File.ReadAllText(jsonPath);
//         var dic = JsonConvert.DeserializeObject<Dictionary<string, List<float>>>(json);
//         var tmpSpriteAsset = AssetDatabase.LoadAssetAtPath<TMP_SpriteAsset>(path);
//         for (var i = 0; i < tmpSpriteAsset.spriteGlyphTable.Count; i++)
//         {
//             var spriteGlyph = tmpSpriteAsset.spriteGlyphTable[i];
//             var name = tmpSpriteAsset.spriteCharacterTable[(int)spriteGlyph.index].name;
//             if (dic.ContainsKey(name))
//             {
//                 var data = dic[name];
//                 var metrics = spriteGlyph.metrics;
//                 metrics.horizontalBearingX = data[0];
//                 metrics.horizontalBearingY = data[1];
//                 spriteGlyph.metrics = metrics;
//             }
//         }
//         EditorUtility.SetDirty(tmpSpriteAsset);
//         AssetDatabase.SaveAssets();
//     }
//     [MenuItem(MenuItemText1, true)]
//     public static bool Validate1()
//     {
//         if (!Selection.activeObject) return false;
//         var path = AssetDatabase.GetAssetPath(Selection.activeObject);
//         return IsTmpSpriteFile(path);
//
//     }
//     private static bool IsTmpSpriteFile(string path)
//     {
//         //判断是否是TMP_SpriteAsset文件，路径自定义
//         return path.StartsWith("Assets/ThirdParty/TextMesh Pro/Resources/Sprite Assets");
//     }
// }
