using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruta : ColetarItens
{
    public BoxCollider _boxCollider;
    public SphereCollider _sphereCollider;

    public override void DestroyItens()
    {
        StartCoroutine(DestruirTime());
    }
    IEnumerator DestruirTime()
    {
        _boxCollider.enabled = false;
        _sphereCollider.enabled = false;
        Testura.enabled = false;
        PartSaida.SetActive(true);
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);

    }
    public void DestroiItem()
    {
        StartCoroutine(DestruirTime());
    }

}
