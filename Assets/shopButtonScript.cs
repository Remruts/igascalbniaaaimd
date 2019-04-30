using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shopButtonScript : MonoBehaviour {
	shopItemsScript.shopItem item;
	public Image img;

	public void setItem(shopItemsScript.shopItem i){
		item = i;
		img.sprite = i.sprite;
	}

	public void pressButton(){		
		shopUIScript.shopUI.setCurrentItem(item);
	}


}
