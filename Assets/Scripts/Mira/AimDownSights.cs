using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDownSights : MonoBehaviour
{
    public Camera mainCamera;
    public Camera gunCamera;
    public GameObject gunRoot;
    public GameObject adsPos;
    public GameObject gunPos;
    public GameObject reloadPos;
    [HideInInspector] public Quaternion originalAngle;
    // Start is called before the first frame update
    void Awake()
    {
        originalAngle = gunRoot.transform.rotation;
    }
}
