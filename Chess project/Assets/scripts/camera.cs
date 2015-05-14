using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {
	public Vector3 black,white;
	public Quaternion blackr,whiter;
	public GameObject chessboard;

	// Use this for initialization
	void Start () {
		chessboard = GameObject.FindGameObjectWithTag("chessboard");

		black = new Vector3 (7, 13,6);
		white = new Vector3 (7, 13, -20);
		whiter.eulerAngles = new Vector3(40,  0,0);
		blackr.eulerAngles = new Vector3 (40,180 , 0);

		this.gameObject.transform.position =white;
		this.gameObject.transform.rotation = whiter;
	}
	
	// Update is called once per frame
	void Update () {
		if (!chessboard.GetComponent<chessboard> ().turn&&this.gameObject.transform.position != white&&this.gameObject.transform.rotation !=whiter)
		{
			this.gameObject.transform.position = Vector3.Lerp (this.gameObject.transform.position,white,0.1f);
			this.gameObject.transform.rotation = Quaternion.Lerp(this.gameObject.transform.rotation,whiter,0.1f);
		} else if (chessboard.GetComponent<chessboard> ().turn&&this.gameObject.transform.position != black&&this.gameObject.transform.rotation !=blackr)
		{
			this.gameObject.transform.position = Vector3.Lerp (this.gameObject.transform.position,black,0.1f);
			this.gameObject.transform.rotation = Quaternion.Lerp(this.gameObject.transform.rotation,blackr,0.1f);
		}
		if(Input.GetAxis("Mouse ScrollWheel")!=0){
			if(Camera.main.fieldOfView<90&&Camera.main.fieldOfView>45)
			Camera.main.fieldOfView-=Input.GetAxis("Mouse ScrollWheel")*10;
				if(Camera.main.fieldOfView>=90)Camera.main.fieldOfView=89;	
			if(Camera.main.fieldOfView<=45)Camera.main.fieldOfView=46;
		}
	}
}
