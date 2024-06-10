using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogoScript : MonoBehaviour
{
    public TextMeshProUGUI textcomponet;
    public string[] lines;
    public float textspeed;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        textcomponet.text = string.Empty;
        come�arDialogo();


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) /// AQUI MUDAR O BOT�O PARA AVAN�AR O TEXTO 
        {
            if (textcomponet.text == lines[index])
            {
                ProxLinha();
            }
            else
            {
                StopAllCoroutines();
                textcomponet.text = lines[index];
            }
        }
        
    }

    void come�arDialogo()
    {
        index = 0;
        StartCoroutine(Typeline());
    }
     IEnumerator Typeline()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textcomponet.text += c;
            yield return new WaitForSeconds(textspeed);
        }
    }

    void ProxLinha()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textcomponet.text =string.Empty;
            StartCoroutine (Typeline());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
}
