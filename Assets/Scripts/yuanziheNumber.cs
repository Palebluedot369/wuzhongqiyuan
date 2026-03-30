using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class yuanziheNumber : MonoBehaviour
{
    public TextMeshProUGUI yuanzihenum;
    public int liziID;

    private void Start()
    {
        var resourcesManager = GameResourceManager.Instance;
        resourcesManager.liziwuzhongChange += updateliziwuzhong;
        double currentcount = resourcesManager.getOtherlizinumber(liziID);
        updateliziwuzhong(liziID, currentcount);
    }

    void updateliziwuzhong(int id,double liziwuzhongcount)
    {
        if(id == liziID)
        {
            updatecount(id,liziwuzhongcount);
        }
    }
    void updatecount(int id,double liziwuzhongcount)
    {
        yuanzihenum.text = formatNumber(liziwuzhongcount);
    }

    string formatNumber(double count)
    {
        if (count >= 100000) return count.ToString("E2");
        else return count.ToString("F0");
    }

}
