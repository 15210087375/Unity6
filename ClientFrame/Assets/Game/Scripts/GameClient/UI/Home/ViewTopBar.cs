
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewTopBar : ViewBase
{

    [SerializeField] private List<ViewTopResCell> nodeResBgs;

    public override void OnInit(object data = null)
    {
        base.OnInit(data);
        InitRes();
    }

    public override void OnClose()
    {
        base.OnClose();
    }

    

   
    private void InitRes()
    {
        for (var i = 0; i < nodeResBgs.Count; i++)
        {
            nodeResBgs[i].InitCell(ResType.Coin+i,i);
        }
    }


    #region Event

    private void AddListener()
    {
        EventCenter.AddListener<bool>(EventCetnerType.TopViewFoldAnim, FoldAnim);
    }
    private void RemoveListener()
    {
        EventCenter.RemoveListener<bool>(EventCetnerType.TopViewFoldAnim, FoldAnim);
    }

    #endregion

    #region Anim

    private void FoldAnim(bool isFold)
    {
        if (isFold)
        {
            foreach (var cell in nodeResBgs)
            {
                cell.FoldAnim();
            }
        }
        else
        {
            foreach (var cell in nodeResBgs)
            {
                cell.UnFoldAnim();
            }
        }
    }


    #endregion
  
    
    private void Start()
    {
        AddListener();
    }
 
    private void OnDestroy()
    {
        RemoveListener();
        foreach (var cell in nodeResBgs)
        {
            cell.OnClear();
        }
    }
}