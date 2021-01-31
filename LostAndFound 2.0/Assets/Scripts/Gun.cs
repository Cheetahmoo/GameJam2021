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
    public int shotsLeft;
    float reloadTimer;
    public GameObject reloadBar;
    public float reloadBarMaxSize;
    SpriteRenderer reloadSprite;
    public bool isReloading;
    // Start is called before the first frame update
    void Start()
    {
        isReloading = false;
        if (!playerControlled)
        {
            shootTime = coolDownTime;
        }
        else
        {
            shootTime = 0;
        }

        shotsLeft = numShots;
        if (reloadBar)
        {
            reloadSprite = reloadBar.GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shootTime > 0)
        {
            shootTime -= Time.deltaTime;
        }

        if (reloadTimer > 0)
        {
            reloadTimer -= Time.deltaTime;
            ScaleReloadBar();
        }
        else
        {
            if (shotsLeft == 0)
            {
                if (reloadSprite)
                {
                    reloadSprite.enabled = false;
                }
                shotsLeft = numShots;
                isReloading = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            shotsLeft = 0;
            Reload();
        }

        if (GetCommand() && shootTime <= 0 && shotsLeft >= 1)
        {
            Fire();
            shootTime = coolDownTime;
            if (shotsLeft <= 0)
            {
                Reload();
            }
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
        shotsLeft--;
        float dTheta = 0;
        if (numBullets > 1)
        {
            dTheta = spreadAngle / (numBullets - 1);

        }
        for (int i = 0; i < numBullets; i++)
        {
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

    public void Reload()
    {
        isReloading = true;
        reloadTimer = reloadTime;
        ScaleReloadBar();
        if (reloadSprite)
        {
            reloadSprite.enabled = true;
        }
    }

    void ScaleReloadBar()
    {
        if (reloadBar && reloadSprite)
        {
            Vector3 scale = Vector3.right * reloadBarMaxSize * (reloadTimer / reloadTime) + reloadBarMaxSize * Vector3.up;
            reloadBar.transform.localScale = scale;
        }

    }
}