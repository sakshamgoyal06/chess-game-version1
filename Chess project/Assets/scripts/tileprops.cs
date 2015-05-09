using UnityEngine;
using System.Collections;

public class tileprops : MonoBehaviour
{
		public  int row;
		public  int col;
		public bool colour;
		public bool haspiece = false;
		public bool isvalidmove = false;
		public bool killmove = false;
		public Color defcol, selectcol, validcol, killcol;
		public GameObject top = null;
		public GameObject chessboard ;
		GameObject x = null;
		Vector3 temp;
		public int castlingtile = 0;
		// Use this for initialization
		void Awake ()
		{
				row = ((int)gameObject.transform.position.z) / (-2);
				col = ((int)gameObject.transform.position.x) / 2;
				if ((row % 2 == 0 && col % 2 == 0) || (row % 2 == 1 && col % 2 == 1)) {
						colour = false;
				} else
						colour = true;
				defcol = gameObject.renderer.material.color;
				selectcol = Color.magenta;
				killcol = Color.blue;
				validcol = Color.green;
				chessboard = GameObject.FindGameObjectWithTag ("chessboard");

		}
	
		// Update is called once per frame
		void Update ()
		{

				if (top != null && top.GetComponent<chesspiece> ().selected)
						gameObject.renderer.material.color = selectcol;
				else if (haspiece && isvalidmove && killmove)
						gameObject.renderer.material.color = killcol;
				
				else if (castlingtile != 0)
						gameObject.renderer.material.color = Color.yellow;
		else if (isvalidmove == true)
			gameObject.renderer.material.color = validcol;
		else 
			gameObject.renderer.material.color = defcol;


		}

		void OnMouseDown ()
		{
				if (isvalidmove) {
						if (!killmove) {

								x = chessboard.GetComponent<chessboard> ().selectedpiece;
								temp = x.transform.position;
								temp.x = gameObject.transform.position.x;
								temp.z = gameObject.transform.position.z;
								x.transform.position = temp;
								chessboard.GetComponent<chessboard> ().board [row, col].GetComponent<tileprops> ().top = x;
								x.GetComponent<chesspiece> ().movecount++;
								x.GetComponent<chesspiece> ().row = row;
								x.GetComponent<chesspiece> ().col = col;
								x.GetComponent<chesspiece> ().selected = false;
								x.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().haspiece = false;
								x.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().top = null;
								x.GetComponent<chesspiece> ().tile = chessboard.GetComponent<chessboard> ().board [row, col];

								chessboard.GetComponent<chessboard> ().selectedpiece = null;
								x = null;
								chessboard.GetComponent<chessboard> ().turn = !chessboard.GetComponent<chessboard> ().turn;
								foreach (GameObject y in chessboard.GetComponent<chessboard>().tiles) {
										y.GetComponent<tileprops> ().haspiece = y.GetComponent<tileprops> ().isvalidmove = y.GetComponent<tileprops> ().killmove = false;
								}

						}
						if (castlingtile == 1) {
								x = chessboard.GetComponent<chessboard> ().board [row, 0].GetComponent<tileprops> ().top;
					
								temp = x.transform.position;
								temp.x = gameObject.transform.position.x + 2;
								temp.z = gameObject.transform.position.z;
								x.transform.position = temp;
								x.GetComponent<chesspiece> ().movecount++;
								x.GetComponent<chesspiece> ().row = row;
								x.GetComponent<chesspiece> ().col = col + 1;
								x.GetComponent<chesspiece> ().selected = false;
								x.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().haspiece = false;
								x.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().top = null;
				x.GetComponent<chesspiece> ().tile = chessboard.GetComponent<chessboard> ().board[row,col+1];
			chessboard.GetComponent<chessboard> ().board[row,col+1].GetComponent<tileprops>().top=x;
				castlingtile = 0;
						}
						if (castlingtile == 2) {
								x = chessboard.GetComponent<chessboard> ().board [row, 7].GetComponent<tileprops> ().top;
								temp = x.transform.position;
								temp.x = gameObject.transform.position.x - 2;
								temp.z = gameObject.transform.position.z;
								x.transform.position = temp;
								x.GetComponent<chesspiece> ().movecount++;
								x.GetComponent<chesspiece> ().row = row;
								x.GetComponent<chesspiece> ().col = col - 1;
								x.GetComponent<chesspiece> ().selected = false;
								x.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().haspiece = false;
								x.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().top = null;
				x.GetComponent<chesspiece> ().tile = chessboard.GetComponent<chessboard> ().board[row,col-1];
				chessboard.GetComponent<chessboard> ().board[row,col-1].GetComponent<tileprops>().top=x;
				castlingtile = 0;
						}
				}
		}
}
