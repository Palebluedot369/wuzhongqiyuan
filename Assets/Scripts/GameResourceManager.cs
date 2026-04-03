using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking; 
using UnityEngine.UI;
using System;

public class GameResourceManager : MonoBehaviour
{
    public static GameResourceManager Instance { get; private set; }

    private void Awake()
    {
        Debug.Log("GameResourceManager Awake 执行");
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //粒子、雷电、尘埃初始数量
    private double lizicount;
    private double leidiancount;
    private double chenaicount;
    private double zhihuicount;
    private double anwuzhicount;

    //粒子物种、尘埃物种数量
    private double[] liziSpecies = new double[22];
    private double[] chenaiSpecies = new double[12];
    
    //粒子物种、尘埃物种生产效率
    private double[] lizishengchanRates = new double[22];
    private double[] chenaishengchanRates = new double[12];

    //合成效率
    private double[] lizihechengMultiplier = new double[22];
    private double[] chenaihechengMultiplier = new double[12];

    //生产等级
    private int[] lizishengjiLevel = new int[22];  
    private int[] chenaishengjiLevel = new int[12];
    //合成等级
    private int[] lizihechengLevel = new int[22];  
    private int[] chenaihechengLevel = new int[12];


    //雷电生产相关
    private double leidianshengchanRate = 1;
    private bool isleidianUnlocked = false;
    private double leidianBaseRate = 1;
    private double leidianpercentBonus = 1;
    private int leidianAddLevel = 0;
    private int leidianPercentLevel = 0;


    //智慧生产相关




    //物种数据存储的物种数据字典
    private Dictionary<int, lizibaseData> lizidataDict = new Dictionary<int, lizibaseData>(); 
    private Dictionary<int, chenaibaseData> chenaidataDict = new Dictionary<int, chenaibaseData>();
    private Dictionary<int, lizihechengData> lizihechengDict = new Dictionary<int, lizihechengData>();
    private Dictionary<int, chenaihechengData> chenaihechengDict = new Dictionary<int, chenaihechengData>();
    private Dictionary<int, lizishengjiData> lizishengjiDict = new Dictionary<int, lizishengjiData>();
    private Dictionary<int, chenaishengjiData> chenaishengjiDict = new Dictionary<int, chenaishengjiData>();



    //加载所有数据
    void LoadAllData()
    {
        //加载粒子物种数据
        TextAsset lizidatajson = Resources.Load<TextAsset>("meta/lizi");
        if (lizidatajson == null)
        { 
            Debug.Log("未找到lizi.json"); 
        }
        else
        {
            var list = JsonUtility.FromJson<liziBaseList>(lizidatajson.text);
            if (list?.lizidatas != null)
            {
                foreach (var data in list.lizidatas)
                    lizidataDict[data.ID] = data;
               // Debug.Log($"成功加载 {lizidataDict.Count} 个粒子物种基础数据");
            }
            else
            {
                Debug.LogError("解析 lizi.json 失败");
            }

        }

        //加载粒子升级数据
        TextAsset lizishengjidatajson = Resources.Load<TextAsset>("meta/lizishengji");
        if(lizishengjidatajson == null)
        {
            Debug.Log("未找到lizishengji.json");
        }
        else
        {
            var list = JsonUtility.FromJson<liziShengJiList>(lizishengjidatajson.text);
            foreach(var data in list.lizishengji)
            {
                lizishengjiDict[data.ID] = data;
            }
        }
        //加载粒子合成数据
        TextAsset lizihechengdatajson = Resources.Load<TextAsset>("meta/lizihecheng");
        if(lizihechengdatajson == null)
        {
            Debug.Log("未找到lizihecheng.json");
        }
        else
        {
            var list = JsonUtility.FromJson<liziHeChengList>(lizihechengdatajson.text);
            foreach (var data in list.lizihecheng)
            {
                lizihechengDict[data.ID] = data;
            }
        }


        //加载尘埃物种数据
        TextAsset chenaidatajson = Resources.Load<TextAsset>("meta/chenai");
        if(chenaidatajson == null)
        {
            Debug.Log("未找到chenai.json");
        }
        else
        {
            var list = JsonUtility.FromJson<chenaiBaseList>(chenaidatajson.text);
            foreach(var data in list.chenaidatas)
            {
                chenaidataDict[data.ID] = data;
            }
        }
        //加载尘埃升级数据
        TextAsset chenaishengjidatajson = Resources.Load<TextAsset>("meta/chenaishengji");
        if (chenaishengjidatajson == null)
        {
            Debug.Log("未找到chenaishengji.json");
        }
        else
        {
            var list = JsonUtility.FromJson<chenaiShengJiList>(chenaishengjidatajson.text);
            foreach (var data in list.chenaishengji)
            {
                chenaishengjiDict[data.ID] = data;
            }
        }
        //加载尘埃合成数据
        TextAsset chenaihechengdatajson = Resources.Load<TextAsset>("meta/chenaihecheng");
        if (chenaihechengdatajson == null)
        {
            Debug.Log("未找到chenaihecheng.json");
        }
        else
        {
            var list = JsonUtility.FromJson<chenaiHeChengList>(chenaihechengdatajson.text);
            foreach(var data in list.chenaihecheng)
            {
                chenaihechengDict[data.ID] = data;
            }
        }
        


    }

    //初始化物种生产数据
    void InitFromData()
    {
        LoadIntialResources();
        //初始化粒子物种基础生产和合成效率
        for (int i = 1;i <= 21; i++)
        {
            if(lizidataDict.TryGetValue(i,out var data))
            {
                lizishengchanRates[i] = data.Production_Rate;    //赋值粒子物种基础生产效率            
            }
            lizihechengMultiplier[i] = 1;//赋值粒子物种基础合成效率
        }


        //初始化尘埃物种基础生产和合成效率
        for(int i = 1;i <= 11; i++)
        {
            if(chenaidataDict.TryGetValue(i,out var data))
            {
                chenaishengchanRates[i] = data.Production_Rate;//赋值尘埃物种基础生产效率     
            }
            chenaihechengMultiplier[i] = 1;//赋值尘埃物种基础合成效率
        }        
    }


    //初始化基础资源数量
    void LoadIntialResources()
    {
        TextAsset baseResources = Resources.Load<TextAsset>("meta/base");
        if(baseResources == null)
        {
            Debug.Log("未找到基础资源json");
            liziAdd(10);leidianAdd(0);chenaiAdd(0);zhihuiAdd(0);anwuzhiAdd(0);
            return;
        }

        var list = JsonUtility.FromJson<BasicResourceList>(baseResources.text);
        if(list == null || list.basedatas ==null)
        {
            Debug.LogError("基础资源 JSON 解析失败！请检查 JSON 格式。");
            liziAdd(10); leidianAdd(0); chenaiAdd(0); zhihuiAdd(0); anwuzhiAdd(0);
            return;
        }

        foreach (var result in list.basedatas)
        {
            switch (result.ResourceID)
            {
                case "A":
                    liziAdd(result.InitialAmount);
                    break;
                case "B":
                    leidianAdd(result.InitialAmount);
                    break;
                case "C":
                    chenaiAdd(result.InitialAmount);
                    break;
                case "D":
                    zhihuiAdd(result.InitialAmount);
                    break;
                case "E":
                    anwuzhiAdd(result.InitialAmount);
                    break;
                default:
                    Debug.Log($"未处理的资源类型：{result.ResourceID}");
                    break;
            }
        }
    }

    //协程生产资源
    IEnumerator ShengchanCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            
            ShengchanResources();
        }
    }


    //资源生产方法
    void ShengchanResources()
    {
        //雷电是否解锁
        if (!isleidianUnlocked && lizicount >= 10)
        {
            isleidianUnlocked = true;
            Debug.Log("雷电已解锁");
        }

        //雷电生产
        if (isleidianUnlocked)
        {
            double totalRate = leidianBaseRate + leidianAddLevel;
            totalRate *= Math.Pow(1.1, leidianPercentLevel);
            Debug.Log($"当前雷电产量{totalRate}");
            leidianAdd(totalRate);
        }

        //粒子生产
        for (int i = 1; i <= 21; i++)
        {
            double amount = liziSpecies[i] * lizishengchanRates[i];
            if (amount == 0) continue;
            if (lizidataDict.TryGetValue(i, out var data))
            {
                int target = data.Production_Target;
                if (target == 0)
                {
                    liziAdd(amount);
                }
                else
                {
                    liziwuzhongAdd(target,amount);
                }
            }
            else
            {
                liziAdd(amount);
            }
        }

        //尘埃生产
        for (int i = 1; i <= 11; i++)
        {
            double amount = chenaiSpecies[i] * chenaishengchanRates[i];
            if(amount > 0)
            {
                chenaiAdd(amount);
            }
        }


        //智慧生产


    }


    //各资源事件
    public event Action<double> leidianChange;//雷电数量事件
    public event Action<double> liziChange;//粒子数量事件
    public event Action<double> chenaiChange;//尘埃数量事件
    public event Action<double> zhihuiChange;//智慧数量事件
    public event Action<double> anwuzhiChange;//暗物质数量事件
    public event Action<int, double> liziwuzhongChange;//粒子物种数量事件
    public event Action<int, double> chenaiwuzhongChange;//尘埃物种数量事件
    public event Action<int, double> liziproductChange;//粒子物种生产效率事件
    public event Action<int, double> chenaiproductChange;//粒子物种生产效率事件
    public event Action<int> OnliziUnlocked;//粒子物种解锁事件
    public event Action<int> OnchenaiUnlocked;//尘埃物种解锁事件


    //公共方法获取GameResourcesManger内数据
    public double getlizinumber() => lizicount;//粒子数量
    public double getOtherlizinumber(int id) => liziSpecies[id];//根据ID获取不同粒子物种的数量
    public double getchenainumber() => chenaicount;//尘埃数量
    public double getOtherchenainumber(int id) => chenaiSpecies[id];//根据ID获取不同尘埃物种的数量
    public double getzhihuinumber() => zhihuicount;//智慧数量
    public double getanwuzhinumber() => anwuzhicount;//暗物质数量
    public lizishengjiData getlizishengji(int id)//获取粒子升级配方
    {
        if (lizishengjiDict.ContainsKey(id))
            return lizishengjiDict[id];
        else return null;
    }
    public double getlizishengchanRate(int id) => lizishengchanRates[id];//获取粒子物种生产效率
    public void setlizishengchanRate(int id, double rate)//粒子生产效率修改方法
    {
        lizishengchanRates[id] = rate;
    }
    public lizihechengData getlizihecheng(int id)//获取粒子合成配方
    {
        if (lizihechengDict.ContainsKey(id))
            return lizihechengDict[id];
        else return null;
    }
    public double getlizihechengMultiplier(int id) => lizihechengMultiplier[id];//获取粒子物种合成效率

    public void setlizihechengMultiplier(int id,double Multiplier)//粒子合成效率修改方法
    {
        lizihechengMultiplier[id] = Multiplier;
    }
    public int getlizishengjiLevel(int id) => lizishengjiLevel[id];//获取粒子升级等级
    public void setlizishengjiLevel(int id,int level) => lizishengjiLevel[id] = level;//粒子升级等级修改方法
    public int getlizihechengLevel(int id) => lizihechengLevel[id];//获取粒子合成等级
    public void setlizihechengLevel(int id,int level) => lizihechengLevel[id] = level;//粒子合成等级修改方法
    public lizibaseData getlizibaseData(int id) //获取粒子物种基础配置数据
    {
        if (lizidataDict == null)
        {
            Debug.LogError("lizidataDict 未初始化！请确保 LoadAllData 已执行。");
            return null;
        }
        return lizidataDict.ContainsKey(id) ? lizidataDict[id] : null;
    }  

    public double getchenaishengchanRate(int id) => chenaishengchanRates[id]; //获取尘埃物种生产效率
    public void setchenaishengchanRate(int id, double Rate)//尘埃生产效率修改方法
    {
        chenaishengchanRates[id] = Rate;
    }

    public chenaishengjiData getchenaishengji(int id)//获取尘埃升级配方
    {
        if(chenaishengjiDict.ContainsKey(id))
            return chenaishengjiDict[id];
        else return null;
    }

    public double getchenaihechengMultiplier(int id,bool islizi)//获取尘埃物种合成效率
    {
        if (islizi) 
            return chenaihechengMultiplier[id];
        else return 1;
    }
    public void setchenaihechengMultiplier(int id,double Multiplier)
    {
        chenaihechengMultiplier[id] = Multiplier;
    }

    public chenaihechengData getchenaihecheng(int id)//获取尘埃合成配方
    {
        if(chenaihechengDict.ContainsKey(id))
            return chenaihechengDict[id]; 
        else return null;
    }
    public int getchenaishengjiLevel(int id) => chenaishengjiLevel[id];//获取尘埃升级等级
    public void setchenaishengjiLevel(int id, int level) => chenaishengjiLevel[id] = level;//粒子升级等级修改方法
    public int getchenaihechengLevel(int id) => chenaihechengLevel[id];//获取粒子合成等级
    public void setchenaihechengLevel(int id, int level) => chenaihechengLevel[id] = level;//粒子合成等级修改方法
    public chenaibaseData getchenaibaseData(int id) => chenaidataDict.ContainsKey(id) ? chenaidataDict[id] : null;




    //雷电数据相关公共方法
    public int getleidianAddLevel() => leidianAddLevel;
    public int getleidianPercentLevel() => leidianPercentLevel;
    public double getleidianCount() => leidiancount;

    //雷电升级方法
    public void leidianshengjiAdd()
    {
        if (!isleidianUnlocked)
        {
            Debug.Log("雷电未解锁");
            return;
        }

        double cost = 10 * Math.Pow(10, leidianAddLevel);
        if(lizicount >= cost)
        {
            lizicount -= cost;
            leidianAddLevel++;
            Debug.Log($"雷电升级成功，当前混沌等级{leidianAddLevel}");
        }
        else
        {
            Debug.Log("粒子不足");
        }
    }
    public void leidianshengjiPercent()
    {
        if (!isleidianUnlocked)
        {
            Debug.Log("雷电未解锁");
            return;
        }
        double cost = 1000 * Math.Pow(3, leidianPercentLevel);
        if(chenaicount >= cost)
        {
            chenaicount -= cost;
            leidianPercentLevel++;
            Debug.Log($"雷电升级成功，当前物质等级{leidianPercentLevel}");
        }
        else
        {
            Debug.Log("当前尘埃不足");
        }

    }



    //各资源增加方法
    public void leidianAdd(double amount)
    {
        leidiancount += amount;
        leidianChange?.Invoke(leidiancount);//雷电数量事件
    }
    public void liziAdd(double amount)
    {
        lizicount += amount;
        liziChange?.Invoke(lizicount);//粒子数量事件
    }
    public void chenaiAdd(double amount)
    {
        chenaicount += amount;
        chenaiChange?.Invoke(chenaicount);//尘埃数量事件
    }
    public void zhihuiAdd(double amount)
    {
        zhihuicount += amount;
        zhihuiChange?.Invoke(zhihuicount);//智慧数量事件
    }
    public void anwuzhiAdd(double amount)
    {
        anwuzhicount += amount;
        anwuzhiChange?.Invoke(anwuzhicount);//暗物质数量事件
    }
    public void liziwuzhongAdd(int id,double amount)
    {
        liziSpecies[id] += amount;
        liziwuzhongChange?.Invoke(id, liziSpecies[id]);//粒子物种数量事件
    }
    public void chenaiwuzhongAdd(int id,double amount)
    {
        chenaiSpecies[id] += amount;
        chenaiwuzhongChange?.Invoke(id, chenaiSpecies[id]);//尘埃物种数量事件
    }
    public void liziproductAdd(int id,double amount)
    {
        lizishengchanRates[id] = amount;
        liziproductChange?.Invoke(id, lizishengchanRates[id]);//粒子物种生产效率事件
    }
    public void chenaiproductAdd(int id,double amount)
    {
        chenaishengchanRates[id] = amount;
        chenaiproductChange?.Invoke(id, chenaishengchanRates[id]);//尘埃物种生产效率事件
    }



    //void printlog()
    //{
    //    Debug.Log($"粒子数量{lizicount}");
    //    Debug.Log($"雷电数量{leidiancount}");

    //}

    void Start()
    {
        Instance = this;
        LoadAllData();
        InitFromData();
        StartCoroutine(ShengchanCoroutine());
        //InvokeRepeating("printlog",0,1);
        



    }
}



