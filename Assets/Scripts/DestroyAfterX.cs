using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterX : MonoBehaviour {

	public float time = 2f;

	// Use this for initialization
	void Start () {
		Invoke("Destroy", time);
	}

	void Destroy(){
		Destroy(gameObject);
	}
}
