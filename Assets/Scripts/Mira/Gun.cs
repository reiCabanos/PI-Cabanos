using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.GraphicsBuffer;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject proj;
    [SerializeField] private bool isAuto = true;
    [SerializeField] private bool useProjectileGavity = true;
    [SerializeField] private float rangeProjectileSpeed = 100f;
    [SerializeField] private float hitscanDamage = 25f;
    [SerializeField] private bool useHitscan = true;
    [SerializeField] private float hitscanRage = 100f;
    [SerializeField] private LayerMask hitscanlayers;
    [SerializeField] private float fireRate = 0.25f;
    private bool canShoot = true;
    private float shootTimer;

    [SerializeField] private string gunName;
    private GameObject ammoPool;
    private ObjectPoolM objectPool;
    [SerializeField] private int ammoCount = 15;
    private int maxGunAmmo;
    private int curGunAmmo;

    [SerializeField] private float reloadTime = 2f;
    private bool canReload = false;
    private bool isReloading = false;
    private float rTimer;
    [SerializeField] private Transform barrelCheckPos;

    private AimDownSights aimDownSights;
    private FPS_Controller fpsController;
    private AimGunAtRaycast aimGunAtRaycast;
    [SerializeField] private bool canADS = true;
    [Header("Lower is higer zoom")]
    [SerializeField] private float adsZoom = 60f;
    [SerializeField] private Vector3 adsOffset = Vector3.zero;
    private bool isAimingDownSight = false;
    private Vector3 posToShootFrom;

    private void Awake()
    {
        aimGunAtRaycast = FindObjectOfType<AimGunAtRaycast>();
        fpsController = GetComponentInParent<FPS_Controller>();
        aimDownSights = GetComponentInParent<AimDownSights>();

        //set ammo
        maxGunAmmo = ammoCount;
        curGunAmmo = maxGunAmmo;

        ammoPool = new GameObject();//spawns
        objectPool = ammoPool.AddComponent<ObjectPoolM>();//adds script
        objectPool.prefab = proj;//set pool prefab
        objectPool.poolSize = ammoCount; //set size of pool
        objectPool.gameObject.name = gunName + " AmmoPool";
    }
    private void OnEnable()
    {
        isAimingDownSight = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * hitscanRage);
    }
    void Update()
    {
        PlayerManager.aimingDownSights = isAimingDownSight;
        PlayerManager.currentAmmo = curGunAmmo;
        PlayerManager.currentMaxAmmo = ammoCount;
        PlayerManager.isReloading = isReloading;

        if (canADS)
        {
            if (Input.GetMouseButton(1) && isReloading == false)
            {
                if (isAimingDownSight == false)
                {
                    float result = Mathf.InverseLerp(0, 60, adsZoom);
                    fpsController.lookSpeed = result * (fpsController.lookSpeed);

                    aimDownSights.mainCamera.fieldOfView = adsZoom;
                    aimDownSights.gunCamera.fieldOfView = adsZoom;
                    aimGunAtRaycast.enabled = false;
                    aimDownSights.gunRoot.transform.position = aimDownSights.adsPos.transform.position + adsOffset;
                    aimDownSights.gunRoot.transform.rotation = Quaternion.LookRotation(aimDownSights.adsPos.transform.forward);

                    isAimingDownSight = true;
                }
            }
            else
            {
                DisableAds();
            }
        }
        else
        {
            DisableAds();
        }


        if (canShoot == false)
        {
            shootTimer += Time.deltaTime;

            if (shootTimer > fireRate)
            {
                canShoot = true;
                shootTimer = 0;
            }
        }

        //reloading
        if (isReloading == true)
        {
            rTimer += Time.deltaTime;
            //while reloading
            aimGunAtRaycast.enabled = false;
            aimDownSights.gunRoot.transform.position = aimDownSights.reloadPos.transform.position + adsOffset;
            aimDownSights.gunRoot.transform.rotation = Quaternion.LookRotation(aimDownSights.reloadPos.transform.forward);

            //when reloading is done
            if (rTimer > reloadTime)
            {
                isReloading = false;

                aimDownSights.gunRoot.transform.position = aimDownSights.gunPos.transform.position;
                aimGunAtRaycast.enabled = true;

                curGunAmmo = maxGunAmmo;
                rTimer = 0;
            }
        }

        if (canReload == true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                isReloading = true;
                canReload = false;
            }
        }

        if (curGunAmmo > 0)
        {
            if (Physics.Linecast(barrelCheckPos.position, transform.position))
            {
                Debug.Log("blocked");
            }
            else
            {
                ShootGun();
            }
        }

        if (curGunAmmo < maxGunAmmo)
        {
            canReload = true;
        }
    }
    private void ShootGun()
    {
        if (isAuto)
        {

            if (canShoot && isReloading == false && Input.GetMouseButton(0))
            {
                if (useHitscan)
                {
                    FireHitscan();
                }
                else
                {
                    CreateProjectile();
                }
                canShoot = false;
            }
        }
        else
        {

            if (canShoot && isReloading == false && Input.GetMouseButtonDown(0))
            {
                if (useHitscan)
                {
                    FireHitscan();
                }
                else
                {
                    CreateProjectile();
                }
                canShoot = false;
            }
        }
    }
    private void CreateProjectile()
    {
        proj = objectPool.GetObject();
        proj.transform.position = transform.position;
        proj.transform.rotation = transform.rotation;
        Rigidbody rb = proj.GetComponent<Rigidbody>();

        if (useProjectileGavity)
        {
            rb.useGravity = true;
        }
        else
        {
            rb.useGravity = false;
        }

        rb.AddForce(gameObject.transform.forward * rangeProjectileSpeed, ForceMode.Force);

        curGunAmmo--;
    }
    private void FireHitscan()
    {

        if (isAimingDownSight)
        {
            posToShootFrom = Camera.main.transform.position;
        }
        else
        {
            posToShootFrom = transform.position;

        }

        if (Physics.Raycast(posToShootFrom, transform.forward, out RaycastHit hit, hitscanRage, hitscanlayers))
        {
            if (hit.collider.GetComponent<EnemyHealth>() != null)
            {
                EnemyHealth eh = hit.collider.GetComponent<EnemyHealth>();
                eh.health -= hitscanDamage;
            }
        }

        curGunAmmo--;
    }

    private void DisableAds()
    {
        //reset look speed
        fpsController.lookSpeed = fpsController.startLookSpeed;

        //reset the fov
        aimDownSights.mainCamera.fieldOfView = 60f;
        aimDownSights.gunCamera.fieldOfView = 60f;
        //reset gun pos
        aimDownSights.gunRoot.transform.position = aimDownSights.gunPos.transform.position;
        aimGunAtRaycast.enabled = true;

        isAimingDownSight = false;

    }


}
