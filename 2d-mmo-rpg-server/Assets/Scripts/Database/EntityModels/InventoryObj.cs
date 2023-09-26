using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryObj
{
    private string username;
    private int charslot;
    private string base64CodedInventory;

    public InventoryObj(string _username, int _charslot, string _base64CodedInventory)
    {
        username = _username;
        charslot = _charslot;
        base64CodedInventory = _base64CodedInventory;
    }

    public InventoryObj(string _username, int _charslot)
    {
        username = _username;
        charslot = _charslot;
    }

    public string Username => username;

    public int Charslot => charslot;

    public string Base64CodedInventory => base64CodedInventory;
}
