using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoEyeDogMouse : MonoBehaviour
{   
    private const int Damage = 30;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // ������ ȣ��
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(Damage, transform.parent.gameObject);
        }
    }
}
