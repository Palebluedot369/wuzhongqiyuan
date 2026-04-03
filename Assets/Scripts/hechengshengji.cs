using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class hechengshengji : MonoBehaviour
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
    private int liziID;
    private lizihechengData peifang;
    private int costliziID;
    private string costliziName;
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
        if(peifang != null)
            UpdateUI();
    }

    //传入物种ID和配方
    public void SetData(int id,lizihechengData peifangData)
    {
        liziID = id;
        peifang = peifangData;
        costliziID = peifang.CraftLevelUp_Consume_ID;


        //Debug.Log($"costliziID:{costliziID}");

        if(costliziID == 0)
        {
            costliziName = "无";
        }
        else
        {
            //获取消耗物种名称
            //resourceManager = GameResourceManager.Instance;
            var liziData = resourceManager.getlizibaseData(costliziID);
            if (liziData == null)
            {
                costliziName = $"物种{costliziID}";
                Debug.LogWarning($"未找到物种 {costliziID} 的基础数据");
            }
            else
            {
                costliziName = liziData.Name;
            }

            costliziName = liziData != null ? liziData.Name : "未知物种";
        }
        
        UpdateUI();
    }

    //UI更新
    void UpdateUI()
    {
        if(peifang == null) return;

        //获取当前合成等级
        int currentLevel = resourceManager.getlizihechengLevel(liziID);
        hechengLevel.text = $"当前合成等级{currentLevel}";

        //计算升级消耗
        double cost = peifang.CraftLevelUp_BaseCount * Math.Pow((float)peifang.CraftLevelUp_Multiplier , currentLevel);
        string costStr = formatNum(cost);

        hechengCost.text = $"升级消耗{costStr}个{costliziName}";

        if(costliziID == 0)
        {
            shengjiButton.interactable = false;
            hechengCost.text = "已达到最高等级";
        }

    }

    void OnUpgradeClick()
    {
        if(resourceManager == null)
        {
            Debug.LogError("resourceManager 未找到");
            return;
        }

        //调用资源管理器合成升级方法
        bool success = hechengManager.Instance.lizihechengshengjiLevel(liziID);
        if (success)
        {
            Debug.Log($"物种{liziID}合成等级升级成功");
            tanchuangClose();
        }
        else
        {
            Debug.Log("升级失败，资源不足或已达最高等级");
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

}
