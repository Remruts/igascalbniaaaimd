using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heartPickupScript : MonoBehaviour {

	public GameObject explosion;
	public float lifeTimer = 3f;
	int lives = 15;
	float height = 0f;
	Vector3 speed;
	public SpriteRenderer spr;
	public Transform heartTransform;
	public LayerMask solidMask = -1;

	void Start(){
		speed = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), Random.Range(0.2f, 0.4f));
	}

	void die(){
		Destroy(gameObject);
	}

	public void changeLives(int l){
		lives = l;
	}

	// Update is called once per frame
	void Update () {
		lifeTimer -= Time.deltaTime;
		if (lifeTimer <= 0){
			die();
		}
		if (lifeTimer <= 3){
			Color tmp = spr.color;
			float t = lifeTimer*2 - Mathf.Round(lifeTimer*2);
			tmp.a = (t > 0) ? 1 : 0;
			spr.color = tmp;
		}

		if (height >= 0){
			speed.z -= Time.deltaTime * 1f;
		} else {
			height = 0f;
			speed.z = -speed.z;
			speed *= 0.7f;
			if (Mathf.Abs(speed.z) < 0.05f){
				speed.z = 0;
			}
		}

		if (Input.GetMouseButton(1)){
			Vector3 diff = transform.position - managerScript.man.playerChar.transform.position;
			if (diff.magnitude < 8f){
				speed -= diff.normalized * (8f - diff.magnitude) * Time.deltaTime * 0.4f;
			}

		}

		if (Mathf.Abs(speed.z) < 0.05f){
			height += speed.z;
		}
		speed.x *= 0.98f;
		speed.y *= 0.98f;

		Vector2 spd = new Vector2(speed.x, speed.y);
		// Check both
		if (speed.x != 0 || speed.y != 0){
			RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(0.5f, 0.5f), 0f, spd, spd.magnitude, solidMask.value);

			if (hit.collider != null){
				if (hit.collider.CompareTag("solid")){
					speed.y = 0f;
					speed.x = 0f;
				}
			}
		}

		//check to see if still colliding
		RaycastHit2D doubleCheck = Physics2D.BoxCast(transform.position, new Vector2(0.5f, 0.5f), 0f, spd, 0f, solidMask.value);
		if (doubleCheck.collider != null){
			transform.position += (transform.position - new Vector3(doubleCheck.point.x, doubleCheck.point.y, 0f)).normalized * 0.1f;
		}

		heartTransform.position = new Vector3(heartTransform.position.x, transform.position.y + height, 0f);

		transform.position += new Vector3(speed.x, speed.y, 0f);

		spr.sortingOrder = Mathf.RoundToInt((transform.position.y + height) * 48f) * -1;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag("player")){
			Instantiate(explosion, transform.position, Quaternion.identity);
			Destroy(gameObject);
			managerScript.man.changeLivesBy(lives);
		}
	}
}
