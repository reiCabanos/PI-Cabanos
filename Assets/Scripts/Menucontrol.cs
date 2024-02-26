using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    [SerializeField] List<Transform> _itensMenu;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimeItens());
        /*for (int i = 0; i < _itensMenu.Count; i++)
        {
            _itensMenu[i].localScale = Vector3.zero;
        }
        for (int i = 0; i < _itensMenu.Count; i++)
        {
            _itensMenu[i].DOScale(1, 5f);
        }*/
    }

    public void MenuOFF()
    {
        for (int i = 0; i < _itensMenu.Count; i++)
        {
            _itensMenu[i].localScale = Vector3.zero;
        }

           
    }

    private IEnumerator TimeItens()
    {
        for (int i = 0; i < _itensMenu.Count; i++)
        {

            _itensMenu[i].localScale = Vector3.zero;
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < _itensMenu.Count; i++)
        {
            _itensMenu[i].DOScale(1.5f, 0.25f);
            yield return new WaitForSeconds(.25f);
            _itensMenu[i].DOScale(1f, .25f);
        }
    }



    // Update is called once per frame
    void Update()
    {

    }




}
