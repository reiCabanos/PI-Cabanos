using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTween : MonoBehaviour
{
    [SerializeField]
    private GameObject panel1, panel2, panel3;
    [SerializeField]
    private GameObject triggerButton;

    private Vector3 panel1InitialScale, panel2InitialPos, panel3InitialScale;

    void Start()
    {
        // Armazena as escalas e posições iniciais dos painéis
        panel1InitialScale = panel1.transform.localScale;
        panel2InitialPos = panel2.transform.localPosition;
        panel3InitialScale = panel3.transform.localScale;

        // Inicializa os estados dos painéis (desativados)
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);

        // Adiciona um listener ao botão que chama o método StartSequence quando clicado
        //triggerButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartSequence);
    }

   public void StartSequence()
    {
        // Ativa os painéis antes de iniciar as animações
        panel1.SetActive(true);
        panel2.SetActive(true);
        panel3.SetActive(true);

        // Inicializa os estados dos painéis (ocultos ou fora de cena)
        panel1.transform.localScale = Vector3.zero;
        panel2.transform.localPosition = new Vector3(-30f, 1266f, 2f);
        panel3.transform.localScale = Vector3.zero;

        // Anima o panel1 para aumentar suavemente de escala
        LeanTween.scale(panel1, panel1InitialScale, 2f).setEase(LeanTweenType.easeOutCirc).setOnComplete(() =>
        {
            // Após um atraso de 4 segundos, diminui suavemente o panel1 de escala
            LeanTween.scale(panel1, Vector3.zero, 2f).setEase(LeanTweenType.easeInCirc).setDelay(1f).setOnComplete(() =>
            {
                // Move o panel2 para a posição especificada ao longo de 0.7 segundos com um atraso de 2 segundos
                LeanTween.moveLocal(panel2, new Vector3(-30f, 29f, 2f), 0.7f).setEase(LeanTweenType.easeInOutCubic).setDelay(0.5f).setOnComplete(() =>
                {
                    // Após um atraso de 3 segundos, move o panel2 de volta à sua posição inicial
                    LeanTween.moveLocal(panel2, panel2InitialPos, 0.7f).setEase(LeanTweenType.easeInOutCubic).setDelay(1f).setOnComplete(() =>
                    {
                        // Anima o panel3 para aumentar suavemente de escala
                        LeanTween.scale(panel3, panel3InitialScale, 2f).setEase(LeanTweenType.easeOutCirc).setOnComplete(() =>
                        {
                            // Após um atraso de 4 segundos, diminui suavemente o panel3 de escala
                            LeanTween.scale(panel3, Vector3.zero, 2f).setEase(LeanTweenType.easeInCirc).setDelay(1f).setOnComplete(() =>
                            {
                                // Opcional: Desativa os painéis novamente após as animações
                                panel1.SetActive(false);
                                panel2.SetActive(false);
                                panel3.SetActive(false);
                            });
                        });
                    });
                });
            });
        });
    }
}
