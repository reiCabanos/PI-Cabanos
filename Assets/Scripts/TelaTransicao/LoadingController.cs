using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    public bool m_isLoading = false;
    public float m_progress = 0f;

    private Animator m_animator;

    public static LoadingController Instance;

    private void Awake()
    {
        Instance = this;

        //Recupera animator e esconde seu gameobject
        m_animator = GetComponentInChildren<Animator>();
        m_animator.gameObject.SetActive(false);

        //impede que o gameobject do loading seja destruido
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Vai para level cujo index é corresponde ao passado como parametro
    /// </summary>
    /// <param name="index"></param>
    public void NextLevel(int index)
    {
        if (!m_isLoading)
        {
            m_isLoading = true;
            m_progress = 0f;
            Time.timeScale = 0f;

            //play animação
            m_animator.gameObject.SetActive(true);
            m_animator.Play("inicia");

            //carrega nova cena
            StartCoroutine(LoadLevel(index));
        }
    }

    /// <summary>
    /// Recarrega fase atual
    /// </summary>
    public void ReloadLevel()
    {
        NextLevel(CurrentLevel());
    }

    /// <summary>
    /// Carrega nova cena
    /// </summary>
    /// <param name="index"></param>
    IEnumerator LoadLevel(int index)
    {
        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        operation.allowSceneActivation = false;

        //enquanto não terminar de carregar a nova cena
        while (!operation.isDone)
        {
            m_progress = operation.progress;

            if (operation.progress.Equals(.9f))
            {
                //espera tempo da primeira transição
                yield return new WaitForSecondsRealtime(0.8f);

                operation.allowSceneActivation = true;
            }

            yield return new WaitForEndOfFrame();
        }

        //terminou de carregar
        Time.timeScale = 1f;
        m_isLoading = false;
        m_progress = 0f;

        //finaliza animação
        m_animator.Play("termina");

        //esconde transição
        yield return new WaitForSeconds(0.8f);
        m_animator.gameObject.SetActive(false);
    }

    /// <summary>
    /// Retorna a index do level atual
    /// </summary>
    /// <returns></returns>
    public int CurrentLevel()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
