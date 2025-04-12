using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform bulletSpawner;
    public GameObject bulletPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot ()
    {
        Instantiate(bulletPrefab, bulletSpawner.position, bulletSpawner.rotation);
    }
}
