using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlayercontrole : MonoBehaviour
{
    // Start is called before the first frame update

    GridItem _gridItem;
    public ItemInventario _itemPlayercontrole;// no scrip é itens Dados
    public SpriteRenderer _spriteRenderer;



    void Start()
    {
        _gridItem = Camera.main.GetComponent<GridItem>();

    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.CompareTag("item"))
        {
            ItensControl _itemObj = collision.GetComponent<ItensControl>();
            int tipoItem = _itemObj._itemInventario._tipo;
          /*  _gridItem._itemArmas[0].CheckSlot = true;*/




                    _gridItem._itemArmas[tipoItem].GetComponent<SlotItem>()._slotNumber++;
                    _gridItem._itemArmas[tipoItem].GetComponent<SlotItem>().NumberItem();
                     



            Debug.Log(_itemObj._itemInventario._nome);


        }
    }
}
