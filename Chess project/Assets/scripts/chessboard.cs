using UnityEngine;
using System.Collections;

public class chessboard : MonoBehaviour
{
	public int checkmate =0 ; //o for nonr; 1 if black looses and 2 if white looses
	public int stalemate = 0; //0 for none; 1 if black has no legal moves; 2 if white has no legal moves;
		public  GameObject[] tiles;
	public GameObject[] kings;
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
				kings = new GameObject[2];
				foreach (GameObject x in tiles) {
						board [x.GetComponent<tileprops> ().row, x.GetComponent<tileprops> ().col] = x;
				}
				foreach (GameObject x in pieces) {
						if (x != null) {
								x.GetComponent<chesspiece> ().tile = board [x.GetComponent<chesspiece> ().row, x.GetComponent<chesspiece> ().col];
								board [x.GetComponent<chesspiece> ().row, x.GetComponent<chesspiece> ().col].GetComponent<tileprops> ().top = x;
								board [x.GetComponent<chesspiece> ().row, x.GetComponent<chesspiece> ().col].GetComponent<tileprops> ().haspiece = true;
								if (x.GetComponent<chesspiece> ().ptype == 6) {
										if (x.GetComponent<chesspiece> ().pcolour == true)
												kings [0] = x;
												
					else if(x.GetComponent<chesspiece> ().pcolour == false)
												kings [1] = x;
								}
						}

				}
		}
	
		// Update is called once per frame
		void Update ()
		{
				foreach (GameObject x in pieces) {
						if (x != null) {
								
								board [x.GetComponent<chesspiece> ().row, x.GetComponent<chesspiece> ().col].GetComponent<tileprops> ().haspiece = true;
								if (x.GetComponent<chesspiece> ().selected)
										selectedpiece = x;

						}

				}

			if (!kings [0].GetComponent<chesspiece> ().checkkingmove (kings [0], kings [0].GetComponent<chesspiece> ().tile))
						check = 1;
				else if (!kings [1].GetComponent<chesspiece> ().checkkingmove (kings [1], kings [1].GetComponent<chesspiece> ().tile))
						check = 2;
				else
						check = 0;
		stalemate = 1;
		foreach (GameObject x in pieces) {
					if(x!=null){
					if(x.GetComponent<chesspiece>().pcolour==true)
			{
				
				x.GetComponent<chesspiece>().validmoves();
				if(x.GetComponent<chesspiece>().validcount!=0||x.GetComponent<chesspiece>().killcount!=0){
					stalemate = 0;
					break;
					}}
			}
		}
		if (stalemate == 0) {
			stalemate = 2;
			foreach (GameObject x in pieces) {
				if(x!=null){
				if(x.GetComponent<chesspiece>().pcolour==false)
				{
					x.GetComponent<chesspiece>().validmoves();
					if(x.GetComponent<chesspiece>().validcount!=0||x.GetComponent<chesspiece>().killcount!=0){
						stalemate = 0;
						break;
						}					}
				}
			}
		}
		if (stalemate != 0) {
				if(stalemate==1&&check==1)
				checkmate =1;
				else if(stalemate==2&&check==2)
				checkmate=2;
				}
				
		}
		
	
}
