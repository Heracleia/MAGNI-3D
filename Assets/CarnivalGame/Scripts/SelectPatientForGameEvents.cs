using UnityEngine;
using UnityEngine.UI;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic; 
using System.Text.RegularExpressions;

public class SelectPatientForGameEvents : MonoBehaviour {

	// connection object
	MySqlConnection con = null;
	// command object
	MySqlCommand cmd = null;
	MySqlDataReader rdr = null;
	// reader object
	//	MySqlDataReader rdr = null;
	// object collection array

	public struct patient
	{
		public string patientId, givenName, lastName;
		public string middleName, suffix, prefix;
		public string address, city, state;
		public string MRN;
		public string DOB, gender, email;
		public string phone;
		public string bloodgroup, injuryName, injuryDate;
		public string injurySeverity, injuryComments, recordCreate_TS;
		public string recordUpdate_TS, familyContactName, familyContactPhone;
		public string allowSharingInfo, commentText;
	}
	public struct therapist
	{
		public string therapistId, givenName, lastName;
		public string middleName, email, phone;
		public string suffix, hospitalName, DOB;
		public string gender, jobTitle, speciality;
		public string permissionNumber, username;
	}
	public struct therapist_paitent
	{
		public string ID, therapistID, patientID;
	}
	public struct session
	{
		public string sessionID, exerciseID;// playlistID;
		public string graphName, resistance, time;
		public string time_UOM, targets_hit, total_targets;
		public string score;
	}
	//	public struct playlist
	//	{
	//		public string playlistID, exerciseID;
	//	}
	public struct encounter
	{
		public string encounterID, patientID, sessionID;
		public string therapistID, height, height_UOM;
		public string weight, weight_UOM, systolic_BP;
		public string diastolic_BP, percentError;
	}
	public struct exercise
	{
		public string exerciseID, exerciseName, exerciseLevel;
		public string graphName;
	}
	
	public patient current_patient = new patient();
	public string id1, id2, id3, id4, id5, id6;
	public List<patient> patientList = new List<patient>();
	public List<exercise> exerciseList = new List<exercise>();
	public List<session> sessionList = new List<session>();
	public int patientListReference = 0;
	public int exerciseListReference = 0;
	public int sessionListReference = 0;
	//List<UnityEngine.UI.Button> dynamicButtons = new List<UnityEngine.UI.Button>();
	// Use this for initialization

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

	public void mainMenuNavigation()
	{
		Application.LoadLevel ("MainMenu");
	}

