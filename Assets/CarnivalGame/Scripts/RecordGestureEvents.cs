using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using MySql.Data.MySqlClient;

public class RecordGestureEvents : MonoBehaviour {
	public string	baseFilePath = "C:\\Users\\Public\\Exercise\\";
	public GameObject depthPlane;
	public float	photoTotalTime = 1;
	public GameObject therapistCamera;

	List<Vector3> coordinates;
	bool recording;

	int exerciseCount;
	Vector3 pointScale = new Vector3(0.1f,0.1f,0.1f);

	MySqlConnection con = null;
	MySqlCommand cmd = null;
	MySqlDataReader rdr = null;

	void Awake()
	{
		try
		{
			string server = "127.0.0.1";
			string database = "magni_db";
			string uid = "root";
			string mySQLpassword = "magnidb1";
			string constr = 
				"SERVER=" + server + ";" + 
					"DATABASE=" + database + ";" + 
					"PORT=" + "3306" + ";" + 
					"UID=" + uid + ";" + 
					"PASSWORD=" + mySQLpassword + ";";
			con = new MySqlConnection(constr);
			Debug.Log("Opening DB Connection");
			con.Open();
			Debug.Log("Connection state: " + con.State);
		}
		catch(Exception e)
		{
			Debug.Log("Error: " + e.ToString());
		}
	}

	void Update() {
		if (recording) {
			coordinates.Add(transform.position);
		}
	}
	
	public void startRecordButton () {
		coordinates = new List<Vector3> ();
		recording = true;
	}


	public void endRecordButton () {
		recording = false;

		exerciseCount = findLastExerciseNum ();
		AddExerciseToDB ();

		string file_name = baseFilePath + exerciseCount + ".csv";
		FileStream fcreate = File.Open(file_name, FileMode.Create); // will create the file or overwrite it if it already exists
		StreamWriter sw = new StreamWriter(fcreate);
		
		foreach (Vector3 value in coordinates) {
			if (value.x < 10 && value.x > -10 && value.x < 10 && value.x > -10 && value.z < 0 && value.z > -10)
			{
				sw.WriteLine(value.x + ", " + value.y + ", " + value.z);
			}
		}
		
		sw.Close();

		List<GameObject> points = new List<GameObject> ();
		int pointNum = 0;
		foreach(Vector3 coords in coordinates) {
			points.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere));
			points[pointNum].transform.position = coords;
			points[pointNum++].transform.localScale = pointScale;
		}

		gameObject.GetComponent<MeshRenderer> ().enabled = false;
		depthPlane.GetComponent<MeshRenderer> ().enabled = false;
		therapistCamera.GetComponent<Camera> ().orthographic = true;

		Invoke ("makeHandVisible", photoTotalTime);
		foreach (GameObject point in points) {
			Destroy(point,photoTotalTime);
		}
		points.Clear ();
		StartCoroutine ("takeScreenshot");
	}

	public void mainMenuButton() {
		Application.LoadLevel ("MainMenu");
	}

	private void makeHandVisible() {
		gameObject.GetComponent<MeshRenderer> ().enabled = true;
		depthPlane.GetComponent<MeshRenderer> ().enabled = true;
		therapistCamera.GetComponent<Camera> ().orthographic = false;
	}

	private IEnumerator takeScreenshot() {
		Debug.Log ("beforeYield");
		yield return new WaitForEndOfFrame();
		Debug.Log ("afterYield");

		Camera cam = therapistCamera.GetComponent<Camera> ();
		int picLeft = Convert.ToInt32(cam.pixelWidth * 0.295f + cam.pixelRect.x);
		int picWidth = Convert.ToInt32(cam.pixelWidth * 0.41f);
		int picBottom = Convert.ToInt32(cam.pixelHeight * 0.215f + cam.pixelRect.y);
		int picHeight = Convert.ToInt32(cam.pixelHeight * 0.525f);

		Debug.Log (picLeft + " , " + picWidth + " , " + picBottom + " , " + picHeight);
		//Debug.Log(therapistCamera.GetComponent<Camera>().pixelRect);

		Texture2D tex = new Texture2D (picWidth, picHeight);
		tex.ReadPixels (new Rect (picLeft, picBottom, picWidth, picHeight), 0, 0);
		tex.Apply ();

		byte[] bytes = tex.EncodeToPNG ();
		Destroy (tex);
		File.WriteAllBytes (baseFilePath + exerciseCount + ".png", bytes);
	}

	private int findLastExerciseNum () {
		try {
			cmd = new MySqlCommand("Select * from magni_exercise", con);
			rdr = cmd.ExecuteReader();
			if(rdr.HasRows) {
				int exerciseNum = 0;
				while(rdr.Read()) {
					exerciseNum = Convert.ToInt32(rdr.GetString(1)); //Field: ExerciseName
				}
				rdr.Close();
				return ++exerciseNum; //New Exercise ID should be the one after the last one.
			} else { //No Rows - probably a new table
				rdr.Close();
				return 0;
			}
		}
		catch(Exception e) {
			Debug.Log(e.ToString());
		}
		return -1; //Error with accessing the database
	}
	
	public void AddExerciseToDB()
	{
		try
		{
			string query = "INSERT INTO magni_exercise (exerciseName, graphName) VALUES (?exerciseName, ?graphName)";
			cmd = new MySqlCommand(query,con);
			MySqlParameter param1 = cmd.Parameters.Add("?exerciseName", MySqlDbType.VarChar);
			param1.Value = exerciseCount.ToString();
			MySqlParameter param2 = cmd.Parameters.Add("?graphName", MySqlDbType.VarChar);
			param2.Value = "";
			cmd.ExecuteNonQuery();
		}
		catch(Exception e)
		{
			Debug.Log(e.ToString());
		}
	}
}
