using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int ExitWeight = 100;

    public void OpenExit()
    {
        Debug.Log("�ⱸ ����");
    }

    public void CheckExitPlatformWeight(int currentWeight)
    {
        if (currentWeight >= ExitWeight)
        {
            OpenExit();
        }
    }
}
