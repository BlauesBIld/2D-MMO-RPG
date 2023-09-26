using System;
using UnityEngine;

[Serializable]
public class Consumable : Item
{

    public Consumable(string name, string consumableFileName, int maxStackSize, int amount) : base(name, maxStackSize, amount,
        "Consumables/" + consumableFileName)
    {
    }

    public void CallActivationMethod()
    {
        switch (name)
        {
            case "MP Crystal":
                ClientSend.RequestMPCrystalEffect();
                break;
            case "HP Crystal":
                ClientSend.RequestHPCrystalEffect();
                break;
            default:
                Debug.LogError("Tried to activate Item: \"" + name + "\", but no method found!");
                break;
        }
    }
    
}
