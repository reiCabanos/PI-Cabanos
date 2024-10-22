using UnityEngine;
using SmallHedge.GerenciadorSom;

public class TocarSomPassos : StateMachineBehaviour
{
    public TipoSom tipoSom;  // Tipo de som (de acordo com o enum TipoSom no GerenciadorSom)
    public float volume = 1.0f;  // Volume base do som

    // Chamado quando a animação entra no estado de movimento
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Toca o som de passos em loop, usando a lógica do GerenciadorSom
        GerenciadorSom.TocarSomLoop(tipoSom, null, volume);
    }

    // Chamado quando o estado de movimento termina (ex: o personagem para de andar)
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Para o som de passos quando o personagem sai do estado de movimento
        GerenciadorSom.PararSomLoop();
    }
}
