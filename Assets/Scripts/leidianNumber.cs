using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class leidianNumber : MonoBehaviour
{
    public TextMeshProUGUI leidian;
    // Start is called before the first frame update
    void Start()
    {
        var resourcesManager = GameResourceManager.Instance;
        resourcesManager.leidianChange += updateleidian;
        updateleidian(resourcesManager.getleidianCount());
    }

    void updateleidian(double leidianCount)//监听雷电数量事件
    {
        // double leidiancount = GameResourceManager.Instance.getleidianCount();
        leidian.text = formatNumber(leidianCount);
    }


    string formatNumber(double num)//大于等于五位数时以科学计数法显示
    {
        if (num >= 100000) return num.ToString("E2");
        else return num.ToString("F0");
    }

    private void OnDestroy()
    {
        if (GameResourceManager.Instance != null)
        {
            GameResourceManager.Instance.leidianChange -= updateleidian;
        }
    }
}
