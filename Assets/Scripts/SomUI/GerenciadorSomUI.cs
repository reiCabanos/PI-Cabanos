
using UnityEngine;

namespace SmallHedge.SomUI
{
    public class GerenciadorSomUI : MonoBehaviour
    {
        private static GerenciadorSomUI instancia = null;
        [SerializeField] private SonsSOUI sonsSO;
        [SerializeField] private AudioSource fonteAudio;

        private void Awake()
        {
            if (!instancia)
            {
                instancia = this;
                fonteAudio = GetComponent<AudioSource>();
            }
        }

        public void TocarSom(TipoSomUI tipoSom, float volume = 1)
        {
            ListaSonsUI listaSons = sonsSO.sons[(int)tipoSom];
            AudioClip[] clipes = listaSons.sons;
            AudioClip clipeAleatorio = clipes[Random.Range(0, clipes.Length)];

            fonteAudio.PlayOneShot(clipeAleatorio, volume * listaSons.volume);
        }
    }
}
