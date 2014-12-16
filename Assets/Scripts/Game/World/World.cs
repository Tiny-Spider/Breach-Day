using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {
    public static World instance { private set; get; }

    public List<Door> doors;
    public List<Wall> walls;

    public Transform playerHolder;

    public Dictionary<NetworkPlayer, Player> players = new Dictionary<NetworkPlayer, Player>();

    void Start() {
        WorldSettings worldSettings = GameManager.instance.worldSettings;

        loadEnvironment(worldSettings);
    }

    private void loadEnvironment(WorldSettings worldSettings) {
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
}
