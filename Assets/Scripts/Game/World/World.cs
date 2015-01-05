using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {
    public static World instance { private set; get; }

    private List<Door> doors;
    private List<Wall> walls;

    public Transform playerHolder;

    void Awake() {
        instance = this;
    }

    void Start() {
        loadEnvironment();
    }

    private void loadEnvironment() {
        GameManager gameManager = GameManager.instance;

        doors = new List<Door>(GameObject.FindObjectsOfType<Door>());
        walls = new List<Wall>(GameObject.FindObjectsOfType<Wall>());

        // Clamp just to make sure you don't go over max
        int doorAmount = Mathf.Clamp(gameManager.levelDoors, 0, doors.Count);
        int wallAmount = Mathf.Clamp(gameManager.levelWalls, 0, walls.Count);

        doors.Shuffle();
        walls.Shuffle();

        for (int i = 0; i < doors.Count; i++) {
            doors[i].SetDoorActive(i < doorAmount);
        }

        for (int i = 0; i < walls.Count; i++) {
            walls[i].SetWallActive(i < wallAmount);
        }
    }
}
