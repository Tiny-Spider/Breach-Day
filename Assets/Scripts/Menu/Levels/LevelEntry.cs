using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelEntry : MonoBehaviour {
    public Text title;
    public Image image;

    private GameObject panel;
    private LevelData levelData;

    public void SetPanel(GameObject panel) {
        this.panel = panel;
    }

	public void SetLevel(LevelData levelData) {
        this.levelData = levelData;

        title.text = levelData.levelName;
        image.sprite = levelData.levelImage;
	}

    public void OnSelect() {
        GameManager gameManager = GameManager.instance;

        gameManager.SetLevel(levelData.levelName);

        panel.SetActive(false);
    }
}
