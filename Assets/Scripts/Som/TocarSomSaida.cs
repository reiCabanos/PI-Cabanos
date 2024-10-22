
using UnityEngine;

namespace SmallHedge.GerenciadorSom
{
    public class TocarSomSaida : StateMachineBehaviour
    {
        [SerializeField] private TipoSom som;
        [SerializeField, Range(0, 1)] private float volume = 1;
        override public void OnStateExit(Animator animador, AnimatorStateInfo infoEstado, int indiceCamada)
        {
            GerenciadorSom.TocarSom(som, null, volume);
        }
    }
}
