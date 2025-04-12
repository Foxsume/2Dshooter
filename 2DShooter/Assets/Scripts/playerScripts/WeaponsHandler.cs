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
    void Update()
    {
        // weapons
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weapons[prevActiveWeapon].SetActive(false);
            weapons[1].SetActive(true);
            prevActiveWeapon = 1;
        }

        // knife's index is 2 instead of 3 for now
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weapons[prevActiveWeapon].SetActive(false);
            weapons[2].SetActive(true);
            prevActiveWeapon = 2;
        }

        // unequip weapon
        if (Input.GetKeyDown(KeyCode.R))
        {
            weapons[prevActiveWeapon].SetActive(false);
            weapons[0].SetActive(true);
            prevActiveWeapon = 0;
        }
    }
}
