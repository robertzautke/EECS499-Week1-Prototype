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
		print("Mouse Down");

		if (ui_controller.GetComponent<UI_Functions>().serverAlreadyStarted &&
		ui_controller.GetComponent<UI_Functions>().playerTurn == 0 &&
		!clickedServer && !clickedClient)
		{
			this.networkView.RPC("networkSignal_OnMouseDown", RPCMode.All, 0);


			string s = this.name;
			char markerNumberChar = s[s.Length - 1];
			double markerValueDouble = Char.GetNumericValue(markerNumberChar);
			int markerNumber = Convert.ToInt32(markerValueDouble);
			print(markerNumber);
			ui_controller.GetComponent<UI_Functions>().networkView.RPC("networkSignal_GameWinConditionCheck", RPCMode.All, markerNumber, 0);
		}

		else if (ui_controller.GetComponent<UI_Functions>().clientConnect &&
			ui_controller.GetComponent<UI_Functions>().playerTurn == 1 &&
			!clickedServer && !clickedClient)
		{

			this.networkView.RPC("networkSignal_OnMouseDown", RPCMode.All, 1);

			string s = this.name;
			char markerNumberChar = s[s.Length - 1];
			double markerValueDouble = Char.GetNumericValue(markerNumberChar);
			int markerNumber = Convert.ToInt32(markerValueDouble);
			print(markerNumber);
			ui_controller.GetComponent<UI_Functions>().networkView.RPC("networkSignal_GameWinConditionCheck", RPCMode.All, markerNumber, 1);
		}

	}

	[RPC]
	void networkSignal_OnMouseDown(int player){
		if (player == 0) { 
			ui_controller.GetComponent<UI_Functions>().playerTurn = 1;
			clickedServer = true;
			this.renderer.material.color = Color.blue;
		}
		else if (player == 1) {
			ui_controller.GetComponent<UI_Functions>().playerTurn = 0;
			clickedClient = true;
			this.renderer.material.color = Color.red;	
		}

	}
}
