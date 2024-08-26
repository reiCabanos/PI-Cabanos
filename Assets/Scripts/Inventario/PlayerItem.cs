using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    InventarioControl _control;
    public ItemDados _itemDados;
    public SpriteRenderer _spriteRenderer;


    void Start()
    {
        _control = Camera.main.GetComponent<InventarioControl>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("item"))
        {
            ItemControl itemControl = collision.gameObject.GetComponent<ItemControl>();

            if (itemControl._itemDados.Tip == 0)// arma ---------------------------------------------------------
            {
                for (int i = 0; i < _control._SlotArmas.Count; i++)
                {

                    if (_control._SlotArmas[i].CheckSlot == false)
                    {
                        _control._SlotArmas[i].CheckSlot = true;
                        _control._SlotArmas[i].ImageSlot(itemControl._itemDados.ImageItem);
                        _control._SlotArmas[i].DadosSlot(itemControl._itemDados);

                        break;
                    }

                }
            }
            else if (itemControl._itemDados.Tip == 1)//coletavel ----------------------------------------------------
            {
                _control._SlotColetaveis[0].PegarColetavel();
            }
            else if (itemControl._itemDados.Tip == 2)//coletavel ----------------------------------------------------
            {
                _control._SlotColetaveis[1].PegarColetavel();
            }
            Debug.Log("Tocou no item");
        }
       
    }
}
