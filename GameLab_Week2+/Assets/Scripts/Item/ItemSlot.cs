using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public Sprite DefaultSprite;
    public GameObject ItemCanvas;

    private Item slotItem;
    private bool isPointerOver;
    void Start()
    {
        isPointerOver = false;
    }
    void FixedUpdate()
    {
        if (isPointerOver)        
            ActiveItemCanvas();        
    }
    void ActiveItemCanvas()
    {
        if (slotItem != null)
        {
            ItemCanvas.SetActive(true);
            ItemCanvas.transform.GetChild(0).GetComponent<Image>().sprite = slotItem.sprite;
            ItemCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = slotItem.description;
            ItemCanvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Weight : " + slotItem.weight;
        }
        else
            ItemCanvas.SetActive(false);
    }    
    public void OnPointerEnter(PointerEventData eventData)
    {        
        isPointerOver = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        ItemCanvas.SetActive(false);
    }
    public void SetSlot(Item item)
    {
        if (item != null)
        {
            slotItem = item;
            transform.GetComponent<Image>().sprite = item.sprite;
        }
        else
        {
            slotItem = null;            
            transform.GetComponent<Image>().sprite = DefaultSprite;
        }
    }
}
