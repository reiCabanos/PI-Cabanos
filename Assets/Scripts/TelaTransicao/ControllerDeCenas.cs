using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerDeCenas : MonoBehaviour
{
    public GameObject m_PauseScreen;
    public bool m_isPaused = false;
    public GameObject loadingPrefab;

    // Referência estática para o painel de loading
    private static GameObject loadingInstance;

    private void Awake()
    {
        // Verifica se o painel de loading já foi instanciado
        if (loadingInstance == null)
        {
            // Instancia o prefab de loading e armazena a referência estática
            loadingInstance = Instantiate(loadingPrefab);
            DontDestroyOnLoad(loadingInstance); // Impede que o loading seja destruído ao mudar de cena
        }
    }

    void Update()
    {
        PauseGame();
    }

    /// <summary>
    /// Pausa o jogo se botão "Cancel" for pressionado
    /// </summary>
    private void PauseGame()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (!m_isPaused)
            {
                m_isPaused = true;
                m_PauseScreen.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
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
    /// Vai para o próximo level
    /// </summary>
   /* public void NextLevel(int indexLevel)
    {
        m_PauseScreen.SetActive(false);
        LoadingController.Instance.NextLevel(indexLevel);
    }*/
}
