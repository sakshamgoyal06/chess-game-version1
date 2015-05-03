using UnityEngine;
using System.Collections;

public class chessboard : MonoBehaviour
{
		public  GameObject[] tiles;
		public  GameObject[,] board = new GameObject[8, 8];
		public  GameObject[] pieces;
		public GameObject selectedpiece = null;
		public int check = 0; //0 no check, 1 black in check, 2 white in check 
		public bool turn = false; //false for white; true for black
		// Use this for initialization
		void Start ()
		{
				tiles = GameObject.FindGameObjectsWithTag ("tile");	//the directory of tiles in the board
				pieces = GameObject.FindGameObjectsWithTag ("piece");	//the pieces in the board
				foreach (GameObject x in tiles) {
						board [x.GetComponent<tileprops> ().row, x.GetComponent<tileprops> ().col] = x;
				}
				foreach (GameObject x in pieces) {
						if (x != null) {
								x.GetComponent<chesspiece> ().tile = board [x.GetComponent<chesspiece> ().row, x.GetComponent<chesspiece> ().col];
								board [x.GetComponent<chesspiece> ().row, x.GetComponent<chesspiece> ().col].GetComponent<tileprops> ().top = x;
								board [x.GetComponent<chesspiece> ().row, x.GetComponent<chesspiece> ().col].GetComponent<tileprops> ().haspiece = true;
						}

				}
		}
	
		// Update is called once per frame
		void Update ()
		{
				foreach (GameObject x in pieces) {
						if (x != null) {
								x.GetComponent<chesspiece> ().tile = board [x.GetComponent<chesspiece> ().row, x.GetComponent<chesspiece> ().col];
								board [x.GetComponent<chesspiece> ().row, x.GetComponent<chesspiece> ().col].GetComponent<tileprops> ().top = x;
								board [x.GetComponent<chesspiece> ().row, x.GetComponent<chesspiece> ().col].GetComponent<tileprops> ().haspiece = true;
								if (x.GetComponent<chesspiece> ().selected)
										selectedpiece = x;
								if (x.GetComponent<chesspiece> ().ptype == 6)
								if (!x.GetComponent<chesspiece> ().checkkingmove (x, x.GetComponent<chesspiece> ().tile)) {
										if (x.GetComponent<chesspiece> ().pcolour)
												check = 1;
										else
												check = 2;
								}
				else check =0;
						}

				}
				


		}
		
	
}
