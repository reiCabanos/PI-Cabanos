using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    /// <summary>
    ///  ESSE AQUI É O SLOTCONTROL 
    /// </summary>
    // Start is called before the first frame update
    public bool _ocupado;
    [SerializeField] bool _checkColetavel;

    public int _slotNumber; // CONtItens ITENS ORIGINAL 
    [SerializeField] Image _imgItem;
    public ItemInventario _ItemInventario;
    Button _btSlot;
    [SerializeField] List<Button> _bts;
    


    public TextMeshProUGUI _textNumber;

    private void Start()
    {

        _btSlot = GetComponent<Button>();

        for (int i = 0; i < _bts.Count; i++)
        {
            _bts[i].transform.localScale = Vector3.zero;
            _bts[i].enabled = false;
        }
    }

    public bool CheckSlot
    {
        get { return _ocupado; }
        set { _ocupado = value; }
    }

    public void ImageSlot(Sprite image)
    {
        _imgItem.sprite = image;
    }
    public void DadosSlot(ItemInventario _dados)
    {
        _ItemDados = _dados;
    }


    public void NumberItem() /// ORGINAL É PegarColetavel 
    {
        _slotNumber++;
        _textNumber.text = "" + _slotNumber;
    }

    public void BtsON(bool on)
    {
        if (_ocupado == true) //  _checkSlot original ; 
        {

            for (int i = 0; i < _bts.Count; i++)
            {
                if (on == true)
                {
                    _btSlot.enabled = false;
                    _bts[i].transform.DOScale(1, .25f);
                    _bts[i].enabled = true;
                }
                else
                {
                    _btSlot.enabled = true;
                    _bts[i].transform.DOScale(0, .25f);
                    _bts[i].enabled = false;
                }
            }
        }


    }


}