	public void PatientSearch()
	{
		GameObject go = GameObject.Find("InputFieldText_PatientSearch_PatientName");
		Text namefield = (Text) go.GetComponent(typeof(Text));
		//go = GameObject.Find ("Canvas_PatientSearch");
		string name = namefield.text;
		
		patientList.Clear();
		patientListReference = 0;
		Text buttontext;
		id1 = id2 = id3 = id4 = id5 = id6 = "";
		go = GameObject.Find("Text_PatientSearch_Patient1");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Patient";
		go = GameObject.Find("Text_PatientSearch_Patient2");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Patient";
		go = GameObject.Find("Text_PatientSearch_Patient3");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Patient";
		go = GameObject.Find("Text_PatientSearch_Patient4");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Patient";
		go = GameObject.Find("Text_PatientSearch_Patient5");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Patient";
		go = GameObject.Find("Text_PatientSearch_Patient6");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Patient";
		
		if(name.Equals(""))
		{
			return;
		}
		
		try
		{
			cmd = new MySqlCommand("Select * from magni_patient where lastName like '"+ name + "%' order by lastName", con);
			rdr = cmd.ExecuteReader();
			if(rdr.HasRows)
			{
				//Sint i = 1;
				while(rdr.Read())// && i <= 6)
				{
					patient item = new patient();
					item.patientId = rdr["patientId"].ToString();
					item.givenName = rdr["givenName"].ToString();
					item.lastName = rdr["lastName"].ToString();
					item.DOB = rdr["DOB"].ToString().Substring(0,10);
					patientList.Add(item);
					Debug.Log(patientList.Count);
				}
				for(int i = 0; (i < 6) && (i < patientList.Count); i++)
				{
					switch (i)
					{
					case 0:
						id1 = patientList[0].patientId;
						go = GameObject.Find("Text_PatientSearch_Patient1");
						buttontext = (Text) go.GetComponent(typeof(Text));
						buttontext.text = patientList[0].lastName + "," + patientList[0].givenName + ", " + patientList[0].DOB;
						break;
					case 1:
						id2 = patientList[1].patientId;
						go = GameObject.Find("Text_PatientSearch_Patient2");
						buttontext = (Text) go.GetComponent(typeof(Text));
						buttontext.text = patientList[1].lastName + "," + patientList[1].givenName + ", " + patientList[1].DOB;;
						break;
					case 2:
						id3 = patientList[2].patientId;
						go = GameObject.Find("Text_PatientSearch_Patient3");
						buttontext = (Text) go.GetComponent(typeof(Text));
						buttontext.text = patientList[2].lastName + "," + patientList[2].givenName + ", " + patientList[2].DOB;;
						break;
					case 3:
						id4 = patientList[3].patientId;
						go = GameObject.Find("Text_PatientSearch_Patient4");
						buttontext = (Text) go.GetComponent(typeof(Text));
						buttontext.text = patientList[3].lastName + "," + patientList[3].givenName + ", " + patientList[3].DOB;;
						break;
					case 4:
						id5 = patientList[4].patientId;
						go = GameObject.Find("Text_PatientSearch_Patient5");
						buttontext = (Text) go.GetComponent(typeof(Text));
						buttontext.text = patientList[4].lastName + "," + patientList[4].givenName  + ", " + patientList[4].DOB;;
						break;
					case 5:
						id6 = patientList[5].patientId;
						go = GameObject.Find("Text_PatientSearch_Patient6");
						buttontext = (Text) go.GetComponent(typeof(Text));
						buttontext.text = patientList[5].lastName + "," + patientList[5].givenName  + ", " + patientList[5].DOB;;
						break;
					}
				}
				rdr.Close();
				if(patientList.Count > 6)
				{
					go = GameObject.Find("Button_PatientSearch_Next");
					Button nextbutton = (Button) go.GetComponent(typeof(Button));
					nextbutton.interactable = true;
				}
			}
		}
		catch(Exception e)
		{
			Debug.Log(e.ToString());
		}
	}
	
	public void PatientSelectPrevious()
	{
		//if(
		patientListReference -= 6;
		GameObject go;
		Text buttontext;
		Button selectedbutton;
		
		id1 = id2 = id3 = id4 = id5 = id6 = "";
		go = GameObject.Find("Text_PatientSearch_Patient1");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Patient";
		go = GameObject.Find("Text_PatientSearch_Patient2");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Patient";
		go = GameObject.Find("Text_PatientSearch_Patient3");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Patient";
		go = GameObject.Find("Text_PatientSearch_Patient4");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Patient";
		go = GameObject.Find("Text_PatientSearch_Patient5");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Patient";
		go = GameObject.Find("Text_PatientSearch_Patient6");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Patient";
		
		for(int i = 1; i <= 6 && i <= patientList.Count; i++)
		{
			switch (i)
			{
			case 1:
				id1 = patientList[0+patientListReference].patientId;
				go = GameObject.Find("Text_PatientSearch_Patient1");
				buttontext = (Text) go.GetComponent(typeof(Text));
				buttontext.text = patientList[0].lastName + "," + patientList[0].givenName + ", " + patientList[0].DOB;
				break;
			case 2:
				id2 = patientList[1+patientListReference].patientId;
				go = GameObject.Find("Text_PatientSearch_Patient2");
				buttontext = (Text) go.GetComponent(typeof(Text));
				buttontext.text = patientList[1].lastName + "," + patientList[1].givenName + ", " + patientList[1].DOB;;
				break;
			case 3:
				id3 = patientList[2+patientListReference].patientId;
				go = GameObject.Find("Text_PatientSearch_Patient3");
				buttontext = (Text) go.GetComponent(typeof(Text));
				buttontext.text = patientList[2].lastName + "," + patientList[2].givenName + ", " + patientList[2].DOB;;
				break;
			case 4:
				id4 = patientList[3+patientListReference].patientId;
				go = GameObject.Find("Text_PatientSearch_Patient4");
				buttontext = (Text) go.GetComponent(typeof(Text));
				buttontext.text = patientList[3].lastName + "," + patientList[3].givenName + ", " + patientList[3].DOB;;
				break;
			case 5:
				id5 = patientList[4+patientListReference].patientId;
				go = GameObject.Find("Text_PatientSearch_Patient5");
				buttontext = (Text) go.GetComponent(typeof(Text));
				buttontext.text = patientList[4].lastName + "," + patientList[4].givenName  + ", " + patientList[4].DOB;;
				break;
			case 6:
				id6 = patientList[5+patientListReference].patientId;
				go = GameObject.Find("Text_PatientSearch_Patient6");
				buttontext = (Text) go.GetComponent(typeof(Text));
				buttontext.text = patientList[5].lastName + "," + patientList[5].givenName  + ", " + patientList[5].DOB;;
				break;
			}
		}
		
		if(patientListReference == 0)
		{
			go = GameObject.Find("Button_PatientSearch_Previous");
			selectedbutton = (Button) go.GetComponent(typeof(Button));
			selectedbutton.interactable = false;
		}
		
		go = GameObject.Find("Button_PatientSearch_Next");
		selectedbutton = (Button) go.GetComponent(typeof(Button));
		selectedbutton.interactable = true;
	}
	
