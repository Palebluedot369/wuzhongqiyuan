using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Openset : MonoBehaviour
{
    public GameObject settingsPopupPrefab; 
    public Transform canvasParent; 

    public void OnSettingsClick()
    {
        Transform parent = canvasParent;
        if (parent == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("Œ¥’“µΩ Canvas");
                return;
            }
            parent = canvas.transform;
        }
        Instantiate(settingsPopupPrefab, parent);
    }
}
