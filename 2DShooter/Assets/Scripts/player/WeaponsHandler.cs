using UnityEngine;

public class WeaponsHandler : MonoBehaviour
{
    int prevActiveWeapon;
    public GameObject[] weapons;

    // at start activate weapons[0] object and set that as previous active weapon
    void Start()
    {
        weapons[0].SetActive(true);
        prevActiveWeapon = 0;
    }

    // Weapons switching is handled in update
    // TODO: when holding down any option button, bring down a gear wheel for the corresponding weapon class
    void Update()
    {
        // primary
        if (UserInput.instance.Primary)
        {
            weapons[prevActiveWeapon].SetActive(false);
            weapons[1].SetActive(true);
            prevActiveWeapon = 1;
        }

        // secondary
        if (UserInput.instance.Secondary)
        {
            weapons[prevActiveWeapon].SetActive(false);
            weapons[2].SetActive(true);
            prevActiveWeapon = 2;
        }

        // knife's index is 2 instead of 3 for now
        if (UserInput.instance.Melee)
        {
            weapons[prevActiveWeapon].SetActive(false);
            weapons[2].SetActive(true);
            prevActiveWeapon = 2;
        }

        // unequip weapon
        if (UserInput.instance.Holster)
        {
            weapons[prevActiveWeapon].SetActive(false);
            weapons[0].SetActive(true);
            prevActiveWeapon = 0;
        }
    }
}
