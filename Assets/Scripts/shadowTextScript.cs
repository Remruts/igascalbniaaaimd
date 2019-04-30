using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class shadowTextScript : MonoBehaviour {

	public TMP_Text currentText;
	public TMP_Text shadowText;

	// Update is called once per frame
	void Update () {
		shadowText.text = currentText.text;
	}
}
