using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hudcontrole : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<MenuControl> _MenuControls;

        void Start()
    {
        for (int i = 0; i < _MenuControls.Count; i++)
        {
            _MenuControls[i].transform.localScale = Vector3.zero;
            _MenuControls[i].gameObject.SetActive(false);

        }

        _MenuControls[0].gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
