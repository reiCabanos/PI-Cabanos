using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiosPool : ItemPool
{
    public static RaiosPool SharedInstance;
    protected override void Awake()
    {
        SharedInstance = this;
    }
}
