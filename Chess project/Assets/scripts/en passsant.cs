/*using UnityEngine;
using System.Collections;

public class enpasssant : MonoBehaviour {
	public GameObject chessboard;
	public bool possible=false;
	public bool done=false;
	// Use this for initialization
	void Start () {
		chessboard = GameObject.FindGameObjectWithTag ("chessboard");
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.GetComponent<chesspiece> ().row == 3 && gameObject.GetComponent<chesspiece> ().pcolour == true) {
			enpassanted();		
		}
		if (gameObject.GetComponent<chesspiece> ().row == 3 && gameObject.GetComponent<chesspiece> ().pcolour == false && gameObject.GetComponent<chesspiece>().movecount==1&&chessboard.GetComponent<chessboard>().turn==true&&done==false) {
			possible=true;		
		}
	}
	public void enpassanted(){
		if (gameObject.GetComponent<chesspiece> ().col - 1 > -1) {
			if(chessboard.GetComponent<chessboard>().board[gameObject.GetComponent<chesspiece>().row,gameObject.GetComponent<chesspiece> ().col - 1].GetComponent<tileprops>().top!=null){
				GameObject tile = chessboard.GetComponent<chessboard>().board[gameObject.GetComponent<chesspiece>().row,gameObject.GetComponent<chesspiece> ().col - 1];
				if(tile.GetComponent<tileprops>().top.GetComponent<chesspiece>().pcolour!= this.gameObject.GetComponent<chesspiece>().pcolour&&tile.GetComponent<tileprops>().top.GetComponent<chesspiece>().ptype==1)
				if(tile.GetComponent<tileprops>().top.GetComponent<enpasssant>().possible==true){

				}
			}		
		}
		if( gameObject.GetComponent<chesspiece> ().col+1 < 8) {
					
		}
	}
}*/
