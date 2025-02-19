// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Reflection;
// using System.Text.RegularExpressions;
// using UnityEditor;
// using UnityEngine;
//
// public class ResHandler : AssetPostprocessor
// {
//     private static MonoScript _uiPathDefine;
//     // private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
//     // {
//     //     // 遍历所有移动前的路径，检查是否有预设路径被修改
//     //     for (var i = 0; i < movedFromAssetPaths.Length; i++)
//     //     {
//     //         var oldPath = movedFromAssetPaths[i];
//     //         var newPath = movedAssets[i];
//     //
//     //   
//     //         var curType = AssetDatabase.GetMainAssetTypeAtPath(newPath);
//     //         if(curType == typeof(GameObject))//预设
//     //         {
//     //             if (!string.Equals(oldPath, newPath))//修改路径
//     //             {
//     //                 Logger.Info($"Prefab at <color=yellow>{oldPath}</color> has been moved to <color=green>{newPath}</color>");
//     //             }
//     //             // OnUIPrefabPathChange(oldPath, newPath);
//     //         }
//     //         else if(curType == typeof(Texture2D))
//     //         {
//     //             
//     //         }
//     //         else if(curType == typeof(AudioClip))
//     //         {
//     //             
//     //         }
//     //     }
//     // }
//
//     private static void OnUIPrefabPathChange(string oldPath, string newPath)
//     {
//         var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(newPath);
//        
//         if (prefab.GetComponent<UIBase>() != null)//是UI预设
//         {
//             // _uiPathDefine ??= GetMonoScriptPathByType();
//             // var realOldPath = oldPath.Replace(UIPathDefine.FrontPath, "");
//             // var realNewPath = newPath.Replace(UIPathDefine.FrontPath, "");
//             // ReplaceScript(_uiPathDefine, realOldPath, realNewPath);
//         }
//        
//     }
//
//     // public static MonoScript GetMonoScriptPathByType()
//     // {
//     //     var defines = AssetDatabase.FindAssets("t:FrameDefine");
//     //     if(defines.Length == 0)
//     //     {
//     //         Logger.Error("No FrameDefine found");
//     //         return null;
//     //     }
//     //     var definePath = AssetDatabase.GUIDToAssetPath(defines[0]);
//     //     var define = AssetDatabase.LoadAssetAtPath<FrameDefine>(definePath);
//     //     return define.uiPathDefine;
//     //
//     // }
//     private static void ReplaceScript(MonoScript script,string oldPath,string newPath)
//     {
//         
//         var text = "";
//         var path = AssetDatabase.GetAssetPath(script);
//         text = File.ReadAllText(path);
//         
//         var matches = new List<Match>();
//         var pattern = oldPath;
//         var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Multiline);
//             
//         // 找到所有匹配的Button
//         var tempMatches = regex.Matches(text);
//         matches.AddRange(tempMatches.Cast<Match>());
//         foreach (var match in matches)
//         {
//             var value = match.Value;
//             var newValue = newPath;
//             text = text.Replace(value, newValue);
//         }
//         
//         File.WriteAllText(path, text);
//
//         AssetDatabase.Refresh();
//     }
//  
// }
