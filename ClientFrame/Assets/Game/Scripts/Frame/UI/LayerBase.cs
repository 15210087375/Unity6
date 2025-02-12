using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public enum WindowAnimType
{
    None,
    MoveToLeft,
    MoveToRight,
}
public class LayerBase : UIBase
{
  
 
  
    public void LayerAnim(WindowAnimType animType, bool moveIn,Action callback = null)
    {
        var node = transform.Find("nodeAnim");
        if (node == null)
        {
            Debug.LogError("当前预设没有界面动画节点");
            return;
        }

        switch (animType)
        {
            case WindowAnimType.MoveToLeft:
            {
                var startPos = moveIn ? new Vector3(1080, 0, 0) : new Vector3(0, 0, 0);
                var endPosX = moveIn ? 0 : -1080;
                node.localPosition = startPos;
                node.gameObject.SetActive(true);
                node.transform.DOLocalMoveX(endPosX, 0.3f).SetEase(Ease.InOutCubic);
                UniTask.Delay(300).ContinueWith(() =>
                {
                    callback?.Invoke();
                });
                break;
            }

            case WindowAnimType.MoveToRight:
            {
                var startPos = moveIn ? new Vector3(-1080, 0, 0) : new Vector3(0, 0, 0);
                var endPosX = moveIn ? 0 : 1080;
                node.localPosition = startPos;
                node.gameObject.SetActive(true);
                node.transform.DOLocalMoveX(endPosX, 0.3f).SetEase(Ease.InOutCubic);
                UniTask.Delay(300).ContinueWith(() =>
                {
                    callback?.Invoke();
                });
                break;
            }
               
        }
    }
    
}
