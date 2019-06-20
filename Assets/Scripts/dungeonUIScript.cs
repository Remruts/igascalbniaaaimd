using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dungeonUIScript : MonoBehaviour {
	public GameObject bubbleButtonPrefab;
	public static dungeonUIScript UI;
	List<GameObject> bubbleButtons;
	int bubbleNumber;

	void Start(){
		UI = this;
		generateBubbleButtons();
		bubbleNumber = countBubbles();
	}

	void Update(){
		int count = countBubbles();
		if (bubbleNumber != count){
			bubbleNumber = count;
			generateBubbleButtons();
		}
	}

	int countBubbles(){
		int num = 0;
		foreach(bool b in managerScript.man.hasBubbles.Values){
			if (b){
				num++;
			}
		}
		//Debug.Log(num);
		return num;
	}

	public void generateBubbleButtons(){
		if (bubbleButtons != null){
			foreach (var b in bubbleButtons){
				Destroy(b);
			}
		}
		bubbleButtons = new List<GameObject>();
		int i = 0;
		foreach (var b in managerScript.man.bubbleList){
			if (managerScript.man.hasBubbles[b.name]){
				GameObject bb = Instantiate(bubbleButtonPrefab, transform.position, Quaternion.identity) as GameObject;
				bb.transform.SetParent(transform);
				bb.transform.localScale = Vector3.one;

				bubbleButtonScript bbScript = bb.GetComponent<bubbleButtonScript>();

				bbScript.bubbleName = b.name;

				bubbleButtons.Add(bb);

				bb.GetComponent<RectTransform>().anchoredPosition = new Vector3(-145f + i * 18f, 90f, 0f);
				i+=1;
			}
		}
	}

}
