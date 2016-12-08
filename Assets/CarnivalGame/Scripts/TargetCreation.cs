using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

public class TargetCreation : MonoBehaviour
{
    public string baseFilePath = "C:\\Users\\Public\\Exercise\\";
    public string matlabBaseFilepath = "C:\\Users\\Public\\Patient\\";
    bool record; //flag to start record a trajectory in a csv file 
    StreamWriter sw;
    FileStream fcreate;


    string[] tempValues;
    List<Vector3> coordinates;
    GameObject[] targets;

    public int currentTarget;
    public int numOfPoints;
    public GameObject targetObj;
    public float targetScale;
    int currentExercise;
    public GameObject targetDepthPlane;
    public GameObject handBall;

    List<string> performedExerciseNames;

    public int sessionID = 1;
    public int patientID = 1;
    public string folder_name;
    public int playlist_temp_ID = 0;
	public int playlistID = 0;
    public double error_analysis = 0;

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

    void Start()
    {
        performedExerciseNames = new List<string>();
        triggerNextExercise(currentExercise);

		patientID = Convert.ToInt32(PlayerPrefs.GetString("playerID"));
		sessionID = findLastSessionNum();

        folder_name = matlabBaseFilepath + "PID_" + patientID.ToString();
        //string guid = AssetDatabase.CreateFolder(matlabBaseFilepath, folder_name);
        Debug.Log("Create Folder");
        Debug.Log(folder_name);
        DirectoryInfo di = Directory.CreateDirectory(folder_name);
        Debug.Log(folder_name);
        folder_name = folder_name + "\\";
        folder_name = folder_name + "Session_" + sessionID.ToString() + "\\";
        DirectoryInfo di2 = Directory.CreateDirectory(folder_name);
        //System.IO.Directory.CreateDirectory(folder_name);


    }

