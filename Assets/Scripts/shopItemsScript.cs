using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopItemsScript : MonoBehaviour {

	public static shopItemsScript shop;

	[System.Serializable]
	public class shopItem
    {
		public string name;
		public string listName;
		public int price;
		public string description;
		public bool bought;
		public Sprite sprite;
		public System.Action function;
    }
	public shopItem[] shopItemList;

	public Dictionary<string, shopItem> shopItemDict;

	// Use this for initialization
	void Start () {
		shop = this;
		shopItemDict = new Dictionary<string, shopItem>();
		foreach (shopItem i in shopItemList){
			shopItemDict.Add(i.name, i);
		}
		shopItemDict["getShotgun"].function = getShotgun;
		shopItemDict["getSpreadBubble"].function = getSpreadBubble;
		shopItemDict["getMachineGun"].function = getMachineGun;
		shopItemDict["getMoreAttack"].function = getMoreAttack;
		shopItemDict["getMoreDefense"].function = getMoreDefense;
		shopItemDict["getMoreSpeed"].function = getMoreSpeed;
	}

	public void ResetItems(){
		foreach (var item in shopItemDict.Values){
			item.bought = false;			
		}
	}

	public void getShotgun(){
		managerScript.man.getBubble("shotgun");
	}

	public void getSpreadBubble(){
		managerScript.man.getBubble("spread");
	}

	public void getMachineGun(){
		managerScript.man.getBubble("machinegun");
	}

	public void getMoreAttack(){
		managerScript.man.upStat("attack");
	}

	public void getMoreDefense(){
		managerScript.man.upStat("defense");
	}

	public void getMoreSpeed(){
		managerScript.man.upStat("speed");
	}

}
