using UnityEngine;
using System.Collections;

public class tileprops : MonoBehaviour
{
		public GameObject x = null;
		public Vector3 temp;
		public  int row;
		public  int col;
		public bool colour;
		public bool isvalidmove = false;
		public bool killmove = false;
		public Color defcol, selectcol, validcol, killcol;
		public GameObject top = null;
		public GameObject chessboard ;
		public int castlingtile = 0;
		public bool moved = false, iscastled = false;
		public int a=0;
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
				else if (isvalidmove && killmove)
						gameObject.renderer.material.color = killcol;
				else if (castlingtile != 0)
						gameObject.renderer.material.color = Color.yellow;
				else if (isvalidmove == true)
						gameObject.renderer.material.color = validcol;
				else 
						gameObject.renderer.material.color = defcol;
		/*if(a==1&&iscastled==true&&moved==true){
			chessboard.GetComponent<undo> ().addinstance ();a=0;}*/
				if (moved == true) {
						if(a==1&&iscastled==false){
			chessboard.GetComponent<undo> ().addinstance ();
				a=0;}
						moveme (x, this.gameObject, temp, false, true);
				}
		if (castlingtile == 1 && iscastled == true) {

			moveme (chessboard.GetComponent<chessboard> ().board [row, 0].GetComponent<tileprops> ().top, chessboard.GetComponent<chessboard> ().board [row, col + 1], new Vector3 (gameObject.transform.position.x + 2, gameObject.transform.position.y, gameObject.transform.position.z), true, false);
		}
		if (castlingtile == 2 && iscastled == true) {

			moveme (chessboard.GetComponent<chessboard> ().board [row, 7].GetComponent<tileprops> ().top, chessboard.GetComponent<chessboard> ().board [row, col - 1], new Vector3 (gameObject.transform.position.x - 2, gameObject.transform.position.y, gameObject.transform.position.z), true, false);
		}
		if (castlingtile == 3) {
					
		}
		
		}

		void moveme (GameObject x, GameObject tile, Vector3 temp, bool iscastleds, bool moveds)
		{
				x.transform.position = Vector3.Lerp (x.transform.position, temp, 0.3f);
				
				if (x.transform.position == temp && moveds == true) {
						if (killmove) {
								for (int i =0; i<32; i++) {
										if (chessboard.GetComponent<chessboard> ().pieces [i] == top) {
												chessboard.GetComponent<chessboard> ().pieces [i] = null;
												break;
										}
								}
								Destroy (top);
						}
						moved = false;
						chessboard.GetComponent<chessboard> ().board [tile.GetComponent<tileprops> ().row, tile.GetComponent<tileprops> ().col].GetComponent<tileprops> ().top = x;
						x.GetComponent<chesspiece> ().movecount++;
						x.GetComponent<chesspiece> ().row = tile.GetComponent<tileprops> ().row;
						x.GetComponent<chesspiece> ().col = tile.GetComponent<tileprops> ().col;
						x.GetComponent<chesspiece> ().selected = false;
						x.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().top = null;
						x.GetComponent<chesspiece> ().tile = chessboard.GetComponent<chessboard> ().board [tile.GetComponent<tileprops> ().row, tile.GetComponent<tileprops> ().col];
						chessboard.GetComponent<chessboard> ().selectedpiece = null;
						
						foreach (GameObject y in chessboard.GetComponent<chessboard>().tiles) {
								y.GetComponent<tileprops> ().isvalidmove = y.GetComponent<tileprops> ().killmove = false;
								if (!y.GetComponent<tileprops> ().iscastled)
										y.GetComponent<tileprops> ().castlingtile = 0;
						}
						chessboard.GetComponent<chessboard> ().turn = !chessboard.GetComponent<chessboard> ().turn;
			for (int l=0; l<32; l++) {
				GameObject z = chessboard.GetComponent<chessboard> ().pieces [l];
				
				if (z != null ) {
					z.GetComponent<chesspiece> ().validmoves ();
					z.GetComponent<chesspiece> ().newcheckmove ();
					for (int i=0; i<64; i++) {
						z.GetComponent<chesspiece> ().legalmoves [i] = z.GetComponent<chesspiece> ().validmoveinstance [i];
						
						z.GetComponent<chesspiece> ().legalkillmoves [i] = z.GetComponent<chesspiece> ().killmoveinstance [i];
						
					}
					
				}
			}for (int l=0; l<32; l++) {
				GameObject z = chessboard.GetComponent<chessboard> ().pieces [l];
				
				if (z != null ) {
					z.GetComponent<chesspiece> ().validmoves ();
					z.GetComponent<chesspiece> ().newcheckmove ();
					for (int i=0; i<64; i++) {
						z.GetComponent<chesspiece> ().legalmoves [i] = z.GetComponent<chesspiece> ().validmoveinstance [i];
						
						z.GetComponent<chesspiece> ().legalkillmoves [i] = z.GetComponent<chesspiece> ().killmoveinstance [i];
						
					}
					
				}
			}

				
				}
				if (x.transform.position == temp && iscastleds == true) {
						iscastled = false;
						chessboard.GetComponent<chessboard> ().board [tile.GetComponent<tileprops> ().row, tile.GetComponent<tileprops> ().col].GetComponent<tileprops> ().top = x;
						x.GetComponent<chesspiece> ().movecount++;
						x.GetComponent<chesspiece> ().row = tile.GetComponent<tileprops> ().row;
						x.GetComponent<chesspiece> ().col = tile.GetComponent<tileprops> ().col;
						x.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().top = null;
						x.GetComponent<chesspiece> ().tile = chessboard.GetComponent<chessboard> ().board [tile.GetComponent<tileprops> ().row, tile.GetComponent<tileprops> ().col];
						chessboard.GetComponent<chessboard> ().selectedpiece = null;
						castlingtile = 0;
				}
		
		
		
		}
	
		void OnMouseDown ()
		{
				if (isvalidmove) {
						if (!killmove) {
								a=1;
								x = chessboard.GetComponent<chessboard> ().selectedpiece;
								temp = x.transform.position;
								temp.x = gameObject.transform.position.x;
								temp.z = gameObject.transform.position.z;
								moved = true;
								if (castlingtile != 0){
					iscastled = true;
				}
								else
										iscastled = false;
								
						}


				}
		}
}
