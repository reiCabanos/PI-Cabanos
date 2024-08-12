using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlayercontrole : MonoBehaviour
{
    // Start is called before the first frame update

    GridItem _gridItem;



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
            for (int i = 0; 1 < _gridItem._itemArmas.Count; i++)
            {
                if (_gridItem._itemArmas[i].GetComponent<SlotItem>()._ocupado == false)
                {
                    _gridItem._itemArmas[i].sprite = _itemObj._itemInventario._img;
                    _gridItem._itemArmas[i].GetComponent<SlotItem>()._ocupado = true;
                   // collision.gameObject.SetActive(false);
                    break;
                }


            }


            Debug.Log(_itemObj._itemInventario._nome);


        }
    }
}
