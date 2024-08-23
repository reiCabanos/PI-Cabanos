using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemDados : ScriptableObject
{
    [SerializeField] string _name;
    [SerializeField] int _tip;
    [SerializeField] Sprite _imageItem;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public int Tip
    {
        get { return _tip; }
        set { _tip = value; }
    }
    public Sprite ImageItem
    {
        get { return _imageItem; }
        set { _imageItem = value; }
    }
}
