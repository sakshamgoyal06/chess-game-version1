using UnityEngine;
using System.Collections;

public class soldierpanelscript : MonoBehaviour {
	public void pannelappear(){
		if (gameObject.activeSelf)
						gameObject.SetActive (false);
		else if (gameObject.activeSelf==false)
			gameObject.SetActive (true);
	}
}
