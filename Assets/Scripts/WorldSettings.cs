using UnityEngine;
using System.Collections;

public class WorldSettings {
    public string level;
    public bool day;
    public int doors;
    public int walls;

    public WorldSettings(string level, bool day, int doors, int walls) {
        this.level = level;
        this.day = day;
        this.doors = doors;
        this.walls = walls;
        
    }
}
