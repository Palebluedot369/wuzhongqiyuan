using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lizishengjiButton : MonoBehaviour
{
    private shengjiManager shengjiManager;
    public int liziID;
    public bool islizi = true;

    public void lizishengjiClick()
    {
        bool shengjisuccess = false;

        if (islizi)
            shengjisuccess = shengjiManager.Instance.lizishengji(liziID);
        else
            shengjisuccess = shengjiManager.Instance.chenaishengji(liziID);


    }
}