	public void PatientSelectNext()
	{
		patientListReference += 6;
		GameObject go;
		Text buttontext;
		Button selectedbutton;
		
		id1 = id2 = id3 = id4 = id5 = id6 = "";
		go = GameObject.Find("Text_PatientSearch_Patient1");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Patient";
		go = GameObject.Find("Text_PatientSearch_Patient2");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Patient";
		go = GameObject.Find("Text_PatientSearch_Patient3");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Patient";
		go = GameObject.Find("Text_PatientSearch_Patient4");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Patient";
		go = GameObject.Find("Text_PatientSearch_Patient5");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Patient";
		go = GameObject.Find("Text_PatientSearch_Patient6");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Patient";
		
		
		for(int i = 1; i <= 6 && i+patientListReference <= patientList.Count; i++)
		{
			switch (i)
			{
			case 1:
				id1 = patientList[0+patientListReference].patientId;
				go = GameObject.Find("Text_PatientSearch_Patient1");
				buttontext = (Text) go.GetComponent(typeof(Text));
				buttontext.text = patientList[0+patientListReference].lastName + "," + patientList[0+patientListReference].givenName + ", " + patientList[0+patientListReference].DOB;
				break;
			case 2:
				id2 = patientList[1+patientListReference].patientId;
				go = GameObject.Find("Text_PatientSearch_Patient2");
				buttontext = (Text) go.GetComponent(typeof(Text));
				buttontext.text = patientList[1].lastName + "," + patientList[1].givenName + ", " + patientList[1].DOB;;
				break;
			case 3:
				id3 = patientList[2+patientListReference].patientId;
				go = GameObject.Find("Text_PatientSearch_Patient3");
				buttontext = (Text) go.GetComponent(typeof(Text));
				buttontext.text = patientList[2].lastName + "," + patientList[2].givenName + ", " + patientList[2].DOB;;
				break;
			case 4:
				id4 = patientList[3+patientListReference].patientId;
				go = GameObject.Find("Text_PatientSearch_Patient4");
				buttontext = (Text) go.GetComponent(typeof(Text));
				buttontext.text = patientList[3].lastName + "," + patientList[3].givenName + ", " + patientList[3].DOB;;
				break;
			case 5:
				id5 = patientList[4+patientListReference].patientId;
				go = GameObject.Find("Text_PatientSearch_Patient5");
				buttontext = (Text) go.GetComponent(typeof(Text));
				buttontext.text = patientList[4].lastName + "," + patientList[4].givenName  + ", " + patientList[4].DOB;;
				break;
			case 6:
				id6 = patientList[5+patientListReference].patientId;
				go = GameObject.Find("Text_PatientSearch_Patient6");
				buttontext = (Text) go.GetComponent(typeof(Text));
				buttontext.text = patientList[5].lastName + "," + patientList[5].givenName  + ", " + patientList[5].DOB;;
				break;
			}
		}
		
		if(patientListReference+6 >= patientList.Count)
		{
			go = GameObject.Find("Button_PatientSearch_Next");
			selectedbutton = (Button) go.GetComponent(typeof(Button));
			selectedbutton.interactable = false;
		}
		
		go = GameObject.Find("Button_PatientSearch_Previous");
		selectedbutton = (Button) go.GetComponent(typeof(Button));
		selectedbutton.interactable = true;
	}
	
