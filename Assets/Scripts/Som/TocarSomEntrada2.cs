using UnityEngine;
using SmallHedge.GerenciadorSom;

public class TocarSomEntrada2 : StateMachineBehaviour
{
    public TipoSom tipoSom;  // Permite selecionar o som via Inspector

    // Nome do parâmetro que controla a velocidade vertical do personagem
    public string parametroPulandoY = "pulandoY";
    public float valorMinimoPulandoY = 3.2f;  // Valor mínimo para considerar que o personagem está pulando

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float velocidadeY = animator.GetFloat(parametroPulandoY);  // Pega a velocidade no eixo Y

        // Verifica se a velocidade Y é maior que o valor mínimo para pulo
        if (Mathf.Abs(velocidadeY) > valorMinimoPulandoY)
        {
            GerenciadorSom.TocarSom(tipoSom);  // Toca o som selecionado no Inspector
        }
    }
}
