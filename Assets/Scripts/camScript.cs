using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camScript : MonoBehaviour {

	bool shaking = false;
	float intensity = 0f;
	float shakeIncrement = 1f;

	Vector3 currentPos;
	Vector3 shakenPos;

	//Camera myCam;

	public static camScript screen;

	// Use this for initialization
	void Start () {
		if (screen == null) {
			screen = this;
		} else {
			Destroy (gameObject);
		}

		//myCam = GetComponent<Camera> ();
		if (managerScript.man.playerChar != null){
			Vector3 playerPos = managerScript.man.playerChar.transform.position;
			transform.position = playerPos;
			currentPos = transform.position;
		}
	}

	// Update is called once per frame
	void Update () {
		if (shaking) {
			intensity += shakeIncrement * Time.deltaTime;
		}

		if (!shaking && intensity > 0) {
			intensity -= shakeIncrement * Time.deltaTime;

			if (intensity < 0) {
				intensity = 0;
			}
		}

		if (managerScript.man.playerChar != null){
			Vector3 playerPos = managerScript.man.playerChar.transform.position;

			currentPos += (playerPos - currentPos) * 0.2f;
		}

		currentPos.x = ((int)(currentPos.x *48f)) / 48f;
		currentPos.y = ((int)(currentPos.y *48f)) / 48f;
		currentPos.z = -10f;

		if (intensity > 0){
			shakenPos.x = currentPos.x + getShake();
			shakenPos.y = currentPos.y + getShake();
			shakenPos.z = currentPos.z;
			transform.position = shakenPos;
		} else {
			transform.position = currentPos;
		}

	}

	float getShake(){
		int shakingNumber = (int)Random.Range(-intensity/2f*48, intensity/2f*48);
		shakingNumber += (int)(Mathf.Sign(shakingNumber) * intensity/2f*48);

		if (shakingNumber == 0){
			shakingNumber = 1;
		}
		return shakingNumber / 48f;
	}

	public void shake(float time, float factor){
		shaking = true;

		if (time > 0){
			Invoke ("stopShaking", time);
		}

		shakeIncrement = factor;
	}

	public void stopShaking(){
		shaking = false;
	}
}
