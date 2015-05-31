using UnityEngine;
using System.Collections;

public class undo : MonoBehaviour
{
		public ArrayList boardinstances;
		public ArrayList pieceinstance;
		public int movecount = 0;
		public MeshFilter bpawn, bbishop, bcastle, bknight, bqueen, wpawn, wbishop, wqueen, wknight, wcastle;
		public Material black, white;
		GameObject chessboard;
		public Vector3 s1, s2, s3, s4, s5;
		public Quaternion q1, q2;
		public Color b, w;

		struct tile
		{
				public GameObject a;
				public GameObject top;
		}

		struct piece
		{
				public GameObject a;
				public Vector3 tem;
				public int row;
				public int col;
				public int movecount;
				public int ptype;
				public GameObject tile;
				public bool pcolor;
				public bool wkilled;
		}
		// Use this for initialization
		void Start ()
		{
				boardinstances = new ArrayList ();
				pieceinstance = new ArrayList ();
				chessboard = GameObject.FindGameObjectWithTag ("chessboard");
				q1.eulerAngles = new Vector3 (270, 0, 0);
				q2.eulerAngles = new Vector3 (270, 180, 0);
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.Z))
						lastmove ();
		}

		public void addinstance ()
		{

				tile[] tiles = new tile[64];
				piece[] pieces = new piece[32];
				for (int i =0; i<64; i++) {
						GameObject x = chessboard.GetComponent<chessboard> ().tiles [i];
						tiles [i].a = x;
						tiles [i].top = x.GetComponent<tileprops> ().top;
				}
				for (int i =0; i<32; i++) {
						GameObject x = chessboard.GetComponent<chessboard> ().pieces [i];
						pieces [i].a = x;
						if (x != null) {
								pieces [i].tile = x.GetComponent<chesspiece> ().tile;
								pieces [i].tem = x.transform.position;
								pieces [i].row = x.GetComponent<chesspiece> ().row;
								pieces [i].col = x.GetComponent<chesspiece> ().col;
								pieces [i].movecount = x.GetComponent<chesspiece> ().movecount;
								pieces [i].ptype = x.GetComponent<chesspiece> ().ptype;
								pieces [i].pcolor = x.GetComponent<chesspiece> ().pcolour;
								pieces [i].wkilled = x.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().killmove;
								if (pieces [i].wkilled == true)
										Debug.Log ("As");
						}

				}
				boardinstances.Add (tiles);
				pieceinstance.Add (pieces);
				movecount++;
		}

		public void lastmove ()
		{
				if (movecount > 0) {
						tile[] tiles = new tile[64];
						piece[] pieces = new piece[32];
						tiles = (tile[])boardinstances [movecount - 1];
						pieces = (piece[])pieceinstance [movecount - 1];
			for (int i =0; i<64; i++) {
				GameObject t = chessboard.GetComponent<chessboard> ().tiles [i];
				for (int j=0; j<64; j++) {
					if (t == tiles [i].a) {
						t.GetComponent<tileprops> ().top = tiles [i].top;
						break;
					}
				}
			}

						for (int i =0; i<32; i++) {
								for (int j=0; j<32; j++) {
										GameObject t = chessboard.GetComponent<chessboard> ().pieces [j];
										if (t != null && t == pieces [i].a) {
												t.GetComponent<chesspiece> ().tile = pieces [i].tile;
												t.GetComponent<chesspiece> ().row = pieces [i].row;
												t.transform.position = pieces [i].tem;
												t.GetComponent<chesspiece> ().col = pieces [i].col;
												t.GetComponent<chesspiece> ().movecount = pieces [i].movecount;
												break;
										}
								}
								if (pieces [i].a == null && pieces [i].wkilled == true) {
										GameObject s = new GameObject ();
										s.AddComponent<MeshFilter> ();
										s.AddComponent<MeshRenderer> ();
										s.transform.position = pieces [i].tem;
										s.AddComponent<chesspiece> ();
										s.GetComponent<chesspiece> ().tile = pieces [i].tile;
										s.GetComponent<chesspiece> ().row = pieces [i].row;
										s.GetComponent<chesspiece> ().col = pieces [i].col;
										s.GetComponent<chesspiece> ().movecount = pieces [i].movecount;
										s.GetComponent<chesspiece> ().ptype = pieces [i].ptype;
										s.GetComponent<chesspiece> ().pcolour = pieces [i].pcolor;
										s.tag = "piece";
										string name;
										name = pieces [i].pcolor ? "black" : "white";
										switch (pieces [i].ptype) {
										case 1:
												name = name + " pawn";
												s.GetComponent<MeshFilter> ().sharedMesh = pieces [i].pcolor ? bpawn.sharedMesh : wpawn.sharedMesh;
												s.transform.localScale = s1;
												break;
										case 2:
												name = name + " bishop";
												s.GetComponent<MeshFilter> ().sharedMesh = pieces [i].pcolor ? bbishop.sharedMesh : wbishop.sharedMesh;
												s.transform.localScale = s2;
												break;
										case 4:
												name = name + " knight";
												s.GetComponent<MeshFilter> ().sharedMesh = pieces [i].pcolor ? bknight.sharedMesh : wknight.sharedMesh;
												s.transform.localScale = s4;
												break;
										case 3:
												name = name + " rook";
												s.GetComponent<MeshFilter> ().sharedMesh = pieces [i].pcolor ? bcastle.sharedMesh : wcastle.sharedMesh;
												s.transform.localScale = s3;
												break;
										case 5:
												name = name + " queen";
												s.GetComponent<MeshFilter> ().sharedMesh = pieces [i].pcolor ? bqueen.sharedMesh : wqueen.sharedMesh;
												s.transform.localScale = s5;
												break;
										}
										s.AddComponent<BoxCollider> ();
										s.renderer.sharedMaterial = pieces [i].pcolor ? black : white;
										s.GetComponent<chesspiece> ().defcol = pieces [i].pcolor ? b : w;
										s.transform.rotation = pieces [i].pcolor ? q2 : q1;
										s.name = name;
										pieces [i].a = s;
										s.GetComponent<chesspiece> ().tile.GetComponent<tileprops> ().top = s;
										for (int l = movecount; l>=1; l--) {

												((piece[])pieceinstance [l - 1]) [i].a = s;


										}
								}
										

						}					
			for(int i =0;i<32;i++){
				chessboard.GetComponent<chessboard>().pieces[i]=pieces[i].a;
			}
		

						chessboard.GetComponent<chessboard> ().selectedpiece = null;
						chessboard.GetComponent<chessboard> ().turn = !chessboard.GetComponent<chessboard> ().turn;
						pieceinstance.Remove (movecount);
						chessboard.GetComponent<chessboard> ().panel.gameObject.SetActive (false);
						boardinstances.Remove (movecount);
						movecount--;
				}

		}
}
