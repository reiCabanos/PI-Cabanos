using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; 
using System.Collections;

public class SceneHandler : MonoBehaviour
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
        sceneIndex = 1; // Cena de mapaBeta 
        OpenScene(0.5f); // Transi��o r�pida
    }

    public void OpenGameScene()
    {
        sceneIndex = 2; // Cena miniGamer
        OpenScene(transitionTime); // Transi��o com tempo ajust�vel
    }

    private void OpenScene(float delay)
    {
        if (isTransitioning) return; // Evitar m�ltiplas ativa��es

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
            float progress = Mathf.Lerp(0, 100, currentTime / delay); 
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

        // Carregar a cena de forma ass�ncrona
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false; 

        // Esperar at� a cena estar completamente carregada
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                // Pequeno atraso antes de ativar a cena
                yield return new WaitForSeconds(1f);
                asyncLoad.allowSceneActivation = true; // Ativar a cena
            }
            yield return null;
        }

        
        yield return new WaitForSeconds(0.5f); 
        LeanTween.scale(fader, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => {
            fader.gameObject.SetActive(false); 
            isTransitioning = false; 
        });
    }
}
