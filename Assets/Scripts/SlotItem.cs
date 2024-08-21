using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    // Start is called before the first frame update
    public bool _ocupado;
    public int _slotNumber;
    [SerializeField] Image _imgItem;
    [SerializeField] ItemInventario _ItemDados;


    public TextMeshProUGUI _textNumber;

    public bool CheckSlot
    {
        get { return _ocupado; }
        set { _ocupado = value; }
    }
    
    public void NumberItem()
    {
        _textNumber.text = "" + _slotNumber;
    }


    
}


