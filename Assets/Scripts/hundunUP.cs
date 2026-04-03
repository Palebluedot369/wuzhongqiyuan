using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;


public class hundunUP : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;
    public Button closeBtn;
    public Button UPBtn;
    public Button maskBtn;

    private GameResourceManager resourceManager;
    private leidianshengjiData leidianUPAdd;
    

    private void Awake()
    {
        resourceManager = GameResourceManager.Instance;
        if (resourceManager == null)
        {
            Debug.LogError("GameResourceManager 未找到");
            ClosePopup();
            return;
        }

        leidianUPAdd = resourceManager.gethundunUPData();
        if (leidianUPAdd == null)
        {
            Debug.LogError("未找到混沌升级配置数据");
            ClosePopup();
            return;
        }

    }

    private void Start()
    {
        if (closeBtn != null)
            closeBtn.onClick.AddListener(ClosePopup);
        else
            Debug.LogError("closeBtn 未绑定");
        if (UPBtn != null)
            UPBtn.onClick.AddListener(UPlevel);
        else
            Debug.LogError("UPBtn 未绑定");
        if (maskBtn != null)
            maskBtn.onClick.AddListener(ClosePopup);
        else
            Debug.LogError("closeBtn 未绑定");
        UpdateUI();
    }

    private void UpdateUI()
    {
        //获取当前等级，并显示
        int currentLevel = resourceManager.getleidianAddLevel();
        levelText.text = $"混沌等级：{currentLevel}";
        //计算下一级消耗，并显示
        double cost = leidianUPAdd.Cost_firsttime * Math.Pow(leidianUPAdd.Cost_Multiplier, currentLevel);
        costText.text = $"升级消耗：{formatNumber(cost)} 粒子";
    }

    void UPlevel()
    {
        if(resourceManager == null) return;

        int currentLevel = resourceManager.getleidianAddLevel() ;
        double cost = leidianUPAdd.Cost_firsttime * Math.Pow(leidianUPAdd.Cost_Multiplier, currentLevel);
        if (resourceManager.getlizinumber() < cost)
        {
            Debug.Log("粒子不足，无法升级");
        }
        resourceManager.leidianshengjiAdd();
        UpdateUI();
    }

    void ClosePopup()
    {
        Destroy(gameObject);
    }

    string formatNumber(double num)
    {
        if (num >= 100000) return num.ToString("E2");
        else return num.ToString("F0");
    }



}
