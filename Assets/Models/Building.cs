using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    int x;
    int y;
    string type;

    public int X { get => x; private set => x = value; }
    public int Y { get => y; private set => y = value; }
    public string Type { get => type; private set => type = value; }

    public Building(int x, int y, string type)
    {
        X = x;
        Y = y;
        Type = type;
    }
}
