using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ResourceValue
{
    public static implicit operator int(ResourceValue instance) { return instance.Amount; }

    //public static Resource operator +(Resource r) => r;
    //public static Resource operator +(Resource r, int i) => r + i;
    //public static Resource operator -(Resource r, int i) => r - i;

    public ResourceType Type;

    [SerializeField] int amount;
    public int Amount
    {
        get => amount;

        set
        {
            if (value < 0) // resources shouldn't go under 0
            {
                amount = 0;
                Debug.LogError($"{this} tried to set to {value}, setting amount to 0 instead");
                return;
            }

            amount = value;
        }
    }

    public ResourceValue(ResourceType _type, int _amount = 0)
    {
        Type = _type;
        Amount = _amount;
    }

    public void TransferTo(ResourceValue target, int amount)
    {
        Math.Clamp(amount, 0, Amount);
        Amount -= amount;
        target.amount += amount;
    }

    public void TransferTo(ResourceValue target)
    {
        TransferTo(target, Amount);
    }

    public void TransferTo(List<ResourceValue> targetList, int amount)
    {
        TransferTo(targetList.First(r => r.Type == Type), amount);
    }

    public override bool Equals(object obj) => obj is ResourceValue value ? Equals(value) : base.Equals(obj);

    public bool Equals(ResourceValue target) => Type == target.Type && Amount == target.Amount;

    public override int GetHashCode() => HashCode.Combine(Type, Amount);

    public override string ToString()
    {
        return $"Resource ({Type.name}: {Amount})";
    }
}
