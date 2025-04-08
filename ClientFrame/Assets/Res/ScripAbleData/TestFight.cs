using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class TestFight : MonoBehaviour
{
    
    [Button]
    public void Fight()
    {
      
        var zhanshiData = AssetDatabase.LoadAssetAtPath<RoleData>("Assets/Res/ScripAbleData/zhanShi.asset");
        var role1 = new Role(1,zhanshiData,1);
        var role2 = new Role(2,zhanshiData,6);
        FightManager.Instance.CheckFight(role1, role2);
    }

    public void DoFightAnim()
    {
        
    }
}
