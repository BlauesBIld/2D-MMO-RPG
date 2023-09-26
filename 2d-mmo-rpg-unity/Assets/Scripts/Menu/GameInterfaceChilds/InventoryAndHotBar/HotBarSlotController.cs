using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HotBarSlotController : MonoBehaviour, IDropHandler
{
    public int id;
    public int invItemSlotNr;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject item = eventData.pointerDrag;

        if (CheckIfInventorySlotControllerExistsAndIsConsumable(item))
        {
            BindInvSlotToThisHotBarSlot(eventData, item);
        }
    }

    private void BindInvSlotToThisHotBarSlot(PointerEventData eventData, GameObject item)
    {
        invItemSlotNr = item.GetComponent<InventorySlotController>().slotNr;
        GetComponent<Image>().sprite = eventData.pointerDrag.GetComponent<Image>().sprite;
        GetComponent<CanvasGroup>().alpha = 1f;
        Debug.Log($"Binded Item on slot: {invItemSlotNr} to hotbar-slot: {id}");
    }

    private static bool CheckIfInventorySlotControllerExistsAndIsConsumable(GameObject item)
    {
        return item.GetComponent<InventorySlotController>() != null &&
               UIManager.instance.GetInventoryController.playerInventory.GetItemOnSlotNr(item
                   .GetComponent<InventorySlotController>().slotNr) is Consumable;
    }

    void Update()
    {
        if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), "Alpha"+id)))
        {
            if (UIManager.instance.GetInventoryController.playerInventory.items[invItemSlotNr] != null && UIManager.instance.GetInventoryController.playerInventory.items[invItemSlotNr] is Consumable)
            {
                ((Consumable) UIManager.instance.GetInventoryController.playerInventory.items[invItemSlotNr])
                    .CallActivationMethod();
            }
            else
            {
                //TODO: In se fjütscher, play sound
            }
        }
    }
}