    void triggerNextExercise(int exerciseNum)
    {
        coordinates = new List<Vector3>();
        string tempFileName = (transform.parent.GetChild(1).GetChild(exerciseNum).GetComponent<ExistingExercise>().exerciseName);
        string filePath = baseFilePath + tempFileName + ".csv";
        targets = new GameObject[numOfPoints];

        try
        {
            if (File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while (sr.Peek() >= 0)
                        {
                            tempValues = sr.ReadLine().Split(',');
                            coordinates.Add(new Vector3(Convert.ToSingle(tempValues[0]), Convert.ToSingle(tempValues[1]), Convert.ToSingle(tempValues[2])));
                        }
                    }
                }

                float gestureLength = 0;
                int coordsSize = coordinates.Count - 1;
                int i = 0;
                while (i < coordsSize)
                {
                    gestureLength += Vector3.Distance(coordinates[i], coordinates[++i]);
                }
                float targetInterval = gestureLength / (numOfPoints - 1);

                targets[0] = Instantiate(targetObj, coordinates[0], Quaternion.identity) as GameObject;
                float nextTargetPos = targetInterval;
                gestureLength = 0;
                i = 0;
                int j = 1;
                while (i < coordsSize)
                {
                    gestureLength += Vector3.Distance(coordinates[i], coordinates[++i]);
                    if (gestureLength > nextTargetPos)
                    {
                        targets[j++] = Instantiate(targetObj, coordinates[i], Quaternion.identity) as GameObject;
                        nextTargetPos += targetInterval;
                    }
                }
                //Small chance that some gesture patterns will produce the end target in above section
                //If end target is not created, go ahead and make the end target
                if (targets[numOfPoints - 1] == null)
                {
                    targets[numOfPoints - 1] = Instantiate(targetObj, coordinates[coordsSize], Quaternion.identity) as GameObject;
                }
                i = 0;
                while (i < numOfPoints)
                {
                    targets[i].transform.localScale = new Vector3(targetScale, targetScale, targetScale);
                    targets[i].GetComponent<TargetParent>().targetNum = i;
                    targets[i++].transform.SetParent(transform);
                }
            }
            else
            {
                Debug.Log("Did not find the file");
            }
        }
        catch (Exception e)
        {
            Debug.Log("File Exception: " + e.ToString());
        }

        targets[0].transform.GetChild(3).GetComponent<ParticleSystem>().Play();
        targetDepthPlane.transform.position = new Vector3(0, 0, targets[0].transform.position.z);
    }



    void dtw(int therapist_exercise, int patient_exercise)
    {
        Debug.Log("DTW_run");
        Debug.Log(therapist_exercise + ", " + patient_exercise);

        System.Net.Sockets.TcpClient clientSocket;
        clientSocket = new System.Net.Sockets.TcpClient();
        clientSocket.Connect("localhost", 13135);
        NetworkStream networkStream = clientSocket.GetStream();
        playlistID = patient_exercise;
        // Sent to Matlab DTW
        // 5_2_1_6
        string dataFromClient = patientID.ToString() + "_" + sessionID.ToString() + "_" + playlistID.ToString() + "_" + therapist_exercise.ToString();
        Debug.Log(dataFromClient);
        Byte[] sendBytes = Encoding.ASCII.GetBytes(dataFromClient);
        networkStream.Write(sendBytes, 0, sendBytes.Length);
        networkStream.Flush();
        Console.WriteLine(" >> " + dataFromClient);
        //Console.ReadLine();
        	
		System.Threading.Thread.Sleep(1000);
      

		
		// Receive Score from Matlab

		byte[] inStream = new byte[100250];
		networkStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
		string error = System.Text.Encoding.ASCII.GetString(inStream);
		//Console.WriteLine(" >> " + error);
		double trajectory_error = Convert.ToDouble(error);
		//Double.TryParse(error, trajectory_error);
		error_analysis = trajectory_error;
		Console.WriteLine (" >> " + error_analysis);

		Debug.Log (error_analysis);

		System.Threading.Thread.Sleep(1000);


		// Save stuff to the database

		// qry1 = sprintf('INSERT INTO magni_encounter(encounterID,patientID,sessionID)VALUES(sessionID,patientID,sessionID)');
		// qry2 = sprintf('INSERT INTO magni_session(sessionID,exerciseID,playlistID,score,graphName)VALUES(sessionID,exerciseID,playlistID,error,csv_figure_name)');

		try
		{
		

			string query2 = "INSERT INTO magni_session(sessionID,exerciseID,playlistID,score) VALUES (?sessionID, ?exerciseID, ?playlistID,?error_analysis)";
			cmd = new MySqlCommand(query2,con);
			MySqlParameter param4 = cmd.Parameters.Add("?sessionID", MySqlDbType.VarChar);
			param4.Value = sessionID.ToString();
			MySqlParameter param5 = cmd.Parameters.Add("?exerciseID", MySqlDbType.VarChar);
			param5.Value = therapist_exercise.ToString();
			MySqlParameter param6 = cmd.Parameters.Add("?playlistID", MySqlDbType.VarChar);
			param6.Value = playlistID.ToString();
			MySqlParameter param7 = cmd.Parameters.Add("?error_analysis", MySqlDbType.VarChar);
			param7.Value = error_analysis.ToString();
		
			cmd.ExecuteNonQuery();
//			Debug.Log("sessionId"+sessionID);
//			Debug.Log("patId"+patientID);
//			Debug.Log("exId"+param5.Value);
		}
		catch(Exception e)
		{
			Debug.Log(e.ToString());
		}
    }


    void Update()
    {
        if (currentTarget == 0 && targets[0] == null)
        {
            //TODO Start Recording
            Debug.Log("First Target hit");

            record = true;
            string file_name = folder_name + "pat_" + "session" + sessionID + "_playlist" + playlist_temp_ID + "_exercise" + transform.parent.GetChild(1).GetChild(currentExercise).GetComponent<ExistingExercise>().exerciseName + ".csv";
            Debug.Log(file_name);
            fcreate = File.Open(file_name, FileMode.Create); // will create the file or overwrite it if it already exists
            sw = new StreamWriter(fcreate);
            playlist_temp_ID++;
        }

        if (record)
        {

            if (handBall.transform.position.x < 10 && handBall.transform.position.x > -10 && handBall.transform.position.y < 10 && handBall.transform.position.y > -10 && handBall.transform.position.z < 0 && handBall.transform.position.z > -10)
            {
                sw.WriteLine(handBall.transform.position.x + ", " + handBall.transform.position.y + ", " + handBall.transform.position.z);
            }

        }

        if (currentTarget < numOfPoints - 1 && targets[currentTarget] == null)
        {
            /*if(currentTarget > 0) {
                //TODO Anything involving a target getting hit recording
                Debug.Log("Middle Target hit");
                sw.WriteLine(handBall.transform.position.x + ", " + handBall.transform.position.y + ", " + handBall.transform.position.z);

            }*/
            currentTarget++;
            targets[currentTarget].transform.GetChild(3).GetComponent<ParticleSystem>().Play();
            targetDepthPlane.transform.position = new Vector3(0, 0, targets[currentTarget].transform.position.z);
        }

        if (currentTarget == numOfPoints - 1 && targets[currentTarget] == null)
        {
            Debug.Log("Last Target hit");

            //TODO Last target destroyed, stop recording
            record = false;
            sw.Close();

            performedExerciseNames.Add(transform.parent.GetChild(1).GetChild(currentExercise).GetComponent<ExistingExercise>().exerciseName);

            if (currentExercise < transform.parent.GetChild(1).childCount - 1)
            {
                currentTarget = 0;
                triggerNextExercise(++currentExercise);
                transform.parent.GetChild(1).GetComponent<PlaylistContentControl>().nextExerciseStart();
                //performedExerciseNames.Add(transform.parent.GetChild(1).GetChild(currentExercise).GetComponent<ExistingExercise>().exerciseName);
            }
            else
            {
                int num_patient = 0;
                int num_therapist = 0;
                Debug.Log("Last exercise completed");
                foreach (string exerciseBlah in performedExerciseNames)
                {
                    Debug.Log(exerciseBlah);
                    int.TryParse(exerciseBlah, out num_therapist);
                    // DTW ANALYSIS
                    dtw(num_therapist, num_patient);
                    Debug.Log("DTW Matlab");
                    num_patient = num_patient + 1;
                }
				try
				{
					string query = "INSERT INTO magni_encounter(encounterID,patientID,sessionID) VALUES (?encounterID, ?patientID, ?sessionID)";
					cmd = new MySqlCommand(query,con);
					
					Debug.Log("Data Base Call");
					MySqlParameter param1 = cmd.Parameters.Add("?encounterID", MySqlDbType.VarChar);
					param1.Value = sessionID.ToString();
					MySqlParameter param2 = cmd.Parameters.Add("?patientID", MySqlDbType.VarChar);
					param2.Value = patientID.ToString();
					MySqlParameter param3 = cmd.Parameters.Add("?sessionID", MySqlDbType.VarChar);
					param3.Value = sessionID.ToString();
					cmd.ExecuteNonQuery();

				}
				catch(Exception e)
				{
					Debug.Log(e.ToString());
				}
                Application.LoadLevel("MainMenu");
            }
        }
    }

	private int findLastSessionNum () {
		try {
			cmd = new MySqlCommand("Select * from magni_session", con);
			rdr = cmd.ExecuteReader();
			if(rdr.HasRows) {
				int sessionNum = 0;
				while(rdr.Read()) {
					sessionNum = Convert.ToInt32(rdr.GetString(0)); //Field: ExerciseName
				}
				rdr.Close();
				Debug.Log("session" + sessionNum);
				return ++sessionNum; //New Exercise ID should be the one after the last one.
			} else { //No Rows - probably a new table
				rdr.Close();
				Debug.Log("session" + 0);
				return 0;
			}
		}
		catch(Exception e) {
			Debug.Log(e.ToString());
		}
		return -1; //Error with accessing the database
	}

}
