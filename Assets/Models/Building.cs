using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    int x;
    int y;
    string type;

    public Action<Building> cbOnChanged;
    public Action<Building> cbOnRemoved;

    public int X { get => x; private set => x = value; }
    public int Y { get => y; private set => y = value; }
    public string Type { get => type; private set => type = value; }

    public Building(int x, int y, string type)
    {
        X = x;
        Y = y;
        Type = type;

        cbOnChanged?.Invoke(this);
    }

    public void Remove()
    {
        cbOnRemoved(this);
    }
}
