using System;
using UnityEngine;

[Serializable]
public class Item : IEquatable<Item>
{
    public string name;

    public int maxStackSize;
    public int amount;

    public string imageFilePath;

    public Item(string name, int maxStackSize, int amount, string imageFilePath)
    {
        this.name = name;
        this.maxStackSize = maxStackSize;
        this.amount = amount;
        this.imageFilePath = imageFilePath;
    }

    #region overrides

    public bool Equals(Item other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return name == other.name;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Item) obj);
    }

    public override int GetHashCode()
    {
        return (name != null ? name.GetHashCode() : 0);
    }

    public static bool operator ==(Item left, Item right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Item left, Item right)
    {
        return !Equals(left, right);
    }

    #endregion

    public override string ToString()
    {
        return $"{nameof(name)}: {name}, {nameof(maxStackSize)}: {maxStackSize}, {nameof(amount)}: {amount}, {nameof(imageFilePath)}: {imageFilePath}";
    }
}