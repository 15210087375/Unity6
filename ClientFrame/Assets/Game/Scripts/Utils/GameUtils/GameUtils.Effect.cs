// using System;
// using System.Collections;
// using System.Collections.Generic;
// using DataTable;
// using UnityEngine;
//
// //工具类
// public static partial class GameUtils
// {
//     public static class Effect
//     {
//         /// <summary>
//         /// 根据特效ID播放一个特效
//         /// </summary>
//         /// <param name="effectId"></param>
//         /// <param name="par"></param>
//         /// <param name="localPos"></param>
//         /// <param name="localScale"></param>
//         /// <param name="localRotation"></param>
//         /// <param name="delay"></param>
//         /// <param name="callBack"></param>
//         public static void PlayEffectId(int effectId, Transform par, Vector3 localPos, Vector3 localScale,
//             Vector3 localRotation, float delay = 2.0f, Action<GameObject> callBack = null)
//         {
//             var tbEffect = Table.Geteffect(effectId);
//             if (tbEffect == null)
//             {
//                 Logger.Error("not find effect = {0}", effectId);
//                 callBack?.Invoke(null);
//                 return;
//             }
//
//             var name = tbEffect.ResPath;
//             if (string.IsNullOrEmpty(name))
//             {
//                 callBack?.Invoke(null);
//                 return;
//             }
//
//             var effectTag = "";
//             if (tbEffect.EffectType == 0)
//             {
//                 //战斗特效
//                 effectTag = "EffectFight";
//             }
//             else if (tbEffect.EffectType == 1)
//             {
//                 effectTag = "EffectUI";
//                 //UI特效
//             }
//
//             PlayEffect(name, par, localPos, localScale * tbEffect.Scale, localRotation, delay, tbEffect.MaxPlayTimes,
//                 effectTag, callBack);
//         }
//
//         /// <summary>
//         /// 延迟播放时间 res：资源路径 pos：坐标 par：父节点 delay：延迟
//         /// </summary>
//         /// <param name="name"></param>
//         /// <param name="par"></param>
//         /// <param name="pos"></param>
//         /// <param name="scale"></param>
//         /// <param name="rota"></param>
//         /// <param name="delay"></param>
//         /// <param name="max"></param>
//         /// <param name="effectTag"></param>
//         /// <param name="callBack"></param>
//         private static void PlayEffect(string name, Transform par, Vector3 pos, Vector3 scale, Vector3 rota,
//             float delay = 2.0f, int max = 0, string effectTag = "", Action<GameObject> callBack = null)
//         {
//             //从缓存池里取特效
//             ObjectPoolManager.NewObject(name, ObjectPoolType.Effect, par, pos, scale, rota, max, effectTag, (h) =>
//             {
//                 if (h != null)
//                 {
//                     //延迟删除
//                     if (delay > 0)
//                     {
//                         var ani = QueueAnimation.WaitFunction(delay, h, () =>
//                         {
//                             ObjectPoolManager.Release(h);
//                             h.DelAni();
//                         });
//                         h.SetAni(ani);
//                     }
//                 }
//                 else
//                 {
//                     Logger.Error("not find effect Name = {0}", name);
//                 }
//
//                 callBack?.Invoke(h);
//                 h.GetOrAddComponent<MeshSorttingOrder>().ExecuteOrder();
//             });
//         }
//     }
// }
//
// public static partial class GameExtensions
// {
//     //数据类型扩展  函数第一个参数为 静态this类型 
// }