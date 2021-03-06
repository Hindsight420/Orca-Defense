using System;
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

    public override string ToString()
    {
        return $"Resource ({Type.name}: {Amount})";
    }
}
