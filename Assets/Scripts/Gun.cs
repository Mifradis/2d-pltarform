using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    PlayerMovement playerMovement;
    [Header("References")]
    [SerializeField] RangedWeapon gunData;

    PlayerInput shootingInput;
    float attackSpeed;
    float nextBulletSpawn;
    public Animator animations;

    [Header("Bullet")]
    Vector2 spawnPosition;
    public GameObject gunPrefab;
    public GameObject bulletPrefab;

    bool CanShoot()
    {
        if (gunData.currentAmmmo > 0 && !gunData.reloading) {
            if (Time.time > nextBulletSpawn)
            {
                nextBulletSpawn = Time.time + attackSpeed;
                return true;
            }
            else
                return false;
        } else
            return false;
    }

    void Shoot()
    {
        if (CanShoot())
        {
            animations.SetBool("IsShooting", true);
            spawnPosition = new Vector2(gunPrefab.transform.position.x, gunPrefab.transform.position.y);

            if (playerMovement.isFacingRight)
            {
                GameObject newBullet = (GameObject)Instantiate(bulletPrefab, spawnPosition, Quaternion.Euler(0, 0, 0));
            }
            else
            {
                GameObject newBulletLeft = (GameObject)Instantiate(bulletPrefab, spawnPosition, Quaternion.Euler(0, 0, 180));
            }
            gunData.currentAmmmo--;
        }
        if(gunData.currentAmmmo <= 0)
        {
            StartReload();
        }
        
    }
    public void StartReload()
    {
        if (!gunData.reloading)
        {
            StartCoroutine(Reload());
        }
    }
    private IEnumerator Reload()
    {
        gunData.reloading = true;
        yield return new WaitForSeconds(gunData.reloadTime);
        gunData.currentAmmmo = gunData.magSize;
        gunData.reloading = false;
        animations = GetComponent<Animator>();
    }
    private void Awake()
    {
        shootingInput = GetComponentInParent<PlayerInput>();
        playerMovement = GetComponentInParent<PlayerMovement>();
    }
    private void Start()
    {
        attackSpeed = 1f / (gunData.fireRate / 60f);
        shootingInput.onShoot += Shoot;
        shootingInput.onReload += StartReload;
        
    }
    private void Update()
    {
        if(Time.time >= nextBulletSpawn)
        {
            animations.SetBool("IsShooting", false);
        }
        Debug.Log(animations.GetBool("IsShooting"));
    }
}
