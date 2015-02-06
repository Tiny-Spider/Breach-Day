using UnityEngine;
using System.Collections;

public class LevelSettings {
    private const char splitChar_1 = '|';
    private const int paramCount = 4;

    public string levelName = LevelManager.instance.GetDefault().levelName;
    public double time = 15.0;

    public int doors = 10;
    public int walls = 10;

    public string ToDataString() {
        return levelName + splitChar_1 + time + splitChar_1 + doors + splitChar_1 + walls;
    }

    public static LevelSettings FromDataString(string data) {
        LevelSettings levelSettings = new LevelSettings();

        string[] splitData = data.Split(splitChar_1);

        double doubleValue;
        int intValue;

        if (splitData.Length == paramCount) {
            levelSettings.levelName = splitData[0];

            if (double.TryParse(splitData[1], out doubleValue)) {
                levelSettings.time = doubleValue;
            }

            if (int.TryParse(splitData[2], out intValue)) {
                levelSettings.doors = intValue;
            }

            if (int.TryParse(splitData[3], out intValue)) {
                levelSettings.walls = intValue;
            }

            return levelSettings;
        }

        return null;
    }
}
