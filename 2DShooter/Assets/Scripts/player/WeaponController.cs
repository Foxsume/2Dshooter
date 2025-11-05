using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Weapon primaryWeapon;
    [SerializeField] private Weapon secondaryWeapon;
    [SerializeField] private Weapon meleeWeapon;

    private Weapon activeWeapon;
    private PlayerInputHandler input;

    private void Start()
    {
        input = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        // Handle switching active weapon
        if (input.Primary) activeWeapon = primaryWeapon;
        if (input.Secondary) activeWeapon = secondaryWeapon;
        if (input.Melee) activeWeapon = meleeWeapon;

        if (input.Holster) activeWeapon = null;

        if (activeWeapon == null) return;

        // Handle attacking
        if (input.AttackPressed) activeWeapon.StartFire();
        if (activeWeapon.isAutomatic && input.AttackHeld) activeWeapon.ContinueFire();
        if (input.AttackReleased) activeWeapon.StopFire();

        // Reload
        if (input.Reload && activeWeapon.CanReload()) activeWeapon.Reload();
    }
}

