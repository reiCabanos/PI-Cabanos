using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class ScenaTransicao : MonoBehaviour
{
    [SerializeField] RectTransform fader;
    [SerializeField] int sceneIndex; 
    [SerializeField] float transitionTime = 10f; 
    [SerializeField] Slider transitionSlider; 
    [SerializeField] TextMeshProUGUI sliderText; 

    private bool isTransitioning = false; 

    private void Start()
    {
        
        transitionSlider.maxValue = 100; 
        transitionSlider.value = 100;    
        UpdateSliderText(100); 
    }

    public void OpenMenuScene()
    {
        sceneIndex = 0; // Cena de menu
        OpenScene(0.5f); // Transição rápida
    }

    public void OpenGameScene()
    {
        sceneIndex = 1; // Cena mapaBeta
        OpenScene(transitionTime); // Transição com tempo ajustável
    }

    private void OpenScene(float delay)
    {
        if (isTransitioning) return; 

        isTransitioning = true; 
        fader.gameObject.SetActive(true); 
        transitionSlider.gameObject.SetActive(true); 

        
        StartCoroutine(UpdateSliderAndFader(delay));
        StartCoroutine(LoadSceneAsync(sceneIndex, delay));
    }

    private IEnumerator UpdateSliderAndFader(float delay)
    {
        float currentTime = 0f;

        
        while (currentTime < delay)
        {
            currentTime += Time.deltaTime;
            float progress = Mathf.Lerp(100, 0, currentTime / delay);
            transitionSlider.value = progress;
            UpdateSliderText(Mathf.CeilToInt(progress));
            yield return null;
        }

        transitionSlider.value = 0;
        UpdateSliderText(0); 
        transitionSlider.gameObject.SetActive(false); 
    }

    private void UpdateSliderText(int value)
    {
        if (sliderText != null)
        {
            sliderText.text = value.ToString(); 
        }
    }

    private IEnumerator LoadSceneAsync(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay); 

        // Carregar a cena de forma assíncrona
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false; 

        // Esperar até a cena estar completamente carregada
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                // Pequeno atraso antes de ativar a cena
                //yield return new WaitForSeconds(10f);
                asyncLoad.allowSceneActivation = true; // Ativar a cena
            }
            //yield return null;
        }

        
        yield return new WaitForSeconds(0.5f); // Pequeno atraso antes de fechar o fader
        LeanTween.scale(fader, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => {
            fader.gameObject.SetActive(false); // Desativar o fader
            isTransitioning = false; 
        });
    }
}
