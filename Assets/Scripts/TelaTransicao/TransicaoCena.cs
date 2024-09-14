using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransicaoCena : MonoBehaviour
{
    public GameObject m_PauseScreen;
    public bool m_isPaused = false;
    public GameObject loadingPrefab;

    private void Awake()
    {
        //verifica se o loading ja foi instanciado
        if (GameObject.Find("=LOADING=") == null)
        {
            GameObject loading = Instantiate(loadingPrefab);
            loading.name = "=LOADING=";
        }
    }

    void Update()
    {
        //PauseGame();
    }

    /// <summary>
    /// Pausa o jogo se botão "cancel" for pressionado
    /// </summary>
    private void PauseGame()
    {
        //Se botão com nome "Cancel" for pressionado
        if (Input.GetButtonDown("Cancel"))
        {
            //verifica se o jogo está pausado
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
        PauseGame();
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
