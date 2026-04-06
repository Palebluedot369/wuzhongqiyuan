using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class chenaihechengshengji : MonoBehaviour
{
    [Header("合成等级")]
    public TextMeshProUGUI hechengLevel;
    [Header("升级消耗")]
    public TextMeshProUGUI hechengCost;
    [Header("升级按钮")]
    public Button shengjiButton;
    [Header("遮罩")]
    public Button maskButton;


    //获取数据
    private int chenaiID;
    private chenaihechengData peifang;

    //资源管理器
    private GameResourceManager _resourceManager;
    private GameResourceManager resourceManager
    {
        get
        {
            if (_resourceManager == null)
                _resourceManager = GameResourceManager.Instance;
            return _resourceManager;
        }
    }

    //传入尘埃ID和合成配方
    public void SetData(int id, chenaihechengData peifangData)
    {
        chenaiID = id;
        peifang = peifangData;

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (peifang == null) return;
        //获取当前合成等级
        int currentLevel = resourceManager.getchenaihechengLevel(chenaiID);
        hechengLevel.text = $"当前合成等级{currentLevel}";

        //计算合成升级消耗
        double cost = peifang.CraftLevelUp_BaseCount * Math.Pow(peifang.CraftLevelUp_Multiplier, currentLevel);
        string costStr = formatNum(cost);
        hechengCost.text = $"升级消耗{costStr}个雷电";

    }

    void OnUpgradeClick()
    {
        if (resourceManager == null)
        {
            Debug.LogError("resourceManager 未找到");
            return;
        }

        //调用资源管理器合成升级方法
        bool success = hechengManager.Instance.chenaihechengshengjiLevel(chenaiID);
        if (success)
        {
            Debug.Log($"尘埃物种{chenaiID}合成等级升级成功");
            tanchuangClose();
        }
        else
        {
            Debug.Log("升级失败，资源不足");
        }
    }

    void tanchuangClose()
    {
        Destroy(gameObject);
    }

    string formatNum(double num)
    {
        if (num >= 100000) return num.ToString("E2");
        else return num.ToString("F0");
    }
    private void Start()
    {
        //resourceManager = GameResourceManager.Instance;
        if (resourceManager == null)
        {
            Debug.LogError("GameResourceManager 未找到");
            tanchuangClose();
            return;
        }

        if (shengjiButton != null)
            shengjiButton.onClick.AddListener(OnUpgradeClick);
        if (maskButton != null)
            maskButton.onClick.AddListener(tanchuangClose);
        if (peifang != null)
            UpdateUI();
    }
}




