using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bubbleButtonScript : MonoBehaviour {

	public string bubbleName;
	public Sprite[] bubbles;
	public Sprite selectedSprite;
	public Sprite normalSprite;
	public Image bubbleImage;
	Image img;

	// Use this for initialization
	void Start () {
		img = GetComponent<Image>();
		switch(bubbleName){
		case "regular":
			bubbleImage.sprite = bubbles[0];
		break;
		case "shotgun":
			bubbleImage.sprite = bubbles[1];
		break;
		case "spread":
			bubbleImage.sprite = bubbles[2];
		break;
		case "machinegun":
			bubbleImage.sprite = bubbles[3];
		break;
		}

	}

	// Update is called once per frame
	void Update () {
		if (managerScript.man.currentBubble == bubbleName){
			img.sprite = selectedSprite;
		} else {
			img.sprite = normalSprite;
		}
	}
}
