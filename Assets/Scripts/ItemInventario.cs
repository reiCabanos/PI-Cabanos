using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemInventario : ScriptableObject
{
    // Start is called before the first frame update

    public string _nome;
    public int _tipo;
    public Sprite _img;
    public int dano;
    public int forca;


}
