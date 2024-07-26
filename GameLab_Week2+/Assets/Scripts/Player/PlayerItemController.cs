using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemController : MonoBehaviour
{
    public GameObject InventoryCanvas;

    public GameObject DetectedItem;
    private GameObject[] Inventory;
    private int InventoryIndex;

    private const int InventoryMin = 0;
    private const int InventoryMax = 3;

    private Color enableColor = new Color(0, 0, 0, 1);
    private Color disableColor = new Color(0, 0, 0, 0.75f);
    void Start()
    {
        Inventory = new GameObject[4];
        InventoryIndex = 0;
        UpdateInventoryCursor();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (DetectedItem != null)
            {
                GetDetectedItem();
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {            
            if(Inventory[InventoryIndex] != null)
            {
                // 아이템 드랍하는 함수.
            }
        }

        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWheel > 0)
            ChangeInventoryIndex(1);
        else if (mouseWheel < 0)
            ChangeInventoryIndex(-1);
    }

    public void SetDetectedItem(GameObject obj)
    {
        DetectedItem = obj;
    }

    void GetDetectedItem()
    {                
        for(int i = InventoryMin; i <= InventoryMax; i++)
        {
            if (Inventory[i] == null)
            {
                Inventory[i] = DetectedItem;
                DetectedItem.SetActive(false);
                return;
            }
        }
        Debug.Log("아이템이 가득 찼습니다.");
    }

    void ChangeInventoryIndex(int offset)
    {        
        InventoryIndex = Mathf.Clamp(InventoryIndex + offset, InventoryMin, InventoryMax);
        UpdateInventoryCursor();
    }

    void UpdateInventoryCursor()
    {
        for (int i = InventoryMin; i <= InventoryMax; i++)
        {
            if (InventoryIndex == i)
                InventoryCanvas.transform.GetChild(i).GetComponent<Image>().color = enableColor;
            else
                InventoryCanvas.transform.GetChild(i).GetComponent<Image>().color = disableColor;
        }
    }
}

// 0.2 0.3 0.5
