using UnityEngine;
using System.Collections;
using UnityEngine.UI;


[ExecuteInEditMode]
public class GridFitter : MonoBehaviour {

    [SerializeField]
    GridLayoutGroup gridLayoutGroup;
    RectTransform rectTransform;
    Vector2 temp;

    [SerializeField] float padding;

	void Update () {
        rectTransform = GetComponent<RectTransform>();
        temp = gridLayoutGroup.cellSize;
        temp.x = Screen.width - padding;
        gridLayoutGroup.cellSize = temp;
	}
}
