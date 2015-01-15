using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
    public bool automatic;
    public float damage;
    public int currentClip;
    public int clipSize;
    public int currentAmmo;
    public int maxAmmo;
    public float reloadTime;
    public float fireRate;
    public float bulletForce;
    public AmmoType ammoType;

    
    public float nextFireTime = 1000;
    
    public float CalculateNextFireTime(float fireRate) {
        return Time.time + fireRate;
    }

    public bool CheckFire() {
        return (Time.time > nextFireTime);
    }

}
