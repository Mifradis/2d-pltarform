using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] RangedWeapon gunData;

    float timeSinceLastShot;
    PlayerInput shootingInput;

    [Header("Bullet")]
    public GameObject gunPrefab;
    public GameObject bulletPrefab;
    bool CanShoot() =>  !gunData.reloading && (timeSinceLastShot > (1f / (gunData.fireRate / 60f)));

    void Shoot()
    {
        print(CanShoot());
        if (CanShoot())
        {
            Vector2 spawnPosition;
            spawnPosition = new Vector2(gunPrefab.transform.position.x, gunPrefab.transform.position.y);
            GameObject newBullet = (GameObject)Instantiate(bulletPrefab, spawnPosition, Quaternion.Euler(0, 0, 180));
            gunData.currentAmmmo--;
            timeSinceLastShot = 0;
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
    }
    private void Awake()
    {
        shootingInput = GetComponentInParent<PlayerInput>();
    }
    private void Start()
    {
        shootingInput.onShoot += Shoot;
        shootingInput.onReload += StartReload;
    }
    private void Update()
    {
        timeSinceLastShot = Time.deltaTime;
    }
}
