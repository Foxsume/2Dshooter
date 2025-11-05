using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected string weaponName;
    protected WeaponType Type;

    protected int maxAmmo;
    protected int currentAmmo;
    protected int reserveAmmo;
    protected float reloadTime;

    public bool isAutomatic;


    // Common interface
    public abstract void StartFire();
    public abstract void ContinueFire();
    public abstract void StopFire();

    // Virtual reaload so melee can ignore
    public virtual void Reload() { }
    public virtual bool CanReload() => false;
}

public enum WeaponType
{
    Primary,
    Secondary,
    Melee
}