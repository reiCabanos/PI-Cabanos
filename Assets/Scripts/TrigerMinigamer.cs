using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
/*using UnityEditor.ShaderGraph;*/
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class TrigerMinigamer : MonoBehaviour
{
    public PlayerMove playerMove;
    public PlayerControle playerControle;
    public Transform[] scores;
    public Transform coinNextPos;
    public GameObject pont1;
    public GameObject pont2;
    public Transform posRestatPlayer;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI coinCounterTex;
    public Transform moveCamera;
    public Transform fim;
    public Button fimG;
    public Button comecarNovamente;

    private int scoreCounter = 0;
    private int index = 0;
    private Transform t;
    [SerializeField] ControlePersonagem _controle;

    private void Start()
    {
        _controle = Camera.main.GetComponent<ControlePersonagem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("filho"))
        {
            playerMove.StopPlayer(true);
            posRestatPlayer = other.GetComponent<Resetar>()._posRestat;
            pont1.SetActive(false);
            StartCoroutine(playerMove.Dano());

            playerMove._isReseting = true;
            playerMove._quantVida--;
            playerControle.CheckIcomVida(playerMove._quantVida);
            lifeText.text = playerMove._quantVida.ToString();
        }

        if (other.gameObject.CompareTag("p2"))
        {
            playerMove.RotacaoDaCamera();
            pont1.SetActive(true);
            playerMove.value *= -1;
        }

        if (other.gameObject.CompareTag("point3"))
        {
            playerMove.SegundaRotacao();
            playerMove.value *= -1;
        }

        if (other.gameObject.CompareTag("point4"))
        {
            playerMove.UltimaRotacao();
            playerMove.value *= -1;
        }

        if (other.gameObject.CompareTag("point5"))
        {
            playerMove.transform.DORotate(new Vector3(playerMove.transform.localEulerAngles.x, -439.518f, playerMove.transform.localEulerAngles.z), 1f, RotateMode.Fast).SetEase(Ease.InSine);
        }

        if (other.gameObject.CompareTag("point6"))
        {
            playerMove.transform.DORotate(new Vector3(playerMove.transform.localEulerAngles.x, -86.897f, playerMove.transform.localEulerAngles.z), 1f, RotateMode.Fast).SetEase(Ease.InSine);
        }

        if (other.gameObject.CompareTag("point7"))
        {
            playerMove.transform.DORotate(new Vector3(playerMove.transform.localEulerAngles.x, 271.7f, playerMove.transform.localEulerAngles.z), 1f, RotateMode.Fast).SetEase(Ease.InSine);
        }

        if (other.gameObject.CompareTag("i"))
        {
            if (playerMove._isReseting)
            {
                playerControle.Recomeca();
            }
        }

        if (other.gameObject.CompareTag("item"))
        {
            if (playerMove._autoCorrer)
            {
                playerMove._speed += 0.5f;
            }
        }

        if (other.gameObject.CompareTag("fimGamer"))
        {
            _controle._stop = true;
            playerMove._anim.SetFloat("correndo", 0);
            playerMove._anim.SetBool("parado", true);

            fim.DOScale(1, 0.5f);
            comecarNovamente.Select();
            fimG.Select();
        }

        if (other.gameObject.CompareTag("teste"))
        {
            if (!playerMove._checkPass)
            {
                playerMove._checkPass = true;

                scores[index].gameObject.SetActive(true);
                other.transform.parent = coinNextPos.parent;
                t = other.transform;
                scores[index].position = other.transform.position;

                if (index < 4)
                {
                    index++;
                }
                else
                {
                    index = 0;
                }

                //StartCoroutine(playerMove.Desativar());

                scoreCounter++;
                coinCounterTex.text = scoreCounter.ToString();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("i"))
        {
            playerMove._isReseting = false;
        }
    }
}
