using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using SRDebugger;
using SRDebugger.Services;
using SRF.Service;
using UnityEngine;

//#if UNITY_DEVELOPMENT || UNITY_EDITOR 
public partial class SROptions
{

    private int _flag;
    private int _rankScore;
    private int _skillId;
    #region GM-1 GM

    [Category("GM-01 <color=red>GM</color>"), Sort(0), DisplayName("清除数据")]
    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
   
   
    [Category("GM-01 <color=red>GM</color>"), Sort(0), DisplayName("RuntimeInspector")]
    public void RuntimeInspector()
    {
        if (EditToolMgr.Instance)
        {
            EditToolMgr.Instance.OpenTool();
            if (Settings.Instance.UnloadOnClose)
            {
                SRServiceManager.GetService<IDebugService>().DestroyDebugPanel();
            }
            else
            {
                SRServiceManager.GetService<IDebugService>().HideDebugPanel();
            }
        }
    }



    #endregion

    // #region 设置标记
    //
    // [Category("GM-02 <color=red>标记位</color>"),NumberRange(0,100000000),Sort(1), DisplayName("扩展计数ID")]
    // public int Flag
    // {
    //     get => _flag;
    //     set { _flag = value; }
    // }
    // [Category("GM-02 <color=red>标记位</color>"),Sort(2), DisplayName("设置为真")]
    // public void SetFlagTrue()
    // {
    //     PlayerInterFace.SetFlag(_flag);
    // }
    // [Category("GM-02 <color=red>标记位</color>"),Sort(3), DisplayName("设置为假")]
    // public void SetFlagFalse()
    // {
    //     PlayerInterFace.CleanFlag(_flag);
    // }
    // #endregion
    //
    //

    
}
//#endif