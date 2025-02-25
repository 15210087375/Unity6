using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public enum UIState
{
    None,
    Open,
    Close
}
public abstract class UIBase : MonoBehaviour,IView
{
    //窗口ID
    [HideInInspector]
    public WindowID windowID;
    //常驻窗口
    [HideInInspector]
    public bool isResident = false;
    
    public UIState uiState = UIState.None;
    
    public virtual void OnInit(object data = null)
    {
       
    }

    public virtual void OnOpen(object data = null)
    {
     
    }
    public virtual void Close()
    {
        
    }
    public virtual void OnClose()
    {
        
    }
    public void UIAnim(WindowAnimType animType, bool moveIn,Action callback = null)
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
            
            case WindowAnimType.FadeIn:
            {
                node.gameObject.SetActive(true);
                var canvasGroup = node.GetComponent<CanvasGroup>();
                canvasGroup.alpha = 0;
                canvasGroup.DOFade(1, 0.3f).SetEase(Ease.InOutCubic);
                UniTask.Delay(300).ContinueWith(() =>
                {
                    callback?.Invoke();
                });
                break;
            }
               
            case WindowAnimType.FadeOut:
            {
                node.gameObject.SetActive(true);
                var canvasGroup = node.GetComponent<CanvasGroup>();
                canvasGroup.alpha = 1;
                canvasGroup.DOFade(0, 0.3f).SetEase(Ease.InOutCubic);
                UniTask.Delay(300).ContinueWith(() =>
                {
                    callback?.Invoke();
                });
                break;
            }
        }
    }
    
}
