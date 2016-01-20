//USE MONO 2.0 INSTEAD OF 2.0SUBSET !!!
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System;
using System.IO;

public class ArduinoAutoDetect : MonoBehaviour
{
	public List<SerialPort> serialPortList = new List<SerialPort> ();
	//Every SerialPort we can open
	int closedPorts = 0;

	void Start ()
	{
		SerialPort monserialport = FindOpenPorts (206);
	}

	SerialPort FindOpenPorts (int arduinoCode)
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
		//Try to open every port
		for (int i = 0; i < devices.Length; i++) {

			try {
				stream = new SerialPort (devices [i], 115200);
				stream.Open ();
				if (WhichSerialPortAreU (stream, arduinoCode)) {
					return stream;
				}
			} catch (Exception e) {
				closedPorts++;
				continue;
			}
			if (stream.IsOpen) {
				Debug.Log ("Port " + i + " is open , named " + devices [i]);
				serialPortList.Add (stream);
				stream.Close ();
			} else
				continue;


		}
		Debug.Log (closedPorts + " closed ports.");
		Debug.Log ("Nothing found !");
		return null;
	}

	public bool WhichSerialPortAreU (SerialPort stream, int arduinoCode)
	{
		
		try {
			stream.ReadTimeout = 500;
			string serialReadTest = stream.ReadLine ();
			if (int.Parse (serialReadTest) == arduinoCode) {
				Debug.Log ("here you are !");
				return true;

			} else {
				Debug.Log ("Bad code : " + serialReadTest);
			}
		} catch (TimeoutException) {
			Debug.Log ("Timeout !");
		}

		return false;
	}
}
