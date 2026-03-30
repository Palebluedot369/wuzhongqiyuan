using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class chenainumber : MonoBehaviour
{
    public TextMeshProUGUI chenai;

    private void Start()
    {
        var resourcesManager = GameResourceManager.Instance;
        resourcesManager.chenaiChange += updatechenai;
        updatechenai(resourcesManager.getchenainumber());
    }


    void updatechenai(double chenaicount)
    {
        chenai.text = format(chenaicount);
    }

    string format(double chenaicount)
    {
        if (chenaicount >= 100000) return chenaicount.ToString("E2");
        else return chenaicount.ToString("F0");
    }
}
