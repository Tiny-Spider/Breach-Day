﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class PlayerNetwork : MonoBehaviour {
    public Camera camera;

    public NetworkPlayer owner;
    public float lerpSpeed = 0.2F;
    public float maxMoveDistance = 1.0F;

    public Vector3 realPosition;
    public Quaternion realRotation;

    void Start() {
        realPosition = transform.position;
        realRotation = transform.rotation;
    }

    void Update() {
        if (owner != null && owner == Network.player) {
            if (Network.isClient)
                networkView.RPC("UpdatePosition", RPCMode.Server, transform.position, transform.rotation);
        }

        // Don't update myself!
        else {
            transform.position = Vector3.Lerp(transform.position, realPosition, lerpSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, lerpSpeed);
        }
    }

    [RPC]
    void SetPlayer(NetworkPlayer player) {
        owner = player;
        transform.parent = FindObjectOfType<PlayerSpawner>().playerHolder;

        string name = NetworkManager.instance.connectedPlayers[player].name;
        transform.name = name;

        if (owner == null || owner != Network.player) {
            Debug.Log("[" + name + " (" + player.ToString() + ")] Disabling Control!");
            Destroy(GetComponent<PlayerController>());
            Destroy(GetComponent<CharacterController>());
            Destroy(camera.gameObject);
        }
        else {
            Debug.Log("[" + name + " (" + player.ToString() + ")] Enabling Control!");
        }
    }

    [RPC] /* Server Only */
    void UpdatePosition(Vector3 position, Quaternion rotation) {
        if (Network.isServer) {
            // He's cheating!
            if (Vector3.Distance(realPosition, position) > maxMoveDistance) {
                networkView.RPC("Teleport", owner, realPosition, realRotation);
            }
            else {
                realPosition = position;
                realRotation = rotation;
            }
        }
    }

    [RPC]
    public void Teleport(Vector3 position, Quaternion rotation) {
        realPosition = position;
        realRotation = rotation;

        transform.position = realPosition;
        transform.rotation = realRotation;
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        // === Server ===
        // This is only from server to client
        if (stream.isWriting) {
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
        }

        // === Client ===
        // This is only reciving from the server
        else {
            // Don't recive my own position
            if (owner != Network.player) {
                stream.Serialize(ref realPosition);
                stream.Serialize(ref realRotation);
            }
        }
    }
}