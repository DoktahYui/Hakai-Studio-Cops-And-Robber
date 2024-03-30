using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public Camera fpsCam;
    public Recoil recoil;
    private WeaponRecoil weaponRecoil;
    private WeaponSwitcherScript weaponSwitcher;
    [SerializeField] private Gun[] gun = new Gun[3];
    public int selectWeapon;
    public float radius = 20f;

    [SerializeField]
    private bool bulletSpread = true;

    [SerializeField]
    private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);

    [SerializeField]
    private ParticleSystem shootingSystem;

    [SerializeField]
    private Transform gunMuzzle;

    [SerializeField]
    private ParticleSystem ImpactParticleSystem;

    [SerializeField]
    private TrailRenderer bulletTrail;

    void Start()
    {
        weaponSwitcher = this.gameObject.GetComponent<WeaponSwitcherScript>();
        recoil = GameObject.Find("CameraRot/CameraRecoil").GetComponent<Recoil>();
        weaponRecoil = transform.GetChild(0).GetComponent<WeaponRecoil>();
    }

    void Update()
    {
        if (weaponSwitcher != null)
        {
            if (weaponSwitcher.selectedWeapon == 0)
            {
                selectWeapon = 0;
            }

            if (weaponSwitcher.selectedWeapon == 1)
            {
                selectWeapon = 1;
            }

            if (weaponSwitcher.selectedWeapon == 2)
            {
                selectWeapon = 2;
            }
        }
    }

    public void Shoot(float range)
    {
        recoil.RecoilFire();
        weaponRecoil.RecoilFire();

        shootingSystem.Play();
        Vector3 direction = GetDirection();

        if (Physics.Raycast(gunMuzzle.position, direction, out RaycastHit hit, range))
        {
            TrailRenderer trail = Instantiate(bulletTrail, gunMuzzle.transform.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, hit));
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;

        if (bulletSpread)
        {
            direction += new Vector3
                (
                  Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
                  Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
                  Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)
                );

            direction.Normalize();
        }

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = hit.point;
        Instantiate(ImpactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(trail.gameObject, trail.time);
    }
}
