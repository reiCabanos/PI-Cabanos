
using UnityEngine;
using UnityEngine.Audio;

namespace SmallHedge.SomUI
{
    [CreateAssetMenu(menuName = "SmallHedge/Sons UI", fileName = "Sons SO UI")]
    public class SonsSOUI : ScriptableObject
    {
        public ListaSonsUI[] sons;
    }

    [System.Serializable]
    public struct ListaSonsUI
    {
        [HideInInspector] public string nome;
        [Range(0, 1)] public float volume;
        public AudioClip[] sons;
        public AudioMixerGroup mixer;
    }
}
