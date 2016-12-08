using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;

public class NewExerciseDrag : MonoBehaviour {

	public string	baseFilePath = "C:\\Users\\Public\\Exercise\\";
	public string exerciseName;
	GameObject draggableImage;

	public void ExerciseInitialization(string exerciseType) {
		exerciseName = exerciseType;

		string filePath = baseFilePath + exerciseType + ".png";
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

	public void StartDrag() {
		draggableImage = new GameObject ("icon");
		draggableImage.transform.parent = transform.parent.parent.parent;
		Image icon = draggableImage.AddComponent<Image> ();
		icon.sprite = GetComponent<Image> ().sprite;
		CanvasGroup group = draggableImage.AddComponent<CanvasGroup> ();
		group.blocksRaycasts = false;

	}

	public void Dragging() {
		draggableImage.transform.position = Input.mousePosition;
	}

	public void EndDrag() {
		Destroy (draggableImage);
	}

	public void MouseEntered() {
		transform.GetChild (0).gameObject.SetActive (true);
	}
	
	public void MouseLeft() {
		transform.GetChild(0).gameObject.SetActive (false);
	}
}
