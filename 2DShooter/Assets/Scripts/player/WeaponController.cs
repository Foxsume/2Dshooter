using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] private Weapon primaryWeapon;
    [SerializeField] private Weapon secondaryWeapon;
    [SerializeField] private Weapon meleeWeapon;
    [Space(2)]

    [SerializeField] private GameObject[] Arms;
    private int activeArms = 0;

    private Weapon activeWeapon;
    private PlayerInputHandler input;
    private PlayerState state;

    private void Start()
    {
        input = GetComponent<PlayerInputHandler>();
        state = GetComponent<PlayerState>();

        SwitchWeapon(0); // Default to holstered state
    }

    private void Update()
    {
        // Handle switching active weapon
        if (input.Primary) SwitchWeapon(1);
        if (input.Secondary) SwitchWeapon(2);
        if (input.Melee) SwitchWeapon(3);

        if (input.Holster) SwitchWeapon(0);

        if (activeWeapon == null) return;

        // Handle attacking
        if (input.AttackPressed) activeWeapon.StartFire();
        if (activeWeapon.isAutomatic && input.AttackHeld) activeWeapon.ContinueFire();
        if (input.AttackReleased) activeWeapon.StopFire();

        // Reload
        if (input.Reload && activeWeapon.CanReload()) activeWeapon.Reload();
    }

    private void SwitchWeapon(int weaponIndex) // Also handles switchin between arms GameObjects
    {
        // Prevent unnecessary action if "trying to siwtch to already equiped weapon or trying to holster when already holstered"
        if (activeArms == weaponIndex) return;

        // Deactivate currently active arms
        Arms[activeArms].SetActive(false);

        // Activate correct arms
        activeArms = weaponIndex;
        Arms[activeArms].SetActive(true);

        switch (weaponIndex)
        {
            case 0:
            activeWeapon = null; break;

            case 1:
            activeWeapon = primaryWeapon; break;
            
            case 2:
            activeWeapon = secondaryWeapon; break;
          
            case 3:
            activeWeapon = meleeWeapon; break;

            default: return;
        }
        return;
    }
}

