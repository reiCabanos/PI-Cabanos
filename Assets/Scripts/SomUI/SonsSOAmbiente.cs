
using UnityEngine;
using UnityEngine.Audio;

namespace SmallHedge.SomAmbiente
{
    [CreateAssetMenu(menuName = "SmallHedge/Sons Ambiente", fileName = "Sons SO Ambiente")]
    public class SonsSOAmbiente : ScriptableObject
    {
        public ListaSonsAmbiente[] sons;
    }

    [System.Serializable]
    public struct ListaSonsAmbiente
    {
        [HideInInspector] public string nome;
        [Range(0, 1)] public float volume;
        public AudioClip[] sons;
        public AudioMixerGroup mixer;
    }
}
