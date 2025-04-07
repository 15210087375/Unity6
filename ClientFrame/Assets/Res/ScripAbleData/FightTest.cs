using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightTest:Singleton<FightTest>
{

    public int TickCount = 0;
    public void CheckFight(Role data1, Role data2)
    {
        //根据传入的数据计算胜利方
        while (data1.State == UnitState.Live && data2.State == UnitState.Live)
        {
            UpdateTick(33, data1, data2);
        }
    }
    private List<Role> _roleList = new List<Role>();
    private void UpdateTick(int timer,Role p1,Role p2)
    {
        var attackData1 = p1.CheckAttack(timer);
        var attackData2 = p2.CheckAttack(timer);
        // Debug.Log($"TickCount: {_tickCount}");
        TickCount++;
        if(attackData1.IsAttack  && attackData2.IsAttack)//本帧ab都发起攻击
        {
            var isSameTimeAttack = attackData1.ReduceTime == attackData2.ReduceTime;
            if (isSameTimeAttack)//同时发起攻击
            {
                p1.DoDamage(p2.GetAttack());
                p2.DoDamage(p1.GetAttack());
            }
            else//非同时攻击，需要判断后发动攻击者是否存活
            {
                var first = attackData1.ReduceTime > attackData2.ReduceTime ? p1 : p2;
                var second = first == p1 ? p2 : p1;
                first.DoDamage(second.GetAttack());
                if (second.State == UnitState.Live)
                {
                    second.DoDamage(first.GetAttack());
                }
            }
            
        }
        else
        {
            if(attackData1.IsAttack)
            {
                p2.DoDamage(p1.GetAttack());
            }
            if(attackData2.IsAttack)
            {
                p1.DoDamage(p2.GetAttack());
            }
        }
    }

    private void UpdateTick(int timer)
    {
        var listAtkTime = new List<AttackTimeData>();
        foreach (var role in _roleList)
        {
            var attackData = role.CheckAttack(timer);
            listAtkTime.Add(attackData);
        }
        
        TickCount++;
    }
}
