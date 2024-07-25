using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [SerializeField]
    private GameObject Flashlight;

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        float dx = mousePos.x - transform.position.x;
        float dy = mousePos.y - transform.position.y;
        float angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg - 90;        
        Flashlight.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}