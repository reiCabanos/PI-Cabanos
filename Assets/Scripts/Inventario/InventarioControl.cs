using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventarioControl : MonoBehaviour
{
    public List<SlotControl> _SlotArmas = new List<SlotControl>();
    public List<SlotControl> _SlotColetaveis = new List<SlotControl>();
    public PlayerItem _playerItem;

    public void EquiparPlayerArma(SlotControl slotControl)
    {

        _playerItem._itemDados = slotControl._itemDados;
        _playerItem._spriteRenderer.sprite = slotControl._itemDados.ImageItem;

    }

    public void RemoverPlayerArma(SlotControl slotControl)
    {
        slotControl.ImageSlot(null);
        slotControl.CheckSlot = false;
        _playerItem._spriteRenderer.sprite = null;

        _playerItem._itemDados = null;
        slotControl._itemDados = null;



    }

}
