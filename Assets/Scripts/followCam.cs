using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCam : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		pos.x = Camera.main.transform.position.x;
		pos.y = Camera.main.transform.position.y;
		transform.position = pos;
	}
}
