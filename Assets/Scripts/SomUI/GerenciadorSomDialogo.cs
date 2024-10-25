using UnityEngine;
using UnityEngine.Audio;

namespace SmallHedge.SomDialogo
{
    [RequireComponent(typeof(AudioSource))]
    public class GerenciadorSomDialogo : MonoBehaviour
    {
        [SerializeField] private SonsSODialogo sonsSO;  // ScriptableObject contendo os sons de diálogo
        private static GerenciadorSomDialogo instancia = null;
        private AudioSource fonteAudio;

        private void Awake()
        {
            if (!instancia)
            {
                instancia = this;
                fonteAudio = GetComponent<AudioSource>(); // Obtém o AudioSource
            }
            else
            {
                Destroy(gameObject);  // Se já existir outra instância, esta será destruída
            }
        }

        // Método estático para tocar o som de diálogo
        public static void TocarSom(TipoSomDialogo som, AudioSource fonte = null, float volume = 1)
        {
            if (instancia == null)
            {
                Debug.LogWarning("GerenciadorSomDialogo não foi inicializado!");
                return;
            }

            ListaSonsDialogo listaSons = instancia.sonsSO.Dialogo1[(int)som];  // Usa a estrutura definida em SonsSODialogo.cs
            AudioClip[] clipes = listaSons.sons;
            AudioClip clipeAleatorio = clipes[UnityEngine.Random.Range(0, clipes.Length)];

            if (fonte)
            {
                fonte.outputAudioMixerGroup = listaSons.mixer;
                fonte.clip = clipeAleatorio;
                fonte.volume = volume * listaSons.volume;
                fonte.Play();
            }
            else
            {
                instancia.fonteAudio.outputAudioMixerGroup = listaSons.mixer;
                instancia.fonteAudio.PlayOneShot(clipeAleatorio, volume * listaSons.volume);
            }
        }

        // Método estático para parar o som de diálogo
        public static void PararSom(AudioSource fonte = null)
        {
            if (instancia == null)
            {
                Debug.LogWarning("GerenciadorSomDialogo não foi inicializado!");
                return;
            }

            // Se uma fonte específica for passada, ela para o som nessa fonte
            if (fonte)
            {
                fonte.Stop();
            }
            else
            {
                // Se nenhuma fonte for passada, para o som na AudioSource padrão do GerenciadorSomDialogo
                instancia.fonteAudio.Stop();
            }
        }
    }
}
