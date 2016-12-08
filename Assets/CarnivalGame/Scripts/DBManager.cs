using UnityEngine;
using UnityEngine.UI;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic; 
using System.Text.RegularExpressions;
using System.IO;

public class DBManager : MonoBehaviour {

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
		public string sessionID, exerciseID, playlistID;
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
	public string eid1, eid2, eid3, eid4, eid5;
	public string slid1, slid2, slid3, slid4, slid5, slid6, slid7, slid8, slid9, slid10;
	public string elid1, elid2, elid3, elid4, elid5, elid6, elid7, elid8, elid9, elid10;
	public string plid1, plid2, plid3, plid4, plid5, plid6;
	public List<patient> patientList = new List<patient>();
	public List<exercise> exerciseList = new List<exercise>();
	public List<encounter> patientsessionList = new List<encounter>();
	public List<session> patientexerciseList = new List<session>();
	public int patientListReference = 0;
	public int exerciseListReference = 0;
	public int patientsessionListReference = 0;
	public int patientexerciseListReference = 0;
	public string currentsessionid = "0";
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

	void Start () 
	{

	}

	// Update is called once per frame
	void Update () 
	{
	
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
		Canvas search;
		Canvas profile;
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
			go = GameObject.Find ("Canvas_PatientSearch");
			search = (Canvas) go.GetComponent(typeof(Canvas));
			go = GameObject.Find ("Canvas_PatientProfile");
			profile = (Canvas) go.GetComponent(typeof(Canvas));

			search.enabled = false;
			profile.enabled = true;

			cmd = new MySqlCommand("Select * from magni_patient where patientId=" + id, con);
			rdr = cmd.ExecuteReader();
			if(rdr.HasRows)
			{
				while(rdr.Read())
				{
					current_patient.patientId = rdr["patientId"].ToString();
					current_patient.givenName = rdr["givenName"].ToString();
					Debug.Log("111" + current_patient.givenName	);
					current_patient.lastName = rdr["lastName"].ToString ();
					current_patient.middleName = rdr["middleName"].ToString();
					current_patient.suffix = rdr["suffix"].ToString();
					current_patient.prefix = rdr["prefix"].ToString();
					current_patient.address = rdr["address"].ToString();
					current_patient.city = rdr["city"].ToString();
					current_patient.state = rdr["state"].ToString();
					current_patient.MRN = rdr["MRN"].ToString();
					current_patient.DOB = rdr["DOB"].ToString().Substring(0,10);
					current_patient.gender = rdr["gender"].ToString();
					current_patient.email = rdr["email"].ToString();
					current_patient.phone = rdr["phone"].ToString();
					current_patient.bloodgroup = rdr["bloodgroup"].ToString();
					current_patient.injuryName = rdr["injuryName"].ToString();
					current_patient.injuryDate = rdr["injuryDate"].ToString();
					current_patient.injurySeverity = rdr["injurySeverity"].ToString();
					current_patient.injuryComments = rdr["injuryComments"].ToString();
					current_patient.recordCreate_TS = rdr["recordCreate_TS"].ToString();
					current_patient.recordUpdate_TS = rdr["recordUpdate_TS"].ToString();
					current_patient.familyContactName = rdr["familyContactName"].ToString();
					current_patient.familyContactPhone = rdr["familyContactPhone"].ToString();
					current_patient.allowSharingInfo = rdr["allowSharingInfo"].ToString();
					current_patient.commentText = rdr["commentText"].ToString();
				}
			}
			rdr.Close();

			Text currentfield = (Text) GameObject.Find("Text_PatientProfile_NameField").GetComponent(typeof(Text));
			currentfield.text = current_patient.lastName + ", " + current_patient.givenName;
			currentfield = (Text) GameObject.Find ("Text_PatientProfile_AddressField").GetComponent(typeof(Text));
			currentfield.text = current_patient.address + ", " + current_patient.city + ", " + current_patient.state;
			currentfield = (Text) GameObject.Find ("Text_PatientProfile_PhoneField").GetComponent(typeof(Text));
			currentfield.text = current_patient.phone;
			currentfield = (Text) GameObject.Find ("Text_PatientProfile_DateofBirthField").GetComponent(typeof(Text));
			currentfield.text = current_patient.DOB;
			currentfield = (Text) GameObject.Find ("Text_PatientProfile_InjuryField").GetComponent(typeof(Text));
			currentfield.text = current_patient.injuryName;
			currentfield = (Text) GameObject.Find ("Text_PatientProfile_GenderField").GetComponent(typeof(Text));
			currentfield.text = current_patient.gender;
			currentfield = (Text) GameObject.Find ("Text_PatientProfile_BloodgroupField").GetComponent(typeof(Text));
			currentfield.text = current_patient.bloodgroup;
			currentfield = (Text) GameObject.Find ("Text_PatientProfile_EmailField").GetComponent(typeof(Text));
			currentfield.text = current_patient.email;
			currentfield = (Text) GameObject.Find ("Text_PatientProfile_InjurydateField").GetComponent(typeof(Text));
			currentfield.text = current_patient.injuryDate;
			currentfield = (Text) GameObject.Find ("Text_PatientProfile_InjurySeverityField").GetComponent(typeof(Text));
			currentfield.text = current_patient.injurySeverity;
			currentfield = (Text) GameObject.Find ("Text_PatientProfile_ContactField").GetComponent(typeof(Text));
			currentfield.text = current_patient.familyContactName;
			currentfield = (Text) GameObject.Find ("Text_PatientProfile_ContactPhoneField").GetComponent(typeof(Text));
			currentfield.text = current_patient.familyContactPhone;
			currentfield = (Text) GameObject.Find ("Text_PatientProfile_CommentsField").GetComponent(typeof(Text));
			currentfield.text = current_patient.commentText;
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

	public void UpdatePatientInit()
	{
		InputField currentfield;

		currentfield = (InputField) GameObject.Find ("InputField_UpdatePatient_NameField").GetComponent(typeof(InputField));
		currentfield.text = current_patient.lastName + ", " + current_patient.givenName;
		Debug.Log("32333" + current_patient.givenName);

		currentfield = (InputField) GameObject.Find ("InputField_UpdatePatient_DateofBirthField").GetComponent(typeof(InputField));
		currentfield.text = current_patient.DOB;

		currentfield = (InputField) GameObject.Find ("InputField_UpdatePatient_AddressField").GetComponent(typeof(InputField));
		currentfield.text = current_patient.address + ", " + current_patient.city + ", " + current_patient.state;

		currentfield = (InputField) GameObject.Find ("InputField_UpdatePatient_PhoneField").GetComponent(typeof(InputField));
		currentfield.text = current_patient.phone;

		currentfield = (InputField) GameObject.Find ("InputField_UpdatePatient_GenderField").GetComponent(typeof(InputField));
		currentfield.text = current_patient.gender;

		currentfield = (InputField) GameObject.Find ("InputField_UpdatePatient_BloodgroupField").GetComponent(typeof(InputField));
		currentfield.text = current_patient.bloodgroup;

		currentfield = (InputField) GameObject.Find ("InputField_UpdatePatient_EmailField").GetComponent(typeof(InputField));
		currentfield.text = current_patient.email;

		currentfield = (InputField) GameObject.Find ("InputField_UpdatePatient_InjuryField").GetComponent(typeof(InputField));
		currentfield.text = current_patient.injuryName;

		currentfield = (InputField) GameObject.Find ("InputField_UpdatePatient_InjurydateField").GetComponent(typeof(InputField));
		currentfield.text = current_patient.injuryDate;

		currentfield = (InputField) GameObject.Find ("InputField_UpdatePatient_InjurySeverityField").GetComponent(typeof(InputField));
		currentfield.text = current_patient.injurySeverity;

		currentfield = (InputField) GameObject.Find ("InputField_UpdatePatient_ContactField").GetComponent(typeof(InputField));
		currentfield.text = current_patient.familyContactName;

		currentfield = (InputField) GameObject.Find ("InputField_UpdatePatient_ContactPhoneField").GetComponent(typeof(InputField));
		currentfield.text = current_patient.familyContactPhone;

		currentfield = (InputField) GameObject.Find ("InputField_UpdatePatient_CommentsField").GetComponent(typeof(InputField));
		currentfield.text = current_patient.commentText;

		Canvas can = (Canvas) GameObject.Find ("Canvas_PatientProfile").GetComponent(typeof(Canvas));
		can.enabled = false;
		can = (Canvas) GameObject.Find ("Canvas_UpdatePatient").GetComponent(typeof(Canvas));
		can.enabled = true;
	}

	public void UpdatePatient()
	{
		Text currentfield;
		string field;

		currentfield = (Text) GameObject.Find ("Text_UpdatePatient_NameField").GetComponent(typeof(Text));
		field = currentfield.text;
		string[] names = Regex.Split(field, ", ");
		current_patient.lastName = names[0];
		current_patient.givenName = names[1];

		currentfield = (Text) GameObject.Find ("Text_UpdatePatient_DateofBirthField").GetComponent(typeof(Text));
		current_patient.DOB = currentfield.text;

		currentfield = (Text) GameObject.Find ("Text_UpdatePatient_AddressField").GetComponent(typeof(Text));
		field = currentfield.text;
		names = Regex.Split(field, ", ");
		current_patient.address = names[0];
		current_patient.city = names[1];
		current_patient.state = names[2];

		currentfield = (Text) GameObject.Find ("Text_UpdatePatient_PhoneField").GetComponent(typeof(Text));
		current_patient.phone = currentfield.text;

		currentfield = (Text) GameObject.Find ("Text_UpdatePatient_GenderField").GetComponent(typeof(Text));
		current_patient.gender = currentfield.text;

		currentfield = (Text) GameObject.Find ("Text_UpdatePatient_BloodgroupField").GetComponent(typeof(Text));
		current_patient.bloodgroup = currentfield.text;

		currentfield = (Text) GameObject.Find ("Text_UpdatePatient_EmailField").GetComponent(typeof(Text));
		current_patient.email = currentfield.text;

		currentfield = (Text) GameObject.Find ("Text_UpdatePatient_InjuryField").GetComponent(typeof(Text));
		current_patient.injuryName = currentfield.text;

		currentfield = (Text) GameObject.Find ("Text_UpdatePatient_InjurydateField").GetComponent(typeof(Text));
		current_patient.injuryDate = currentfield.text;

		currentfield = (Text) GameObject.Find ("Text_UpdatePatient_InjurySeverityField").GetComponent(typeof(Text));
		current_patient.injurySeverity = currentfield.text;

		currentfield = (Text) GameObject.Find ("Text_UpdatePatient_ContactField").GetComponent(typeof(Text));
		current_patient.familyContactName = currentfield.text;

		currentfield = (Text) GameObject.Find ("Text_UpdatePatient_ContactPhoneField").GetComponent(typeof(Text));
		current_patient.familyContactPhone = currentfield.text;

		currentfield = (Text) GameObject.Find ("Text_UpdatePatient_CommentsField").GetComponent(typeof(Text));
		current_patient.commentText = currentfield.text;

		
		try
		{
			string query = "UPDATE magni_patient SET givenName=?givenName, lastName=?lastName, DOB=?DOB, address=?address, city=?city, state=?state, " +
				"gender=?gender, bloodgroup=?bloodgroup, email=?email, injuryName=?injuryName, injuryDate=?injuryDate, injurySeverity=?injurySeverity, " +
					"familyContactName=?familyContactName, commentText=?commentText WHERE patientId=?patientId";
			cmd = new MySqlCommand(query,con);
			MySqlParameter param1 = cmd.Parameters.Add("?givenName", MySqlDbType.VarChar);
			param1.Value = current_patient.givenName;
			MySqlParameter param2 = cmd.Parameters.Add("?lastName", MySqlDbType.VarChar);
			param2.Value = current_patient.lastName;
			MySqlParameter param3 = cmd.Parameters.Add("?DOB", MySqlDbType.VarChar);
			param3.Value = current_patient.DOB;
			MySqlParameter param4 = cmd.Parameters.Add("?address", MySqlDbType.VarChar);
			param4.Value = current_patient.address;
			MySqlParameter param5 = cmd.Parameters.Add("?city", MySqlDbType.VarChar);
			param5.Value = current_patient.city;
			MySqlParameter param6 = cmd.Parameters.Add("?state", MySqlDbType.VarChar);
			param6.Value = current_patient.state;
//			MySqlParameter param7 = cmd.Parameters.Add("?phone", MySqlDbType.Int32);
//			param7.Value = Int32.Parse(current_patient.phone);
			MySqlParameter param8 = cmd.Parameters.Add("?gender", MySqlDbType.VarChar);
			param8.Value = current_patient.gender;
			MySqlParameter param9 = cmd.Parameters.Add("?bloodgroup", MySqlDbType.VarChar);
			param9.Value = current_patient.bloodgroup;
			MySqlParameter param10 = cmd.Parameters.Add("?email", MySqlDbType.VarChar);
			param10.Value = current_patient.email;
			MySqlParameter param11 = cmd.Parameters.Add("?injuryName", MySqlDbType.VarChar);
			param11.Value = current_patient.injuryName;
			MySqlParameter param12 = cmd.Parameters.Add("?injuryDate", MySqlDbType.VarChar);
			param12.Value = current_patient.injuryDate;
			MySqlParameter param13 = cmd.Parameters.Add("?injurySeverity", MySqlDbType.VarChar);
			param13.Value = current_patient.injurySeverity;
			MySqlParameter param14 = cmd.Parameters.Add("?familyContactName", MySqlDbType.VarChar);
			param14.Value = current_patient.familyContactName;
			//MySqlParameter param15 = cmd.Parameters.Add("?familyContactPhone", MySqlDbType.Int64);
			//param15.Value = Int64.Parse(item.familyContactPhone);
			MySqlParameter param16 = cmd.Parameters.Add("?commentText", MySqlDbType.VarChar);
			param16.Value = current_patient.commentText;
			MySqlParameter param17 = cmd.Parameters.Add("?patientId", MySqlDbType.Int32);
			param17.Value = current_patient.patientId;
			//			MySqlParameter param17 = cmd.Parameters.Add("?MRN", MySqlDbType.Int64);
			//			param17.Value = 123;
			//			MySqlParameter param18 = cmd.Parameters.Add("?recordCreate_TS", MySqlDbType.Int64);
			//			param18.Value = 123;
			cmd.ExecuteNonQuery();
			
			Canvas can = (Canvas) GameObject.Find ("Canvas_UpdatePatient").GetComponent(typeof(Canvas));
			can.enabled = false;
			can = (Canvas) GameObject.Find ("Canvas_PatientProfile").GetComponent(typeof(Canvas));
			can.enabled = true;
			id1 = current_patient.patientId;
			PatientSelect("1");
		}
		catch(Exception e)
		{
			Debug.Log(e.ToString());
		}
	}

	public void PatientSessionListInit()
	{
		slid1 = slid2 = slid3 = slid4 = slid5 = slid6 = slid7 = slid8 = slid9 = slid10 = "";
		Text buttontext;
		GameObject go;
		patientsessionList.Clear();
		patientsessionListReference = 0;

		go = GameObject.Find("Text_PatientSessionList_Session1");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session2");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session3");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session4");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session5");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session6");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session7");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session8");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session9");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session10");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";

		try
		{
			cmd = new MySqlCommand("Select * from magni_encounter WHERE patientID =" + current_patient.patientId, con);
			rdr = cmd.ExecuteReader();

			if(rdr.HasRows)
			{
				while(rdr.Read())
				{
					encounter item = new encounter();
					item.encounterID = rdr["encounterID"].ToString();
					item.sessionID = rdr["sessionID"].ToString();
					patientsessionList.Add(item);
				}
				for(int i = 0; (i < 10) && (i < patientsessionList.Count); i++)
				{
					switch(i)
					{
					case 0:
						buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session1").GetComponent(typeof(Text));
						buttontext.text = patientsessionList[0].sessionID;
						slid1 = patientsessionList[0].sessionID;
						break;
					case 1:
						buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session2").GetComponent(typeof(Text));
						buttontext.text = patientsessionList[1].sessionID;
						slid2 = patientsessionList[1].sessionID;
						break;
					case 2:
						buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session3").GetComponent(typeof(Text));
						buttontext.text = patientsessionList[2].sessionID;
						slid3 = patientsessionList[2].sessionID;
						break;
					case 3:
						buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session4").GetComponent(typeof(Text));
						buttontext.text = patientsessionList[3].sessionID;
						slid4 = patientsessionList[3].sessionID;
						break;
					case 4:
						buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session5").GetComponent(typeof(Text));
						buttontext.text = patientsessionList[4].sessionID;
						slid5 = patientsessionList[4].sessionID;
						break;
					case 5:
						buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session6").GetComponent(typeof(Text));
						buttontext.text = patientsessionList[5].sessionID;
						slid6 = patientsessionList[5].sessionID;
						break;
					case 6:
						buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session7").GetComponent(typeof(Text));
						buttontext.text = patientsessionList[6].sessionID;
						slid7 = patientsessionList[6].sessionID;
						break;
					case 7:
						buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session8").GetComponent(typeof(Text));
						buttontext.text = patientsessionList[7].sessionID;
						slid8 = patientsessionList[7].sessionID;
						break;
					case 8:
						buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session9").GetComponent(typeof(Text));
						buttontext.text = patientsessionList[8].sessionID;
						slid9 = patientsessionList[8].sessionID;
						break;
					case 9:
						buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session10").GetComponent(typeof(Text));
						buttontext.text = patientsessionList[9].sessionID;
						slid10 = patientsessionList[9].sessionID;
						break;
					}
				}
				rdr.Close();

				if(patientsessionList.Count > 10)
				{
					go = GameObject.Find("Button_PatientSessionList_Next");
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

	public void PatientSessionListPrevious()
	{
		patientsessionListReference -= 10;
		GameObject go;
		Text buttontext;
		Button selectedbutton;

		slid1 = slid2 = slid3 = slid4 = slid5 = slid6 = slid7 = slid8 = slid9 = slid10 = "";
		go = GameObject.Find("Text_PatientSessionList_Session1");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session2");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session3");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session4");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session5");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session6");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session7");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session8");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session9");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session10");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";

		for(int i = 1; i <= 10 && i+patientsessionListReference <= patientsessionList.Count; i++)
		{
			switch(i)
			{
			case 1:
				buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session1").GetComponent(typeof(Text));
				buttontext.text = patientsessionList[0+patientsessionListReference].sessionID;
				slid1 = patientsessionList[0+patientsessionListReference].sessionID;
				break;
			case 2:
				buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session2").GetComponent(typeof(Text));
				buttontext.text = patientsessionList[1+patientsessionListReference].sessionID;
				slid2 = patientsessionList[1+patientsessionListReference].sessionID;
				break;
			case 3:
				buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session3").GetComponent(typeof(Text));
				buttontext.text = patientsessionList[2+patientsessionListReference].sessionID;
				slid3 = patientsessionList[2+patientsessionListReference].sessionID;
				break;
			case 4:
				buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session4").GetComponent(typeof(Text));
				buttontext.text = patientsessionList[3+patientsessionListReference].sessionID;
				slid4 = patientsessionList[3+patientsessionListReference].sessionID;
				break;
			case 5:
				buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session5").GetComponent(typeof(Text));
				buttontext.text = patientsessionList[4+patientsessionListReference].sessionID;
				slid5 = patientsessionList[4+patientsessionListReference].sessionID;
				break;
			case 6:
				buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session6").GetComponent(typeof(Text));
				buttontext.text = patientsessionList[5+patientsessionListReference].sessionID;
				slid6 = patientsessionList[5+patientsessionListReference].sessionID;
				break;
			case 7:
				buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session7").GetComponent(typeof(Text));
				buttontext.text = patientsessionList[6+patientsessionListReference].sessionID;
				slid7 = patientsessionList[6+patientsessionListReference].sessionID;
				break;
			case 8:
				buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session8").GetComponent(typeof(Text));
				buttontext.text = patientsessionList[7+patientsessionListReference].sessionID;
				slid8 = patientsessionList[7+patientsessionListReference].sessionID;
				break;
			case 9:
				buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session9").GetComponent(typeof(Text));
				buttontext.text = patientsessionList[8+patientsessionListReference].sessionID;
				slid9 = patientsessionList[8+patientsessionListReference].sessionID;
				break;
			case 10:
				buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session10").GetComponent(typeof(Text));
				buttontext.text = patientsessionList[9+patientsessionListReference].sessionID;
				slid10 = patientsessionList[9+patientsessionListReference].sessionID;
				break;
			}
		}

		if(patientListReference-10 < 0)
		{
			go = GameObject.Find("Button_PatientSessionList_Previous");
			selectedbutton = (Button) go.GetComponent(typeof(Button));
			selectedbutton.interactable = false;
		}
		
		go = GameObject.Find("Button_PatientSessionList_Next");
		selectedbutton = (Button) go.GetComponent(typeof(Button));
		selectedbutton.interactable = true;
	}

	public void PatientSessionListNext()
	{

		patientsessionListReference += 10;
		GameObject go;
		Text buttontext;
		Button selectedbutton;
		
		slid1 = slid2 = slid3 = slid4 = slid5 = slid6 = slid7 = slid8 = slid9 = slid10 = "";
		go = GameObject.Find("Text_PatientSessionList_Session1");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session2");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session3");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session4");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session5");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session6");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session7");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session8");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session9");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		go = GameObject.Find("Text_PatientSessionList_Session10");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Session";
		
		for(int i = 1; i <= 10 && i+patientsessionListReference <= patientsessionList.Count; i++)
		{
			switch (i)
			{
			case 1:
				buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session1").GetComponent(typeof(Text));
				buttontext.text = patientsessionList[0+patientsessionListReference].sessionID;
				slid1 = patientsessionList[0+patientsessionListReference].sessionID;
				break;
			case 2:
				buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session2").GetComponent(typeof(Text));
				buttontext.text = patientsessionList[1+patientsessionListReference].sessionID;
				slid2 = patientsessionList[1+patientsessionListReference].sessionID;
				break;
			case 3:
				buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session3").GetComponent(typeof(Text));
				buttontext.text = patientsessionList[2+patientsessionListReference].sessionID;
				slid3 = patientsessionList[2+patientsessionListReference].sessionID;
				break;
			case 4:
				buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session4").GetComponent(typeof(Text));
				buttontext.text = patientsessionList[3+patientsessionListReference].sessionID;
				slid4 = patientsessionList[3+patientsessionListReference].sessionID;
				break;
			case 5:
				buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session5").GetComponent(typeof(Text));
				buttontext.text = patientsessionList[4+patientsessionListReference].sessionID;
				slid5 = patientsessionList[4+patientsessionListReference].sessionID;
				break;
			case 6:
				buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session6").GetComponent(typeof(Text));
				buttontext.text = patientsessionList[5+patientsessionListReference].sessionID;
				slid6 = patientsessionList[5+patientsessionListReference].sessionID;
				break;
			case 7:
				buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session7").GetComponent(typeof(Text));
				buttontext.text = patientsessionList[6+patientsessionListReference].sessionID;
				slid7 = patientsessionList[6+patientsessionListReference].sessionID;
				break;
			case 8:
				buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session8").GetComponent(typeof(Text));
				buttontext.text = patientsessionList[7+patientsessionListReference].sessionID;
				slid8 = patientsessionList[7+patientsessionListReference].sessionID;
				break;
			case 9:
				buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session9").GetComponent(typeof(Text));
				buttontext.text = patientsessionList[8+patientsessionListReference].sessionID;
				slid9 = patientsessionList[8+patientsessionListReference].sessionID;
				break;
			case 10:
				buttontext = (Text) GameObject.Find("Text_PatientSessionList_Session10").GetComponent(typeof(Text));
				buttontext.text = patientsessionList[9+patientsessionListReference].sessionID;
				slid10 = patientsessionList[9+patientsessionListReference].sessionID;
				break;
			}
		}
		
		if(patientsessionListReference+10 >= patientsessionList.Count)
		{
			go = GameObject.Find("Button_ExerciseInfo_Next");
			selectedbutton = (Button) go.GetComponent(typeof(Button));
			selectedbutton.interactable = false;
		}
		
		go = GameObject.Find("Button_ExerciseInfo_Previous");
		selectedbutton = (Button) go.GetComponent(typeof(Button));
		selectedbutton.interactable = true;
		
	}


	public void PatientSessionListSelect(string s)
	{
		int i = int.Parse(s);
		Text pressedbutton;
		GameObject go;
		Canvas sessionlist;
		Canvas sessioninfo;
		string id = "";
		switch (i)
		{
		case 1:
			go = GameObject.Find ("Text_PatientSessionList_Session1");
			pressedbutton = (Text) go.GetComponent(typeof(Text));
			if(!pressedbutton.text.Equals("No Session"))
			{	
				id = slid1;
			}
			break;
		case 2:
			go = GameObject.Find ("Text_PatientSessionList_Session2");
			pressedbutton = (Text) go.GetComponent(typeof(Text));
			if(!pressedbutton.text.Equals("No Patient"))
			{
				id = slid2;
			}
			break;
		case 3:
			go = GameObject.Find ("Text_PatientSessionList_Session3");
			pressedbutton = (Text) go.GetComponent(typeof(Text));
			if(!pressedbutton.text.Equals("No Patient"))
			{
				id = slid3;
			}
			break;
		case 4:
			go = GameObject.Find ("Text_PatientSessionList_Session4");
			pressedbutton = (Text) go.GetComponent(typeof(Text));
			if(!pressedbutton.text.Equals("No Patient"))
			{
				id = slid4;
			}
			break;
		case 5:
			go = GameObject.Find ("Text_PatientSessionList_Session5");
			pressedbutton = (Text) go.GetComponent(typeof(Text));
			if(!pressedbutton.text.Equals("No Patient"))
			{
				id = slid5;
			}
			break;
		case 6:
			go = GameObject.Find ("Text_PatientSessionList_Session6");
			pressedbutton = (Text) go.GetComponent(typeof(Text));
			if(!pressedbutton.text.Equals("No Patient"))
			{
				id = slid6;
			}
			break;
		case 7:
			go = GameObject.Find ("Text_PatientSessionList_Sessiont7");
			pressedbutton = (Text) go.GetComponent(typeof(Text));
			if(!pressedbutton.text.Equals("No Patient"))
			{
				id = slid7;
			}
			break;
		case 8:
			go = GameObject.Find ("Text_PatientSessionList_Session8");
			pressedbutton = (Text) go.GetComponent(typeof(Text));
			if(!pressedbutton.text.Equals("No Patient"))
			{
				id = slid8;
			}
			break;
		case 9:
			go = GameObject.Find ("Text_PatientSessionList_Session9");
			pressedbutton = (Text) go.GetComponent(typeof(Text));
			if(!pressedbutton.text.Equals("No Patient"))
			{
				id = slid9;
			}
			break;
		case 10:
			go = GameObject.Find ("Text_PatientSessionList_Session10");
			pressedbutton = (Text) go.GetComponent(typeof(Text));
			if(!pressedbutton.text.Equals("No Patient"))
			{
				id = slid10;
			}
			break;
		}

		if(!id.Equals(""))
		{
			go = GameObject.Find ("Canvas_PatientSessionList");
			sessionlist = (Canvas) go.GetComponent(typeof(Canvas));
			go = GameObject.Find ("Canvas_PatientSessionDetail");
			sessioninfo = (Canvas) go.GetComponent(typeof(Canvas));
			
			sessionlist.enabled = false;
			sessioninfo.enabled = true;

			elid1 = elid2 = elid3 = elid4 = elid5 = "";
			patientexerciseList.Clear();
			patientexerciseListReference = 0;
			Text buttontext;
			
			go = GameObject.Find("Text_PatientSessionDetail_Exercise1");
			buttontext = (Text) go.GetComponent(typeof(Text));
			buttontext.text = "No Exercise";
			go = GameObject.Find("Text_PatientSessionDetail_Exercise2");
			buttontext = (Text) go.GetComponent(typeof(Text));
			buttontext.text = "No Exercise";
			go = GameObject.Find("Text_PatientSessionDetail_Exercise3");
			buttontext = (Text) go.GetComponent(typeof(Text));
			buttontext.text = "No Exercise";
			go = GameObject.Find("Text_PatientSessionDetail_Exercise4");
			buttontext = (Text) go.GetComponent(typeof(Text));
			buttontext.text = "No Exercise";
			go = GameObject.Find("Text_PatientSessionDetail_Exercise5");
			buttontext = (Text) go.GetComponent(typeof(Text));
			buttontext.text = "No Exercise";

			cmd = new MySqlCommand("Select * from magni_session where sessionID=" + id, con);
			rdr = cmd.ExecuteReader();
			currentsessionid = id;
			if(rdr.HasRows)
			{
				while(rdr.Read())
				{
					session item = new session();
					item.exerciseID = rdr["exerciseID"].ToString();
					item.playlistID = rdr["playlistID"].ToString();
					patientexerciseList.Add(item);
				}
				for(int j = 0; (j < 5) && (j < patientexerciseList.Count); j++)
				{
					switch(j)
					{
					case 0:
						buttontext = (Text) GameObject.Find("Text_PatientSessionDetail_Exercise1").GetComponent(typeof(Text));
						buttontext.text = patientexerciseList[0].exerciseID;
						elid1 = patientexerciseList[0].exerciseID;
						plid1 = patientexerciseList[0].playlistID;
						break;
					case 1:
						buttontext = (Text) GameObject.Find("Text_PatientSessionDetail_Exercise2").GetComponent(typeof(Text));
						buttontext.text = patientexerciseList[1].exerciseID;
						elid2 = patientexerciseList[1].exerciseID;
						plid2 = patientexerciseList[1].playlistID;
						break;
					case 2:
						buttontext = (Text) GameObject.Find("Text_PatientSessionDetail_Exercise3").GetComponent(typeof(Text));
						buttontext.text = patientexerciseList[2].exerciseID;
						elid3 = patientexerciseList[2].exerciseID;
						plid3 = patientexerciseList[2].playlistID;
						break;
					case 3:
						buttontext = (Text) GameObject.Find("Text_PatientSessionDetail_Exercise4").GetComponent(typeof(Text));
						buttontext.text = patientexerciseList[3].exerciseID;
						elid4 = patientexerciseList[3].exerciseID;
						plid4 = patientexerciseList[3].playlistID;;
						break;
					case 4:
						buttontext = (Text) GameObject.Find("Text_PatientSessionDetail_Exercise5").GetComponent(typeof(Text));
						buttontext.text = patientexerciseList[4].exerciseID;
						elid5 = patientexerciseList[4].exerciseID;
						plid5 = patientexerciseList[4].playlistID;
						break;
					}
				}
				rdr.Close();
				Image image = (Image) GameObject.Find("Image_PatientSessionDetail_ExerciseImage").GetComponent(typeof(Image));
				image.sprite = null;

				if(patientexerciseList.Count > 5)
				{
					go = GameObject.Find("Button_PatientSessionDetail_Next");
					Button nextbutton = (Button) go.GetComponent(typeof(Button));
					nextbutton.interactable = true;
				}
			}
		}
	}

	public void SessionDetailSelect(string s)
	{
		int i = int.Parse(s);
		Image image;
		Text buttontext;
		string id = "";
		string pyid = "";
		string name = "";
		string path = "C:\\Users\\Public\\Patient\\PID_" + current_patient.patientId + "\\Session_" + currentsessionid + "\\dtw_session" + currentsessionid + "_playlist";
		switch (i)
		{
		case 1:
			id = elid1;
			pyid = plid1;
			buttontext = (Text) GameObject.Find("Text_PatientSessionDetail_Exercise1").GetComponent(typeof(Text));
			name = buttontext.text;
			break;
		case 2:
			id = elid2;
			pyid = plid2;
			buttontext = (Text) GameObject.Find("Text_PatientSessionDetail_Exercise2").GetComponent(typeof(Text));
			name = buttontext.text;
			break;
		case 3:
			id = elid3;
			pyid = plid3;
			buttontext = (Text) GameObject.Find("Text_PatientSessionDetail_Exercise3").GetComponent(typeof(Text));
			name = buttontext.text;
			break;
		case 4:
			id = elid4;
			pyid = plid4;
			buttontext = (Text) GameObject.Find("Text_PatientSessionDetail_Exercise4").GetComponent(typeof(Text));
			name = buttontext.text;
			break;
		case 5:
			id = elid5;
			pyid = plid5;
			buttontext = (Text) GameObject.Find("Text_PatientSessionDetail_Exercise5").GetComponent(typeof(Text));
			name = buttontext.text;
			break;
		}

		if(!id.Equals(""))
		{
			i = int.Parse(id);
			image = (Image) GameObject.Find("Image_PatientSessionDetail_ExerciseImage").GetComponent(typeof(Image));
			path = path + pyid + "_exercise" + i + ".png";
			Debug.Log(path);
			try {
				if(File.Exists(path)) {
					byte[] bytes = File.ReadAllBytes(path);
					Texture2D tex = new Texture2D(1,1);
					tex.LoadImage(bytes);
					Sprite sprite = Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f),1.0f);
					image.sprite = sprite;
				}
			} catch (Exception e) {
				Debug.Log ("File Exception: " + e.ToString());
			}
		}
	}

	public void ExerciseListInit()
	{
		eid1 = eid2 = eid3 = eid4 = eid5 = "";
		Text buttontext;
		GameObject go;
		exerciseList.Clear();
		exerciseListReference = 0;

		go = GameObject.Find("Text_ExerciseInfo_Exercise1");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Exercise";
		go = GameObject.Find("Text_ExerciseInfo_Exercise2");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Exercise";
		go = GameObject.Find("Text_ExerciseInfo_Exercise3");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Exercise";
		go = GameObject.Find("Text_ExerciseInfo_Exercise4");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Exercise";
		go = GameObject.Find("Text_ExerciseInfo_Exercise5");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Exercise";

		try
		{
			cmd = new MySqlCommand("Select * from magni_exercise", con);
			rdr = cmd.ExecuteReader();

			if(rdr.HasRows)
			{
				while(rdr.Read())
				{
					exercise item = new exercise();
					item.exerciseID = rdr["exerciseID"].ToString();
					item.exerciseName = rdr["exerciseName"].ToString();
					exerciseList.Add(item);
				}
				for(int i = 0; (i < 5) && (i < exerciseList.Count); i++)
				{
					switch(i)
					{
					case 0:
						buttontext = (Text) GameObject.Find("Text_ExerciseInfo_Exercise1").GetComponent(typeof(Text));
						buttontext.text = exerciseList[0].exerciseName;
						eid1 = exerciseList[0].exerciseID;
						break;
					case 1:
						buttontext = (Text) GameObject.Find("Text_ExerciseInfo_Exercise2").GetComponent(typeof(Text));
						buttontext.text = exerciseList[1].exerciseName;
						eid2 = exerciseList[1].exerciseID;
						break;
					case 2:
						buttontext = (Text) GameObject.Find("Text_ExerciseInfo_Exercise3").GetComponent(typeof(Text));
						buttontext.text = exerciseList[2].exerciseName;
						eid3 = exerciseList[2].exerciseID;
						break;
					case 3:
						buttontext = (Text) GameObject.Find("Text_ExerciseInfo_Exercise4").GetComponent(typeof(Text));
						buttontext.text = exerciseList[3].exerciseName;
						eid4 = exerciseList[3].exerciseID;
						break;
					case 4:
						buttontext = (Text) GameObject.Find("Text_ExerciseInfo_Exercise5").GetComponent(typeof(Text));
						buttontext.text = exerciseList[4].exerciseName;
						eid5 = exerciseList[4].exerciseID;
						break;
					}
				}
				rdr.Close();

				if(exerciseList.Count > 5)
				{
					go = GameObject.Find("Button_ExerciseInfo_Next");
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

	public void ExerciseInfoPrevious()
	{
		exerciseListReference -= 5;
		GameObject go;
		Text buttontext;
		Button selectedbutton;

		eid1 = eid2 = eid3 = eid4 = eid5 = "";
		go = GameObject.Find("Text_ExerciseInfo_Exercise1");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Exercise";
		go = GameObject.Find("Text_ExerciseInfo_Exercise2");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Exercise";
		go = GameObject.Find("Text_ExerciseInfo_Exercise3");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Exercise";
		go = GameObject.Find("Text_ExerciseInfo_Exercise4");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Exercise";
		go = GameObject.Find("Text_ExerciseInfo_Exercise5");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Exercise";
		
		for(int i = 1; i <= 5 && i+exerciseListReference <= exerciseList.Count; i++)
		{
			switch (i)
			{
			case 1:
				buttontext = (Text) GameObject.Find("Text_ExerciseInfo_Exercise1").GetComponent(typeof(Text));
				buttontext.text = exerciseList[0+exerciseListReference].exerciseName;
				eid1 = exerciseList[0+exerciseListReference].exerciseID;
				break;
			case 2:
				buttontext = (Text) GameObject.Find("Text_ExerciseInfo_Exercise2").GetComponent(typeof(Text));
				buttontext.text = exerciseList[1+exerciseListReference].exerciseName;
				eid2 = exerciseList[1+exerciseListReference].exerciseID;
				break;
			case 3:
				buttontext = (Text) GameObject.Find("Text_ExerciseInfo_Exercise3").GetComponent(typeof(Text));
				buttontext.text = exerciseList[2+exerciseListReference].exerciseName;
				eid3 = exerciseList[2+exerciseListReference].exerciseID;
				break;
			case 4:
				buttontext = (Text) GameObject.Find("Text_ExerciseInfo_Exercise4").GetComponent(typeof(Text));
				buttontext.text = exerciseList[3+exerciseListReference].exerciseName;
				eid4 = exerciseList[3+exerciseListReference].exerciseID;
				break;
			case 5:
				buttontext = (Text) GameObject.Find("Text_ExerciseInfo_Exercise5").GetComponent(typeof(Text));
				buttontext.text = exerciseList[4+exerciseListReference].exerciseName;
				eid5 = exerciseList[4+exerciseListReference].exerciseID;
				break;
			}
		}
		
		if(exerciseListReference-5 < 0)
		{
			go = GameObject.Find("Button_ExerciseInfo_Previous");
			selectedbutton = (Button) go.GetComponent(typeof(Button));
			selectedbutton.interactable = false;
		}
		
		go = GameObject.Find("Button_ExerciseInfo_Next");
		selectedbutton = (Button) go.GetComponent(typeof(Button));
		selectedbutton.interactable = true;
	}

	public void ExerciseInfoNext()
	{
		exerciseListReference += 5;
		GameObject go;
		Text buttontext;
		Button selectedbutton;

		eid1 = eid2 = eid3 = eid4 = eid5 = "";
		go = GameObject.Find("Text_ExerciseInfo_Exercise1");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Exercise";
		go = GameObject.Find("Text_ExerciseInfo_Exercise2");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Exercise";
		go = GameObject.Find("Text_ExerciseInfo_Exercise3");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Exercise";
		go = GameObject.Find("Text_ExerciseInfo_Exercise4");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Exercise";
		go = GameObject.Find("Text_ExerciseInfo_Exercise5");
		buttontext = (Text) go.GetComponent(typeof(Text));
		buttontext.text = "No Exercise";

		for(int i = 1; i <= 5 && i+exerciseListReference <= exerciseList.Count; i++)
		{
			switch (i)
			{
			case 1:
				buttontext = (Text) GameObject.Find("Text_ExerciseInfo_Exercise1").GetComponent(typeof(Text));
				buttontext.text = exerciseList[0+exerciseListReference].exerciseName;
				eid1 = exerciseList[0+exerciseListReference].exerciseID;
				break;
			case 2:
				buttontext = (Text) GameObject.Find("Text_ExerciseInfo_Exercise2").GetComponent(typeof(Text));
				buttontext.text = exerciseList[1+exerciseListReference].exerciseName;
				eid2 = exerciseList[1+exerciseListReference].exerciseID;
				break;
			case 3:
				buttontext = (Text) GameObject.Find("Text_ExerciseInfo_Exercise3").GetComponent(typeof(Text));
				buttontext.text = exerciseList[2+exerciseListReference].exerciseName;
				eid3 = exerciseList[2+exerciseListReference].exerciseID;
				break;
			case 4:
				buttontext = (Text) GameObject.Find("Text_ExerciseInfo_Exercise4").GetComponent(typeof(Text));
				buttontext.text = exerciseList[3+exerciseListReference].exerciseName;
				eid4 = exerciseList[3+exerciseListReference].exerciseID;
				break;
			case 5:
				buttontext = (Text) GameObject.Find("Text_ExerciseInfo_Exercise5").GetComponent(typeof(Text));
				buttontext.text = exerciseList[4+exerciseListReference].exerciseName;
				eid5 = exerciseList[4+exerciseListReference].exerciseID;
				break;
			}
		}

		if(exerciseListReference+5 >= exerciseList.Count)
		{
			go = GameObject.Find("Button_ExerciseInfo_Next");
			selectedbutton = (Button) go.GetComponent(typeof(Button));
			selectedbutton.interactable = false;
		}
		
		go = GameObject.Find("Button_ExerciseInfo_Previous");
		selectedbutton = (Button) go.GetComponent(typeof(Button));
		selectedbutton.interactable = true;
	}

	public void ExerciseInfoSelect(string num)
	{
		int i = int.Parse(num);
		Image image;
		Text buttontext;
		string id = "";
		string name = "";
		string path = "C:\\Users\\Public\\Exercise\\";
		switch (i)
		{
		case 1:
			id = eid1;
			buttontext = (Text) GameObject.Find("Text_ExerciseInfo_Exercise1").GetComponent(typeof(Text));
			name = buttontext.text;
			break;
		case 2:
			id = eid2;
			buttontext = (Text) GameObject.Find("Text_ExerciseInfo_Exercise2").GetComponent(typeof(Text));
			name = buttontext.text;
			break;
		case 3:
			id = eid3;
			buttontext = (Text) GameObject.Find("Text_ExerciseInfo_Exercise3").GetComponent(typeof(Text));
			name = buttontext.text;
			break;
		case 4:
			id = eid4;
			buttontext = (Text) GameObject.Find("Text_ExerciseInfo_Exercise4").GetComponent(typeof(Text));
			name = buttontext.text;
			break;
		case 5:
			id = eid5;
			buttontext = (Text) GameObject.Find("Text_ExerciseInfo_Exercise5").GetComponent(typeof(Text));
			name = buttontext.text;
			break;
		}
	
		if(!id.Equals(""))
		{
			image = (Image) GameObject.Find("Image_ExerciseInfo_ExerciseImage").GetComponent(typeof(Image));
			path = path + name + ".png";
			Debug.Log(path);
			try {
				if(File.Exists(path)) {
					byte[] bytes = File.ReadAllBytes(path);
					Texture2D tex = new Texture2D(1,1);
					tex.LoadImage(bytes);
					Sprite sprite = Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f),1.0f);
					image.sprite = sprite;
				}
			} catch (Exception e) {
				Debug.Log ("File Exception: " + e.ToString());
			}
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
