using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chenaiPanelManager : MonoBehaviour
{
    [Header("尘埃面板预制体")]
    public GameObject chenaiPanelPrefab;
    [Header("content")]
    public Transform contentParent;

    private GameResourceManager resourceManager;
    private List<GameObject> spawnedPanel = new List<GameObject>();
    private HashSet<int> chenaiUnlocked = new HashSet<int>();

    private void Awake()
    {
        StartCoroutine(WaitForManager());
    }

    private IEnumerator WaitForManager()
    {
        while (GameResourceManager.Instance == null)
            yield return null;
        resourceManager = GameResourceManager.Instance;
        if (resourceManager == null)
        {
            Debug.LogError("liziPanelManager:未找到GameResourceManager");
        }
    }

    //当任意粒子物种数量变化时，检查是否可以解锁尘埃
    private void OnliziCountChanged(double count)
    {
        for(int chenaiID = 1; chenaiID <= 11; chenaiID++) 
        {
            //获取尘埃配置
            var chenaiData = resourceManager.getchenaibaseData(chenaiID);
            if (chenaiData != null && !chenaiUnlocked.Contains(chenaiID))
            {
                double required = chenaiData.Unlock_A_Required;
                if (count >= required)
                {
                    chenaiUnlock(chenaiID);
                }
            }
        }
        
    }
    // 解锁并生成指定物种的面板
    private void chenaiUnlock(int chenaiID)
    {
        if (chenaiUnlocked.Contains(chenaiID)) return;
        //实例化面板
        GameObject panelObj = Instantiate(chenaiPanelPrefab, contentParent);
        panelObj.transform.SetAsFirstSibling();
        chenaiPanel panel = panelObj.GetComponent<chenaiPanel>();
        if (panel == null)
        {
            Debug.LogError($"预制体上未找到 chenaiPanel 组件");
            Destroy(panelObj);
            return;
        }
        //初始化面板（传入物种ID）
        panel.Init(chenaiID);
        spawnedPanel.Add(panelObj);
        chenaiUnlocked.Add(chenaiID);
        Debug.Log($"解锁并生成尘埃物种：ID={chenaiID}, 名称={panel.nameText.text}");

    }
    private IEnumerator Start()
    {

        GameResourceManager rm = null;
        while (rm == null)
        {
            rm = GameResourceManager.Instance;
            yield return null;
        }
        resourceManager = rm;


        while (resourceManager.getlizibaseData(1) == null)
            yield return null;

        //订阅物种数量变化事件
        resourceManager.liziChange += OnliziCountChanged;

    }

    private void OnDestroy()
    {
        resourceManager.liziChange -= OnliziCountChanged;
    }

}