	public void PatientSelect(string s)
	{
		int i = int.Parse(s);
		Text pressedbutton;
		GameObject go;
		string id = "";
		switch (i)
		{
		case 1:
			go = GameObject.Find ("Text_PatientSearch_Patient1");
			pressedbutton = (Text) go.GetComponent(typeof(Text));
			if(!pressedbutton.text.Equals("No Patient"))
			{	
				id = id1;
			}
			break;
		case 2:
			go = GameObject.Find ("Text_PatientSearch_Patient2");
			pressedbutton = (Text) go.GetComponent(typeof(Text));
			if(!pressedbutton.text.Equals("No Patient"))
			{
				id = id2;
			}
			break;
		case 3:
			go = GameObject.Find ("Text_PatientSearch_Patient3");
			pressedbutton = (Text) go.GetComponent(typeof(Text));
			if(!pressedbutton.text.Equals("No Patient"))
			{
				id = id3;
			}
			break;
		case 4:
			go = GameObject.Find ("Text_PatientSearch_Patient4");
			pressedbutton = (Text) go.GetComponent(typeof(Text));
			if(!pressedbutton.text.Equals("No Patient"))
			{
				id = id4;
			}
			break;
		case 5:
			go = GameObject.Find ("Text_PatientSearch_Patient5");
			pressedbutton = (Text) go.GetComponent(typeof(Text));
			if(!pressedbutton.text.Equals("No Patient"))
			{
				id = id5;
			}
			break;
		case 6:
			go = GameObject.Find ("Text_PatientSearch_Patient6");
			pressedbutton = (Text) go.GetComponent(typeof(Text));
			if(!pressedbutton.text.Equals("No Patient"))
			{
				id = id6;
			}
			break;
		}
		
		if(!id.Equals(""))
		{
			PlayerPrefs.SetString("playerID", id);
			Application.LoadLevel ("GamePlay");
			//TODO go to game
		}
	}
	
