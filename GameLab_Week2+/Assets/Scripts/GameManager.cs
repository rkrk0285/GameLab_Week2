using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int ExitWeight = 100;

    public void OpenExit()
    {
        Debug.Log("출구 조사");
    }

    public void CheckExitPlatformWeight(int currentWeight)
    {
        if (currentWeight >= ExitWeight)
        {
            OpenExit();
        }
    }
}
