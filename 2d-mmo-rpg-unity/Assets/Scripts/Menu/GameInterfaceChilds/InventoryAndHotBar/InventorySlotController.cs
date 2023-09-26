using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotController : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler,
    IDragHandler, IDropHandler
{
    public int slotNr;
    public Vector3 positionInInventory;

    public void Start()
    {
        positionInInventory = transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        UIManager.instance.GetInventoryController.isDraggingItem = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = positionInInventory;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        UIManager.instance.GetInventoryController.isDraggingItem = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<InventorySlotController>() != null && eventData.pointerDrag.GetComponent<Image>().sprite != null)
        {
            UIManager.instance.GetInventoryController.SetSpriteForSlot(eventData.pointerDrag.GetComponent<Image>().sprite, slotNr);
            UIManager.instance.GetInventoryController.ClearSpriteForSlot(eventData.pointerDrag.GetComponent<InventorySlotController>().slotNr);
            ClientSend.MoveItemFromSlotToSlot(eventData.pointerDrag.GetComponent<InventorySlotController>().slotNr, GetComponent<InventorySlotController>().slotNr);
        }
    }
}