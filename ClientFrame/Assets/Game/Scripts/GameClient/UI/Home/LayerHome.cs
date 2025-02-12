using System;
using System.Collections.Generic;
using System.IO;
using Luban;
using SimpleJSON;
using UnityEngine;

public class LayerHome : LayerBase
{
 
    private WindowID windowID = WindowID.LayerHome; 
    private void Awake()
    {

    }

    public void OnClickSetting()
    {
        UIManager.Instance.OpenView(WindowID.ViewSetting);
    }
    public void OnShopClick()
    {
        UIManager.Instance.SwitchLayer(WindowID.LayerShop);
    }
    public void OnGameClick()
    {
        UIManager.Instance.SwitchLayer(WindowID.LayerGame);
    }
   
}
