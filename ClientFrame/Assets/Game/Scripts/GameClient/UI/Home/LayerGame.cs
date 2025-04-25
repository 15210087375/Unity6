using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LayerGame:LayerBase
{
    
    [Button]
    public void Fight()
    {
      
        var data= LoadManager.Instance.LoadAsset<FightRoleData>("Assets/Res/ScripAbleData/HeroData.asset");
    
        var role1 = new Role(1,data,1);
        var role2 = new Role(2,data,6);
        FightManager.Instance.CheckFight(role1, role2);
    }
}
