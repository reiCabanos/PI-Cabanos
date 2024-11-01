using UnityEngine;
using UnityEngine.UI;

namespace SmallHedge.SomAmbiente
{
    public class GerenciadorMusicaAmbiente : MonoBehaviour
    {
        private static GerenciadorMusicaAmbiente instancia = null;
        [SerializeField] private SonsSOAmbiente sonsSO;
        [SerializeField] private AudioSource fonteAudio;
        [SerializeField] private TipoSomAmbiente tipoSomAmbiente;
        [SerializeField] private Text muteStatusText; // Texto para exibir o estado de mute na UI

        private bool isMuted = false; // Estado de mute

        private void Awake()
        {
            if (!instancia)
            {
                instancia = this;
                fonteAudio = GetComponent<AudioSource>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            fonteAudio.loop = true;
            TocarMusicaAmbiente();
        }

        public void TocarMusicaAmbiente()
        {
            if (isMuted) return;  // Ignora se est� mutado

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

        // Fun��o para tocar sons de UI usando TipoSomAmbiente
        public void TocarSomUI(TipoSomAmbiente tipoSomUI)
        {
            if (isMuted) return;  // Ignora se est� mutado

            ListaSonsAmbiente listaSons = sonsSO.sons[(int)tipoSomUI];
            AudioClip[] clipes = listaSons.sons;
            AudioClip clipeAleatorio = clipes[Random.Range(0, clipes.Length)];

            fonteAudio.PlayOneShot(clipeAleatorio, listaSons.volume);
        }

        // Alterna o estado de mute e atualiza o texto da UI
        public void AlternarMute()
        {
            isMuted = !isMuted;
            fonteAudio.mute = isMuted;
            AtualizarUI();
        }

        // Atualiza o texto da UI com o estado de mute
        private void AtualizarUI()
        {
            if (muteStatusText != null)
            {
                muteStatusText.text = isMuted ? "Mute" : "Som";
            }
        }

        public void DefinirVolumeGeral(float volume)
        {
            fonteAudio.volume = volume;
        }

        public void DefinirVolumeMusica(float volume)
        {
            // Configura��o espec�fica para volume da m�sica
        }

        public void DefinirVolumeVoz(float volume)
        {
            // Configura��o espec�fica para volume da narra��o
        }

        public void DefinirVolumeEfeitos(float volume)
{
    // C�digo para ajustar o volume dos efeitos sonoros
}

public void AlternarMuteEfeitos(bool mute)
{
    // C�digo para mutar/desmutar os efeitos sonoros
}


    }
}
