using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NetworkView))]
public class PlayerNetwork : MonoBehaviour {
    public Component[] clientComponents;
    public GameObject[] clientGameobjects;
    public Transform head;

    public NetworkPlayer owner;
    public float lerpSpeed = 0.2F;
    public float maxMoveDistance = 1.0F;

    Vector3 realPosition;
    Quaternion realRotation;
    Quaternion realHeadRotation;

    public LayerMask rayLayerMask;
    public LayerMask playerLayer;

    public Dictionary<int, InventoryItem> weaponListID = new Dictionary<int, InventoryItem>();

    void Start() {
        realPosition = transform.position;
        realRotation = transform.rotation;
        realHeadRotation = head.rotation;
    }

    void FixedUpdate() {
        if (owner != null) {
            if (owner == Network.player) {
                if (Network.isClient) {
                    networkView.RPC("UpdatePosition", RPCMode.Server, transform.position, transform.rotation, head.rotation);
                }
                else {
                    realPosition = transform.position;
                    realRotation = transform.rotation;
                    realHeadRotation = head.rotation;
                }
            }

            // Don't update myself!
            else {
                transform.position = Vector3.Lerp(transform.position, realPosition, lerpSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, lerpSpeed);
                head.rotation = Quaternion.Lerp(head.rotation, realHeadRotation, lerpSpeed);
            }
        }
    }

    [RPC]
    void SetPlayer(NetworkPlayer player, NetworkMessageInfo info) {
        owner = player;
        transform.parent = FindObjectOfType<Level>().playerHolder;

        string name = NetworkManager.instance.connectedPlayers[player].name;
        transform.name = name;

        if (owner == null || owner != Network.player) {
            // Debug.Log("[" + name + " (" + player.ToString() + ")] Disabling Control!");

            foreach(Component component in clientComponents) {
               ((Behaviour) component).enabled = false;
            }

            foreach (GameObject gameObject in clientGameobjects) {
                gameObject.SetActive(false);
            }
        }
        else {
           // Debug.Log("[" + name + " (" + player.ToString() + ")] Enabling Control!");
        }
    }

    [RPC] /* Server Only */
    void UpdatePosition(Vector3 position, Quaternion rotation, Quaternion headRotation) {
        if (Network.isServer) {
            // He's cheating!
            if (Vector3.Distance(realPosition, position) > maxMoveDistance) {
                networkView.RPC("Teleport", owner, realPosition, realRotation, realHeadRotation);
            }
            else {
                realPosition = position;
                realRotation = rotation;
                realHeadRotation = headRotation;
            }
        }
    }

    [RPC]
    public void Teleport(Vector3 position, Quaternion rotation, Quaternion headRotation) {
        realPosition = position;
        realRotation = rotation;
        realHeadRotation = headRotation;

        transform.position = realPosition;
        transform.rotation = realRotation;
        head.rotation = realHeadRotation;
    }

    public void Shoot(NetworkPlayer networkPlayer, int weaponID) {
        print("Shoot");
        networkView.RPC("_Shoot", RPCMode.All,networkPlayer,weaponID);
    }

    [RPC]
    void _Shoot(NetworkPlayer networkPlayer, int weaponID) {
        print("Shooting with ID " + weaponID);
        print("Shot with : " + weaponListID[weaponID].name);
        Debug.DrawLine(head.position, head.forward * 100, Color.green, 10f);

        RaycastHit hit;
        if (Physics.Raycast(head.position, head.forward * 100 , out hit))
        {
            print(hit.transform.gameObject.name);
            if (hit.transform.gameObject.layer.Equals(playerLayer))
            {
                print("Hit player");
            }
            else if (hit.transform.gameObject.layer.Equals(rayLayerMask.value))
            {
                print("Hit one of the other stupid layers");
            }
        }
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        // === Server ===
        // This is only from server to client
        if (stream.isWriting) {
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;
            Quaternion headRot = head.rotation;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref headRot);
        }

        // === Client ===
        // This is only reciving from the server
        else {
            // Don't recive my own position
            if (owner != Network.player) {
                stream.Serialize(ref realPosition);
                stream.Serialize(ref realRotation);
                stream.Serialize(ref realHeadRotation);
            }
        }
    }
}
