using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class livesChangeUIScript : MonoBehaviour {

	public TMP_Text livesText;
	public TMP_Text livesTextShadow;

	float alpha = 1f;
	int lives = 100;

	void Update(){
		Transform foregroundTrans =  transform.GetChild(1);
		Vector3 pos2 = foregroundTrans.position;
		pos2.z = -1;
		foregroundTrans.position = pos2;
		Vector3 pos = transform.position;
		pos.y += 0.025f;
		transform.position = pos;


		//alpha -= Time.deltaTime;

		if (lives < 0){
			/*
			string col = ColorUtility.ToHtmlStringRGBA(new Color(1f, 125f/255f, 125f/255f, alpha));
			string al = col.Substring(6);

			livesText.text = "<color=#" + col + "><sprite name=\"heart\" color=#FFFFFF"+ al +">" + lives.ToString();
			livesText.color = new Color32(255, 125, 125, 255);
			*/
			livesTextShadow.text = "<sprite name=\"heart\">"+ lives.ToString();
			livesText.color = new Color32(255, 125, 125, 255);
		} else {
			livesTextShadow.text = "<sprite name=\"heart\">+"+ lives.ToString();
			livesText.color = new Color32(255, 255, 255, 255);
			/*
			string col = ColorUtility.ToHtmlStringRGBA(new Color(1f, 1f, 1f, alpha));
			string al = col.Substring(6);
			livesText.text = "<color=#" + col + "><sprite name=\"heart\" color=#FFFFFF"+ al +">+" + lives.ToString();
			*/
		}
	}

	public void setLives(int l){
		lives = l;
	}

}
