using System;
using cfg.Res;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewTopResCell : MonoBehaviour
{
    [SerializeField] private Image imgBg;
    [SerializeField] private Image imgIcon;
    [SerializeField] private TextMeshProUGUI textCount;
    [SerializeField] private Image imgBtn;
    
    //数据
    private Item _item;
    private int _index;
    
    // 折叠动画
    private Sequence _foldSeq;
    private const int CellWidthFold = 200;
    public void InitCell(ResType type,int index)
    {
        _index = index;
        _item = TableManager.Instance.Tables.ItemRecord.Get((int)type);
        InitUI();
    }


    private void InitUI()
    {
        imgIcon.SetIconId(_item.Icon);
    }
    public void OnClickCell()
    {
        Debug.Log("OnClickCell");
        
    }

    #region 动画

    public void FoldAnim()
    {
        InitFoldAnim();
        _foldSeq.PlayForward();
    }
    public void UnFoldAnim()
    {
        InitFoldAnim();
        _foldSeq.PlayBackwards();
    }
    private void InitFoldAnim()
    {
        if (_foldSeq != null) return;
        const float timer = 0.3f;
        var size = imgBg.rectTransform.sizeDelta;
        var targetSize = new Vector2(CellWidthFold, size.y);
        _foldSeq = DOTween.Sequence().SetAutoKill(false);
        _foldSeq.Append(imgBg.rectTransform.DOSizeDelta(targetSize,timer));
        _foldSeq.Join(imgBtn.DOFade(0, timer));
        _foldSeq.Join(textCount.transform.DOBlendableMoveBy(Vector3.right*10, timer));
        if (_index > 0)
        {
            _foldSeq.Join(transform.DOBlendableLocalMoveBy(Vector3.right* 80,timer));
        }
    }

    #endregion


    public void OnClear()
    {
        _foldSeq?.Kill();
    }
}
