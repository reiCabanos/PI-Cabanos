
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace SmallHedge.GerenciadorSom
{
    [CustomEditor(typeof(SonsSO))]
    public class EditorSonsSO : Editor
    {
        private void AoHabilitar()
        {
            ref ListaSons[] listaSons = ref ((SonsSO)target).sons;

            if (listaSons == null)
                return;

            string[] nomes = Enum.GetNames(typeof(TipoSom));
            bool tamanhoDiferente = nomes.Length != listaSons.Length;

            Dictionary<string, ListaSons> sons = new();

            if (tamanhoDiferente)
            {
                for (int i = 0; i < listaSons.Length; ++i)
                {
                    sons.Add(listaSons[i].nome, listaSons[i]);
                }
            }

            Array.Resize(ref listaSons, nomes.Length);
            for (int i = 0; i < listaSons.Length; i++)
            {
                string nomeAtual = nomes[i];
                listaSons[i].nome = nomeAtual;
                if (listaSons[i].volume == 0) listaSons[i].volume = 1;

                if (tamanhoDiferente)
                {
                    if (sons.ContainsKey(nomeAtual))
                    {
                        ListaSons atual = sons[nomeAtual];
                        AtualizarElemento(ref listaSons[i], atual.volume, atual.sons, atual.mixer);
                    }
                    else
                        AtualizarElemento(ref listaSons[i], 1, new AudioClip[0], null);

                    static void AtualizarElemento(ref ListaSons elemento, float volume, AudioClip[] sons, AudioMixerGroup mixer)
                    {
                        elemento.volume = volume;
                        elemento.sons = sons;
                        elemento.mixer = mixer;
                    }
                }
            }
        }
    }
}
#endif
