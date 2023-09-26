using System;

[Serializable]
public class Armor : Item
{
    public float physicalArmor;
    public float magicResistance;
    public Armor(string name, float physicalArmor, float magicResistance, string armorFileName) : base(name, 1, 1, "Armor/" + armorFileName)
    {
        this.magicResistance = magicResistance;
        this.physicalArmor = physicalArmor;
    }
}
