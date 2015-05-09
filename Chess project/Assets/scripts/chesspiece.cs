using UnityEngine;
using System.Collections;

public class chesspiece : MonoBehaviour //inheriting from chessboard
{
		public int row, col, movecount = 0;	//basic variables for a chesspiece
		public bool pcolour;				//colour (black true) (white false)
		public GameObject tile;				//tile the piece is on
		public bool selected = false;		//is the piece selected?
		public bool alive = true;			//is the piece alive
		public int ptype;					//(1-pawn)(2-bishop)(3-castle)(4-knight)(5-queen)(6-king)	
		Color defcol;						//default material colour of the piece
		Color selectcol;					//the colour when it is selected
		public static GameObject chessboard;
		Vector3 temp;
		GameObject x, a;
		public GameObject[] validmoveinstance;
		public GameObject[] killmoveinstance ;
		public int validcount = 0;
		public int killcount = 0;
		
		// Use this for initialization
		void Awake ()
		{
				validmoveinstance = new GameObject[64];
				killmoveinstance = new GameObject[64];
				chessboard = GameObject.FindGameObjectWithTag ("chessboard");
				defcol = gameObject.renderer.material.color;
				selectcol = new Color (0.9f, 0.5f, 0.7f, 0.5f);
				row = ((int)gameObject.transform.position.z) / (-2);
				col = ((int)gameObject.transform.position.x) / 2;
				for (int i=0; i<16; i++) {
						validmoveinstance [i] = killmoveinstance [i] = null;
				}
		
		}
		// Update is called once per frame
		void Update ()
		{
				if (selected)
						gameObject.renderer.material.color = selectcol;
				else
						gameObject.renderer.material.color = defcol;
		}

		void OnMouseDown ()
		{
				if (selected == false && tile.GetComponent<tileprops> ().killmove == true) {
			
						x = chessboard.GetComponent<chessboard> ().selectedpiece;
						temp = x.transform.position;
						temp.x = gameObject.transform.position.x;
						temp.z = gameObject.transform.position.z;
						chessboard.GetComponent<chessboard> ().board [row, col].GetComponent<tileprops> ().top = x;

						int i = 0;
						foreach (GameObject y in chessboard.GetComponent<chessboard>().pieces) {
								if (y == this.gameObject) {
										chessboard.GetComponent<chessboard> ().pieces [i] = null;
										break;
								} else
										i++;
						}
						Destroy (this.gameObject);
						x.GetComponent<chesspiece> ().movecount++;
						x.GetComponent<chesspiece> ().row = row;
						x.GetComponent<chesspiece> ().col = col;
						x.GetComponent<chesspiece> ().selected = false;
						x.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().haspiece = false;
						x.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().top = null;
						x.transform.position = temp;
						x.GetComponent<chesspiece> ().tile = chessboard.GetComponent<chessboard> ().board [row, col];
						chessboard.GetComponent<chessboard> ().selectedpiece = null;
						x = null;
						chessboard.GetComponent<chessboard> ().turn = !chessboard.GetComponent<chessboard> ().turn;
						chessboard.GetComponent<chessboard> ().selectedpiece = null;
						foreach (GameObject y in chessboard.GetComponent<chessboard>().tiles) {
								y.GetComponent<tileprops> ().haspiece = y.GetComponent<tileprops> ().isvalidmove = y.GetComponent<tileprops> ().killmove = false;
						}
						foreach (GameObject piece in chessboard.GetComponent<chessboard>().pieces) {
								if (piece != null)
										piece.GetComponent<chesspiece> ().selected = false;
						}
				} else if (selected == false && pcolour == chessboard.GetComponent<chessboard> ().turn) {
						
						foreach (GameObject piece in chessboard.GetComponent<chessboard>().tiles) {
								piece.GetComponent<tileprops> ().isvalidmove = false;
								piece.GetComponent<tileprops> ().killmove = false;
						}
						foreach (GameObject piece in chessboard.GetComponent<chessboard>().pieces) {
								if (piece != null && piece != this.gameObject)
										piece.GetComponent<chesspiece> ().selected = false;
						}

						selected = true;
						validmoves ();
					
						
				}
			

		}

