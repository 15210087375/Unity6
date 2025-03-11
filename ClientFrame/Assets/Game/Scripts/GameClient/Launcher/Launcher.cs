using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Luban;
using UnityEngine;

public class Launcher:MonoSingleton<Launcher>
{
  
    public void Awake()
    {
        //GM
        if (GameDefine.GameMode == GameMode.Dev)
        {
            SRDebug.Init();
        }
    }
    private IEnumerator Start()
    {
        yield return LoadManager.Instance.Init(null);
        
        //读表
        TableManager.Instance.InitTable();
        
        Player.Instance.Init();
       
        
        //UI
        UIManager.Instance.InitFirstScene();
    }

    private void Update()
    {
        Player.Instance.Update();
    }
}
