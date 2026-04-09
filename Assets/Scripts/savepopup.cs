using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class savepopup : MonoBehaviour
{
    public Button closeButton;
    public Button saveButton;

    private void Start()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePopup);
        if (saveButton != null)
            saveButton.onClick.AddListener(SaveGameAndClose);
    }

    private void SaveGameAndClose()
    {
        // 调用存档管理器保存
        if (SaveLoadManager.Instance != null)
            SaveLoadManager.Instance.SaveGame();
        else
            Debug.LogError("SaveLoadManager 未找到");

        ClosePopup();
    }

    private void ClosePopup()
    {
        Destroy(gameObject);
    }
}
