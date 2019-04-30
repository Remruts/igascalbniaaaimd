using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bubbleScript : MonoBehaviour {

	public managerScript.Bubble bubbleType;
	public GameObject bubblePrefab;
	public GameObject bubblePopPrefab;
	public LayerMask solidMask = -1;
	SpriteRenderer spr;
	BoxCollider2D col;

	float life;
	int divisions;
	int size;
	public Vector3 speed;

	// Use this for initialization
	void Awake () {
		spr = GetComponent<SpriteRenderer>();
		col = GetComponent<BoxCollider2D>();
	}

	public void SetBubbleType(string bubbletype){
		bubbleType = managerScript.man.bubbleDict[bubbletype];
		spr.sprite = bubbleType.sprites[4 - bubbleType.size];
		size = bubbleType.size;

		life = bubbleType.life;
		divisions = bubbleType.divideOnDeath;

		switch(bubbleType.size){
			case 4:
				col.size = new Vector2(1f, 1f);
			break;
			case 3:
				col.size = new Vector2(0.75f, 0.75f);
			break;
			case 2:
				col.size = new Vector2(0.5f, 0.5f);
			break;
			case 1:
				col.size = new Vector2(0.25f,0.25f);
			break;
		}
	}

	void die(){
		if (divisions > 0){
			float bulletAngle = Mathf.Atan2(speed.y, speed.x) * Mathf.Rad2Deg + 90f/2f;

			for (int i=0; i < 2; i++){
				GameObject bubble;
				bubble = Instantiate(bubblePrefab, transform.position, Quaternion.identity) as GameObject;

				bubbleScript bscr = bubble.GetComponent<bubbleScript>();
				bscr.SetBubbleType(managerScript.man.currentBubble);
				bscr.divisions = divisions - 1;
				bscr.size = size;
				if (size > 1){
					bscr.size--;
				}
				bscr.updateSize();

				float spread = Random.Range(-bubbleType.spread, bubbleType.spread);

				bscr.speed = new Vector3(Mathf.Cos((bulletAngle + spread)*Mathf.Deg2Rad), Mathf.Sin((bulletAngle + spread)*Mathf.Deg2Rad), 0f) * bubbleType.speed;

				bulletAngle -= 90f;
			}
		}

		Instantiate(bubblePopPrefab, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag("enemy")){
			die();
		}
	}

	void updateSize(){
		spr.sprite = bubbleType.sprites[4 - size];
	}

	// Update is called once per frame
	void Update () {
		life -= Time.deltaTime;
		if (life <= 0){
			die();
		}

		speed *= 1 - bubbleType.drag;

		RaycastHit2D hit = Physics2D.CircleCast(transform.position, col.size.x, speed, 0, solidMask.value);

		if (hit.collider != null){
			if (hit.collider.CompareTag("solid")){
				speed = -speed;
				if (!bubbleType.ricochet){
					transform.position += speed;
					die();
				}
			}
		}

		transform.position += speed;
	}
}
