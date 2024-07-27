using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public void OnClickGameStart()
    {
        SceneManager.LoadSceneAsync("TestScene");
    }
    public void OnClickExit()
    {
        Application.Quit();
    }
}
