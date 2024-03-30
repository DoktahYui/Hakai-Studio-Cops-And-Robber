using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [SerializeField] public float recoilX;
    [SerializeField] public float recoilY;
    [SerializeField] public float recoilZ;

    [SerializeField] public float aimrecoilX;
    [SerializeField] public float aimrecoilY;
    [SerializeField] public float aimrecoilZ;

    [SerializeField] public float snappiness;
    [SerializeField] public float returnSpeed;

    //public int damage;
    public float range;
    public float fireRate;

    //public int maxAmmo;
    //public int currentAmmo;
    //public float reloadTime;

    private float nextTimeToFire = 0f;

    //public WeaponSwitcherScript weaponSwitcher;

    public Shooting shoot;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            shoot.Shoot(range);
        }
    }
}
