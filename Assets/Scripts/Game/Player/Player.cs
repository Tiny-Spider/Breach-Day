using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/* To do:
 * Reloading with R// Fix for switching during reload.
 * hit other players.
 * Drop weapons.
 * Pick up weapons.
 * assign weapons to specific slots.
 * 
 * */
public class Player : MonoBehaviour {
    public int health;

    public SlotType selectedSlot;
    public Weapon primary;
    public Weapon secondary;
    public Throwable throwable;
    public Equipment equipment;
    public float throwPower;

    [HideInInspector]
    public bool fireRelease;
    [HideInInspector]
    public PlayerHUD playerHUD;

    private PlayerNetwork playerNetwork;
    private Dictionary<AmmoType, int> ammo = new Dictionary<AmmoType, int>();
    private Dictionary<SlotType, InventoryItem> inventoryItemSlotType = new Dictionary<SlotType, InventoryItem>();

    private bool reloading;

    void Start() {
        primary = GetComponentInChildren<Weapon>();
        playerNetwork = GetComponent<PlayerNetwork>();
        ChangeSelectedSlot(SlotType.Primary);
        UpdateHUD();
        UpdateWeaponListID();
        AwakeInitializeItems();
        print("Primary weapon ID " + primary.itemID);
    }

    public void ChangeSelectedSlot(SlotType type) {
        selectedSlot = type;
        switch (type)
        {
            case SlotType.Primary:
                primary.nextFireTime = Time.time + primary.pullOutTime;
                break;
            case SlotType.Secondary:
                secondary.nextFireTime = Time.time + secondary.pullOutTime;
                break;
            case SlotType.Throwable:
                throwable.nextFireTime = Time.time + secondary.pullOutTime;
                break;
            case SlotType.Equipment:
                secondary.nextFireTime = Time.time + secondary.pullOutTime;
                break;

        }
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
            FireBullet(primary);
        }
        else if(fireRelease && primary.CheckFire()/* && primary.currentClip > 0*/)
        {
            fireRelease = false;
            FireBullet(primary);
        }

        UpdateHUD();
    }

    void FireBullet(Weapon weapon) {
        if (weapon.currentClip > 0 && !reloading)
        {
            weapon.nextFireTime = weapon.CalculateNextFireTime(weapon.fireRate);
            playerNetwork.Shoot(networkView.owner, weapon.itemID);
            weapon.currentClip -= 1;
            if (weapon.currentClip <= 0 && weapon.currentAmmo > 0)
            {
                StartCoroutine(Reload(weapon));
            }
        }
        else if (weapon.currentAmmo > 0 && !reloading)
        {
            print("Else if");
            StartCoroutine(Reload(weapon));
        }
       
    }

   public IEnumerator Reload(Weapon weapon) {
        print("RELOAD");
        reloading = true;
        float setReloadTime;
        setReloadTime = Time.time + weapon.reloadTime;

        yield return new WaitForSeconds(weapon.reloadTime);
        if (weapon.clipSize > weapon.currentAmmo)
        {
            weapon.currentClip = weapon.currentAmmo;
            weapon.currentAmmo -= weapon.currentAmmo;
        }
        else
        {
            weapon.currentClip = weapon.clipSize;
            weapon.currentAmmo -= weapon.clipSize;
        }
        UpdateHUD();
        reloading = false; 
    }

   void AwakeInitializeItems() {
       if (primary)
       {
           InitializeItem(primary);
       }
       if (secondary)
       {
           InitializeItem(secondary);
       }
       if (throwable)
       {
           InitializeItem(throwable);
       }
       if (equipment)
       {
           InitializeItem(equipment);
       }

       foreach (KeyValuePair<SlotType,InventoryItem> keyPair in inventoryItemSlotType) { 
           print("Item dictionary: Key("+keyPair.Key+") Value("+keyPair.Value+")");
       }

   }
    //Could shorten the code below. I was thinking of a way how to do this and I can remember I needed to seperate the
    //types for a reason, but can't remember what. Keeping this here for now.
   void InitializeItem(InventoryItem item)   {
       if (typeof(Weapon) == item.GetType())
       {
           inventoryItemSlotType.Add(item.InventoryType, item);
           print("Add Weapon to dictionary: " + item.gameObject.name.ToString());
           item.itemID = InventoryItem.staticItemID;
           InventoryItem.staticItemID++;
       }
       if (typeof(Throwable) == item.GetType())
       {
           inventoryItemSlotType.Add(item.InventoryType, item);
       }
       if (typeof(Equipment) == item.GetType())
       {
           inventoryItemSlotType.Add(item.InventoryType, item);
       }
       print("Set kinematic true");
       item.rigidbody.isKinematic = true;
   }

   public void DropItem() {
       InventoryItem tempItem;
       tempItem = inventoryItemSlotType[selectedSlot];
       tempItem.rigidbody.isKinematic = false;
       //tempItem.gameObject.layer = 
       print("tempItem = " + tempItem);
       tempItem.rigidbody.AddForce(Vector3.up * throwPower, ForceMode.Impulse);
       inventoryItemSlotType[selectedSlot] = null;
       tempItem.transform.parent = null;
   }

    void UpdateHUD() {
        playerHUD.clipAmount.text = primary.currentClip.ToString();
        playerHUD.ammoAmount.text = primary.currentAmmo.ToString();
    }

    void UpdateHUD(InventoryItem item) {

    }

    void UpdateWeaponListID() {
        primary.AssignToList(playerNetwork);
    }

    public void Damage(float damage, NetworkPlayer networkPlayer) {
        networkView.RPC("_Damage", RPCMode.All, damage, networkPlayer);
    }

    [RPC]
    void _Damage(float damage, NetworkPlayer networkPlayer) {

    }

    


}
