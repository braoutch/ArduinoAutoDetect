using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;

public class Script : MonoBehaviour {

	public int i=0;
	public SerialPort[] streamTest = new SerialPort[SerialPort.GetPortNames ().Length];
	public string serialReadTest;
	public string goodPort;
	public string arduinoCode = "code";

	// Use this for initialization
	void Awake () {

		var devices = SerialPort.GetPortNames ();
		Debug.Log (devices.Length + " port(s) série trouvé(s)");
		for (int j = 0; j < devices.Length; j++) {
			Debug.Log (devices [j]);
		}
		//On tente d'ouvrir tous les ports
		foreach (string thisDevice in devices) {
			try {
				streamTest [i] = new SerialPort (thisDevice, 115200);
				streamTest [i].Open ();
				if (streamTest [i].IsOpen) {
					Debug.Log ("Le port " + i + " est ouvert !");	
				}
				Debug.Log ("initialisation réussie pour i = " + i);
				try{
					serialReadTest = streamTest [i].ReadLine ();
				}
				catch(Exception f)
				{
					Debug.Log("oh bah ça marche pas");
				}
				if (serialReadTest == arduinoCode) {
					Debug.Log ("MUHAHAHAHAHA c'est le bon code");
					goodPort = thisDevice;
					} else {
						streamTest [i].Close ();
						Debug.Log ("Mauvais code !");
					}
					} catch (Exception e) {
						Debug.Log ("raté pour i = " + i);
					}
					i++;
				}
		//On teste tous les ports qu'on a réussi à ouvrir
	}
	
	// Update is called once per frame
	void Update () {

	}
}
