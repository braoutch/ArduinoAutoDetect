//USE MONO 2.0 INSTEAD OF 2.0SUBSET !!!
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System;
using System.IO;

public class Script : MonoBehaviour
{

	[SerializeField] private UnityEngine.UI.Image connectedButton;

	public List<SerialPort> serialPortList = new List<SerialPort> ();
	//Every SerialPort we can open
	int closedPorts = 0;
	public static bool detected = false;
	//Le code est lu non seulement par unity mais aussi par l'arduino ; les codes peuvent être différents.
	private string code = "206";
	SerialPort monSerialPort;

	public void Connect ()
	{
		Debug.Log ("Recherche de Vibroduino...");
		try {
			monSerialPort = FindOpenPorts (code);
			Write(code);
		}
		catch (Exception e) {
			Debug.Log ("Pas trouvé !");
			Debug.Log (e);

            //Si on veut mettre un bouton qui signale la connexion...
            //connectedButton.color = Color.red;


		}
	}

	SerialPort FindOpenPorts (string arduinoCode)
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
			//Debug.Log (devices [j]);
		}
		//Try to open every port
		Debug.Log ("On essaie d'ouvrir tous les ports");
		for (int i = 0; i < devices.Length; i++) {

			try {
				stream = new SerialPort (devices [i], 9600);
				stream.Open ();
				Debug.Log ("Port ouvert avec succès : " + stream.PortName);
				if (WhichSerialPortAreU (stream, arduinoCode)) {
					return stream;
					detected = true;
				}
			} catch (Exception e) {
				closedPorts++;
				continue;
			}
			if (stream.IsOpen && !devices [i].Contains ("Bluetooth")) {
				Debug.Log ("Port " + i + " is open , named " + devices [i]);
				serialPortList.Add (stream);

				stream.Close ();
				} else
				continue;


			}
			Debug.Log (closedPorts + " ports série fermés.");
			closedPorts = 0;
			return null;
		}

		public bool WhichSerialPortAreU (SerialPort stream, string arduinoCode)
		{

			try {
				stream.ReadTimeout = 150;
				Debug.Log("Lecture du port...");
				string serialReadTest = stream.ReadLine ();
				Debug.Log ("Réception : " + serialReadTest);
				if (String.CompareOrdinal (serialReadTest.Substring (0, 3), arduinoCode) == 0) {
					Debug.Log ("Port ouvert avec succès !");
					connectedButton.color = Color.green;
					return true;

				} 
				else {
					Debug.Log ("Mauvais code : " + serialReadTest.Substring (0, 3) + ", on cherchait le code " + arduinoCode);
					return false;
				}
			} catch (Exception f) {
				Debug.Log ("Echec de la lecture :(");
				Debug.Log (f);
			}
			Debug.Log ("Fin de la détection du port série");
			return false;
		}

		public void Write (string stringToWrite)
		{
			monSerialPort.Write ("#" + stringToWrite + "%");
		}

		public string Read ()
		{
			return  monSerialPort.ReadLine ().Substring (3);
		}

		void OnApplicationQuit ()
		{
			monSerialPort.Close ();
		}
	}


///////////////////////
// CODE DE L'ARDUINO //
///////////////////////
/*
void setup(){
	Serial.begin(9600);
}

void loop(){

	if(started == 0)
	{
    	Serial.println("206");  
    	if (Serial.peek() == '#' && Serial.available() > 0 )
    	{
      		char a=Serial.read();
         	started = 1;
    	}
	}

	if(started == 1)
  	{

	//Là on met le code normal de la loop....
	//Par exemple pour lire : 
		if (Serial.peek() != '#' && Serial.available() > 0 )
    	{
     		char a=Serial.read();
    	}

  		if (Serial.peek() == '#' && Serial.available() > 0) 
  		{ 
    		valeur = Serial.parseInt();
  		}
  	}
 }

 */