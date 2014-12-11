using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {
    public List<Door> doors;
    public List<Wall> walls;

    public Dictionary<NetworkPlayer, Player> players = new Dictionary<NetworkPlayer, Player>();

    void Start() {
        loadEnvironment();
        spawnPlayer();
    }

    private void loadEnvironment() {
        WorldSettings worldSettings = GameManager.instance.worldSettings;

        // Clamp just to make sure you don't go over max
        int doorAmount = Mathf.Clamp(worldSettings.doors, 0, doors.Count);
        int wallAmount = Mathf.Clamp(worldSettings.walls, 0, walls.Count);

        doors.Shuffle();
        walls.Shuffle();

        for (int i = 0; i < doors.Count; i++) {
            doors[i].SetActive(i < doorAmount);
        }

        for (int i = 0; i < walls.Count; i++) {
            walls[i].SetActive(i < wallAmount);
        }
    }

    private void spawnPlayer() {

    }
}
