using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class lizilist
{
    public Lizidata[] lizidatas;
}



[Serializable]
public class Lizidata
{
    public int ID; // 粒子ID
    public string Name; // 名称
    public double Unlock_A_Required; // 解锁所需粒子数量
    public double Unlock_Precursor_ID; // 前驱物种的 ID
    public double Unlock_Precursor_Required; // 解锁所需前驱物种数量
    public double Production_Target; // 生产的目标资源 ID
    public double Production_Rate; // 基础生产效率
}