using UnityEngine;
using System.Collections;

public class quit : MonoBehaviour {

	public void quitt(){
		Application.Quit ();
	}
	public void quittomain(){
		Application.LoadLevel (0);
	}
}
