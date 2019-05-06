using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buyButtonScript : MonoBehaviour {
	public void pressButton(){
		shopUIScript.shopUI.buy();
	}
}
