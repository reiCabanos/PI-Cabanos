using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PainelTutorial : MonoBehaviour
{
    // Variáveis para o painel de tutorial
    public TextMeshProUGUI _nomeTexto;
    public TextMeshProUGUI _contTexto;
    public Button _btFechar;
    public Transform _painelTutor;
    public List<Dialogo> dialogos = new List<Dialogo>();

    // Variáveis para o diálogo
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index;

    // Variáveis para animação do sprite
    public GameObject spriteGameObject;

    public void PainelOn(bool value, Dialogo dialogo)
    {
        if (value == true)
        {
            _nomeTexto.text = dialogo._nome;
            _btFechar.Select();
            _painelTutor.DOScale(1, .25f);
            IniciarDialogo(dialogo._texto); // Inicia o diálogo com o texto do Dialogo passado
            AnimarSprite(); // Inicia a animação do sprite, se houver
        }
        else
        {
            _painelTutor.DOScale(0, .25f);
        }
    }

    void Start()
    {
        _painelTutor.localScale = Vector3.zero;
        textComponent.text = string.Empty;
    }

    void Update()
    {
        // Avança o diálogo ao clicar
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                ProximaLinha();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    // Inicia o diálogo
    public void IniciarDialogo(string texto)
    {
        lines = new string[] { texto }; // Define o texto passado como linha de diálogo
        index = 0;
        StartCoroutine(TypeLine());
    }

    // Coroutine para exibir o texto letra por letra
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    // Avança para a próxima linha de diálogo
    void ProximaLinha()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false); // Fecha o painel de diálogo
        }
    }

    // Animação do sprite
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
