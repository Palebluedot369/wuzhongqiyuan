using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;    


public class liziName : MonoBehaviour
{
    public TextMeshProUGUI liziname;
    public int lizinameID;

    void Start()
    {
        TextAsset lizijson = Resources.Load<TextAsset> ("meta/lizi");
        if (lizijson != null)
        {
            string jsoncontent = lizijson.text;
            Debug.Log("JSON 加载成功，内容：" + jsoncontent);
            lizilist datalist = JsonUtility.FromJson<lizilist>(jsoncontent);
            if (datalist != null && datalist.lizidatas != null)
            {

                foreach (var lizidata in datalist.lizidatas)
                {
                    if (lizidata.ID == lizinameID)
                    {
                        liziname.text = lizidata.Name;
                        Debug.Log($"加载粒子名称：{lizidata.Name}");
                        return;
                    }

                }
                Debug.LogWarning($"未找到 ID 为 {lizinameID} 的粒子");
            }
            else
            {
                Debug.LogError("反序列化失败！datalist 或 lizidatas 为 null。");
            }
        }
        else
        {
            Debug.LogError("未找到 Resources/meta/lizi.json 文件，请检查路径和文件是否存在。");
        }
    }



     

}

