using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tabuas : ColetarItens
{
    // Start is called before the first frame update
    
    public override void DestroyItens()
    {
        
        StartCoroutine(DestruirTime());
    }
    IEnumerator DestruirTime()
    {
        Testura.enabled = false;transform.DOMove(PosTaboa.position, 2f);
        PartSaida.SetActive(true);
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);

    }
}
