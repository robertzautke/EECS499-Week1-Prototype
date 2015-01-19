using UnityEngine;
using System.Collections;

public class BackScript : MonoBehaviour {

	[SerializeField]
	public GameObject[] markerArray;

	BoardSpace[] bs;
	private class BoardSpace
	{
		public bool taken;
		public int player;
	}


	// Use this for initialization
	void Start () {
		bs = new BoardSpace[9];
		for (int i = 0; i < 9; i++) {
			bs[i] = new BoardSpace();
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void GameWinConditionCheck(int i, int playerNum)
	{

		bs[i].taken = true;
		bs[i].player = playerNum;

		if (bs[0].taken && bs[0].player == 0 && bs[1].taken && bs[1].player == 0 && bs[2].taken && bs[2].player == 0 ||
			bs[3].taken && bs[3].player == 0 && bs[4].taken && bs[4].player == 0 && bs[5].taken && bs[5].player == 0 ||
			bs[6].taken && bs[6].player == 0 && bs[7].taken && bs[7].player == 0 && bs[8].taken && bs[8].player == 0 ||
			bs[0].taken && bs[0].player == 0 && bs[3].taken && bs[3].player == 0 && bs[6].taken && bs[6].player == 0 ||
			bs[1].taken && bs[1].player == 0 && bs[4].taken && bs[4].player == 0 && bs[7].taken && bs[7].player == 0 ||
			bs[2].taken && bs[2].player == 0 && bs[5].taken && bs[5].player == 0 && bs[8].taken && bs[8].player == 0 ||
			bs[0].taken && bs[0].player == 0 && bs[4].taken && bs[4].player == 0 && bs[8].taken && bs[8].player == 0 ||
			bs[2].taken && bs[2].player == 0 && bs[4].taken && bs[4].player == 0 && bs[6].taken && bs[6].player == 0)
		{

			print("Player 1 wins!");

		}
		else if (bs[0].taken && bs[0].player == 1 && bs[1].taken && bs[1].player == 1 && bs[2].taken && bs[2].player == 1 ||
		  bs[3].taken && bs[3].player == 1 && bs[4].taken && bs[4].player == 1 && bs[5].taken && bs[5].player == 1 ||
		  bs[6].taken && bs[6].player == 1 && bs[7].taken && bs[7].player == 1 && bs[8].taken && bs[8].player == 1 ||
		  bs[0].taken && bs[0].player == 1 && bs[3].taken && bs[3].player == 1 && bs[6].taken && bs[6].player == 1 ||
		  bs[1].taken && bs[1].player == 1 && bs[4].taken && bs[4].player == 1 && bs[7].taken && bs[7].player == 1 ||
		  bs[2].taken && bs[2].player == 1 && bs[5].taken && bs[5].player == 1 && bs[8].taken && bs[8].player == 1 ||
		  bs[0].taken && bs[0].player == 1 && bs[4].taken && bs[4].player == 1 && bs[8].taken && bs[8].player == 1 ||
		  bs[2].taken && bs[2].player == 1 && bs[4].taken && bs[4].player == 1 && bs[6].taken && bs[6].player == 1)
		{

			print("Player 2 wins!");

		}
	}
}
