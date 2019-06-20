using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class shopUIScript : MonoBehaviour {


	public GameObject shopButtonPrefab;
	public static shopUIScript shopUI;
	shopItemsScript.shopItem currentItem;

	public TMP_Text itemName;
	public TMP_Text itemDescription;
	public TMP_Text itemPrice;

	public TMP_Text livesText;
	public TMP_Text attkText;
	public TMP_Text defText;
	public TMP_Text spdText;

	List<GameObject> theButtons;

	// Use this for initialization
	void Start () {
		shopUIScript.shopUI = this;
		setCurrentItem(null);
		buildItemGrid();
		updateStatsTexts();
	}

	void updateStatsTexts(){
		livesText.text = managerScript.man.lives.ToString();
		attkText.text = managerScript.man.attack.ToString();
		defText.text = managerScript.man.defense.ToString();
		spdText.text = managerScript.man.speed.ToString();
	}

	void buildItemGrid(){
		List<string> theKeys = new List<string>(shopItemsScript.shop.shopItemDict.Keys);
		var theDict = shopItemsScript.shop.shopItemDict;

		if (theButtons != null){
			foreach (GameObject b in theButtons){
				Destroy(b);
			}
		}

		theButtons = new List<GameObject>();

		int maxItems = 12;
		int itemCount = 0;
		for (int i= 0; i < theDict.Count; i++){
			var currentItem = theDict[theKeys[i]];
			if (currentItem.bought){
				continue;
			}
			if (itemCount >= maxItems){
				break;
			}

			GameObject button = Instantiate(shopButtonPrefab, transform.position, Quaternion.identity) as GameObject;

			button.transform.SetParent(transform);
			RectTransform butRect = button.GetComponent<RectTransform>();
			butRect.anchoredPosition = new Vector3(-100f + (itemCount % 4) * 34f, 48f - (itemCount / 4) * 34f, 0f);
			butRect.localScale = Vector3.one;
			itemCount++;

			button.GetComponent<shopButtonScript>().setItem(currentItem);

			theButtons.Add(button);
		}
	}

	public void setCurrentItem(shopItemsScript.shopItem i){
		currentItem = i;
		if (i != null){
			itemName.text = currentItem.listName;
			itemDescription.text = currentItem.description;
			itemPrice.text = currentItem.price.ToString();
		} else {
			itemName.text = "nothing";
			itemDescription.text = "";
			itemPrice.text = "0";
		}

	}

	public void buy(){
		if (currentItem != null){
			var theDict = shopItemsScript.shop.shopItemDict;
			Debug.Log(managerScript.man.lives - currentItem.price);
			if (managerScript.man.lives - currentItem.price > 0){
				Debug.Log("doing this");
				managerScript.man.changeLivesBy(- currentItem.price);
				theDict[currentItem.name].bought = true;
				currentItem.function();
				setCurrentItem(null);
				buildItemGrid();
				updateStatsTexts();
			}
		}
	}
}
