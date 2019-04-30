using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetScript : MonoBehaviour {
	Animator anim;

	void Start(){
		anim = GetComponent<Animator>();
	}

	public void startOver(){
		managerScript.man.Reset();
		anim.Play("fadeIn");
	}
}
