using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamicOrderScript : MonoBehaviour {

	SpriteRenderer spr;
	// Use this for initialization
	void Start () {
		spr = GetComponent<SpriteRenderer>();
		spr.sortingOrder = Mathf.RoundToInt(transform.position.y * 48f) * -1;
	}

	// Update is called once per frame
	void Update () {
		if (!gameObject.isStatic){
			spr.sortingOrder = Mathf.RoundToInt(transform.position.y * 48f) * -1;
		}
	}
}
