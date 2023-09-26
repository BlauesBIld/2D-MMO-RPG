using System;
using UnityEngine;

[Serializable]
public class Consumable : Item
{

    public Consumable(string name, string consumableFileName, int maxStackSize, int amount) : base(name, maxStackSize, amount,
        "Consumables/" + consumableFileName)
    {
    }
}
