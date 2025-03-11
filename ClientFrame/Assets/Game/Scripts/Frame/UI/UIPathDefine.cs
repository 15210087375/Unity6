using System;
using System.Collections.Generic;
using UnityEngine;
public enum WindowID
{
    LayerShop,
    LayerHero,
    LayerHome,
    LayerGame,
    LayerEvent,
    LayerSetting,
    ViewSetting,
    ViewTabBar,
    LayerTemp1,
   
    ViewTopBar,
    ViewTestData,
    ViewGameCell,
    //WindowID Tag 
    //请勿删除或修改，用于自动生成代码
}
public static class UIPathDefine
{
    public const string FrontPath = "Assets/Res/UI/Prefabs/";

    public static readonly Dictionary<WindowID, UIPath> WindowPath = new Dictionary<WindowID, UIPath>()
    {
        { WindowID.LayerHome, new UIPath(WindowID.LayerHome, "Home/LayerHome.prefab", LayerIndex.Layer) },
        { WindowID.LayerShop, new UIPath(WindowID.LayerShop, "Home/LayerShop.prefab", LayerIndex.Layer) },
        { WindowID.LayerGame, new UIPath(WindowID.LayerGame, "Home/LayerGame.prefab", LayerIndex.Layer) },
        { WindowID.ViewSetting, new UIPath(WindowID.ViewSetting, "Home/ViewSetting.prefab", LayerIndex.View) },
        { WindowID.ViewTabBar, new UIPath(WindowID.ViewTabBar, "Home/ViewTabBar.prefab", LayerIndex.Bar) },
        { WindowID.LayerHero, new UIPath(WindowID.LayerHero, "Home/LayerHero.prefab", LayerIndex.Layer) },
        { WindowID.LayerEvent, new UIPath(WindowID.LayerEvent, "Home/LayerEvent.prefab", LayerIndex.Layer) },
        { WindowID.LayerSetting, new UIPath(WindowID.LayerSetting, "Home/LayerSetting.prefab", LayerIndex.Layer) },
        { WindowID.ViewTopBar, new UIPath(WindowID.ViewTopBar, "Home/ViewTopBar.prefab", LayerIndex.Bar) },
        { WindowID.ViewTestData, new UIPath(WindowID.ViewTestData, "Home/ViewTestData.prefab", LayerIndex.View) },
        { WindowID.ViewGameCell, new UIPath(WindowID.ViewGameCell, "Home/ViewGameCell.prefab", LayerIndex.View) },
        //WindowPath Tag 
        //请勿删除或修改，用于自动生成代码
    };


   
    public static WindowGroup Group(this WindowID id)
    {
        if (id >= WindowID.LayerShop && id <= WindowID.LayerSetting)
        {
            return WindowGroup.Home;
        }
        else
        {
            return WindowGroup.None;
        }
        
    }
}
public struct UIPath
{
    private WindowID ID;
    public readonly string Path;
    public readonly LayerIndex LayerIndex;
    public UIPath(WindowID id, string path, LayerIndex layerIndex)
    {
        ID = id;
        Path = path;
        LayerIndex = layerIndex;
    }
    
}

public enum LayerIndex
{
    Low = 0,
    Layer,
    View,
    Bar,
    High
}
public enum WindowGroup
{
    None,
    Home,
    Game,
}
