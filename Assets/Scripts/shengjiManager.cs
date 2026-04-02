using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class shengjiManager : MonoBehaviour
{
    public static shengjiManager Instance;
    private GameResourceManager resourceManager;
    //粒子、尘埃物种升级数据字典
    private Dictionary<int,lizishengjiData> lizishengjiDict = new Dictionary<int, lizishengjiData> ();
    private Dictionary<int, chenaishengjiData> chenaishengjiDict = new Dictionary<int, chenaishengjiData>();

    private void Awake()
    {
        if(Instance != null && Instance != this)
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
        LoadShengjiData();
    }

    void LoadShengjiData()
    {
        //加载粒子升级数据
        TextAsset lizishengjiJson = Resources.Load<TextAsset>("meta/lizishengji");
        if(lizishengjiJson == null)
        {
            Debug.LogError("寻找lizishengji.json失败");
        }
        else
        {
            var list = JsonUtility.FromJson<liziShengJiList>(lizishengjiJson.text);
            foreach(var data in list.lizishengji)
            {
                lizishengjiDict[data.ID] = data;
                Debug.Log($"加载了{lizishengjiDict.Count}个粒子升级配方");
            }

        }


        //加载尘埃升级数据
        TextAsset chenaishengjiJson = Resources.Load<TextAsset>("meta/chenaishengji");
        if(chenaishengjiJson == null)
        {
            Debug.LogError("寻找chenaishengji.json失败");

        }
        else
        {
            var list = JsonUtility.FromJson<chenaiShengJiList>(chenaishengjiJson.text);
            foreach(var data in list.chenaishengji)
            {
                chenaishengjiDict[data.ID] = data;
                Debug.Log($"加载了{chenaishengjiDict.Count}个尘埃升级配方");
            }
        }
    }

    //粒子升级
    public bool lizishengji(int liziID)
    {
        if (!lizishengjiDict.TryGetValue(liziID, out var peifang))
        {
            Debug.LogError($"未找到物种 {liziID} 的升级配方");
            return false;
        }       
        //当前生产等级和升级消耗计算
        int currentLevel = resourceManager.getlizishengjiLevel(liziID);
        double cost = peifang.Upgrade_Base_Cost * Math.Pow(peifang.Upgrade_Multiplier, currentLevel);
        //检查物种数量
        if (resourceManager.getOtherlizinumber(liziID) < cost)
        {
            Debug.Log($"{peifang.Name}物种数量不足");
            return false;
        }
        //扣除升级消耗
        resourceManager.liziwuzhongAdd(liziID , -cost);
        //刷新生产效率
        double newMult = resourceManager.getlizishengchanRate(liziID) * peifang.Upgrade_EffectMultiplier;
        resourceManager.setlizishengchanRate (liziID , newMult);
        //刷新生产等级
        resourceManager.setlizishengjiLevel(liziID, currentLevel + 1);
        Debug.Log($"成功升级生产效率，当前生产等级{currentLevel + 1}，当前生产效率{newMult}");
        return true;

    }

    //尘埃升级
    public bool chenaishengji(int chenaiID)
    {
        if(!chenaishengjiDict.TryGetValue(chenaiID,out var peifang))
        {
            Debug.LogError("加载尘埃合成配方失败");
            return false;
        }
        //获取当前生产等级和计算升级消耗
        int currentLevel = resourceManager.getchenaishengjiLevel (chenaiID);
        double cost = peifang.Upgrade_ACount * Math.Pow(peifang.Upgrade_Multiplier,currentLevel);
        double have = resourceManager.getlizinumber();
        if (have < cost)
        {
            Debug.Log("当前粒子数量不足");
            return false;
        }
        //扣除升级消耗
        resourceManager.liziAdd(-cost);
        //刷新生产效率
        double newMult = resourceManager.getchenaishengchanRate(chenaiID) * peifang.Upgrade_EffectMultiplier;
        resourceManager.setchenaishengchanRate(chenaiID , newMult);
        //刷新生产等级
        resourceManager.setchenaishengjiLevel(chenaiID, currentLevel + 1);
        Debug.Log($"成功升级尘埃生产效率，当前等级{currentLevel + 1}，当前生产效率{newMult}");
        return true;

    }



}
