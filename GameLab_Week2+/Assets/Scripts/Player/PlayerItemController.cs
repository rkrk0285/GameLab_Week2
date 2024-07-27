using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PlayerItemController : MonoBehaviour
{        
    public GameObject InventoryCanvas;                
    public GameObject ItemCanvas;
    public Sprite DefaultSprite;

    private GameObject[] Inventory;
    private int InventoryIndex;
    private GameObject DetectedItem;

    private const int InventoryMin = 0;
    private const int InventoryMax = 3;

    private Color enableColor = new Color(1, 1, 1, 1);
    private Color disableColor = new Color(1, 1, 1, 0.3f);

    void Start()
    {
        Inventory = new GameObject[4];
        InventoryIndex = 0;
        UpdateInventory();
    }    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))        
            GetDetectedItem();        
        if (Input.GetKeyDown(KeyCode.G))        
            DropSelectedItem();                    

        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWheel > 0)
            ChangeInventoryIndex(1);
        else if (mouseWheel < 0)
            ChangeInventoryIndex(-1);
    }
    public void SetDetectedItem(GameObject obj)
    {
        DetectedItem = obj;
        if (obj != null)
        {            
            ItemCanvas.transform.GetChild(0).GetComponent<Image>().sprite = obj.GetComponent<Item>().sprite;
            ItemCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = obj.GetComponent<Item>().description;
            ItemCanvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Weight : " + obj.GetComponent<Item>().weight;
            ItemCanvas.SetActive(true);
        }
        else        
            ItemCanvas.SetActive(false);

    }
    void GetDetectedItem()
    {
        if (DetectedItem == null)
            return;        
        else
        {
            if (DetectedItem.CompareTag("Core"))            
                transform.Find("CoreLight").gameObject.SetActive(true);
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
        GameManager.instance.ChangePlayerWeight(CalculateInventoryItem());
    }
    void DropSelectedItem()
    {
        if (Inventory[InventoryIndex] == null)
            return;
        if (Inventory[InventoryIndex].CompareTag("Core"))
            transform.Find("CoreLight").gameObject.SetActive(false);

        Inventory[InventoryIndex].SetActive(true);
        Inventory[InventoryIndex].transform.position = this.transform.position;
        Inventory[InventoryIndex] = null;

        for (int i = InventoryMin; i <= InventoryMax; i++)
        {
            if (Inventory[i] != null)
            {
                InventoryIndex = i;
                break;
            }
        }

        UpdateInventory();        
        GameManager.instance.ChangePlayerWeight(CalculateInventoryItem());
    }
    public void ChangeInventoryIndex(int offset)
    {        
        InventoryIndex = Mathf.Clamp(InventoryIndex + offset, InventoryMin, InventoryMax);        
        UpdateInventory();
    }
    void UpdateInventory()
    {
        // 아이템 스프라이트
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

        // 아이템 커서 하이라이트
        for (int i = InventoryMin; i <= InventoryMax; i++)
        {
            if (InventoryIndex == i)
                InventoryCanvas.transform.GetChild(i).GetComponent<Image>().color = enableColor;
            else
                InventoryCanvas.transform.GetChild(i).GetComponent<Image>().color = disableColor;
        }
    }
    public int CalculateInventoryItem()
    {
        int result = 0;
        for (int i = InventoryMin; i <= InventoryMax; i++)
        {
            if (Inventory[i] != null)            
                result += Inventory[i].GetComponent<Item>().weight;                        
        }        
        return result;
    }
}

// 0.2 0.3 0.5
