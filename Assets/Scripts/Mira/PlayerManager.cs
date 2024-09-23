using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector] public static int currentAmmo;
    [HideInInspector] public static int currentMaxAmmo;
    [HideInInspector] public static bool isReloading = false;
    [HideInInspector] public static bool aimingDownSights = false;
    // Start is called before the first frame update
    void Start()
    {
        isReloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
