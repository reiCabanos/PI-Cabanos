using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabuaPool : ItemPool
{
    public static TabuaPool SharedInstance;


    protected override void Awake()
    {
        SharedInstance = this;
    }
}
