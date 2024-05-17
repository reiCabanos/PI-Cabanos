using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hudcontrole : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<MenuControl> _MenuControls;
    MenuControl _menu;
        void Start()
    {
        _menu = Camera.main.GetComponent<MenuControl>();

        for (int i = 0; i < _MenuControls.Count; i++)
        {
            _MenuControls[i].transform.localScale = Vector3.zero;
            _MenuControls[i].gameObject.SetActive(false);

        }
        _MenuControls[0].gameObject.SetActive(true);
        _MenuControls[0].MenuOFF();
        _MenuControls[0].transform.DOScale(1,00f);
        _MenuControls[0].ChamaMenu();

    }



    public void ChamarMenuControl(int value)
    {
        for (int i = 0; i < _MenuControls.Count; i++)
        {
            _MenuControls[i].transform.localScale = Vector3.zero;
            _MenuControls[i].MenuOFF();
            _MenuControls[i].gameObject.SetActive(false);
        }
        _MenuControls[value].gameObject.SetActive(true);
        _MenuControls[value].transform.DOScale(1, 00F);
        _MenuControls[value].ChamaMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
