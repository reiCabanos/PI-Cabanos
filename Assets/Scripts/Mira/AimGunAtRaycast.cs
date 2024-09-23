using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimGunAtRaycast : MonoBehaviour
{
    [SerializeField] private float aimAtRaycastSpeed = 10f;
    [SerializeField] private GameObject gunRoot;
    private Color rayColor = Color.blue;
    private RaycastHit hit;
    private Quaternion originalAngle;
    [SerializeField] private LayerMask aimMask;

    private void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 999999999999999);
    }
    private void Awake()
    {
        originalAngle = gunRoot.transform.localRotation;
    }
    private void Update()
    {
        if(PlayerManager.isReloading == false)
        {
            if(Physics.Raycast(transform.position, transform.forward, out hit, 999999999999999, aimMask))
            {
                //aim the gun at raycast
                Vector3 directionToTarget = hit.point - gunRoot.transform.position;
                Quaternion targRot = Quaternion.LookRotation(directionToTarget);
                gunRoot.transform.rotation = Quaternion.Slerp(gunRoot.transform.rotation, targRot, Time.deltaTime * aimAtRaycastSpeed);
            }
        }

        if(hit.collider == null)
        {
            //reset the gun position
            gunRoot.transform.localRotation = originalAngle;
        }
    }
}
