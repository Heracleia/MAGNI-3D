using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class GestureDraw : MonoBehaviour {
	public string nameOfFile;
	string[] tempValues;
	List<Vector3> coordinates = new List<Vector3>();
	GameObject point;
	Vector3 pointScale = new Vector3(0.1f,0.1f,0.1f);

	// Use this for initialization
	void Awake () {
string filePath = nameOfFile + ".csv";
		try {
			if(File.Exists(filePath)) {
				using(FileStream fs = new FileStream(filePath, FileMode.Open)) {
					using(StreamReader sr = new StreamReader(fs)) {
						while(sr.Peek() >= 0) {
							tempValues = sr.ReadLine().Split(',');
							coordinates.Add(new Vector3(Convert.ToSingle(tempValues[0]),Convert.ToSingle(tempValues[1]),Convert.ToSingle(tempValues[2])));
						}
					}
				}

				foreach(Vector3 coords in coordinates) {
					point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					point.transform.position = coords;
					point.transform.localScale = pointScale;
				}
			} else {
				Debug.Log("Did not find the file");
			}
		} catch (Exception e) {
			Debug.Log ("File Exception: " + e.ToString());
		}
	}
}
