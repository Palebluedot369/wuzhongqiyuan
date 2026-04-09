using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    private string savePath;
    private GameResourceManager resourceManager;
    public static SaveLoadManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        StartCoroutine(WaitForManager());

        //resourceManager = GameResourceManager.Instance;
        //if (resourceManager == null)
        //{
        //    Debug.LogError("SaveLoadManager: GameResourceManager instance not found!");
        //    return;
        //}
        
    }


    private IEnumerator WaitForManager()
    {
        while (GameResourceManager.Instance == null)
            yield return null;
        resourceManager = GameResourceManager.Instance;
        savePath = Path.Combine(Application.persistentDataPath, "game_save.json");
        LoadGame();
    }


    private void Start()
    {
        // 自动加载存档（如果存在）
        //LoadGame();
        // 可选：启动自动保存协程
        StartCoroutine(AutoSaveCoroutine());
    }

    /// <summary>
    /// 手动保存游戏
    /// </summary>
    public void SaveGame()
    {
        if (resourceManager == null) return;
        SaveData data = resourceManager.GetSaveData();
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"游戏已保存到: {savePath}");
    }

    /// <summary>
    /// 手动加载游戏
    /// </summary>
    public void LoadGame()
    {
        if (resourceManager == null) return;
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            if (data != null)
            {
                resourceManager.LoadFromSaveData(data);
                Debug.Log("游戏加载成功");
            }
            else
            {
                Debug.LogError("存档文件损坏");
            }
        }
        else
        {
            Debug.Log("没有找到存档，开始新游戏");
        }
    }

    /// <summary>
    /// 自动保存协程（每60秒保存一次）
    /// </summary>
    private IEnumerator AutoSaveCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f);
            SaveGame();
        }
    }

    private void OnApplicationQuit()
    {
        // 退出时自动保存
        SaveGame();
    }
}