		public void validmoves ()
		{
				for (int i=0; i<64; i++) {
						validmoveinstance [i] = killmoveinstance [i] = null;
				}
				validcount = 0;
				killcount = 0;
				switch (ptype) {
				case 1:
						validpawnmoves ();
						break;
				case 2:
						bishopmoves ();
						break;
				case 3:
						castlemoves ();
						break;
				case 4:
						knightmoves ();
						break;
				case 5:
						queenmoves ();
						break;
				case 6:
						kingmove ();
						break;
				}
			
				if (ptype != 6 && selected == true) {
						
						for (int i=0; i<64; i++) {
								if (validmoveinstance [i] != null) {
										GameObject temp2 = validmoveinstance [i].GetComponent<tileprops> ().top;
										validmoveinstance [i].GetComponent<tileprops> ().top = this.gameObject;
										tile.GetComponent<tileprops> ().top = null;
										foreach (GameObject x in chessboard.GetComponent<chessboard>().pieces) {
												if (x != null)
												if (x.GetComponent<chesspiece> ().pcolour == this.pcolour && x.GetComponent<chesspiece> ().ptype == 6) {
														if (!checkkingmove (x, x.GetComponent<chesspiece> ().tile)) {
																validmoveinstance [i].GetComponent<tileprops> ().top = temp2;
																validmoveinstance [i] = null;
														} else
																validmoveinstance [i].GetComponent<tileprops> ().top = temp2;
														break;
												}
										}
										tile.GetComponent<tileprops> ().top = this.gameObject;
								}
						}
						for (int i=0; i<64; i++) {

								if (killmoveinstance [i] != null) {
										int l = 0;
										GameObject temp2 = killmoveinstance [i].GetComponent<tileprops> ().top;
										for (l=0; l<32; l++) {
												if (chessboard.GetComponent<chessboard> ().pieces [l] == temp2) {
														chessboard.GetComponent<chessboard> ().pieces [l] = null;
														
														break;
												}
										}
										killmoveinstance [i].GetComponent<tileprops> ().top = this.gameObject;
										tile.GetComponent<tileprops> ().top = null;
										foreach (GameObject x in chessboard.GetComponent<chessboard>().pieces) {
												if (x != null)
												if (x.GetComponent<chesspiece> ().pcolour == this.pcolour && x.GetComponent<chesspiece> ().ptype == 6) {
														if (!checkkingmove (x, x.GetComponent<chesspiece> ().tile)) {

																killmoveinstance [i].GetComponent<tileprops> ().top = temp2;
																killmoveinstance [i] = null;
														} else {
																killmoveinstance [i].GetComponent<tileprops> ().top = temp2;
														}
														break;
												}
										}
										tile.GetComponent<tileprops> ().top = this.gameObject;
										chessboard.GetComponent<chessboard> ().pieces [l] = temp2;
								}
						}
				}
		validcount = 0;;killcount = 0;
		for (int i =0; i<64; i++) {
			if(killmoveinstance[i]!=null)killcount++;
			if(validmoveinstance[i]!=null)validcount++;
			
		}
	
	
				if (selected) {
						for (int i=0; i<64; i++) {
								if (killmoveinstance [i] != null)
									
										killmoveinstance [i].GetComponent<tileprops> ().isvalidmove = killmoveinstance [i].GetComponent<tileprops> ().killmove = true;
						}
						for (int i=0; i<64; i++) {
								if (validmoveinstance [i] != null)
										
										validmoveinstance [i].GetComponent<tileprops> ().isvalidmove = true;
						}
				}
		}

