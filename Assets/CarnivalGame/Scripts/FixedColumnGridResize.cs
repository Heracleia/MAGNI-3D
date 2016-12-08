using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FixedColumnGridResize : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GridLayoutGroup grid = GetComponent<GridLayoutGroup> ();
		grid.cellSize = Vector2.one * Mathf.RoundToInt(GetComponent<RectTransform>().rect.width) / grid.constraintCount;
	}
}
