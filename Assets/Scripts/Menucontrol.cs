using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    [SerializeField] List<Transform> _itensMenu;

    // Start is called before the first frame update
   

    public void MenuOFF()
    {
        for (int i = 0; i < _itensMenu.Count; i++)
        {
            _itensMenu[i].localScale = Vector3.zero;
        }
        for (int i = 0; i < _itensMenu.Count; i++)
        {
            _itensMenu[i].DOScale(1, 0.5f);
        }


    }


    public void ChamaMenu()
    {
        _itensMenu[0].GetComponent<Button>().Select();
        StartCoroutine(TimeItens());

    }

    private IEnumerator TimeItens()
    {
        for (int i = 0; i < _itensMenu.Count; i++)
        {
            yield return new WaitForSeconds(0.25f);
            _itensMenu[i].DOScale(1.5f, 0.5F);
            yield return new WaitForSeconds(0.25f);
            _itensMenu[i].DOScale(1f, 0.25F);
        }
           
    }
    // Update is called once per frame
    void Update()
    {



    }


}
