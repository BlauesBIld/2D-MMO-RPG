using System.Xml;
using UnityEngine;

public class Inventory
{
    private static readonly int MAXSIZE = 12;

    public Item[] items = new Item[MAXSIZE];

    public bool PutItemInSlot(Item item, int slot)
    {
        if (items[slot] == null)
        {
            items[slot] = item;
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
            return true;
        }

        return false;
    }

    public bool MoveItemFromSlotToSlot(int fromSlot, int toSlot)
    {
        bool successfulMoved = PutItemInSlot(items[fromSlot], toSlot) && RemoveItemFromSlot(fromSlot);
        return successfulMoved;
    }

    public Item GetItemOnSlotNr(int slotNr)
    {
        return items[slotNr];
    }
}