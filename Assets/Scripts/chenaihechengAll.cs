using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class chenaihechengAll : MonoBehaviour
{
    [Header("小弹窗预制体")]
    public GameObject chenaihechengshengjiPrefab;
    [Header("父级Canvas")]
    public Transform canvasParent;
    [Header("升级按钮")]
    public Button chenaishengji;

    public bool ischenai = true;
    private GameResourceManager resourceManager;
    public Slider amountSlider;
    public TextMeshProUGUI hechengNum;
    public TextMeshProUGUI xiaohaoNum;
    private chenaihechengData peifang;
    private int chenaiID;

    private void Start()
    {
        resourceManager = GameResourceManager.Instance;

        if (amountSlider != null)
        {
            amountSlider.onValueChanged.AddListener(OnSliderValueChange);//添加监听滑动条事件           
        }
        chenaishengji.onClick.AddListener(Openshengji);

        if (canvasParent == null)
        {
            canvasParent = FindObjectOfType<Canvas>().transform;
            if (canvasParent == null)
                Debug.LogError("场景中没有找到 Canvas");
        }

    }
    //打开合成升级弹窗
    void Openshengji()
    {
        if (chenaihechengshengjiPrefab == null || canvasParent == null)
        {
            Debug.LogError("小弹窗预制体或父级 Canvas 未设置");
            return;
        }

        GameObject tanchuang = Instantiate(chenaihechengshengjiPrefab, canvasParent);
        tanchuang.transform.SetAsLastSibling();

        //获取升级弹窗上的脚本组件
        var shengjiCtrl = tanchuang.GetComponent<chenaihechengshengji>();
        if (shengjiCtrl != null)
        {
            //传递当前物种ID和配方
            shengjiCtrl.SetData(chenaiID, peifang);

        }
        else
        {
            Debug.LogError("小弹窗预制体缺少 chenaihechengshengjiPrefab 组件");
            Destroy(tanchuang);
        }
    }
    //传配方数据并获取滑动条数据
    public void SetPeifang(int id, chenaihechengData recipe, bool ischenaiType = true)
    {
        chenaiID = id;
        peifang = recipe;        
        ischenai = ischenaiType;

        if (amountSlider != null)
        {
            amountSlider.value = 0.5f;
            OnSliderValueChange(amountSlider.value);
        }

    }
    //获取滑动条数据并计算合成数量
    void OnSliderValueChange(float percent)
    {
        resourceManager = GameResourceManager.Instance;
        double maxHechengcount = hechengMaxCalculate();
        double hechengCount = Math.Floor(maxHechengcount * percent);
        string hechengStr = formatNum(resourceManager.getchenaihechengMultiplier(chenaiID,true) * hechengCount);
        string text = $"合成：{hechengStr}";
        hechengNum.text = text;
        updateCostInfo(hechengCount);
    }
    //计算当前可合成的数量最大值
    double hechengMaxCalculate()
    {
        //获取当前粒子、雷电数量
        double lizinum = GameResourceManager.Instance.getlizinumber();
        double leidiannum = GameResourceManager.Instance.getleidianCount();
        //获取单次合成所需材料数量
        double liziPer = peifang.Craft_A_Cost;
        double leidianPer = peifang.Craft_B_Cost;

        //计算当前每种资源可合成的数量
        double maxFromlizi = Math.Floor(lizinum / liziPer);
        double maxFromleidian = Math.Floor(leidiannum / leidianPer);  
       
        //计算两者最小值作为可合成的数量上限
        double maxHecheng = Math.Min(maxFromlizi, maxFromleidian);
        return maxHecheng;
    }
    void updateCostInfo(double hechengCount)
    {
        //计算总消耗
        double liziCost = peifang.Craft_A_Cost * hechengCount;
        double leidianCost = peifang.Craft_B_Cost * hechengCount;
        string liziStr = formatNum(liziCost);
        string leidianStr = formatNum(leidianCost);
        string text = $"粒子消耗：{liziStr}\n雷电消耗：{leidianStr}";

        xiaohaoNum.text = text;
    }


    string formatNum(double number)
    {
        if (number > 100000) return number.ToString("E2");
        else return number.ToString("F0");
    }

    public void chenaihechengClick()
    {
        //获取当前合成数量
        string numText = hechengNum.text.Replace("合成：", "").Trim();
        if (!double.TryParse(numText, out double hechengCount))
        {
            Debug.LogError("无法解析合成数量");
            return;
        }
        if (hechengCount <= 0)
        {
            return;
        }

        //调用尘埃合成方法
        bool chenaisuccess = false;
        if (ischenai)
            chenaisuccess = hechengManager.Instance.chenaihecheng(chenaiID, hechengCount);

        if (chenaisuccess)
        {

            //resourceManager = GameResourceManager.Instance;
            //resourceManager.liziwuzhongAdd(liziID,hechengCount);

            var popupRoot = GetComponentInParent<tanchaungController>();
            if (popupRoot != null)
            {
                popupRoot.ClosePopup();
            }
            else
                Debug.LogWarning("未找到弹窗控制器，无法自动关闭");
        }
        else
        {
            Debug.Log("合成失败，资源不足");
        }

    }

}
