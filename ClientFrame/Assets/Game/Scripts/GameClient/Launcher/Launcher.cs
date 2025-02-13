using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Luban;
using UnityEngine;

public class Launcher:MonoSingleton<Launcher>
{
    public UIRoot uiRoot;

    private IEnumerator Start()
    {
        yield return LoadManager.Instance.Init(null);
        
        TableManager.Instance.InitTable();
        
        var global = TableManager.Instance.Tables.GlobalSettingRecord;
        for (var i = 0; i < global.DataList.Count; i++)
        {
            Debug.Log(global.DataList[i].Id  +" " +  global.DataList[i].Value);
        }
        
        
        UIManager.Instance.InitFirstScene();
    }
  
    
}
