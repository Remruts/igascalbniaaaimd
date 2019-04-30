using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class continueButtonScript : MonoBehaviour {

	public void press(){
		managerScript.man.GoToNextFloor();
	}
}
