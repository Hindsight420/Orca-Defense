using Assets._Scripts._1Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : DataEntity
{
    public SpriteRenderer WingRight;
    public SpriteRenderer WingLeft;
    public SpriteRenderer LegLeft;
    public SpriteRenderer LegRight;
    public SpriteRenderer FootLeft;
    public SpriteRenderer FootRight;
    public SpriteRenderer EyeLeft;
    public SpriteRenderer EyeRight;
    public SpriteRenderer PupilLeft;
    public SpriteRenderer PupilRight;
    public SpriteRenderer BeakTop;
    public SpriteRenderer BeakBottom;
    public SpriteRenderer Head;
    public SpriteRenderer Body;

    private PenguinData _penguinData;

    private void Awake()
    {
        _penguinData = new PenguinData(transform);
    }

    public override ISelectionData GetSelectionData()
    {
        return _penguinData;
    }
}
