using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject projectile;
    public int damage;
    public int bulletSpeed;
    public int numBullets;
    public float spreadAngle;
    public int numShots;
    public float reloadTime;
    public GameObject firePoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(projectile, firePoint.transform.position, transform.rotation);
        bullet.GetComponent<BulletMovement>().SetProperties(bulletSpeed, damage, false);
        float offset = GetComponent<GunMovement>().rotationOffset;
        bullet.transform.rotation = Quaternion.AngleAxis(offset + bullet.transform.rotation.eulerAngles.z, Vector3.forward);
    }
}
