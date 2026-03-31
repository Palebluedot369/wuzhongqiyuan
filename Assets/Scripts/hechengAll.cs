using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class hechengAll : MonoBehaviour
{
    [Header("小弹窗预制体")]
    public GameObject hechengshengjiPrefab;
    [Header("父级Canvas")]
    public  Transform canvasParent;
    [Header("升级按钮")]
    public Button hechengshengji;



    public Slider amountSlider;
    public TextMeshProUGUI hechengNum;
    public TextMeshProUGUI xiaohaoNum;
    private lizihechengData peifang;
    private int qianzhiID;
    private int liziID;

    private void Start()
    {
        if(amountSlider  != null)
        {
            amountSlider.onValueChanged.AddListener(OnSliderValueChange);//添加监听滑动条事件           
        }
        hechengshengji.onClick.AddListener(Openshengji);

        if (canvasParent == null)
        {
            canvasParent = FindObjectOfType<Canvas>().transform;
            if (canvasParent == null)
                Debug.LogError("场景中没有找到 Canvas");
        }

    }

    void Openshengji()
    {
        if(hechengshengjiPrefab == null || canvasParent == null)
        {
            Debug.LogError("小弹窗预制体或父级 Canvas 未设置");
            return;
        }

        GameObject tanchuang = Instantiate(hechengshengjiPrefab, canvasParent);
        tanchuang.transform.SetAsLastSibling();

        //获取升级弹窗上的脚本组件
        var shengjiCtrl = tanchuang.GetComponent<hechengshengji>();
        if(shengjiCtrl != null)
        {
            //传递当前物种ID和配方
            shengjiCtrl.SetData(liziID, peifang);

        }
        else
        {
            Debug.LogError("小弹窗预制体缺少 hechengshengji 组件");
            Destroy(tanchuang);
        }
    }



    public void SetRecipe(int id,lizihechengData recipe, int precursorId)
    {
        liziID = id;
        peifang = recipe;
        qianzhiID = precursorId;

        if (amountSlider != null)
        {
            amountSlider.value = 0.5f;
            OnSliderValueChange(amountSlider.value);
        }

    }
    //滑动条变动时调用计算并显示实际合成数量
    void OnSliderValueChange(float percent)
    {
        double maxHechengcount = hechengMaxCalculate();
        double hechengCount = Math.Floor(maxHechengcount * percent);
        string hechengStr = formatNum(hechengCount);
        string text = $"合成：{hechengStr}";
        hechengNum.text = text;
        updateCostInfo(hechengCount);
    }


    double hechengMaxCalculate()
    {
        //获取当前粒子、雷电、前置物种数量
        double lizinum = GameResourceManager.Instance.getlizinumber();
        double leidiannum = GameResourceManager.Instance.getleidianCount();
        //获取单次合成所需材料数量
        double liziPer = peifang.Craft_A_Cost;
        double leidianPer = peifang.Craft_B_Cost;
        //double wuzhongPer = peifang.Craft_Precursor_Cost;

        //计算当前每种资源可合成的数量
        double maxFromlizi = Math.Floor(lizinum / liziPer);
        double maxFromleidian = Math.Floor(leidiannum / leidianPer);
        double maxFromwuzhong = double.MaxValue;

        
        if (qianzhiID != 0)
        {
            double wuzhongnum = GameResourceManager.Instance.getOtherlizinumber(qianzhiID);
            double wuzhongPer = peifang.Craft_Precursor_Cost;
            if(wuzhongPer > 0)
            {
                maxFromwuzhong = Math.Floor(wuzhongnum / wuzhongPer);
            }
        }
           
        //计算三者最小值作为可合成的数量上限
        double maxHecheng = Math.Min(maxFromlizi, Math.Min(maxFromwuzhong, maxFromleidian));
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

        if (qianzhiID != 0)
        {
            double wuzhongCost = peifang.Craft_Precursor_Cost * hechengCount;
            string wuzhongStr = formatNum(wuzhongCost);
            text += $"\n物种消耗：{wuzhongStr}";
        }
        xiaohaoNum.text = text ;
    }

    string formatNum(double number)
    {
        if (number > 100000) return number.ToString("E2");
        else return number.ToString("F0");
    }


}
