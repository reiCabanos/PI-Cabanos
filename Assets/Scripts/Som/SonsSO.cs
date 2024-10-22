
using UnityEngine;

namespace SmallHedge.GerenciadorSom
{
    [CreateAssetMenu(menuName = "Small Hedge/Sons SO", fileName = "Sons SO")]
    public class SonsSO : ScriptableObject
    {
        public ListaSons[] sons;
    }
}
