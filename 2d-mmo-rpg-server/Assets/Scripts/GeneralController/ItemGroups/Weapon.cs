using System;

[Serializable]
public class Weapon : Item
{
    public float physicalDamage;
    public float magicDamage;

    public Weapon(string name, float physicalDamage, float magicDamage, string weaponFileName) : base(name, 1, 1, "Weapons/"+weaponFileName)
    {
        this.physicalDamage = physicalDamage;
        this.magicDamage = magicDamage;
    }

    public override string ToString()
    {
        return base.ToString() + $", {nameof(physicalDamage)}: {physicalDamage}, {nameof(magicDamage)}: {magicDamage}";
    }
}
