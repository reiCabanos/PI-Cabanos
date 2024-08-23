using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemInventario : ScriptableObject
{
    // Start is called before the first frame update

    /// <summary>
    /// ORIGINAL ITENNS DADOS 
    /// </summary>

    public string _nome;
    public int _tipo;
    public Sprite _img;
    public int dano;
    public int forca;

        public string Name
    {
        get { return _nome; }
        set { _nome = value; }
    }

    public int Tip
    {
        get { return _tipo; }
        set { _tipo = value; }
    }
    public Sprite ImageItem
    {
        get { return _img; }
        set { _img = value; }
    }

}
