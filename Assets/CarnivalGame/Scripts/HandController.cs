using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

public class HandController : MonoBehaviour {

    System.Net.Sockets.TcpClient clientSocket;
    Vector3 m_baseOffset;
    string dataFromClient;
    string[] coordinates;
    Vector3 Barrett;
    FileStream fcreate;
    //int num_exercise = 0; // number of exercise that we record


	public GameObject depthPlane;

	// Use this for initialization
	void Start () {
        clientSocket = new System.Net.Sockets.TcpClient();
        clientSocket.Connect("192.168.1.2", 13131);
	}
	
	// Update is called once per frame
	void Update () {


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

        transform.position = Barrett;

        
        if (Barrett.x < 10 && Barrett.x > -10 && Barrett.x < 10 && Barrett.x > -10 && Barrett.z < 0 && Barrett.z > -10)
        {
            //sw.WriteLine(Barrett.x + ", " + Barrett.y + ", " + Barrett.z);
        }
        
        depthPlane.transform.position = new Vector3(0, 0, transform.position.z);
	}

	/*
    // Stop record
    void dtw()
    {

        System.Net.Sockets.TcpClient clientSocket;
        clientSocket = new System.Net.Sockets.TcpClient();
        clientSocket.Connect("localhost", 13132);
        NetworkStream networkStream = clientSocket.GetStream();

        string dataFromClient = "12";
        Byte[] sendBytes = Encoding.ASCII.GetBytes(dataFromClient);
        networkStream.Write(sendBytes, 0, sendBytes.Length);
        networkStream.Flush();
        //Console.WriteLine(" >> " + dataFromClient);
        //Console.ReadLine();
        System.Threading.Thread.Sleep(1000);

    }
	*/

    // Show Buttons
    void OnGUI()
    {

		/*
        // Make the Start button
        if (GUI.Button(new Rect(20, 40, 80, 20), "Start"))
        {
            num_exercise = num_exercise + 1;
            Record();
        }
        // Make the End button
        if (GUI.Button(new Rect(20, 70, 80, 20), "End"))
        {
            End_record();
        }


        // Make the End button
        if (GUI.Button(new Rect(20, 100, 80, 20), "Analysis"))
        {
            dtw();
        }
		*/


    }
}
