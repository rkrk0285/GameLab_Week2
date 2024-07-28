using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    [SerializeField]
    private GameObject ControlKeyUI;
    public void OnClickGameStart()
    {
        SceneManager.LoadSceneAsync("TestScene");
    }
    public void OnClickControlKeys()
    {
        ControlKeyUI.SetActive(true);
    }
    public void OnClickBackButton()
    {
        ControlKeyUI.SetActive(false);
    }
    public void OnClickExit()
    {
        Application.Quit();
    }
}