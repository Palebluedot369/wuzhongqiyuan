using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wuzhiUPBTN : MonoBehaviour
{
    public GameObject wuzhiPrefab;
    public Transform canvasParent;

    public void OnwuzhiUPClick()
    {
        if (wuzhiPrefab == null)
        {
            Debug.LogError("混沌升级弹窗预制体未设置");
            return;
        }

        Transform parent = canvasParent;
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

        Instantiate(wuzhiPrefab, parent);
    }
}
