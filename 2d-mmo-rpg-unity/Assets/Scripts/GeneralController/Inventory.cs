using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class Inventory
{
    private static readonly int MAXSIZE = 12;

    public Item[] items = new Item[MAXSIZE];

    public bool PutItemInSlot(Item item, int slot)
    {
        if (items[slot] == null)
        {
            items[slot] = item;
            UIManager.instance.GetInventoryController.SetSpriteForSlot(item.image, slot);
            return true;
        }
        if (items[slot].Equals(item) && item.maxStackSize > item.amount)
        {
            Debug.Log("seas");
        }

        return false;
    }

    public bool RemoveItemFromSlot(int fromSlot)
    {
        if (items[fromSlot] != null)
        {
            items[fromSlot] = null;
            UIManager.instance.GetInventoryController.ClearSpriteForSlot(fromSlot);
            return true;
        }

        return false;
    }

    public bool MoveItemFromSlotToSlot(int fromSlot, int toSlot)
    {
        return PutItemInSlot(items[fromSlot], toSlot) &&  RemoveItemFromSlot(fromSlot);
    }

    public Item GetItemOnSlotNr(int slotNr)
    {
        return items[slotNr];
    }

    public void LoadInventory(string base64DecodedInventory)
    {
        items = JsonConvert.DeserializeObject<Item[]>(CryptoMethods.EncodeFromBase64String(base64DecodedInventory));
        
        UIManager.instance.GetInventoryController.ReloadInventoryUI();
    }
}