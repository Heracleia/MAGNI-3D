using UnityEngine;
using System.Collections;
using System.IO;
using System;
using MySql.Data.MySqlClient;

public class GestureContentControl : MonoBehaviour {

	public GameObject existingExercisePrefab;
	public string	baseFilePath = "C:\\Users\\Public\\Exercise\\";
	//char[] delimiterChars = { '\\', '.' };

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

	// Use this for initialization
	void Start () {
		try {
			cmd = new MySqlCommand("Select * from magni_exercise", con);
			rdr = cmd.ExecuteReader();
			if(rdr.HasRows) {
				while(rdr.Read()) {
					GameObject potentialExercise = Instantiate(existingExercisePrefab, Vector3.one, Quaternion.identity) as GameObject;
					potentialExercise.transform.SetParent(transform);
					potentialExercise.transform.SetAsLastSibling();
					potentialExercise.GetComponent<NewExerciseDrag>().ExerciseInitialization(rdr.GetString(1));
				}
				rdr.Close();
			} else { //No Rows - probably a new table
				rdr.Close();
				Debug.Log("No Exercises!");
			}
		}
		catch(Exception e) {
			Debug.Log(e.ToString());
		}
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
}
