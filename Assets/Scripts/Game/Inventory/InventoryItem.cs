using UnityEngine;
using System.Collections;

public class InventoryItem : MonoBehaviour {
    public static int staticItemID;
    public int itemID;
    public string name;
    public SlotType InventoryType;

    [HideInInspector]
    public float nextFireTime = 1000;

    void Start() {
        itemID = staticItemID;
        staticItemID++;
    }

    public void AssignToList(PlayerNetwork playerNetwork) {
        playerNetwork.weaponListID.Add(itemID, this);
        print("Assigned2 " + this + " with ID " + itemID);
    }

    public bool CheckFire() {
        return (Time.time > nextFireTime);
    }

    public float CalculateNextFireTime(float fireRate) {
        return Time.time + fireRate;
    }
}
