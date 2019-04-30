using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chooseRandomSprite : MonoBehaviour {

	SpriteRenderer spr;
	public Sprite[] spriteList;
	// Use this for initialization
	void Start () {
		spr = GetComponent<SpriteRenderer>();
		spr.sprite = spriteList[Random.Range(0, spriteList.Length-1)];
	}
}
