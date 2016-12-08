using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Threading.Tasks;
using System;

public class CalibrationData : MonoBehaviour {

	public bool	isTherapistCalibration;

	public Text PatientTextBox;
	public Text TherapistTextBox;
	public Image patientFrontImage;
	public Image patientSideImage;
	public Image therapistFrontImage;
	public Image therapistSideImage;

	public Sprite[] frontImages = new Sprite[7];
	public Sprite[] sideImages = new Sprite[7];

	float[] maxPosition = new float[9]; //Bottom, Top, Left, Right, Back, Forward
	int		calibrationStep = 0;
	bool	nextStep;

	TcpClient clientSocket;
	string dataFromClient;
	string[] coordinates;
	Vector3 Barrett;

	float	timePassed;
	bool	calibrationStarted;

	void Start() {
		clientSocket = new System.Net.Sockets.TcpClient();
		clientSocket.Connect("192.168.1.2", 13131);
		PlayerPrefs.SetInt ("Calibrated", 0);
	}

	void Update() {
		
		// Server should receive a message from the client
		NetworkStream networkStream = clientSocket.GetStream();
		byte[] bytesFrom = new byte[100000];
		networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
		dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
		dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("\n"));
		
		char[] delimiterChars = { ' ', ',', '\n' };
		coordinates = dataFromClient.Split(delimiterChars);
		//ToDo - Socket info from the Barrett Arm
		double xx = Convert.ToDouble(coordinates[0]);
		double yy = Convert.ToDouble(coordinates[1]);
		double zz = Convert.ToDouble(coordinates[2]);
		
		/*
         * Before tranformations
        Barrett.x = (float) yy * 10;
        Barrett.y = (float) zz * 10;
        Barrett.z = (float) xx * (-10);
        */
		
		Barrett.x = (float)xx * (-10);
		Barrett.y = (float)zz * 10;
		Barrett.z = (float)yy * (-10);

		if (isTherapistCalibration && calibrationStarted) {
			timePassed += Time.deltaTime;
			if(calibrationStep == 1 && timePassed > 2) {
				calibrationStateUpdate ();
				timePassed = 0;
			}
			if(calibrationStep > 1 && timePassed > 2) {
				calibrationStateUpdate ();
				timePassed = 0;
			}
		}
	}
	public void TherapistButtonPress () {
		if (isTherapistCalibration && calibrationStep == 0) {
			calibrationStateUpdate();
		}
		if (!isTherapistCalibration) {
			calibrationStateUpdate();
		}
	}
	
	public void calibrationStateUpdate () {
		switch (calibrationStep) {
		case 0:
			maxPosition [6] = Barrett.x;
			maxPosition [7] = Barrett.y;
			maxPosition [8] = Barrett.z;
			PatientTextBox.text = "Move all the way UP!";
			TherapistTextBox.text = "Click when arm is at max UP position";
			setImages (calibrationStep);
			timePassed = 0;
			calibrationStarted = true;
			break;
		case 1:
			maxPosition [1] = Barrett.y;
			PatientTextBox.text = "Move all the way DOWN!";
			TherapistTextBox.text = "Click when arm is at max DOWN position";
			setImages (calibrationStep);
			break;
		case 2:
			maxPosition [0] = Barrett.y;
			PatientTextBox.text = "All the way to the LEFT!";
			TherapistTextBox.text = "Click when arm is at max LEFT";
			setImages (calibrationStep);
			break;
		case 3:
			maxPosition [2] = Barrett.x;
			PatientTextBox.text = "And to the RIGHT!";
			TherapistTextBox.text = "Click when arm is at max RIGHT";
			setImages (calibrationStep);
			break;
		case 4:
			maxPosition [3] = Barrett.x;
			PatientTextBox.text = "All the way FORWARD!";
			TherapistTextBox.text = "Click when arm is at max FORWARD";
			setImages (calibrationStep);
			break;
		case 5:
			maxPosition [5] = Barrett.z;
			PatientTextBox.text = "And all the way BACK!";
			TherapistTextBox.text = "Click when arm is at max BACK";
			setImages (calibrationStep);
			break;
		case 6:
			maxPosition [4] = Barrett.z;
			PatientTextBox.text = "Nicely done!  Get ready to play!";
			TherapistTextBox.text = "Calibration complete.  Click to return to main menu.";
			setImages (calibrationStep);
			break;
		case 7:
			calibrate ();
			Application.LoadLevel ("MainMenu");
			break;
		default:
			PatientTextBox.text = "If you see this text, something has gone wrong...";
			TherapistTextBox.text = "Something has gone wrong...";
			break;
		}
		calibrationStep ++;
	}

	void setImages (int imageNum) {
		patientFrontImage.sprite = frontImages[imageNum];
		patientSideImage.sprite = sideImages[imageNum];
		therapistFrontImage.sprite = frontImages[imageNum];
		therapistSideImage.sprite = sideImages[imageNum];
	}

	void calibrate () {
		PlayerPrefs.SetFloat ("HydraDown", maxPosition [0]);
		PlayerPrefs.SetFloat ("HydraLeft", maxPosition[2]);
		PlayerPrefs.SetFloat ("HydraBack", maxPosition [4]);
		PlayerPrefs.SetFloat ("HydraUp", maxPosition [1]);
		PlayerPrefs.SetFloat ("HydraRight", maxPosition [3]);
		PlayerPrefs.SetFloat ("HydraFront", maxPosition [5]);
		PlayerPrefs.SetFloat ("NeutralX", maxPosition [6]);
		PlayerPrefs.SetFloat ("NeutralY", maxPosition [7]);
		PlayerPrefs.SetFloat ("NeutralZ", maxPosition [8]);
		PlayerPrefs.SetInt ("Calibrated", 1);

		Debug.Log(PlayerPrefs.GetFloat("HydraDown"));
		Debug.Log(PlayerPrefs.GetFloat("HydraLeft"));
		Debug.Log(PlayerPrefs.GetFloat("HydraBack"));
		Debug.Log(PlayerPrefs.GetFloat("HydraUp"));
		Debug.Log(PlayerPrefs.GetFloat("HydraRight"));
		Debug.Log(PlayerPrefs.GetFloat("HydraFront"));
		Debug.Log(PlayerPrefs.GetFloat("NeutralX"));
		Debug.Log(PlayerPrefs.GetFloat("NeutralY"));
		Debug.Log(PlayerPrefs.GetFloat("NeutralZ"));
		Debug.Log(PlayerPrefs.GetFloat("Calibrated"));
	}

	/** returns true if a controller is enabled and not docked */
	bool IsControllerActive( SixenseInput.Controller controller ) {
		return ( controller != null && controller.Enabled && !controller.Docked );
	}
}
