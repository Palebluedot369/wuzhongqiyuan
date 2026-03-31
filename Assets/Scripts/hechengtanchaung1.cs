using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class hechengtanchaung1 : MonoBehaviour
{
    [Header("弹窗预制体")]
    public GameObject tanchuangPrefab;
    [Header("父级Canvas")]
    public Transform parentCanvas;
    [Header("要合成的物种ID")]
    public int hechengID;

    private lizihechengData peifang;
    private int qianzhiID;



    public void SetPeifang(lizihechengData peifangData,int qianzhiID)
    {
        this.peifang = peifangData;
        this.qianzhiID = qianzhiID;
        InitDisplay();
    }


    void InitDisplay()
    {

    }

    //点击合成按钮时调用,打开合成弹窗
    public void OnTanchuangButtonClick()
    {
        //实例化弹窗
        GameObject popup = Instantiate(tanchuangPrefab, parentCanvas);
        popup.transform.SetAsLastSibling();
        //获取弹窗上负责显示合成内容的组件
        var display = popup.GetComponentInChildren<hechengAll>();
        if (display == null)
        {
            Debug.Log("弹窗预制体缺少 hechengAll 组件");
            Destroy(popup);
            return;
        }

        //获取物种配方
        lizihechengData peifang = GameResourceManager.Instance.getlizihecheng(hechengID);
        if (peifang == null)
        {
            Debug.LogError($"未找到物种 {hechengID} 的合成配方");
            Destroy(popup);
            return;
        }
        //获取前置ID
        qianzhiID = peifang.Craft_Precursor_ID;
        //配方数据传给显示组件
        display.SetRecipe(hechengID,peifang, qianzhiID );
     

    }





}