		public void validpawnmoves ()
		{

				if (pcolour) {
						int i;

						if (movecount == 0)
								i = 2;
						else
								i = 1;
						for (int j=1; j<=i; j++) {
								if (row + j < 8) {
										GameObject a = chessboard.GetComponent<chessboard> ().board [row + j, col];
										if (a.gameObject.GetComponent<tileprops> ().top == null) {
												validmoveinstance [validcount] = a;
												validcount++;
										} else
												break;
								}
						}

						if (row + 1 < 8 && col - 1 > -1) {
								a = chessboard.GetComponent<chessboard> ().board [row + 1, col - 1];
								if (a.gameObject.GetComponent<tileprops> ().top != null) {
										if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != this.pcolour) {
												killmoveinstance [killcount] = a;
												killcount++;
												validmoveinstance [validcount] = a;
												validcount++;
										}
								}
						}
						if (row + 1 < 8 && col + 1 < 8) {
								a = chessboard.GetComponent<chessboard> ().board [row + 1, col + 1];
								if (a.gameObject.GetComponent<tileprops> ().top != null) {
										if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != this.pcolour) {
												killmoveinstance [killcount] = a;
												killcount++;
												validmoveinstance [validcount] = a;
												validcount++;
										}
								}
						}
				
				}
				if (!pcolour) {
						int i;
			
						if (movecount == 0)
								i = 2;
						else
								i = 1;
						for (int j=1; j<=i; j++) {
								if (row - j > -1) {
										a = chessboard.GetComponent<chessboard> ().board [row - j, col];
										if (a.gameObject.GetComponent<tileprops> ().top == null) {
												validmoveinstance [validcount] = a;
												validcount++;
										} else
												break;
								}
						}
			
						if (row - 1 > -1 && col - 1 > -1) {
								a = chessboard.GetComponent<chessboard> ().board [row - 1, col - 1];
								if (a.gameObject.GetComponent<tileprops> ().top != null) {
										if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != this.pcolour) {
												killmoveinstance [killcount] = a;
												killcount++;
												validmoveinstance [validcount] = a;
												validcount++;
										}
								}
						}
						if (row - 1 > -1 && col + 1 < 8) {
								a = chessboard.GetComponent<chessboard> ().board [row - 1, col + 1];
								if (a.gameObject.GetComponent<tileprops> ().top != null) {
										if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != this.pcolour) {
												killmoveinstance [killcount] = a;
												killcount++;
												validmoveinstance [validcount] = a;
												validcount++;
										}
								}
						}
			
				}
		}

		public void castlemoves ()
		{
				for (int j=1; row-j>-1; j++) {
				
						a = chessboard.GetComponent<chessboard> ().board [row - j, col];
						if (a.gameObject.GetComponent<tileprops> ().top == null) {
								validmoveinstance [validcount] = a;
								validcount++;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
								{
										killmoveinstance [killcount] = a;
										killcount++;
										validmoveinstance [validcount] = a;
										validcount++;}
								break;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour == pcolour)
								break;
							
				}
				for (int j=1; row+j<8; j++) {
			
						a = chessboard.GetComponent<chessboard> ().board [row + j, col];
						if (a.gameObject.GetComponent<tileprops> ().top == null) {
								validmoveinstance [validcount] = a;
								validcount++;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
								{
										killmoveinstance [killcount] = a;
										killcount++;
										validmoveinstance [validcount] = a;
										validcount++;}
								break;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour == pcolour)
								break;
			
				}
				for (int j=1; col-j>-1; j++) {
			
						a = chessboard.GetComponent<chessboard> ().board [row, col - j];
						if (a.gameObject.GetComponent<tileprops> ().top == null) {
								validmoveinstance [validcount] = a;
								validcount++;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
								{
										killmoveinstance [killcount] = a;
										killcount++;
										validmoveinstance [validcount] = a;
										validcount++;}
								break;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour == pcolour)
								break;
			
				}
				for (int j=1; col+j<8; j++) {
			
						a = chessboard.GetComponent<chessboard> ().board [row, col + j];
						if (a.gameObject.GetComponent<tileprops> ().top == null) {
								validmoveinstance [validcount] = a;
								validcount++;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
								{
										killmoveinstance [killcount] = a;
										killcount++;
										validmoveinstance [validcount] = a;
										validcount++;}
								break;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour == pcolour)
								break;
			
				}

		}

		public void bishopmoves ()
		{
				for (int j=1; col+j<8&&row+j<8; j++) {
						a = chessboard.GetComponent<chessboard> ().board [row + j, col + j];
						if (a.gameObject.GetComponent<tileprops> ().top == null) {
								validmoveinstance [validcount] = a;
								validcount++;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
								{
										killmoveinstance [killcount] = a;
										killcount++;
										validmoveinstance [validcount] = a;
										validcount++;}
								break;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour == pcolour)
								break;
				}
				for (int j=1; col-j>-1&&row+j<8; j++) {
						a = chessboard.GetComponent<chessboard> ().board [row + j, col - j];
						if (a.gameObject.GetComponent<tileprops> ().top == null) {
								validmoveinstance [validcount] = a;
								validcount++;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
								{
										killmoveinstance [killcount] = a;
										killcount++;
										validmoveinstance [validcount] = a;
										validcount++;}
								break;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour == pcolour)
								break;
				}
				for (int j=1; col-j>-1&&row-j>-1; j++) {
						a = chessboard.GetComponent<chessboard> ().board [row - j, col - j];
						if (a.gameObject.GetComponent<tileprops> ().top == null) {
								validmoveinstance [validcount] = a;
								validcount++;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
								{
										killmoveinstance [killcount] = a;
										killcount++;
										validmoveinstance [validcount] = a;
										validcount++;}
								break;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour == pcolour)
								break;
				}
				for (int j=1; col+j<8&&row-j>-1; j++) {
						a = chessboard.GetComponent<chessboard> ().board [row - j, col + j];
						if (a.gameObject.GetComponent<tileprops> ().top == null) {
								validmoveinstance [validcount] = a;
								validcount++;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
								{
										killmoveinstance [killcount] = a;
										killcount++;
										validmoveinstance [validcount] = a;
										validcount++;}
								break;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour == pcolour)
								break;
				}
		}

		public void queenmoves ()
		{
				bishopmoves ();
				castlemoves ();
		}

		public void knightmoves ()
		{

				if (row + 2 < 8 && col + 1 < 8) {
						a = chessboard.GetComponent<chessboard> ().board [row + 2, col + 1];
						if (a.gameObject.GetComponent<tileprops> ().top == null) {
								validmoveinstance [validcount] = a;
								validcount++;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
								killmoveinstance [killcount] = a;
								killcount++;
								validmoveinstance [validcount] = a;
								validcount++;
						}
				}
	
				if (row + 2 < 8 && col - 1 > -1) {
						a = chessboard.GetComponent<chessboard> ().board [row + 2, col - 1];
						if (a.gameObject.GetComponent<tileprops> ().top == null) {
								validmoveinstance [validcount] = a;
								validcount++;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
								killmoveinstance [killcount] = a;
								killcount++;
								validmoveinstance [validcount] = a;
								validcount++;
						}
				}
		
				if (row + 1 < 8 && col + 2 < 8) {
						a = chessboard.GetComponent<chessboard> ().board [row + 1, col + 2];
						if (a.gameObject.GetComponent<tileprops> ().top == null) {
								validmoveinstance [validcount] = a;
								validcount++;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
								killmoveinstance [killcount] = a;
								killcount++;
								validmoveinstance [validcount] = a;
								validcount++;
						}
				}

				if (row + 1 < 8 && col - 2 > -1) {
						a = chessboard.GetComponent<chessboard> ().board [row + 1, col - 2];
						if (a.gameObject.GetComponent<tileprops> ().top == null) {
								validmoveinstance [validcount] = a;
								validcount++;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
								killmoveinstance [killcount] = a;
								killcount++;
								validmoveinstance [validcount] = a;
								validcount++;
						}
				}
		
				if (row - 2 > -1 && col + 1 < 8) {
						GameObject a = chessboard.GetComponent<chessboard> ().board [row - 2, col + 1];
						if (a.gameObject.GetComponent<tileprops> ().top == null) {
								validmoveinstance [validcount] = a;
								validcount++;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
								killmoveinstance [killcount] = a;
								killcount++;
								validmoveinstance [validcount] = a;
								validcount++;
						}
				}

				if (row - 2 > -1 && col - 1 > -1) {
						a = chessboard.GetComponent<chessboard> ().board [row - 2, col - 1];
						if (a.gameObject.GetComponent<tileprops> ().top == null) {
								validmoveinstance [validcount] = a;
								validcount++;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
								killmoveinstance [killcount] = a;
								killcount++;
								validmoveinstance [validcount] = a;
								validcount++;
						}
				}

				if (row - 1 > -1 && col + 2 < 8) {
						a = chessboard.GetComponent<chessboard> ().board [row - 1, col + 2];
						if (a.gameObject.GetComponent<tileprops> ().top == null) {
								validmoveinstance [validcount] = a;
								validcount++;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
								killmoveinstance [killcount] = a;
								killcount++;
								validmoveinstance [validcount] = a;
								validcount++;
						}
				}
				if (row - 1 > -1 && col - 2 > -1) {
						a = chessboard.GetComponent<chessboard> ().board [row - 1, col - 2];
						if (a.gameObject.GetComponent<tileprops> ().top == null) {
								validmoveinstance [validcount] = a;
								validcount++;
						} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
								killmoveinstance [killcount] = a;
								killcount++;
								validmoveinstance [validcount] = a;
								validcount++;
						}
				}
		}

		public bool checkkingmove (GameObject king, GameObject tile)
		{
				if (king.GetComponent<chesspiece> ().ptype != 6)
						return false;
				GameObject temp;
				temp = tile.GetComponent<tileprops> ().top;
		king.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().top = null;
				tile.GetComponent<tileprops> ().top = king; 
				foreach (GameObject x in chessboard.GetComponent<chessboard> ().pieces) {
	
						if (x != null && x.GetComponent<chesspiece> ().pcolour != king.GetComponent<chesspiece> ().pcolour) {
								if (x.GetComponent<chesspiece> ().ptype != 6) {
										x.GetComponent<chesspiece> ().validmoves ();
										for (int i =0; i<64; i++) {
												if (x.GetComponent<chesspiece> ().validmoveinstance [i] != null && x.GetComponent<chesspiece> ().validmoveinstance [i] == tile) {
														tile.GetComponent<tileprops> ().top = temp;
							king.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().top = king;

														return false;
												}
												if (x.GetComponent<chesspiece> ().killmoveinstance [i] != null && x.GetComponent<chesspiece> ().killmoveinstance [i] == tile) {
														tile.GetComponent<tileprops> ().top = temp;
							king.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().top = king;

		
														return false;
												}
										}
								} else if (x.GetComponent<chesspiece> ().ptype == 6) {
										int t = Mathf.Abs (tile.GetComponent<tileprops> ().row - x.GetComponent<chesspiece> ().row);
										int p = Mathf.Abs (tile.GetComponent<tileprops> ().col - x.GetComponent<chesspiece> ().col);
										if ((p == 1 || p == 0) && (t == 1 || t == 0)) {
												tile.GetComponent<tileprops> ().top = temp;
						king.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().top = king;

												return false;
										}
								}
						}
				}
				tile.GetComponent<tileprops> ().top = temp;
		king.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().top = king;

				return true;
				
		}

		public void castlemebaby ()
		{
				int i = 1, rc = 1, lc = 1;
				if (pcolour == true && chessboard.GetComponent<chessboard> ().check == 1)
						return;
				if (pcolour == false && chessboard.GetComponent<chessboard> ().check == 2)
						return;
				if (movecount != 0)
						return;
				if (chessboard.GetComponent<chessboard> ().board [row, 0].gameObject.GetComponent<tileprops> ().top == null || chessboard.GetComponent<chessboard> ().board [row, 0].gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().movecount != 0)
						rc = 0;
				if (chessboard.GetComponent<chessboard> ().board [row, 7].gameObject.GetComponent<tileprops> ().top == null || chessboard.GetComponent<chessboard> ().board [row, 7].gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().movecount != 0)
						lc = 0;
				while (col-i>0||col+i<7) {
						if (col - i > 0) {
								a = chessboard.GetComponent<chessboard> ().board [row, col - i];
								if (a.gameObject.GetComponent<tileprops> ().top != null) {
										rc = 0;
								}
								if (i == 1 || i == 2)
								if (!checkkingmove (this.gameObject, a)) {
										rc = 0;
								}
						}
						if (col + i < 7) {
								a = chessboard.GetComponent<chessboard> ().board [row, col + i];
								if (a.gameObject.GetComponent<tileprops> ().top != null) {
										lc = 0;
								}
								if (i == 1 || i == 2)
								if (!checkkingmove (this.gameObject, a)) {
										lc = 0;
								}
						}
						i++;
				}
				if (lc == 1) {
						validmoveinstance [validcount] = a = chessboard.GetComponent<chessboard> ().board [row, col + 2];
						a.GetComponent<tileprops> ().castlingtile = 2;
						validcount++;
				}
				if (rc == 1) {
						validmoveinstance [validcount] = a = chessboard.GetComponent<chessboard> ().board [row, col - 2];
						a.GetComponent<tileprops> ().castlingtile = 1;
						validcount++;
				}
		}

		public void kingmove ()
		{
				if (movecount == 0)
						castlemebaby ();
				{

						if (col + 1 < 8) {
								a = chessboard.GetComponent<chessboard> ().board [row, col + 1];
								if (checkkingmove (this.gameObject, a)) {
										if (a.gameObject.GetComponent<tileprops> ().top == null) {
												validmoveinstance [validcount] = a;
												validcount++;
										} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
												killmoveinstance [killcount] = a;
												killcount++;
												validmoveinstance [validcount] = a;
												validcount++;
										}
								}
						}
						if (col - 1 > -1) {
								a = chessboard.GetComponent<chessboard> ().board [row, col - 1];
								if (checkkingmove (this.gameObject, a)) {
										if (a.gameObject.GetComponent<tileprops> ().top == null) {
												validmoveinstance [validcount] = a;
												validcount++;
										} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
												killmoveinstance [killcount] = a;
												killcount++;
												validmoveinstance [validcount] = a;
												validcount++;
										}
								}
						}
						if (row + 1 < 8) {
								a = chessboard.GetComponent<chessboard> ().board [row + 1, col];
								if (checkkingmove (this.gameObject, a)) {
										if (a.gameObject.GetComponent<tileprops> ().top == null) {
												validmoveinstance [validcount] = a;
												validcount++;
										} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
												killmoveinstance [killcount] = a;
												killcount++;
												validmoveinstance [validcount] = a;
												validcount++;
										}
								}
								if (col + 1 < 8) {
										a = chessboard.GetComponent<chessboard> ().board [row + 1, col + 1];
										if (checkkingmove (this.gameObject, a)) {
												if (a.gameObject.GetComponent<tileprops> ().top == null) {
														validmoveinstance [validcount] = a;
														validcount++;
												} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
														killmoveinstance [killcount] = a;
														killcount++;
														validmoveinstance [validcount] = a;
														validcount++;
												}
										}
								}
								if (col - 1 > -1) {
										a = chessboard.GetComponent<chessboard> ().board [row + 1, col - 1];
										if (checkkingmove (this.gameObject, a)) {
												if (a.gameObject.GetComponent<tileprops> ().top == null) {
														validmoveinstance [validcount] = a;
														validcount++;
												} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
														killmoveinstance [killcount] = a;
														killcount++;
														validmoveinstance [validcount] = a;
														validcount++;
												}
										}
								}

			
						}
						if (row - 1 > -1) {
								a = chessboard.GetComponent<chessboard> ().board [row - 1, col];
								if (checkkingmove (this.gameObject, a)) {
										if (a.gameObject.GetComponent<tileprops> ().top == null) {
												validmoveinstance [validcount] = a;
												validcount++;
										} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
												killmoveinstance [killcount] = a;
												killcount++;
												validmoveinstance [validcount] = a;
												validcount++;
										}
								}
								if (col + 1 < 8) {
										a = chessboard.GetComponent<chessboard> ().board [row - 1, col + 1];
										if (checkkingmove (this.gameObject, a)) {
												if (a.gameObject.GetComponent<tileprops> ().top == null) {
														validmoveinstance [validcount] = a;
														validcount++;
												} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
														killmoveinstance [killcount] = a;
														killcount++;
														validmoveinstance [validcount] = a;
														validcount++;
												}
										}
								}
								if (col - 1 > -1) {
										a = chessboard.GetComponent<chessboard> ().board [row - 1, col - 1];
										if (checkkingmove (this.gameObject, a)) {
												if (a.gameObject.GetComponent<tileprops> ().top == null) {
														validmoveinstance [validcount] = a;
														validcount++;
												} else if (a.gameObject.GetComponent<tileprops> ().top.GetComponent<chesspiece> ().pcolour != pcolour) {
														killmoveinstance [killcount] = a;
														killcount++;
														validmoveinstance [validcount] = a;
														validcount++;
												}
										}
								}
			
			
						}
				}
		}
}



