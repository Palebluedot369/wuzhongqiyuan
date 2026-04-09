using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class SaveData
{
    public double lizicount;
    public double leidiancount;
    public double chenaicount;
    public double zhihuicount;
    public double anwuzhicount;

    public double[] liziSpecies;
    public double[] chenaiSpecies;
    public double[] lizishengchanRates;
    public double[] chenaishengchanRates;
    public double[] lizihechengMultiplier;
    public double[] chenaihechengMultiplier;
    public int[] lizishengjiLevel;
    public int[] chenaishengjiLevel;
    public int[] lizihechengLevel;
    public int[] chenaihechengLevel;

    public double leidianshengchanRate;
    public bool isleidianUnlocked;
    public double leidianBaseRate;
    public double leidianpercentBonus;
    public int leidianAddLevel;
    public int leidianPercentLevel;


}
