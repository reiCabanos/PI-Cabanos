using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruta : ColetarItens
{
    public override void DestroyItens()
    {
        StartCoroutine(DestruirTime());
    }
    IEnumerator DestruirTime()
    {
        Testura.enabled = false;
        PartSaida.SetActive(true);
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);

    }

}
