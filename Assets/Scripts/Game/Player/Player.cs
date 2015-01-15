using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
    public int health;

    public SlotType selectedSlot;
    public Weapon primary;
    public Weapon secondary;
    public Throwable throwable;
    public Equipment equipment;

    [HideInInspector]
    public bool fireRelease;
    private PlayerNetwork playerNetwork;
    private Dictionary<AmmoType, int> ammo = new Dictionary<AmmoType, int>();

    public PlayerHUD playerHUD;


    void Start() {
        primary = GetComponentInChildren<Weapon>();
        playerNetwork = GetComponent<PlayerNetwork>();
    }

    public void ChangeSelectedSlot(SlotType type) {
        selectedSlot = type;
    }

    public void Fire() {
        switch(selectedSlot)
        {
            case SlotType.Primary:
                FirePrimary();
                break;
            case SlotType.Secondary:
                break;
            case SlotType.Throwable:
                break;
            case SlotType.Equipment:
                break;

        }
    }

    public void FirePrimary() {
        if (primary.automatic && primary.CheckFire()/* && primary.currentClip > 0*/)
        {
            primary.nextFireTime = primary.CalculateNextFireTime(primary.fireRate);
            FireBullet();
        }
        else if(fireRelease && primary.CheckFire()/* && primary.currentClip > 0*/)
        {
            fireRelease = false;
            primary.nextFireTime = primary.CalculateNextFireTime(primary.fireRate);
            FireBullet();
        }

        playerHUD.clipAmountText.text = primary.currentClip.ToString();
        playerHUD.ammoAmount.text = primary.currentAmmo.ToString();
    }

    void FireBullet() {
        playerNetwork.Shoot(networkView.owner, primary.damage);
        if (primary.currentClip > 0)
        {
            playerNetwork.Shoot(networkView.owner, primary.damage);
            primary.currentClip -= 1;
            if (primary.currentClip < 0)
            {
                StartCoroutine(Reload());
            }
        }
        else if (primary.currentAmmo > 0)
        {
            StartCoroutine(Reload());
        }
       
    }

    IEnumerator Reload() {
        print("RELOAD");
        float setReloadTime;
        setReloadTime = Time.time + primary.reloadTime;

        yield return new WaitForSeconds(primary.reloadTime);
        primary.currentClip = primary.clipSize;
    }

    void UpdateHUD(Weapon weapon) {

    }

    void UpdateHUD(Throwable throwable) {

    }

    void UpdateHUD(Equipment equipment){

    }

    public void Damage(float damage, NetworkPlayer networkPlayer) {
        networkView.RPC("_Damage", RPCMode.All, damage, networkPlayer);
    }

    [RPC]
    void _Damage(float damage, NetworkPlayer networkPlayer) {

    }


}
