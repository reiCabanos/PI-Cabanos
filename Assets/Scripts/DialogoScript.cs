using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DialogoScript : MonoBehaviour
{
    public TextMeshProUGUI textcomponet;
    public string[] lines;
    public float textspeed;
    private int index;

    public GameObject spriteGameObject;

    void Start()
    {
        textcomponet.text = string.Empty;
        comecarDialogo();
        AnimarSprite(); // Inicia a animação do vento imediatamente
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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

    void comecarDialogo()
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
            textcomponet.text = string.Empty;
            StartCoroutine(Typeline());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void AnimarSprite()
    {
        if (spriteGameObject != null)
        {
            Vector3 posicaoOriginal = spriteGameObject.transform.position;

            Sequence sequencia = DOTween.Sequence();
            sequencia.Append(spriteGameObject.transform
                .DOMoveX(posicaoOriginal.x + 0.2f, 1f)
                .SetEase(Ease.InOutSine))
              .Join(spriteGameObject.transform
                .DORotate(new Vector3(0, 0, 5), 1f)
                .SetEase(Ease.InOutSine))
              .Append(spriteGameObject.transform
                .DOMoveY(posicaoOriginal.y + 0.1f, 0.8f)
                .SetEase(Ease.InOutSine))
              .Join(spriteGameObject.transform
                .DORotate(new Vector3(0, 0, -5), 0.8f)
                .SetEase(Ease.InOutSine))
              .Append(spriteGameObject.transform
                .DOMoveX(posicaoOriginal.x - 0.2f, 1f)
                .SetEase(Ease.InOutSine))
              .Join(spriteGameObject.transform
                .DORotate(new Vector3(0, 0, 0), 1f)
                .SetEase(Ease.InOutSine))
              .Append(spriteGameObject.transform
                .DOMoveY(posicaoOriginal.y, 0.8f)
                .SetEase(Ease.InOutSine))
              .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
