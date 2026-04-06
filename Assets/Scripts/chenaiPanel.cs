using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class chenaiPanel : MonoBehaviour
{
    [Header("（请在预制体中绑定）")]

    [Header("名称")]
    public TextMeshProUGUI nameText;
    [Header("数量")]
    public TextMeshProUGUI numText;
    [Header("生产效率")]
    public TextMeshProUGUI productText;
    [Header("合成按钮")]
    public Button hechengBTN;
    [Header("精炼按钮")]
    public Button shengjiBTN;

    [Header("大弹窗")]
    public GameObject hechengtanchuang;
    public Transform canvasParent;
    public Transform cachedCanvasParent;

    private int chenaiID;
    private GameResourceManager resourceManager;

    private void Awake()
    {
        if (canvasParent != null)
        {
            cachedCanvasParent = canvasParent;
        }
        else
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
                Debug.LogError("场景中没有找到 Canvas");
            else
                cachedCanvasParent = canvas.transform;
        }
        StartCoroutine(WaitForManager());
    }

    private IEnumerator WaitForManager()
    {
        while (GameResourceManager.Instance == null)
            yield return null;
        resourceManager = GameResourceManager.Instance;

    }

    //订阅尘埃物种数量、生产效率事件
    private void OnEnable()
    {

        if (resourceManager != null)
        {
            resourceManager.chenaiwuzhongChange += OnchenaiCountChanged;
        }
        if (resourceManager != null)
        {
            resourceManager.chenaiproductChange += OnchenaiproductChanged;
        }


    }
    private void OnDisable()
    {

        if (resourceManager != null)
        {
            resourceManager.chenaiwuzhongChange -= OnchenaiCountChanged;

        }
        if (resourceManager != null)
        {
            resourceManager.chenaiproductChange -= OnchenaiproductChanged;
        }
    }

    //初始化面板
    public void Init(int ID)
    {
        resourceManager = GameResourceManager.Instance;
        chenaiID = ID;
        if (resourceManager == null)
        {
            Debug.LogError("resourceManager 未就绪，请稍后调用 Init");
            return;
        }

        //获取名称
        var baseData = resourceManager.getchenaibaseData(chenaiID);
        if (baseData != null)
            nameText.text = baseData.Name;
        else
            nameText.text = $"物种ID{chenaiID}";
        //获取数量
        double count = resourceManager.getOtherchenainumber(chenaiID);
        numText.text = "数量：" + formatNum(count);
        //获取生产效率
        double product = resourceManager.getchenaishengchanRate(chenaiID);
        productText.text = "生产：" + formatNum(product);

        //绑定按钮事件
        shengjiBTN.onClick.RemoveAllListeners();
        shengjiBTN.onClick.AddListener(OnshengjiClick);
        hechengBTN.onClick.RemoveAllListeners();
        hechengBTN.onClick.AddListener(OnhechengClick);
    }
    //尘埃数量改变事件
    private void OnchenaiCountChanged(int id, double count)
    {
        if (id == chenaiID)
            numText.text = "数量：" + formatNum(count);

    }
    //尘埃生产效率改变事件
    private void OnchenaiproductChanged(int id, double count)
    {
        if (id == chenaiID)
            productText.text = "生产：" + formatNum(count);
    }

    //生产效率升级按钮点击
    private void OnshengjiClick()
    {
        if (shengjiManager.Instance != null)
            shengjiManager.Instance.chenaishengji(chenaiID);
        else
            Debug.LogError("升级管理器未找到");

    }

    //合成按钮点击,打开大弹窗
    private void OnhechengClick()
    {
        if (hechengtanchuang == null)
        {
            Debug.LogError("合成弹窗预制体未设置");
            return;
        }

        if (cachedCanvasParent == null)
        {
            Debug.LogError("未找到父 Canvas，无法实例化弹窗");
            return;
        }

        Transform parent = cachedCanvasParent;
        if (parent == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("场景中没有找到 Canvas");
                return;
            }
            parent = canvas.transform;
        }


        //实例化弹窗
        GameObject tanchuang = Instantiate(hechengtanchuang, parent);
        tanchuang.transform.SetAsLastSibling();

        //获取弹窗内组件
        var display = tanchuang.GetComponentInChildren<chenaihechengAll>(true);
        if (display == null)
        {
            Debug.LogError("弹窗内未找到 chenaihechengAll 组件");
            Destroy(tanchuang);
            return;
        }
        //获取合成配方
        var peifang = resourceManager.getchenaihecheng(chenaiID);
        if (peifang == null)
        {
            Debug.LogError($"未找到{chenaiID}合成配方");
            return;
        }
      
        display.SetPeifang(chenaiID, peifang);
    }

    private string formatNum(double num)
    {
        if (num >= 100000) return num.ToString("E2");
        else return num.ToString("F0");
    }

}
