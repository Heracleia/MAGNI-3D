using UnityEngine;
using System.Collections;

public class MainMenuEvents : MonoBehaviour {

	public int screenWidth = 3840;
	public int screenHeight = 1000;

	void Start() {
		Screen.SetResolution (screenWidth, screenHeight, false);
	}

	public void calibrateButton () {
		Application.LoadLevel ("CalibrationPatient");
	}

	public void playButton (GameObject CalibrateMessage) {
		if (PlayerPrefs.GetInt ("Calibrated") == 1) {
			Application.LoadLevel ("SelectPatientForGame");
		} else {
			CalibrateMessage.SetActive(true);
		}
	}

	public void recordButton (GameObject CalibrateMessage) {
		if (PlayerPrefs.GetInt ("Calibrated") == 1) {
			Application.LoadLevel ("RecordGesture");
		} else {
			CalibrateMessage.SetActive(true);
		}
	}

	public void dataButton () {
		Application.LoadLevel("Magni");
	}

	public void quitButton () {
		PlayerPrefs.SetInt ("Calibrated", 0);
		Application.Quit ();
	}

	public void calibrateWarningOKButton (GameObject CalibrateMessage) {
		CalibrateMessage.SetActive(false);
	}
}
