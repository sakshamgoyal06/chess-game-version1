using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class pawnpromotion : MonoBehaviour
{
		public MeshFilter q, b, c, k;
		public GameObject panel;
		bool promote = false;
		public static GameObject sel = null;
	public Vector3 s2,s3,s4,s5;
		// Use this for initialization
		void Start ()
	{	s2 = new Vector3 (0.6f, 0.6f, 0.05f);s3 = new Vector3 (0.6f, 0.6f, 0.03f);s4 = new Vector3 (0.62f, 0.62f, 0.72f);s5 = new Vector3 (0.65f, 0.65f, 0.052f);
				panel.SetActive (false);
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (gameObject.GetComponent<chesspiece> ().row == 0 && this.gameObject.GetComponent<chesspiece> ().pcolour == false && promote == false) {
						panel.SetActive (true);
						Time.timeScale = 0.0f;
			Debug.Log(Time.timeScale);
						sel = this.gameObject;
						promote = true;
				}
				if (gameObject.GetComponent<chesspiece> ().row == 7 && this.gameObject.GetComponent<chesspiece> ().pcolour == true && promote == false) {
						panel.SetActive (true);
						sel = this.gameObject;

			promote = true;
				}
		}

		public void ppawn (int t)
	{	
				if (t == 5) {
						sel.GetComponent<MeshFilter> ().sharedMesh = q.sharedMesh;
						sel.gameObject.GetComponent<chesspiece> ().ptype = 5;
						sel.transform.localScale = s5;
						panel.SetActive (false);



				}
				if (t == 2) {
						sel.GetComponent<MeshFilter> ().sharedMesh = b.sharedMesh;
						sel.gameObject.GetComponent<chesspiece> ().ptype = 2;
						sel.transform.localScale = s2;

						panel.SetActive (false);



				}
				if (t == 4) {
						sel.GetComponent<MeshFilter> ().sharedMesh = k.sharedMesh;
						sel.gameObject.GetComponent<chesspiece> ().ptype = 4;
						sel.transform.localScale = s4;

						panel.SetActive (false);



				}
				if (t == 3) {
						sel.GetComponent<MeshFilter> ().sharedMesh = c.sharedMesh;
						sel.gameObject.GetComponent<chesspiece> ().ptype = 3;
						sel.transform.localScale = s3;

						panel.SetActive (false);


				}
		}
}
