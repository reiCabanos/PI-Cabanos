using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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


    private void Awake()
    {
        //set ammo
        maxGunAmmo = ammoCount;
        curGunAmmo = maxGunAmmo;

        ammoPool = new GameObject();//spawns
        objectPool = ammoPool.AddComponent<ObjectPoolM>();//adds script
        objectPool.prefab = proj;//set pool prefab
        objectPool.poolSize = ammoCount; //set size of pool
        objectPool.gameObject.name = gunName + " AmmoPool";
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * hitscanRage);
    }
    void Update()
    {
        PlayerManager.currentAmmo = curGunAmmo;
        PlayerManager.currentMaxAmmo = ammoCount;
        PlayerManager.isReloading = isReloading;

        if (canShoot == false)
        {
            shootTimer += Time.deltaTime;

            if (shootTimer > fireRate)
            {
                canShoot = true;
                shootTimer = 0;
            }
        }

        if (isReloading == true)
        {
            rTimer += Time.deltaTime;

            if (rTimer > reloadTime)
            {
                isReloading = false;
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

            if (canShoot && Input.GetMouseButton(0))
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

            if (canShoot && Input.GetMouseButtonDown(0))
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
    public void SetAtirarProjectio(InputAction.CallbackContext callbackContext)
    {
        
        if (isAuto)
        {

            if (canShoot && callbackContext.performed)
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

            if (canShoot && callbackContext.performed)
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
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, hitscanRage, hitscanlayers))
        {
            if (hit.collider.GetComponent<EnemyHealth>() != null)
            {
                EnemyHealth eh = hit.collider.GetComponent<EnemyHealth>();
                eh.health -= hitscanDamage;
            }
        }

        curGunAmmo--;
      
    }
}