	public void AddPatient()
	{
		patient item = new patient();
		Text currentfield;
		string field;
		
		currentfield = (Text) GameObject.Find ("Text_AddPatient_NameField").GetComponent(typeof(Text));
		field = currentfield.text;
		string[] names = Regex.Split(field, ", ");
		item.lastName = names[0];
		item.givenName = names[1];
		
		currentfield = (Text) GameObject.Find ("Text_AddPatient_DateofBirthField").GetComponent(typeof(Text));
		item.DOB = currentfield.text;
		
		currentfield = (Text) GameObject.Find ("Text_AddPatient_AddressField").GetComponent(typeof(Text));
		field = currentfield.text;
		names = Regex.Split(field, ", ");
		item.address = names[0];
		item.city = names[1];
		item.state = names[2];
		
		currentfield = (Text) GameObject.Find ("Text_AddPatient_PhoneField").GetComponent(typeof(Text));
		item.phone = currentfield.text;
		
		currentfield = (Text) GameObject.Find ("Text_AddPatient_GenderField").GetComponent(typeof(Text));
		item.gender = currentfield.text;
		
		currentfield = (Text) GameObject.Find ("Text_AddPatient_BloodgroupField").GetComponent(typeof(Text));
		item.bloodgroup = currentfield.text;
		
		currentfield = (Text) GameObject.Find ("Text_AddPatient_EmailField").GetComponent(typeof(Text));
		item.email = currentfield.text;
		
		currentfield = (Text) GameObject.Find ("Text_AddPatient_InjuryField").GetComponent(typeof(Text));
		item.injuryName = currentfield.text;
		
		currentfield = (Text) GameObject.Find ("Text_AddPatient_InjurydateField").GetComponent(typeof(Text));
		item.injuryDate = currentfield.text;
		
		currentfield = (Text) GameObject.Find ("Text_AddPatient_InjurySeverityField").GetComponent(typeof(Text));
		item.injurySeverity = currentfield.text;
		
		currentfield = (Text) GameObject.Find ("Text_AddPatient_ContactField").GetComponent(typeof(Text));
		item.familyContactName = currentfield.text;
		
		currentfield = (Text) GameObject.Find ("Text_AddPatient_ContactPhoneField").GetComponent(typeof(Text));
		item.familyContactPhone = currentfield.text;
		
		//put therapist stuff here
		
		currentfield = (Text) GameObject.Find ("Text_AddPatient_CommentsField").GetComponent(typeof(Text));
		item.commentText = currentfield.text;
		
		
		try
		{
			string query = "INSERT INTO magni_patient (givenName, lastName, DOB, address, city, state, gender, bloodgroup, " +
				"email, injuryName, injuryDate, injurySeverity, familyContactName, commentText) VALUES (?givenName, ?lastName, " +
					"?DOB, ?address, ?city, ?state, ?gender, ?bloodgroup, ?email, ?injuryName, ?injuryDate, ?injurySeverity, " +
					"?familyContactName, ?commentText)";//, ?MRN, ?recordCreate_TS)";
			cmd = new MySqlCommand(query,con);
			MySqlParameter param1 = cmd.Parameters.Add("?givenName", MySqlDbType.VarChar);
			param1.Value = item.givenName;
			MySqlParameter param2 = cmd.Parameters.Add("?lastName", MySqlDbType.VarChar);
			param2.Value = item.lastName;
			MySqlParameter param3 = cmd.Parameters.Add("?DOB", MySqlDbType.VarChar);
			param3.Value = item.DOB;
			MySqlParameter param4 = cmd.Parameters.Add("?address", MySqlDbType.VarChar);
			param4.Value = item.address;
			MySqlParameter param5 = cmd.Parameters.Add("?city", MySqlDbType.VarChar);
			param5.Value = item.city;
			MySqlParameter param6 = cmd.Parameters.Add("?state", MySqlDbType.VarChar);
			param6.Value = item.state;
			//MySqlParameter param7 = cmd.Parameters.Add("?phone", MySqlDbType.Int64);
			//param7.Value = Int64.Parse(item.phone);
			MySqlParameter param8 = cmd.Parameters.Add("?gender", MySqlDbType.VarChar);
			param8.Value = item.gender;
			MySqlParameter param9 = cmd.Parameters.Add("?bloodgroup", MySqlDbType.VarChar);
			param9.Value = item.bloodgroup;
			MySqlParameter param10 = cmd.Parameters.Add("?email", MySqlDbType.VarChar);
			param10.Value = item.email;
			MySqlParameter param11 = cmd.Parameters.Add("?injuryName", MySqlDbType.VarChar);
			param11.Value = item.injuryName;
			MySqlParameter param12 = cmd.Parameters.Add("?injuryDate", MySqlDbType.VarChar);
			param12.Value = item.injuryDate;
			MySqlParameter param13 = cmd.Parameters.Add("?injurySeverity", MySqlDbType.VarChar);
			param13.Value = item.injurySeverity;
			MySqlParameter param14 = cmd.Parameters.Add("?familyContactName", MySqlDbType.VarChar);
			param14.Value = item.familyContactName;
			//MySqlParameter param15 = cmd.Parameters.Add("?familyContactPhone", MySqlDbType.Int64);
			//param15.Value = Int64.Parse(item.familyContactPhone);
			MySqlParameter param16 = cmd.Parameters.Add("?commentText", MySqlDbType.VarChar);
			param16.Value = item.commentText;
			//			MySqlParameter param17 = cmd.Parameters.Add("?MRN", MySqlDbType.Int64);
			//			param17.Value = 123;
			//			MySqlParameter param18 = cmd.Parameters.Add("?recordCreate_TS", MySqlDbType.Int64);
			//			param18.Value = 123;
			cmd.ExecuteNonQuery();
			
			Canvas can = (Canvas) GameObject.Find ("Canvas_AddPatient").GetComponent(typeof(Canvas));
			can.enabled = false;
			can = (Canvas) GameObject.Find ("Canvas_PatientSearch").GetComponent(typeof(Canvas));
			can.enabled = true;
		}
		catch(Exception e)
		{
			Debug.Log(e.ToString());
		}
	}

	void OnApplicationQuit()
	{
		Debug.Log("Closing DB Connection");
		if(con != null)
		{
			if(!con.State.Equals(ConnectionState.Closed))
				con.Close();
			Debug.Log("Connection state: " + con.State);
			con.Dispose();
			Debug.Log("DB Connection Disposed");
		}
	}
}
