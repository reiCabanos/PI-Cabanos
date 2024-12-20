using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrasicaoDeCena_teste : MonoBehaviour
{
    public GameObject m_PauseScreen;
    public bool m_isPaused = false;

    private void Awake()
    {
        //verifica se o loading ja foi instanciado
        if (GameObject.Find("=LOADING=") == null)
        {
            GameObject loading = Instantiate(Resources.Load<GameObject>("TelaTransicao/Loading"));
            loading.name = "=LOADING=";
        }
    }

    void Update()
    {
        PauseGame();
    }

    /// <summary>
    /// Pausa o jogo se bot�o "cancel" for pressionado
    /// </summary>
    private void PauseGame()
    {
        //Se bot�o com nome "Cancel" for pressionado
        if (Input.GetButtonDown("Cancel"))
        {
            //verifica se o jogo est� pausado
            if (!m_isPaused)
            {
                //pausa jogo
                m_isPaused = true;

                m_PauseScreen.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                //despausa jogo
                m_isPaused = false;

                m_PauseScreen.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }

    /// <summary>
    /// Reinicia level atual
    /// </summary>
    public void RestartLevel()
    {
        m_PauseScreen.SetActive(false);
        LoadingController.Instance.ReloadLevel();
    }

    /// <summary>
    /// Vai para o proximo level
    /// </summary>
    public void NextLevel(int indexLevel)
    {
        m_PauseScreen.SetActive(false);
        LoadingController.Instance.NextLevel(indexLevel);
    }
}
