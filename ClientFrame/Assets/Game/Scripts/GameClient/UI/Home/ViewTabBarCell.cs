using System;
using System.Collections.Generic;
using cfg.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewTabBarCell : MonoBehaviour
{
    [SerializeField] private Transform nodeAnim;
    [SerializeField] private Image imgBgGrey;
    [SerializeField] private Image imgBgWhite;
    [SerializeField] private Image imgIconGrey;
    [SerializeField] private Image imgIconWhite;
    [SerializeField] private TextMeshProUGUI txtName;

    private Action<int> _selectAction;
    private HomeTab _homeTab;
    private bool _isSelect = false;

    private Tweener _anim;
    public void InitCell(HomeTab homeTab,bool isSelect,Action<int> selectAction)
    {
        _homeTab = homeTab;
        _selectAction = selectAction;
        _isSelect = isSelect;
        InitSelect();
        InitUI();
    }

    private void InitUI()
    {
        imgIconGrey.SetIconId(_homeTab.IconIds[0]);
        imgIconWhite.SetIconId(_homeTab.IconIds[1]);
        txtName.SetText(_homeTab.Name);
    }
    public void OnClickSelect()
    {
        _selectAction.Invoke(_homeTab.Id);
    }

    public void OnSelect(int index)
    {
       
        if(index == _homeTab.Id)
        {
            ShowAnim();
        }
        else
        {
            HideAnim();
        }
    }
    private void ShowAnim()
    {
        if(_isSelect)
        {
            return;
        }
        _isSelect = true;
        imgBgWhite.SetAlpha(0);
        imgIconWhite.SetAlpha(0);
        imgBgWhite.gameObject.SetActive(true);
        imgIconWhite.gameObject.SetActive(true);

        nodeAnim.DOLocalMoveY(0f, 0.2f).SetEase(Ease.InOutCubic);
        
        DOTween.To(()=>0, (value) =>
        {
            var color = imgBgWhite.color;
            color.a = value;
            imgBgWhite.color = color;
            imgIconWhite.color = color;
        }, 1, 0.2f).onComplete = () =>
        {
            imgBgGrey.gameObject.SetActive(false);
            imgIconGrey.gameObject.SetActive(false);
        };
    }

    private void HideAnim()
    {
        if(_isSelect == false)
        {
            return;
        }
        _isSelect = false;
        imgBgGrey.gameObject.SetActive(true);
        imgIconGrey.gameObject.SetActive(true);
        
        nodeAnim.DOLocalMoveY(-80f, 0.2f).SetEase(Ease.InOutCubic);
        
        DOTween.To(()=>1, (value) =>
        {
            var color = imgBgWhite.color;
            color.a = value;
            imgBgWhite.color = color;
            imgIconWhite.color = color;
        }, 0, 0.2f).onComplete = () =>
        {
            imgBgWhite.gameObject.SetActive(false);
            imgIconWhite.gameObject.SetActive(false);
        };
    }
    
    private void InitSelect()
    {
        imgBgGrey.gameObject.SetActive(!_isSelect);
        imgIconGrey.gameObject.SetActive(!_isSelect);
        imgBgWhite.gameObject.SetActive(_isSelect);
        imgIconWhite.gameObject.SetActive(_isSelect);
        imgBgWhite.SetAlpha(_isSelect ? 1 : 0);
        imgIconWhite.SetAlpha(_isSelect ? 1 : 0);
        nodeAnim.localPosition = _isSelect ? Vector3.zero : new Vector3(0, -80, 0);
    }
}
