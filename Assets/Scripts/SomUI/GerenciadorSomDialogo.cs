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

            ListaSonsDialogo listaSons = instancia.sonsSO.sons[(int)som];  // Usa a estrutura definida em SonsSODialogo.cs
            AudioClip[] clipes = listaSons.sons;
            AudioClip clipeAleatorio = clipes[UnityEngine.Random.Range(0, clipes.Length)];

            Debug.Log($"Tentando tocar som: {clipeAleatorio.name}");

            // Verifica se está usando uma fonte personalizada ou a fonte padrão
            if (fonte)
            {
                Debug.Log($"Usando AudioSource personalizado: {fonte.name}");

                // Verifica se o áudio não está sendo reproduzido para evitar duplicação
                if (!fonte.isPlaying || fonte.clip != clipeAleatorio)
                {
                    fonte.outputAudioMixerGroup = listaSons.mixer;
                    fonte.clip = clipeAleatorio;
                    fonte.volume = volume * listaSons.volume;
                    fonte.Play();
                }
                else
                {
                    Debug.Log($"Som {clipeAleatorio.name} já está sendo reproduzido nesta fonte.");
                }
            }
            else
            {
                Debug.Log("Usando AudioSource padrão do GerenciadorSomDialogo");

                // Verifica se o áudio não está sendo reproduzido para evitar duplicação no PlayOneShot
                if (!instancia.fonteAudio.isPlaying)
                {
                    instancia.fonteAudio.outputAudioMixerGroup = listaSons.mixer;
                    instancia.fonteAudio.PlayOneShot(clipeAleatorio, volume * listaSons.volume);
                }
                else
                {
                    Debug.Log($"Som {clipeAleatorio.name} já está sendo reproduzido na fonte padrão.");
                }
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
                Debug.Log($"Parando som na fonte personalizada: {fonte.name}");
                fonte.Stop();
            }
            else
            {
                // Se nenhuma fonte for passada, para o som na AudioSource padrão do GerenciadorSomDialogo
                Debug.Log("Parando som na fonte padrão do GerenciadorSomDialogo");
                instancia.fonteAudio.Stop();
            }
        }
    }
}
