
using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SmallHedge.GerenciadorSom
{
    [RequireComponent(typeof(AudioSource))]
    public class GerenciadorSom : MonoBehaviour
    {
        [SerializeField] private SonsSO SO;
        private static GerenciadorSom instancia = null;
        private AudioSource fonteAudio;

        private void Awake()
        {
            if (!instancia)
            {
                instancia = this;
                fonteAudio = GetComponent<AudioSource>();
            }
        }

        public static void TocarSom(TipoSom som, AudioSource fonte = null, float volume = 1)
        {
            ListaSons listaSons = instancia.SO.sons[(int)som];
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

        public static void TocarSomLoop(TipoSom tipoSom, AudioSource fonte = null, float volume = 1)
        {
            ListaSons listaSons = instancia.SO.sons[(int)tipoSom];
            AudioClip[] clipes = listaSons.sons;
            AudioClip clipeAleatorio = clipes[UnityEngine.Random.Range(0, clipes.Length)];

            if (fonte)
            {
                fonte.outputAudioMixerGroup = listaSons.mixer;
                fonte.clip = clipeAleatorio;
                fonte.volume = volume * listaSons.volume;
                fonte.loop = true;  // Configura o som para tocar em loop
                fonte.Play();
            }
            else
            {
                instancia.fonteAudio.outputAudioMixerGroup = listaSons.mixer;
                instancia.fonteAudio.clip = clipeAleatorio;
                instancia.fonteAudio.volume = volume * listaSons.volume;
                instancia.fonteAudio.loop = true;  // Configura o som para tocar em loop
                instancia.fonteAudio.Play();
            }
        }

        public static void PararSomLoop(AudioSource fonte = null)
        {
            if (fonte)
            {
                fonte.Stop();
            }
            else
            {
                instancia.fonteAudio.Stop();
            }
        }


    }

    [Serializable]
    public struct ListaSons
    {
        [HideInInspector] public string nome;
        [Range(0, 1)] public float volume;
        public AudioMixerGroup mixer;
        public AudioClip[] sons;
    }
}
