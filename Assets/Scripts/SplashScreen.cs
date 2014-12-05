using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplashScreen : MonoBehaviour {
    public string sceneToLoad = "Menu";

    public Text title;
    public float titleTime;

    public Text subtitle;
    public float subtitleTime;

    public float splashTime;
    
	void Start () {
        StartCoroutine(PlayAnimation());
	}

    // TODO: Mechanim
    IEnumerator PlayAnimation() {
        float time = 0;

        while (time <= titleTime) {
            time += Time.deltaTime;

            float percentage = (titleTime / 1) * time;
            Color color = title.color;
            color.a = percentage;
            title.color = color;

            yield return new WaitForEndOfFrame();
        }

        time = 0;

        while (time <= subtitleTime) {
            time += Time.deltaTime;

            float percentage = (subtitleTime / 1) * time;

            Color color = subtitle.color;
            color.a = percentage;
            subtitle.color = color;

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(splashTime);

        Application.LoadLevel(sceneToLoad);
    }
}
