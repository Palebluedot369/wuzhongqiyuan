using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class lizinumber : MonoBehaviour
{
    public TextMeshProUGUI lizi;

    private void Start()
    {
        var resourcesManger = GameResourceManager.Instance;
        resourcesManger.liziChange += updatelizi;
        updatelizi(resourcesManger.getlizinumber());
    }

    void updatelizi(double liziCount)
    {
        lizi.text = formatNumber(liziCount);
    }

    string formatNumber(double lizicount)
    {
        if (lizicount >= 100000) return lizicount.ToString("E2");
        else return lizicount.ToString("F0");
    }

    private void OnDestroy()
    {
        if(GameResourceManager.Instance != null)
        {
            GameResourceManager.Instance.liziChange -= updatelizi;
        }
    }
}
