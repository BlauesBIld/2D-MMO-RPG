using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Inventory playerInventory = new Inventory();
    public GameObject inventoryUI;
    public List<GameObject> inventorySlots;

    public bool isDraggingItem;

    private Vector3 dragOffset = Vector3.zero;

    public void Awake()
    {
        foreach (GameObject invSlot in inventorySlots)
        {
            invSlot.GetComponent<CanvasGroup>().alpha = 0f;
        }
    }
    
    public void Start()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].GetComponent<InventorySlotController>().slotNr = i;
        }
    }

    public void InitializePlayerInventory(Inventory inventory)
    {
        playerInventory = inventory;
    }

    public void SetSpriteForSlot(Sprite sprite, int slot)
    {
        inventorySlots[slot].GetComponent<Image>().sprite = sprite;
        inventorySlots[slot].GetComponent<CanvasGroup>().alpha = 1f;
    }

    public void ClearSpriteForSlot(int slot)
    {
        inventorySlots[slot].GetComponent<Image>().sprite = null;
        inventorySlots[slot].GetComponent<CanvasGroup>().alpha = 0f;
    }

    public void CloseInventory()
    {
        inventoryUI.SetActive(false);
        isDraggingItem = false;
    }
    
    public void OpenInventory()
    {
        inventoryUI.SetActive(true);
    }

    public bool IsInventoryOpen()
    {
        return inventoryUI.activeInHierarchy;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragOffset = Input.mousePosition - transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    { 
        transform.position = eventData.position - new Vector2(dragOffset.x, dragOffset.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        foreach (GameObject slot in inventorySlots)
        {
            slot.GetComponent<InventorySlotController>().positionInInventory = slot.transform.position;
        }
    }

    public void ReloadInventoryUI()
    {
        for (int i = 0; i < playerInventory.items.Length; i++)
        {
            if (playerInventory.items[i] != null)
            {
                UIManager.instance.GetInventoryController.SetSpriteForSlot(playerInventory.items[i].image, i);
            }
            else
            {
                UIManager.instance.GetInventoryController.ClearSpriteForSlot(i);
            }
        }
    }
}
