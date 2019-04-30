using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stairsScript : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag("player")){
			managerScript.man.GoToShop();
		}
	}
}
