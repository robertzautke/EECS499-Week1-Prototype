using System;
using UnityEngine;
using System.Collections;

public class marker : MonoBehaviour {

	public GameObject ui_controller;
	public GameObject back;
	public bool clickedServer = false;
	public bool clickedClient = false;



	void Start () {
		ui_controller = GameObject.Find("UI_Controller");
		back = this.transform.parent.gameObject;
	}

	void Update () {
	
	}

	void OnMouseDown() {
		if (ui_controller.GetComponent<UI_Functions>().serverAlreadyStarted &&
			ui_controller.GetComponent<UI_Functions>().playerTurn == 0 &&
			!clickedServer && !clickedClient) {
			
			ui_controller.GetComponent<UI_Functions>().playerTurn = 1;
			clickedServer = true;
			this.renderer.material.color = Color.blue;

			string s = this.name;
			char markerNumberChar = s[s.Length - 1];
			double markerValueDouble = Char.GetNumericValue(markerNumberChar);
			int markerNumber = Convert.ToInt32(markerValueDouble);
			print(markerNumber);
			back.GetComponent<BackScript>().GameWinConditionCheck(markerNumber, 0);
		 }

		else if (ui_controller.GetComponent<UI_Functions>().clientConnect &&
			ui_controller.GetComponent<UI_Functions>().playerTurn == 1 &&
			!clickedServer && !clickedClient)
		{

			ui_controller.GetComponent<UI_Functions>().playerTurn = 0;
			clickedClient = true;
			this.renderer.material.color = Color.red;

			string s = this.name;
			int markerNumber = Convert.ToInt32(s[s.Length - 1]);
			print(markerNumber);
			back.GetComponent<BackScript>().GameWinConditionCheck(markerNumber, 1);
		}
	}
}
