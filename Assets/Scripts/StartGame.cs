using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class StartGame : MonoBehaviour
{
    public TextMeshProUGUI startGame;

    public void LoadSence(string SampleScene)
    {
        SceneManager.LoadScene(SampleScene);
    }




}
