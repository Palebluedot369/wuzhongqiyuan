using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class liziPanelManager : MonoBehaviour
{
    [Header("粒子面板预制体")]
    public GameObject liziPanelPrefab;
    [Header("content")]
    public Transform contentParent;

    private GameResourceManager resourceManager;
    private List<GameObject> spawnedPanel = new List<GameObject>();
    private HashSet<int> liziUnlocked = new HashSet<int>();


    private void Awake()
    {
        StartCoroutine(WaitForManager());
    }

    private IEnumerator WaitForManager()
    {
        while (GameResourceManager.Instance == null)
            yield return null;
        resourceManager = GameResourceManager.Instance;
        if(resourceManager == null)
        {
            Debug.LogError("liziPanelManager:未找到GameResourceManager");
        }
    }
    //private void Awake()
    //{
    //    resourceManager = GameResourceManager.Instance;
    //    if(resourceManager == null)
    //    {
    //        Debug.LogError("liziPanelManager: GameResourceManager 未找到");
    //        return;
    //    }
    //}

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

        //解锁初始原子核
        liziUnlock(1);

        //订阅物种数量变化事件
        resourceManager.liziwuzhongChange += OnliziCountChanged;

    }

    private void OnDestroy()
    {
        resourceManager.liziwuzhongChange -= OnliziCountChanged;
    }

    //当任意粒子物种数量变化时，检查是否可以解锁下一级物种
    private void OnliziCountChanged(int liziID,double count)
    {
        //获取下一级物种配置
        var nextliziData = resourceManager.getlizibaseData(liziID + 1);
        if(nextliziData != null && !liziUnlocked.Contains(liziID + 1))
        {
            double required = nextliziData.Unlock_Precursor_Required;
            if(count >= required)
            {
                liziUnlock(liziID + 1);
            }
        }
    }
    // 解锁并生成指定物种的面板
    private void liziUnlock(int liziID)
    {
        if(liziUnlocked.Contains(liziID)) return;
        //实例化面板
        GameObject panelObj = Instantiate(liziPanelPrefab, contentParent);
        panelObj.transform.SetAsFirstSibling();

        liziPanel panel = panelObj.GetComponent<liziPanel>();

        if (panel == null)
        {
            Debug.LogError($"预制体上未找到 liziPanel 组件");
            Destroy(panelObj);
            return;
        }
        //初始化面板（传入物种ID）
        panel.Init(liziID);
        spawnedPanel.Add(panelObj);
        liziUnlocked.Add(liziID);
        Debug.Log($"解锁并生成粒子物种：ID={liziID}, 名称={panel.nameText.text}");

    }




}
