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
    public float coolDownTime;
    public bool playerControlled;
    bool mechFire;
    float shootTime;
    // Start is called before the first frame update
    void Start()
    {
        shootTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(shootTime > 0)
        {
            shootTime -= Time.deltaTime;
        }
        if (GetCommand() && shootTime <= 0)
        {
            Fire();
            shootTime = coolDownTime;
        }
    }

    bool GetCommand()
    {
        if (playerControlled)
        {
            return Input.GetMouseButtonDown(0);
        }
        return mechFire;
    }

    void Fire()
    {
        mechFire = false;
        for (int i = 0; i < numBullets; i++)
        {
            float dTheta;
            if (numBullets > 1)
            {
                dTheta = spreadAngle / (numBullets - 1);

            }
            else
            {
                dTheta = spreadAngle / 2;
            }
            GameObject bullet = Instantiate(projectile, firePoint.transform.position, transform.rotation);
            bullet.GetComponent<BulletMovement>().SetProperties(bulletSpeed, damage, !playerControlled);
            float offset = GetComponent<GunMovement>().rotationOffset;
            if (numBullets != 1)
            {
                bullet.transform.rotation = Quaternion.AngleAxis(offset + bullet.transform.rotation.eulerAngles.z + (i * dTheta - spreadAngle / 2), Vector3.forward);
            }
            else
            {
                bullet.transform.rotation = Quaternion.AngleAxis(offset + bullet.transform.rotation.eulerAngles.z, Vector3.forward);
            }
        }
    }

    public void MechFire()
    {
        mechFire = true;
    }
}
