using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotControl : MonoBehaviour
{
    bool _checkSlot;
    [SerializeField] bool _checkColetavel;
    [SerializeField] Image _imgItem;
    public ItemDados _itemDados;
    Button _btSlot;
    [SerializeField] List<Button> _bts;

    public int _contItem;
    public TextMeshProUGUI _textCont;


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
        get { return _checkSlot; }
        set { _checkSlot = value; }
    }

    public void ImageSlot(Sprite image)
    {
        _imgItem.sprite = image;
    }
    public void DadosSlot(ItemDados _dados)
    {
        _itemDados = _dados;
    }

    public void PegarColetavel()
    {
        /*_contItem++;*/
        _textCont.text = "" + _contItem;

    }


    public void BtsON(bool on)
    {
        if (_checkSlot == true)
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
