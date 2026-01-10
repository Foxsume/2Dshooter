using System.Collections;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] private float reloadTime;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int currentAmmo;
    [SerializeField] private int reserveAmmo;
    [SerializeField] private int maxReserveAmmo;

    [SerializeField] private float FireRate = 5f; // bullets per second
    private float lastFireTime;
    private bool isReloading = false;

    public override void StartFire()
    {
        if (isReloading || currentAmmo <= 0) return;
        TryShoot();
    }

    public override void ContinueFire()
    {
        if (isAutomatic && !isReloading && currentAmmo > 0) TryShoot();
    }

    public override void StopFire()
    {
        // reset timers if needed
    }

    public override void Reload()
    {
        StartCoroutine(ReloadRoutine());
    }

    public override bool CanReload() => currentAmmo < maxAmmo && !isReloading; // also check for reserve ammo

    private void TryShoot()
    {
        if (Time.time - lastFireTime < 1f / FireRate) return;
        lastFireTime = Time.time;
        Shoot();
    }

    private void Shoot()
    {
        currentAmmo--;
        // spawn bullet, play VFX, sound, etc.
    }

    private IEnumerator ReloadRoutine()
    {
        isReloading = true;
        // Play reload animation and sounds here
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        // Minus from reserve ammo
        isReloading = false;
    }
}

