using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "HeroData", menuName = "HeroData")]
public class FightRoleData :SerializedScriptableObject
{
    
    /// <summary>
    /// 职业
    /// </summary>
    [PropertyOrder(0),LabelText("AttributeType - (职业)")]
    public AttributeType attributeType = AttributeType.zhanshi;
    
    [PropertyOrder(0),ProgressBar(1,100), LabelText("Level - (等级)")]
    public int level = 1;           //等级
    
    /// <summary>
    /// 最大生命值
    /// </summary>
    [ReadOnly,ShowInInspector,PropertyOrder(1), LabelText("最大生命值")]
    public int Hp => basicHp + (constitution.value +(level-1)*growthConstitution) * hpRate/100;
    
    /// <summary>
    /// 攻击力
    /// </summary>
    [ReadOnly,ShowInInspector,PropertyOrder(1), LabelText("攻击力")]
    public int Attack => basicAttack + (strength.value +(level-1)*growthStrength) * atkRate/100;
    
    /// <summary>
    /// 防御力
    /// </summary>
    [ReadOnly,ShowInInspector,PropertyOrder(1), LabelText("防御力")]
    public int Defense => basicDef + (constitution.value +(level-1)*growthConstitution) * defRate/100; 
    
    /// <summary>
    /// 攻击间隔
    /// </summary>
    [ReadOnly,ShowInInspector,PropertyOrder(1),LabelText("攻击间隔(毫秒)"), SuffixLabel(" 毫秒")]
    public int AttackCd=> (int)(1000*1000 / (Speed == 0?1:Speed));        //攻击CD 1000毫秒  *1000是速度的值是*1000的
    
    /// <summary>
    /// 攻速
    /// </summary>
    [ReadOnly,ShowInInspector,PropertyOrder(1), LabelText("攻速"), SuffixLabel(" 次/1000")]
    private int Speed=> basicSpeed *(1000+ (agility.value +(level-1)*growthAgility)*1000 * speedRate/100)/1000 ; // 当前速度
    
  
    /// <summary>
    /// 体质
    /// </summary>
    [Title("属性")]
    [PropertyOrder(10), LabelText("Constitution - (体质)")][BoxGroup("基础属性")]
    public AttributeData constitution;    //体质
    
    /// <summary>
    /// 力量
    /// </summary>
    [PropertyOrder(11),LabelText("Strength - (力量)")][BoxGroup("基础属性")]
    public AttributeData strength;        
    
    /// <summary>
    /// 敏捷
    /// </summary>
    [PropertyOrder(12),LabelText("Agility - (敏捷)")][BoxGroup("基础属性")]
    public AttributeData agility;        //敏捷
    
    /// <summary>
    /// 基础生命值
    /// </summary>
    [Title("基础属性")]
    [PropertyOrder(20),LabelText("BasicHp - (基础生命值)"),Range(0,1000), SuffixLabel(" int")][BoxGroup("基础属性")]//, SuffixLabel(" HP")
    public int basicHp;
    
    /// <summary>
    /// 基础攻击力
    /// </summary>
    [PropertyOrder(21),LabelText("BasicHp - (基础攻击力)"),Range(0,1000), SuffixLabel(" int")][BoxGroup("基础属性")]
    public int basicAttack;    //基础攻击力
    
    /// <summary>
    /// 基础防御
    /// </summary>
    [PropertyOrder(22),LabelText("BasicDef - (基础防御)"),Range(0,1000), SuffixLabel(" int")][BoxGroup("基础属性")]
    public int basicDef;    //基础防御
    
    /// <summary>
    /// 基础攻速
    /// </summary>
    [PropertyOrder(23),LabelText("BasicSpeed - (基础攻速)"),Range(1,1000), SuffixLabel(" 次/1000")][BoxGroup("基础属性")]// 比如900 代表 1秒0.9次
    public int basicSpeed = 500;    //基础速度
    
    /// <summary>
    /// 体质转化生命值比例
    /// </summary>
    [Title("转化率")]
    [PropertyOrder(101),LabelText("HpRate  - (体质转化生命值比例)"),Range(1,10000), SuffixLabel("  %/1000")][BoxGroup("基础属性")]
    public int hpRate = 10;          //体质转化生命值比例

    /// <summary>
    /// 体质转化防御比例
    /// </summary>
    [PropertyOrder(102),LabelText("DefenseRate - (体质转化防御比例)"),Range(1,1000), SuffixLabel("  %")][BoxGroup("基础属性")]
    public int defRate = 50;     //体质转化防御比例
    
    /// <summary>
    /// 力量转换攻击比例
    /// </summary>
    [PropertyOrder(111),LabelText("AtkRate - (力量转换攻击比例)"),Range(1,1000), SuffixLabel("  %")][BoxGroup("基础属性")]
    public int atkRate = 50;         //力量转化攻击比例

    /// <summary>
    /// 速度转化攻速比例
    /// </summary>
    [PropertyOrder(121),LabelText("SpeedRate - (速度转化攻速比例)"), Range(0, 100), SuffixLabel("  %")][BoxGroup("基础属性")]
    public int speedRate;       //速度转化攻速比例

    [Title("成长属性")]
    [PropertyOrder(151),LabelText("GrowthCon - (成长体质)"), Range(0, 10), SuffixLabel("  int")][BoxGroup("基础属性")]
    public int growthConstitution = 1;    //体质成长
    
    [PropertyOrder(152),LabelText("GrowthStr - (成长力量)"), Range(0, 10), SuffixLabel("  int")][BoxGroup("基础属性")]
    public int growthStrength = 1;        //力量成长
    
    [PropertyOrder(153),LabelText("GrowthAgi - (成长敏捷)"), Range(0, 10), SuffixLabel("  int")][BoxGroup("基础属性")]
    public int growthAgility = 1;        //敏捷成长

   
    
    public FightRoleData Clone(FightRoleData data)
    {
        this.basicHp = data.basicHp;
        this.basicAttack = data.basicAttack;
        this.basicDef = data.basicDef;
        this.basicSpeed = data.basicSpeed;
        this.hpRate = data.hpRate;
        this.defRate = data.defRate;
        this.atkRate = data.atkRate;
        this.speedRate = data.speedRate;
        this.growthConstitution = data.growthConstitution;
        this.growthStrength = data.growthStrength;
        this.growthAgility = data.growthAgility;
        this.constitution = data.constitution;
        this.strength = data.strength;
        this.agility = data.agility;
        return this;
    }
  
}


public enum AttributeType
{
    [LabelText("战士")]
    zhanshi = 1,
}

[Serializable,HideLabel]
public struct AttributeData
{
    [HorizontalGroup("Img",width:20),  PreviewField(20, ObjectFieldAlignment.Left), HideLabel]
    public Texture icon;
    [ProgressBar(1,100),VerticalGroup("Img/desc"),SuffixLabel(" int"),HideLabel] 
    public int value;
}