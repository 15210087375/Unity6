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

    private Sequence _seqUIAnimIn;
    private Sequence _seqUIAnimOut;
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
        
        if (moveIn)
        {
            _seqUIAnimOut?.Kill();
            _seqUIAnimIn = DG.Tweening.DOTween.Sequence();
           
        }
        else
        {
            _seqUIAnimIn?.Kill(true);
            _seqUIAnimOut = DG.Tweening.DOTween.Sequence();
        }
        var seq = moveIn ? _seqUIAnimIn : _seqUIAnimOut;
        switch (animType)
        {
            case WindowAnimType.MoveToLeft:
            {
                var startPos = moveIn ? new Vector3(1080, 0, 0) : new Vector3(0, 0, 0);
                var endPosX = moveIn ? 0 : -1080;
                
                node.localPosition = startPos;
                node.gameObject.SetActive(true);
                seq.Append(node.transform.DOLocalMoveX(endPosX, 0.3f).SetEase(Ease.InOutCubic));
                seq.OnComplete(() =>
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
                seq.Append(node.transform.DOLocalMoveX(endPosX, 0.3f).SetEase(Ease.InOutCubic));
                seq.OnComplete(() =>
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
                seq.Append(canvasGroup.DOFade(1, 0.3f).SetEase(Ease.InOutCubic));
                seq.OnComplete(() =>
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
                seq.Append(canvasGroup.DOFade(0, 0.3f).SetEase(Ease.InOutCubic));
                seq.OnComplete(() =>
                {
                    callback?.Invoke();
                });
                
                break;
            }
        }
        seq.Play();
    }
    
}
