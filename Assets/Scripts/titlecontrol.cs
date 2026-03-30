using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class titlecontrol : MonoBehaviour
{

    public TextMeshProUGUI title;
    
    public void SetTitle(string newTitle)
    {
        title.text = newTitle;
    }

    private void Start()
    {
        title.text = "СЃзг";
    }
}
