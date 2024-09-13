using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LoadingCenas : MonoBehaviour
{
    public bool m_isLoading = false;
    public float m_progress = 0f;

    private RectTransform loadingPanelRect;  // Referência ao RectTransform do painel de loading
    public static LoadingCenas Instance;

    private void Awake()
    {
        Instance = this;

        // Recupera o RectTransform do painel de loading e esconde-o inicialmente
        loadingPanelRect = GetComponentInChildren<RectTransform>();
        loadingPanelRect.gameObject.SetActive(false);

        // Impede que o painel de loading seja destruído ao mudar de cena
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Vai para o level cujo index corresponde ao passado como parâmetro
    /// </summary>
    public void NextLevel(int index)
    {
        if (!m_isLoading)
        {
            m_isLoading = true;
            m_progress = 0f;
            Time.timeScale = 0f;

            // Exibe o painel de loading e inicia a animação de entrada usando DOTween
            loadingPanelRect.gameObject.SetActive(true);
            loadingPanelRect.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.InOutQuad);

            // Inicia a transição de cena
            StartCoroutine(LoadLevel(index));
        }
    }

    /// <summary>
    /// Recarrega a fase atual
    /// </summary>
    public void ReloadLevel()
    {
        NextLevel(CurrentLevel());
    }

    /// <summary>
    /// Carrega nova cena de forma assíncrona
    /// </summary>
    IEnumerator LoadLevel(int index)
    {
        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        operation.allowSceneActivation = false;

        // Enquanto não terminar de carregar a nova cena
        while (!operation.isDone)
        {
            m_progress = operation.progress;

            if (operation.progress >= 0.9f)
            {
                // Espera tempo da primeira transição
                yield return new WaitForSecondsRealtime(0.8f);

                operation.allowSceneActivation = true;
            }

            yield return new WaitForEndOfFrame();
        }

        // Terminou de carregar, retoma o tempo de jogo
        Time.timeScale = 1f;
        m_isLoading = false;
        m_progress = 0f;

        // Anima o painel de loading saindo da tela e depois o esconde
        loadingPanelRect.DOAnchorPos(new Vector2(0, -loadingPanelRect.rect.height), 0.5f).SetEase(Ease.InOutQuad)
            .OnComplete(() => loadingPanelRect.gameObject.SetActive(false));
    }

    /// <summary>
    /// Retorna o index do level atual
    /// </summary>
    public int CurrentLevel()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
