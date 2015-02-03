using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : InventoryItem
{
    public bool automatic;
    public float damage;
    public int currentClip;
    public int clipSize;
    public int currentAmmo;
    public int maxAmmo;
    public float reloadTime;
    public float fireRate;
    public float pullOutTime;
    public float bulletForce;
    public AmmoType ammoType;

    public Vector3 playerHoldPosition;
    public Vector3 playerHoldRotation;



}
