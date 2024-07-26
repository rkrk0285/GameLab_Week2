using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDetect : MonoBehaviour
{
    [SerializeField]
    private PlayerItemController playerItemController;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            playerItemController.SetDetectedItem(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            playerItemController.SetDetectedItem(null);
        }
    }
}
