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
		public Color defcol;						//default material colour of the piece
		Color selectcol;					//the colour when it is selected
		public static GameObject chessboard;
		Vector3 temp;
		GameObject x, a;
		public GameObject[] validmoveinstance;
		public GameObject[] killmoveinstance ;
		public GameObject[] legalmoves;
		public GameObject[] legalkillmoves ;
		public int validcount = 0;
		public int killcount = 0;
		// Use this for initialization
		public void Awake ()
		{
				validmoveinstance = new GameObject[64];
				killmoveinstance = new GameObject[64];
				legalmoves = new GameObject[64];
				legalkillmoves = new GameObject[64];
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
				if (selected && chessboard.GetComponent<chessboard> ().selectedpiece == this.gameObject)
						gameObject.renderer.material.color = selectcol;
				else
						gameObject.renderer.material.color = defcol;

		}

		public void OnMouseDown ()
		{				

				if (selected == false && tile.GetComponent<tileprops> ().killmove == true) {
						this.gameObject.SetActive (false);
						GameObject killtile = tile;
						killtile.GetComponent<tileprops> ().a = 1;
						killtile.GetComponent<tileprops> ().x = chessboard.GetComponent<chessboard> ().selectedpiece;
						killtile.GetComponent<tileprops> ().temp = killtile.GetComponent<tileprops> ().x .transform.position;
						killtile.GetComponent<tileprops> ().temp.x = gameObject.transform.position.x;
						killtile.GetComponent<tileprops> ().temp.z = gameObject.transform.position.z;
						killtile.GetComponent<tileprops> ().moved = true;
				} else if (selected == false && pcolour == chessboard.GetComponent<chessboard> ().turn) {
						
						foreach (GameObject piece in chessboard.GetComponent<chessboard>().tiles) {
								piece.GetComponent<tileprops> ().isvalidmove = false;
								piece.GetComponent<tileprops> ().killmove = false;
								piece.GetComponent<tileprops> ().castlingtile = 0;
						}
						foreach (GameObject piece in chessboard.GetComponent<chessboard>().pieces) {
								if (piece != null)
										piece.GetComponent<chesspiece> ().selected = false;
						}

						selected = true;
						chessboard.GetComponent<chessboard> ().selectedpiece = this.gameObject;
				}
				for (int l=0; l<32; l++) {
						GameObject x = chessboard.GetComponent<chessboard> ().pieces [l];

						if (x != null && x.GetComponent<chesspiece> ().pcolour == chessboard.GetComponent<chessboard> ().turn) {
								x.GetComponent<chesspiece> ().validmoves ();
								x.GetComponent<chesspiece> ().newcheckmove ();
								for (int i=0; i<64; i++) {
										x.GetComponent<chesspiece> ().legalmoves [i] = x.GetComponent<chesspiece> ().validmoveinstance [i];

										x.GetComponent<chesspiece> ().legalkillmoves [i] = x.GetComponent<chesspiece> ().killmoveinstance [i];

								}
								
						}

				}
				if (selected) {
						for (int i=0; i<64; i++) {
								if (legalkillmoves [i] != null)
					
										legalkillmoves [i].GetComponent<tileprops> ().isvalidmove = legalkillmoves [i].GetComponent<tileprops> ().killmove = true;
						}
						for (int i=0; i<64; i++) {
								if (legalmoves [i] != null)
					
										legalmoves [i].GetComponent<tileprops> ().isvalidmove = true;
						}
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
			
				
	
	
		}
	
		public void newcheckmove ()
		{
				if (ptype != 6) {
			
						for (int i=0; i<64; i++) {
								if (validmoveinstance [i] != null) {
										GameObject temp2 = validmoveinstance [i].GetComponent<tileprops> ().top;
										validmoveinstance [i].GetComponent<tileprops> ().top = this.gameObject;
										tile.GetComponent<tileprops> ().top = null;
										foreach (GameObject x in chessboard.GetComponent<chessboard>().kings) {
												if (x.GetComponent<chesspiece> ().pcolour == this.pcolour) {
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
										foreach (GameObject x in chessboard.GetComponent<chessboard>().kings) {
												if (x.GetComponent<chesspiece> ().pcolour == this.pcolour) {
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


		}
	
		public bool checkkingmove (GameObject king, GameObject tiles)
		{
				if (king.GetComponent<chesspiece> ().ptype != 6)
						return false;
				GameObject temp;
				temp = tiles.GetComponent<tileprops> ().top;
				king.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().top = null;
				tiles.GetComponent<tileprops> ().top = king; 
				foreach (GameObject x in chessboard.GetComponent<chessboard> ().pieces) {
			
						if (x != null && x.GetComponent<chesspiece> ().pcolour != king.GetComponent<chesspiece> ().pcolour) {
								if (x.GetComponent<chesspiece> ().ptype != 6) {
										x.GetComponent<chesspiece> ().validmoves ();
										for (int i =0; i<64; i++) {
												if (x.GetComponent<chesspiece> ().validmoveinstance [i] != null && x.GetComponent<chesspiece> ().validmoveinstance [i] == tiles) {
														tiles.GetComponent<tileprops> ().top = temp;
														king.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().top = king;
							
														return false;
												}
												if (x.GetComponent<chesspiece> ().killmoveinstance [i] != null && x.GetComponent<chesspiece> ().killmoveinstance [i] == tiles) {
														tiles.GetComponent<tileprops> ().top = temp;
														king.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().top = king;
							
							
														return false;
												}
										}
								} else if (x.GetComponent<chesspiece> ().ptype == 6) {
										int t = Mathf.Abs (tiles.GetComponent<tileprops> ().row - x.GetComponent<chesspiece> ().row);
										int p = Mathf.Abs (tiles.GetComponent<tileprops> ().col - x.GetComponent<chesspiece> ().col);
										if ((p == 1 || p == 0) && (t == 1 || t == 0)) {
												tiles.GetComponent<tileprops> ().top = temp;
												king.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().top = king;
						
												return false;
										}
								}
						}
				}
				tiles.GetComponent<tileprops> ().top = temp;
				king.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().top = king;
		
				return true;
		
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
						if (row == 0)
								chessboard.GetComponent<chessboard> ().panel1.gameObject.SetActive (true);

			
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

		public void castlemebaby ()
		{
				int i = 1, rc = 1, lc = 1;
				if (pcolour == true && chessboard.GetComponent<chessboard> ().check == 1)
						return;
				if (pcolour == false && chessboard.GetComponent<chessboard> ().check == 2)
						return;
				if (movecount != 0)
						return;
				if (chessboard.GetComponent<chessboard> ().kings [chessboard.GetComponent<chessboard> ().turn ? 0 : 1].GetComponent<chesspiece> ().selected == false)
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



