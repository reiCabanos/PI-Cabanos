
using UnityEngine;
using UnityEngine.Audio;

namespace SmallHedge.SomDialogo
{
    [CreateAssetMenu(menuName = "SmallHedge/Sons Dialogo", fileName = "Sons SO Dialogo")]
    public class SonsSODialogo : ScriptableObject
    {
        public ListaSonsDialogo[] sons;
    }

    [System.Serializable]
    public struct ListaSonsDialogo
    {
        [HideInInspector] public string nome;
        [Range(0, 1)] public float volume;
        public AudioClip[] sons;
        public AudioMixerGroup mixer;
    }
}
