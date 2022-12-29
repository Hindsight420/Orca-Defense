using Assets._Scripts._1Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : DataEntity
{
    [SerializeField]
    public SpriteRenderer WingRight;
    [SerializeField]
    public SpriteRenderer WingLeft;
    [SerializeField]
    public SpriteRenderer LegLeft;
    [SerializeField]
    public SpriteRenderer LegRight;
    [SerializeField]
    public SpriteRenderer FootLeft;
    [SerializeField]
    public SpriteRenderer FootRight;
    [SerializeField]
    public SpriteRenderer EyeLeft;
    [SerializeField]
    public SpriteRenderer EyeRight;
    [SerializeField]
    public SpriteRenderer PupilLeft;
    [SerializeField]
    public SpriteRenderer PupilRight;
    [SerializeField]
    public SpriteRenderer BeakTop;
    [SerializeField]
    public SpriteRenderer BeakBottom;
    [SerializeField]
    public SpriteRenderer Head;
    [SerializeField]
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
