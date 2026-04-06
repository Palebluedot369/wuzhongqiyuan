using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class hechengManager : MonoBehaviour
{
    public static hechengManager Instance { get; private set; }
    private GameResourceManager resourceManager;
    //粒子、尘埃物种合成数据字典
    private Dictionary<int, lizihechengData> lizihechengDict = new Dictionary<int, lizihechengData>();
    private Dictionary<int, chenaihechengData> chenaihechengDict = new Dictionary<int, chenaihechengData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        StartCoroutine(WaitForManager());
    }
    private IEnumerator WaitForManager()
    {
        while (GameResourceManager.Instance == null)
            yield return null;
        resourceManager = GameResourceManager.Instance;
        LoadHechengData();
    }

    void LoadHechengData()
    {
        //加载粒子合成数据
        TextAsset lizihechengJson = Resources.Load<TextAsset>("meta/lizihecheng");
        if(lizihechengJson == null)
        {
            Debug.LogError("未找到lizihecheng.json");

        }
        else
        {
            var list = JsonUtility.FromJson<liziHeChengList>(lizihechengJson.text);
            foreach (var data in list.lizihecheng)
            {
                lizihechengDict[data.ID] = data;
               // Debug.Log($"加载了 {lizihechengDict.Count} 个粒子合成配方");
            }
        }
        //加载尘埃合成数据
        TextAsset chenaichengJson = Resources.Load<TextAsset>("meta/chenaihecheng");
        if(chenaichengJson == null)
        {
            Debug.LogError("未找到chenaihecheng.json");

        }
        else
        {
            var list = JsonUtility.FromJson<chenaiHeChengList>(chenaichengJson.text);
            foreach (var data in list.chenaihecheng)
            {
                chenaihechengDict[data.ID] = data;
              //  Debug.Log($"加载了 {chenaihechengDict.Count} 个尘埃合成配方");
            }
        }

    }

    //粒子物种合成
    public bool lizihecheng(int liziID,double hechengcount)
    {
        //配方检查
        if(!lizihechengDict.TryGetValue(liziID,out var peifang))
        {
            Debug.LogError($"未找到物种 {liziID} 的合成配方");
            return false;
        }
        double turecost = hechengcount / resourceManager.getlizihechengMultiplier(liziID);
        //基础资源数量检查
        if (resourceManager.getlizinumber() < peifang.Craft_A_Cost * turecost)
            return false;
        if(resourceManager.getleidianCount() < peifang.Craft_B_Cost * turecost)
            return false;

        //检查前置物种
        if(peifang.Craft_Precursor_ID != 0)
        {
            double qianzhiHave = resourceManager.getOtherlizinumber(peifang.Craft_Precursor_ID);
            if(qianzhiHave < peifang.Craft_Precursor_Cost * hechengcount)
                return false;
        }


        //资源消耗
        
        resourceManager.liziAdd(-peifang.Craft_A_Cost * turecost);
        resourceManager.leidianAdd(-peifang.Craft_B_Cost * turecost);

        if(peifang.Craft_Precursor_ID != 0)
        {
            resourceManager.liziwuzhongAdd(peifang.Craft_Precursor_ID, -peifang.Craft_Precursor_Cost * turecost);

        }
        //合成产出
        double output = peifang.Craft_Output * hechengcount;
        resourceManager.liziwuzhongAdd(liziID, output);
        Debug.Log($"合成成功，获得 {output} 个 {peifang.Name}");

        return true;



    }

    //尘埃合成
    public bool chenaihecheng(int chenaiID,double hechengcount)
    {
        double truecost = hechengcount / resourceManager.getchenaihechengMultiplier(chenaiID, true);
        //配方检查
        if (!chenaihechengDict.TryGetValue(chenaiID,out var peifang))
        {
            Debug.LogError($"未找到尘埃 {chenaiID} 的合成配方");
            return false;
        }
        //基础资源检查
        if(resourceManager.getlizinumber() < peifang.Craft_A_Cost * truecost)
            return false;
        if(resourceManager.getleidianCount() <  peifang.Craft_B_Cost * truecost)
            return false;
        //资源消耗
        
        resourceManager.liziAdd(-peifang.Craft_A_Cost * truecost);
        resourceManager.leidianAdd(-peifang.Craft_B_Cost * truecost);
        //合成产出
        double output = peifang.Craft_Output * hechengcount;
        resourceManager.chenaiwuzhongAdd(chenaiID,output);
        Debug.Log($"合成成功，获得 {output} 个 {peifang.Name}");
        return true;
    }
    //粒子物种合成等级升级
    public bool lizihechengshengjiLevel(int liziID)
    {
        //配方检查
         if(!lizihechengDict.TryGetValue(liziID,out var peifang))
            return false;

        int xiaohaoID = peifang.CraftLevelUp_Consume_ID;
        if(xiaohaoID == 0)
        {
            Debug.Log($"{liziID}已是最高物种，无法升级");
            return false;
        }
        //当前合成等级和升级消耗计算
        int currentLevel = resourceManager.getlizihechengLevel(liziID);
        double cost = peifang.CraftLevelUp_BaseCount * Math.Pow(peifang.CraftLevelUp_Multiplier, currentLevel);
        double have = resourceManager.getOtherlizinumber(xiaohaoID);
        if(have < cost)
        {
            Debug.Log($"{xiaohaoID}不足");
            return false;
        }

        //扣除升级消耗
        resourceManager.liziwuzhongAdd(xiaohaoID, -cost);
        //计算新倍率
        double newMult = resourceManager.getlizihechengMultiplier(liziID) * peifang.CraftLevelUp_EffectMultiplier;
        resourceManager.setlizihechengMultiplier(liziID, newMult);//刷新合成效率
        resourceManager.setlizihechengLevel(liziID, currentLevel + 1);//刷新合成等级
        Debug.Log($"物种{liziID}合成等级提升至{currentLevel + 1}，合成效率为{newMult}");
        return true;        
    }

    //尘埃物种合成等级升级
    public bool chenaihechengshengjiLevel(int chenaiID)
    {
        //配方检查
        if(!chenaihechengDict.TryGetValue(chenaiID, out var peifang))
        {
            Debug.Log($"{chenaiID}合成配方不存在");
            return false;
        }
        
        //当前合成等级和合成升级消耗计算
        int currentLevel = resourceManager.getchenaihechengLevel(chenaiID);
        double cost = peifang.CraftLevelUp_BaseCount * Math.Pow(peifang.CraftLevelUp_Multiplier,currentLevel);
        double have = resourceManager.getleidianCount();
        if (have < cost)
        {
            Debug.Log("当前雷电不足");
            return false;
        }
        //扣除升级消耗
        resourceManager.leidianAdd(-cost);
        //计算新合成效率
        double newMult = resourceManager.getchenaihechengMultiplier(chenaiID,true) * peifang.CraftLevelUp_EffectMultiplier;
        //刷新合成效率
        resourceManager.setchenaihechengMultiplier(chenaiID, newMult);
        //刷新合成等级
        resourceManager.setchenaihechengLevel(chenaiID,currentLevel + 1);
        Debug.Log($"物种{chenaiID}合成等级提升至{currentLevel + 1}，合成效率为{newMult}");
        return true;

    }

}
