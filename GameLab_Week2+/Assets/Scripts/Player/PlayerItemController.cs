using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PlayerItemController : MonoBehaviour
{
    public GameObject InventoryCanvas;    
    public Sprite DefaultSprite;

    public GameObject[] Inventory;
    private int InventoryIndex;
    private GameObject DetectedItem;

    private const int InventoryMin = 0;
    private const int InventoryMax = 3;

    private Color enableColor = new Color(1, 1, 1, 1);
    private Color disableColor = new Color(1, 1, 1, 0.75f);
    void Start()
    {
        Inventory = new GameObject[4];
        InventoryIndex = 0;
        UpdateInventory();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetDetectedItem();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            DropSelectedItem();            
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
        if (DetectedItem == null)
            return;
        else if (DetectedItem.CompareTag("Core"))
        {

        }
        else
        {
            for (int i = InventoryMin; i <= InventoryMax; i++)
            {
                if (Inventory[i] == null)
                {
                    Inventory[i] = DetectedItem;
                    DetectedItem.SetActive(false);
                    break;
                }
            }
        }
        UpdateInventory();        
    }
    void DropSelectedItem()
    {
        if (Inventory[InventoryIndex] == null)
            return;

        Inventory[InventoryIndex].SetActive(true);
        Inventory[InventoryIndex].transform.position = this.transform.position;
        Inventory[InventoryIndex] = null;
        UpdateInventory();
    }

    void ChangeInventoryIndex(int offset)
    {        
        InventoryIndex = Mathf.Clamp(InventoryIndex + offset, InventoryMin, InventoryMax);
        UpdateInventory();
    }

    void UpdateInventory()
    {
        for (int i = InventoryMin; i <= InventoryMax; i++)
        {
            if (Inventory[i] != null)
            {
                Sprite sprite = Inventory[i].GetComponent<Item>().sprite;
                InventoryCanvas.transform.GetChild(i).GetComponent<Image>().sprite = sprite;
            }
            else
                InventoryCanvas.transform.GetChild(i).GetComponent<Image>().sprite = DefaultSprite;
        }

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
