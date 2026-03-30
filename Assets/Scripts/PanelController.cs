using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public GameObject[] PanelControl;
    public Button[] selectbutton;
    public Color normalcolor = Color.white;
    public Color selectcolor = Color.yellow;
    private int currtentindex = 0;

    // Start is called before the first frame update
    void Start()
    {
        SelectButton(0);
    }


    public void SelectButton(int index)
    {
        if(index < 0 || index >= PanelControl.Length) return;
        for (int i = 0; i < PanelControl.Length; i++)
        {
            PanelControl[i].SetActive(i == index);
        }
        UpdateButton(index);
        currtentindex = index;

    }

    void UpdateButton(int selectedindex)
    {
        if (selectbutton == null || selectbutton.Length == 0) return;
        for (int i = 0; i < selectbutton.Length; i++)
        {
            if (selectbutton[i] != null)
            {
                Image btnImage = selectbutton[i].GetComponent<Image>();
                if (btnImage != null)
                {
                    btnImage.color = (selectedindex == i ? selectcolor : normalcolor);
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
