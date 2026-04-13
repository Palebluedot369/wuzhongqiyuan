using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frameManager : MonoBehaviour
{
    void Awake()
    {
        // 重要：确保这个对象在加载新场景时不会被销毁
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // 1. 关闭垂直同步（移动端会被忽略，但在编辑器和其他平台生效）
        QualitySettings.vSyncCount = 0;
        // 2. 关闭 Player Settings 中的帧率限制，让代码接管
        Application.targetFrameRate = -1;

        // --- 选择下面其中一种方式设置目标帧率 ---

        // 方式A: 直接设置一个固定值 (简单直接)
        // Application.targetFrameRate = 120;

        // 方式B: 自动设置为屏幕支持的最高刷新率 (更智能)
        SetHighestRefreshRate();

        // 打印最终生效的帧率设置，方便调试
        Debug.Log($"帧率已设置为: {Application.targetFrameRate} FPS");
    }

    // 自动获取并设置屏幕支持的最高刷新率
    void SetHighestRefreshRate()
    {
        int highestRefreshRate = 0;
        foreach (var resolution in Screen.resolutions)
        {
            // 确保 refreshRate 是整数
            int rate = (int)resolution.refreshRateRatio.value;
            if (rate > highestRefreshRate)
            {
                highestRefreshRate = rate;
            }
        }

        if (highestRefreshRate > 0)
        {
            Application.targetFrameRate = highestRefreshRate;
        }
        else
        {
            // 如果获取失败，则使用一个安全的高帧率值
            Application.targetFrameRate = 60;
        }
    }
}
