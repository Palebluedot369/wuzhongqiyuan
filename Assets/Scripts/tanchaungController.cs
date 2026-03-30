using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tanchaungController : MonoBehaviour
{
    public Button closeButton;



    // Start is called before the first frame update
    void Start()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClosePopup);
        }
        
    }

    void ClosePopup()
    {
        Destroy(gameObject);
    }



}
    