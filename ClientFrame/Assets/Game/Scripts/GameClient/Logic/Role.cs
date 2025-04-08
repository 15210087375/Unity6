using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum UnitState{
    Live,
    Dead
}

public struct AttackTimeData
{
    public bool IsAttack;
    public int ReduceTime;
}
public class Role 
{
    public ulong Id;
    private RoleData _data;
    /// <summary>
    /// 蓄力时间
    /// </summary>
    private int _chargingTime;
    private int _hp;
    public UnitState State;
   
   
    public Role(ulong id,RoleData data, int level)
    {
        Id = (ulong)id;
        _data = ScriptableObject.CreateInstance<RoleData>().Clone(data); 
        _data.level = level;
        _hp = data.Hp;
        Logger.Info($"{id} hp={_hp} attack={_data.Attack} cd={_data.AttackCd} level={_data.level} defense={_data.Defense}");
    }
    public AttackTimeData CheckAttack(int timer)
    {
        _chargingTime += timer;
        if (_chargingTime >= _data.AttackCd)
        {
            _chargingTime -= _data.AttackCd;
            return new AttackTimeData()
            {
                IsAttack = true,
                ReduceTime = _chargingTime
            };
        }
        else
        {
            return new AttackTimeData()
            {
                IsAttack = false,
            };
        }
    }
    public int GetHp()
    {
        return _hp;
    }
    public int GetAttack()
    {
        return _data.Attack;
    }
    //这里计算防御，减伤等效果
    public void DoDamage(int damage)
    {
        var realDamage = GetRealDamage(damage);
        _hp -= realDamage;
        
        Debug.Log($"TickCount:{FightManager.Instance.TickCount}  Unit {Id} 受到伤害:{realDamage} Hp:{_hp}");
        State = _hp <= 0 ? UnitState.Dead : UnitState.Live;
        if(State == UnitState.Dead)
        {
            Debug.Log($"Unit {Id} is Dead");
        }
    }

    private int GetRealDamage(int damage)
    {
        
        var realDamage = damage * 100/(100 + _data.Defense);
        
        // todo格挡/ 减伤等
        realDamage = realDamage <= 1 ? 1 : realDamage;
        return Mathf.CeilToInt(realDamage);
    }
}
