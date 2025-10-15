using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform bulletSpawner;
    public GameObject bulletPrefab;

    // Update is called once per frame
    void Update()
    {
        if (UserInput.instance.Attack)
        {
            Shoot();
        }
    }

    void Shoot ()
    {
        Instantiate(bulletPrefab, bulletSpawner.position, bulletSpawner.rotation);
    }
}
