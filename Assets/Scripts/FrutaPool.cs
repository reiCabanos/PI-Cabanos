using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrutaPool : ItemPool
{  
    public static FrutaPool SharedInstance;
    

    protected override void Awake()
    {
        SharedInstance = this;
    }
}
