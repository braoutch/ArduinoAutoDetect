//IF ITS NOT WORKING TRY TO USE MONO 2.0 OR 2.0SUBSET !!!
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System;
using System.IO;

public class Script : MonoBehaviour
{
	public SerialPort goodStream;
	public int arduinoCode;
	public List<SerialPort> serialPortList = new List<SerialPort> ();
	int portfermes = 0;

	// Use this for initialization
	void Awake ()
	{
		SerialPort stream;
		var devices = SerialPort.GetPortNames ();
	
		if (devices.Length == 0) {
			// try manual enumeration
			devices = System.IO.Directory.GetFiles ("/dev/");
			Debug.Log ("No ports found with GetPortNames");
		}
	
		Debug.Log (devices.Length + " port(s) série trouvé(s)");
		
		for (int j = 0; j < devices.Length; j++) {
			Debug.Log (devices [j]);
		}
		//On tente d'ouvrir tous les ports
		for (int i = 0; i < devices.Length; i++) {

			try {
				stream = new SerialPort (devices [i], 115200);
				stream.Open ();
			} catch (Exception e) {
				portfermes++;
				continue;
			}
			if (stream.IsOpen) {
				Debug.Log ("Le port " + i + " est ouvert , son nom est " + devices [i]);
				serialPortList.Add (stream);
				stream.Close ();
			} else
				continue;
			//Debug.Log ("initialisation réussie pour i = " + i);
		

		}
		//On teste tous les ports qu'on a réussi à ouvrir
		Debug.Log (portfermes + " ports fermés.");
	}

	public void Detecter ()
	{
		for (int i = 0; i < serialPortList.Count; i++) {

			try {
				serialPortList [i].Open ();
				serialPortList [i].ReadTimeout = 500;
				string serialReadTest = serialPortList [i].ReadLine ();
				if (int.Parse (serialReadTest) == arduinoCode) {
					Debug.Log ("MUHAHAHAHAHA c'est le bon code");
					goodStream = serialPortList [i];
					break;
	
				} else {
					serialPortList [i].Close ();
					Debug.Log ("Mauvais code : " + serialReadTest);
				}
			} catch (TimeoutException) {
				Debug.Log ("oh bah il parle pas celui-là !");
				serialPortList [i].Close ();
				continue;
			}
		}
	}
}
