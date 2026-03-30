using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//基础资源数据
[Serializable]
public class BasicResourceData
{
    public int ID;
    public string ResourceID;           // "A", "B", "C"
    public string Name;
    public double InitialAmount;
    public string UnlockCondition_Resource;
    public double UnlockCondition_Count;
}

[System.Serializable]
public class BasicResourceList { public BasicResourceData[] basedatas; }

//粒子物种基础数据
[Serializable]
public class lizibaseData
{
    public int ID;
    public string Name;
    public double Unlock_A_Required;
    public int Unlock_Precursor_ID;      // 0 表示无
    public double Unlock_Precursor_Required;
    public int Production_Target;        // 0 表示生产粒子，其他表示生产对应ID的物种
    public double Production_Rate;
}

[System.Serializable]
public class liziBaseList { public lizibaseData[] lizidatas; }

//尘埃物种基础数据
[Serializable]
public class chenaibaseData
{
    public int ID;
    public string Name;
    public double Unlock_A_Required;
    public string Production_Target;     // 固定为 "C"
    public double Production_Rate;
}

[System.Serializable]
public class chenaiBaseList { public chenaibaseData[] chenaidatas; }

//粒子物种合成数据
[Serializable]
public class lizihechengData
{
    public int ID;
    public string Name;
    public double Craft_A_Cost;
    public int Craft_Precursor_ID;       // 前置物种 ID,0表示粒子
    public double Craft_Precursor_Cost;
    public double Craft_B_Cost;
    public double Craft_Output;
    public int CraftLevelUp_Consume_ID;  // 合成等级升级消耗的物种 ID（0 表示无）
    public double CraftLevelUp_BaseCount;
    public double CraftLevelUp_Multiplier;
    public double CraftLevelUp_EffectMultiplier;
}

[System.Serializable]
public class liziHeChengList { public lizihechengData[] lizihecheng; }

//尘埃物种合成数据
[Serializable]
public class chenaihechengData
{
    public int ID;
    public string Name;
    public double Craft_A_Cost;
    public double Craft_B_Cost;
    public double Craft_Output;
    public string CraftLevelUp_Consume_Resource; // "B"
    public double CraftLevelUp_BaseCount;
    public double CraftLevelUp_Multiplier;
    public double CraftLevelUp_EffectMultiplier;
}

[System.Serializable]
public class chenaiHeChengList { public chenaihechengData[] chenaihecheng; }

//粒子升级数据
[Serializable]
public class lizishengjiData
{
    public int ID;
    public string Name;
    public double Upgrade_Base_Cost;     // 首次升级所需本级物种数量
    public double Upgrade_Multiplier;    // 每次升级消耗倍数
    public double Upgrade_EffectMultiplier; // 效果倍数
}

[System.Serializable]
public class liziShengJiList { public lizishengjiData[] lizishengji; }

//尘埃升级数据
[Serializable]
public class chenaishengjiData
{
    public int ID;
    public string Name;
    public double Upgrade_ACount;        // 消耗的粒子数量
    public double Upgrade_Multiplier;    // 倍数
    public double Upgrade_EffectMultiplier;
}

[System.Serializable]
public class chenaiShengJiList { public chenaishengjiData[] chenaishengji; }

//雷电升级数据
[Serializable]
public class leidianshengjiData
{
    public int ID;
    public string Name;
    public string Type;                  // "add" 或 "percent"
    public string Cost_Resource;         // "A" 或 "B"
    public double Cost_firsttime;
    public double Cost_Multiplier;
    public double Effect_Base;
    public double Effect_Multiplier;
}

[System.Serializable]
public class leidianShengJiList { public leidianshengjiData[] leidianshengji; }