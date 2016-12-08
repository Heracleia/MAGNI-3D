using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System;

public class ExistingExercise : MonoBehaviour, IDropHandler {

	public string	baseFilePath = "C:\\Users\\Public\\Exercise\\";
	public string exerciseName;
	public GameObject existingExercisePrefab;

	void Start() {
		string filePath = baseFilePath + exerciseName + ".png";
		try {
			if(File.Exists(filePath)) {
				byte[] bytes = File.ReadAllBytes(filePath);
				Texture2D tex = new Texture2D(1,1);
				tex.LoadImage(bytes);
				Sprite sprite = Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f),1.0f);
				GetComponent<Image>().sprite = sprite;
			}
		} catch (Exception e) {
			Debug.Log ("File Exception: " + e.ToString());
		}
	}

	public void playlistExerciseInitialization(string exerciseType) {
		exerciseName = exerciseType;

		string filePath = baseFilePath + exerciseName + ".png";
		try {
			if(File.Exists(filePath)) {
				byte[] bytes = File.ReadAllBytes(filePath);
				Texture2D tex = new Texture2D(1,1);
				tex.LoadImage(bytes);
				Sprite sprite = Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f),1.0f);
				GetComponent<Image>().sprite = sprite;
			}
		} catch (Exception e) {
			Debug.Log ("File Exception: " + e.ToString());
		}
	}

	public void MouseEntered() {
		if (transform.GetSiblingIndex () > transform.parent.GetComponent<PlaylistContentControl> ().currentExercise) {
			transform.GetChild (0).gameObject.SetActive (true);
		}
		if (transform.GetSiblingIndex () >= transform.parent.GetComponent<PlaylistContentControl> ().currentExercise) {
			transform.GetChild (2).gameObject.SetActive (true);
		}
	}

	public void MouseLeft() {
		transform.GetChild(0).gameObject.SetActive (false);
		transform.GetChild(2).gameObject.SetActive (false);
	}

	public void ShiftExerciseRight() {
		if (transform.GetSiblingIndex() > transform.parent.GetComponent<PlaylistContentControl> ().currentExercise) {
			transform.SetSiblingIndex (transform.GetSiblingIndex () + 1);
		}
	}

	public void ShiftExerciseLeft() {
		if (transform.GetSiblingIndex() > transform.parent.GetComponent<PlaylistContentControl> ().currentExercise + 1) {
			transform.SetSiblingIndex (transform.GetSiblingIndex () - 1);
		}
	}

	public void DeleteExercise() {
		if (transform.GetSiblingIndex() > transform.parent.GetComponent<PlaylistContentControl> ().currentExercise) {
			Destroy(gameObject);
		}
	}

	public void DuplicateExercise() {
		if (transform.GetSiblingIndex() >= transform.parent.GetComponent<PlaylistContentControl> ().currentExercise) {
			GameObject dupeExercise = Instantiate(existingExercisePrefab, Vector3.one, Quaternion.identity) as GameObject;
			dupeExercise.transform.SetParent(transform.parent);
			dupeExercise.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
			dupeExercise.GetComponent<ExistingExercise>().playlistExerciseInitialization(exerciseName);
			dupeExercise.transform.GetChild(1).gameObject.SetActive(false);
		}
	}

	public void OnDrop(PointerEventData data) {
		if (data.pointerDrag.tag == "potentialExercise") {
			string dropExerciseType = data.pointerDrag.GetComponent<NewExerciseDrag> ().exerciseName;
			if (transform.GetSiblingIndex() >= transform.parent.GetComponent<PlaylistContentControl> ().currentExercise) {
				GameObject dupeExercise = Instantiate(existingExercisePrefab, Vector3.one, Quaternion.identity) as GameObject;
				dupeExercise.transform.SetParent(transform.parent);
				dupeExercise.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
				dupeExercise.GetComponent<ExistingExercise>().playlistExerciseInitialization(dropExerciseType);
				dupeExercise.transform.GetChild(1).gameObject.SetActive(false);
			}
		}
	}
}
