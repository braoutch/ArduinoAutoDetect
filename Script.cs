//IF ITS NOT WORKING TRY TO USE MONO 2.0 OR 2.0SUBSET !!!
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System;

public class Script : MonoBehaviour
{
	public SerialPort goodStream;
	public string arduinoCode = "code";
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
			} else
				continue;
			Debug.Log ("initialisation réussie pour i = " + i);
		

		}
		//On teste tous les ports qu'on a réussi à ouvrir
		Debug.Log (portfermes + " ports fermés.");
	}

	public void Detecter ()
	{
		string serialReadTest;
		for (int i = 0; i < serialPortList.Count; i++) {
			
			try {
				serialReadTest = serialPortList [i].ReadLine ();
			} catch (Exception f) {
				Debug.Log ("oh bah ça marche pas");
				continue;
			}
			if (serialReadTest == arduinoCode) {
				Debug.Log ("MUHAHAHAHAHA c'est le bon code");
				goodStream = serialPortList [i];
				break;

			} else {
				serialPortList [i].Close ();
				Debug.Log ("Mauvais code !");
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{

	}
}
