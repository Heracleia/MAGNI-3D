using UnityEngine;
using System.Collections;

public class PlaylistContentControl : MonoBehaviour {

	public int currentExercise = 0;

	void Start () {
		transform.GetChild (0).GetChild (1).gameObject.SetActive (true);
	}

	public void nextExerciseStart () {
		transform.GetChild (currentExercise++).GetChild (1).gameObject.SetActive (false);
		transform.GetChild (currentExercise).GetChild (1).gameObject.SetActive (true);
	}
}
