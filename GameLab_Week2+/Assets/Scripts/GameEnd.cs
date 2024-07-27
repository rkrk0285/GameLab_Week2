using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameEnd : MonoBehaviour
{
    public Light2D Light;
    public float EndingTimer;
    public GameObject EndingObject;
    private void Start()
    {
        StartCoroutine(CutScene());
    }

    IEnumerator CutScene()
    {        
        float timer = 0f;
        while(true)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
            if (timer > EndingTimer)
                break;

            float offset = timer / EndingTimer;
            Light.pointLightInnerRadius = 1 * offset;
            Light.pointLightOuterRadius = 18 * offset;                                    
        }
        EndingObject.SetActive(true);
        StopAllCoroutines();
    }

    public void OnClickEscapeButton()
    {
        Application.Quit();
    }
}
