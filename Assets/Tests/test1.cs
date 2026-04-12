using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class test1
{
    private GameResourceManager resourceManager;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        //等待加载主场景
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        yield return null;
        // 等待资源管理器实例化
        while (GameResourceManager.Instance == null)
            yield return null;
        resourceManager = GameResourceManager.Instance;
        yield return null;

    }

    [Test]
    public void InitliziNum()
    {
        double liziNum = resourceManager.getlizinumber();
        Assert.AreEqual(10, liziNum, 0.001, "粒子初始化错误");
    }
    [Test]
    public void InitleidianNum()
    {
        double leidianNum = resourceManager.getleidianCount();
        Assert.AreEqual(0, leidianNum, 0.001, "雷电初始化错误");
    }
    [Test]
    public void InitchenaiNum()
    {
        double chenaiNum = resourceManager.getchenainumber();
        Assert.AreEqual(0, chenaiNum, 0.001, "尘埃初始化错误");
    }
    [Test]
    public void InitzhihuiNum()
    {
        double zhihuiNum = resourceManager.getzhihuinumber();
        Assert.AreEqual(0, zhihuiNum, 0.001, "智慧初始化错误");
    }
    [Test]
    public void InitanwuzhiNum()
    {
        double anwuzhiNum = resourceManager.getanwuzhinumber();
        Assert.AreEqual(0, anwuzhiNum, 0.001, "暗物质初始化错误");
    }





    //[UnityTest]
    //public IEnumerator test1WithEnumeratorPasses()
    //{
    //    // Use the Assert class to test conditions.
    //    // Use yield to skip a frame.
    //    yield return null;
    //}
}
