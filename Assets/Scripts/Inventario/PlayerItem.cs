using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    InventarioControl _control;
    public ItemDados _itemDados;
    public SpriteRenderer _spriteRenderer;
   public int _totalItensColetados = 0;
    public MoveNew _moveNew;


    void Start()
    {
        _control = Camera.main.GetComponent<InventarioControl>();
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("item"))
        {
            ItemControl itemControl = collision.gameObject.GetComponent<ItemControl>();

            if (itemControl._itemDados.Tip == 1 || itemControl._itemDados.Tip == 2) // itens coletáveis
            {
                _control._SlotColetaveis[0].PegarColetavel();
                _totalItensColetados++; // Incrementa o contador de itens
                Debug.Log("Total de Itens Coletados: " + _totalItensColetados);

                if (_totalItensColetados >= 10)
                {
                    // Ativar o ponto de troca de cena
                    _moveNew.AtivarPontoTroca();
                    _moveNew.AtivarTutorManga();
                }
            }

            Debug.Log("Tocou no item");
        }

    }
}
