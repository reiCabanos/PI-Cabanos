using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemControl : MonoBehaviour
{
    public ItemDados _itemDados;
    SpriteRenderer _image;

    public bool _checkColetavel;


    void Start()
    {
        if (_checkColetavel == false)
        {
            _image = GetComponent<SpriteRenderer>();
            _image.sprite = _itemDados.ImageItem;
        }
    }

}
