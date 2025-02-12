
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewTabBar : ViewBase
{
    [SerializeField] private List<HomeTabCell> tabCells;

    private readonly List<WindowID> _tabWindowIDs = new List<WindowID>
    {
        WindowID.LayerShop,
        WindowID.LayerHome,
        WindowID.LayerGame,
    };
    
    
    private int _curIndex = -1;
    public override void OnInit(object data = null)
    {
        base.OnInit(data);
        InitCells();
    }

    private void InitCells()
    {
        var initIndex = 1;
        for (var i = 0; i < tabCells.Count; i++)
        {
            var cell = tabCells[i];
            cell.InitCell(i,initIndex == i, OnSelect);
        }
        OnSelect(initIndex);
    }

    private void OnSelect(int index)
    {
        if (index == _curIndex)
        {
            return;
        }
        _curIndex = index;
        OnSelectCell(index);
        
        //打开对应界面
        var id = _tabWindowIDs[index];
        switch (id)
        {
            case WindowID.LayerShop:
                UIManager.Instance.SwitchLayer(id);
                break;
            case WindowID.LayerHome:
                UIManager.Instance.SwitchLayer(id);
                break;
            case WindowID.LayerGame:
                UIManager.Instance.SwitchLayer(id);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }
    
    //切换选中状态
    private void OnSelectCell(int index)
    {
        for (var i = 0; i < tabCells.Count; i++)
        {
            tabCells[i].OnSelect(index);
        }
    }
}