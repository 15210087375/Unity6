using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Luban;
using UnityEngine;

public class Launcher:MonoSingleton<Launcher>
{
 
    private IEnumerator Start()
    {
        yield return LoadManager.Instance.Init(null);
        
        //读表
        TableManager.Instance.InitTable();
        
        //GM
        if (GameDefine.GameMode == GameMode.Dev)
        {
            SRDebug.Init();
        }
        
        //UI
        UIManager.Instance.InitFirstScene();
    }
  
    
}
