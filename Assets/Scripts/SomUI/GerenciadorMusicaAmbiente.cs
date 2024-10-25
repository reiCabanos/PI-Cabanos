
using UnityEngine;

namespace SmallHedge.SomAmbiente
{
    public class GerenciadorMusicaAmbiente : MonoBehaviour
    {
        private static GerenciadorMusicaAmbiente instancia = null;
        [SerializeField] private SonsSOAmbiente sonsSO;
        [SerializeField] private AudioSource fonteAudio;
        [SerializeField] private TipoSomAmbiente tipoSomAmbiente;

        private void Awake()
        {
            if (!instancia)
            {
                instancia = this;
                fonteAudio = GetComponent<AudioSource>();
            }
        }

        private void Start()
        {
            fonteAudio.loop = true;
            TocarMusicaAmbiente();
        }

        public void TocarMusicaAmbiente()
        {
            ListaSonsAmbiente listaSons = sonsSO.sons[(int)tipoSomAmbiente];
            AudioClip[] clipes = listaSons.sons;
            AudioClip clipeAleatorio = clipes[Random.Range(0, clipes.Length)];

            fonteAudio.clip = clipeAleatorio;
            fonteAudio.volume = listaSons.volume;
            fonteAudio.Play();
        }

        public void PararMusica()
        {
            fonteAudio.Stop();
        }

        public void PausarMusica()
        {
            fonteAudio.Pause();
        }

        public void RetomarMusica()
        {
            fonteAudio.Play();
        }
    }
}
