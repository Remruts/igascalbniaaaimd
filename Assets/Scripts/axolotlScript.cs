using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class axolotlScript : MonoBehaviour {

	public GameObject axolotlHead;
	public GameObject axolotlBody;
	public ParticleSystem absorbEffect;

	public GameObject bubblePrefab;


	public LayerMask solidMask = -1;

	private Animator anim;
	private AnimatorStateInfo bodyState;
	private AnimatorStateInfo headState;
	private Rigidbody2D rb;
	private SpriteRenderer sprHead;
	private SpriteRenderer sprBody;

	private Vector2 speed = Vector3.zero;
	public float maxSpeed = 0.05f;

	private bool hurt = false;
	private bool dead = false;

	Vector3 mousePos;
	Vector3 currentPos;
	float headAngle;
	Vector3 currentScale;

	float knockback = 50f;
	float cooldown = 0f;

	void Start(){
		anim = GetComponent<Animator>();
		anim.Play("idle");

		rb = GetComponent<Rigidbody2D>();
		sprHead = axolotlHead.GetComponent<SpriteRenderer>();
		sprBody = axolotlBody.GetComponent<SpriteRenderer>();

		maxSpeed += 0.01f * (managerScript.man.speed - 1f);

		sortDrawing();
	}

	void sortDrawing(){
		sprHead.sortingOrder = Mathf.RoundToInt(transform.position.y * 48f) * -1 + 1;
		sprBody.sortingOrder = Mathf.RoundToInt(transform.position.y * 48f) * -1;
	}

	void checkIfHurt(){
		if (hurt){
			Color col = sprHead.color;
			col.r = 173f/255f;
			col.g = 94f/255f;
			col.b = 94f/255f;
			sprHead.color = col;
			sprBody.color = col;

			if (bodyState.IsName("walk")){
				anim.Play("idle");
			}
		} else {
			Color col = sprHead.color;
			col.r = 1f;
			col.g = 1f;
			col.b = 1f;
			sprHead.color = col;
			sprBody.color = col;
		}
	}

	public void die(){
		dead = true;
		anim.Play("die");
		axolotlHead.transform.rotation = Quaternion.Euler(0, 0, 0);
	}

	public void controlChar(){
		if (Input.GetKey("d")){
			if (!Input.GetKey("a")){
				move(1, 0);
			}
		}
		if (Input.GetKey("a")){
			if (!Input.GetKey("d")){
				move(-1, 0);
			}
		}
		if (Input.GetKey("s")){
			if (!Input.GetKey("w")){
				move(0, 1);
			}
		}
		if (Input.GetKey("w")){
			if (!Input.GetKey("s")){
				move(0, -1);
			}
		}
		if (!Input.GetKey("w") && !Input.GetKey("s") && !Input.GetKey("a") && !Input.GetKey("d")){
			if (bodyState.IsName("walk")){
				anim.Play("idle");
			}
		}

		if (Input.GetKey("w") && Input.GetKey("s")){
			speed.y = 0;
			if (bodyState.IsName("walk")){
				anim.Play("idle");
			}
		}
		if (Input.GetKey("a") && Input.GetKey("d")){
			speed.x = 0;
			if (bodyState.IsName("walk")){
				anim.Play("idle");
			}
		}

		if (Input.GetMouseButton(1)){
			anim.Play("head_mouth_open");

			var em = absorbEffect.emission;
        	em.enabled = true;
		} else {
			var em = absorbEffect.emission;
        	em.enabled = false;

			if (Input.GetMouseButton(0)){
				anim.Play("head_mouth_open");

				if (cooldown == 0){
					managerScript.Bubble bubbleType = managerScript.man.bubbleDict[managerScript.man.currentBubble];
					cooldown = bubbleType.cooldown;

					float bulletAngle = headAngle + (bubbleType.number * 15f - 15f)/2f;

					for (int i=0; i < bubbleType.number; i++){
						Vector3 bubbleVector = new Vector3(Mathf.Cos(bulletAngle * Mathf.Deg2Rad), Mathf.Sin(bulletAngle * Mathf.Deg2Rad), 0f);

						GameObject bubble;
						bubble = Instantiate(bubblePrefab, axolotlHead.transform.position + bubbleVector * 1f, Quaternion.identity) as GameObject;

						bubbleScript bscr = bubble.GetComponent<bubbleScript>();
						bscr.SetBubbleType(managerScript.man.currentBubble);

						float spread = Random.Range(-bubbleType.spread, bubbleType.spread);

						bscr.speed = new Vector3(Mathf.Cos((bulletAngle + spread)*Mathf.Deg2Rad), Mathf.Sin((bulletAngle + spread)*Mathf.Deg2Rad), 0f) * bubbleType.speed;

						bulletAngle -= 15f;
					}
				}
			}
		}


		/*
		if (Input.GetKeyDown("z")){
			attack();
		}
		*/

		if (!Input.GetKey("a") && !Input.GetKey("d")){
			speed.x = 0;
		}

		if (!Input.GetKey("w") && !Input.GetKey("s")){
			speed.y = 0;
		}
	}

	public void move(float faceX, float faceY){
		if (bodyState.IsName("idle") || bodyState.IsName("walk")){

			if (faceX != 0){
				speed.x += maxSpeed * faceX;
			}
			if (faceY != 0){
				speed.y -= maxSpeed * faceY;
			}

			if (bodyState.IsName("idle")){
				anim.Play("walk");
			}
		}
	}

	public void stopMoving(){
		speed.x = 0;
		speed.y = 0;
	}

	void Update(){

		sortDrawing();
		checkIfHurt();

		// Get Animation State
		bodyState = anim.GetCurrentAnimatorStateInfo(0);
		headState = anim.GetCurrentAnimatorStateInfo(1);
		if (!dead){
			//Handle Head rotation
			currentScale = transform.localScale;
			mousePos = Input.mousePosition;
			mousePos.z = 0;
			currentPos = Camera.main.WorldToScreenPoint(axolotlHead.transform.position);
			currentPos.z = 0;
			mousePos.x = mousePos.x - currentPos.x;
			mousePos.y = mousePos.y - currentPos.y;

			currentScale.x = Mathf.Abs(currentScale.x) * Mathf.Sign(mousePos.x);

			transform.localScale = currentScale;

			headAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
			if (Mathf.Sign(currentScale.x) == -1){
				axolotlHead.transform.rotation = Quaternion.Euler(0, 0, headAngle - 180);
			} else {
				axolotlHead.transform.rotation = Quaternion.Euler(0, 0, headAngle);
			}

			// controlChar
			if (!hurt){
				controlChar();
			}
		}

	}

	void LateUpdate(){
		if (!dead){
			if (headState.normalizedTime >= 1){
				if (headState.IsName("head_mouth_open")){
					if (!Input.GetMouseButton(1)){
						anim.Play("head_idle");
					}
				}
			}
		}

		// Check both
		/*
		if (speed.x != 0 || speed.y != 0){
			RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(0.87f, 0.29f), 0f, speed, maxSpeed, solidMask.value);

			if (hit.collider != null){
				if (hit.collider.CompareTag("solid")){
					speed.y = 0f;
					speed.x = 0f;
				}
			}
		}
		*/

		//check to see if still colliding
		/*
		RaycastHit2D doubleCheck = Physics2D.BoxCast(transform.position, new Vector2(0.87f, 0.29f), 0f, speed, 0, solidMask.value);
		if (doubleCheck.collider != null){
			transform.position += (transform.position - new Vector3(doubleCheck.point.x, doubleCheck.point.y, 0f)).normalized * maxSpeed;
		}
		*/

		if (cooldown > 0){
			cooldown -= Time.deltaTime;
			if (cooldown < 0){
				cooldown = 0;
			}
		}

		if (speed.magnitude > maxSpeed){
			speed = speed.normalized * maxSpeed;
		}
		if (!hurt && !dead){
			rb.MovePosition(rb.position + speed);
		}
		if (dead){
			anim.Play("die");
			anim.Play("head_die");
			axolotlHead.transform.rotation = Quaternion.Euler(0, 0, 0);
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.CompareTag("enemy")){
			if (!hurt){
				camScript.screen.shake(0.15f, 1f);
				hurt = true;
				stopMoving();
				StartCoroutine(stopHurting());

				managerScript.man.changeLivesBy((int)(-10f / managerScript.man.defense));

				Vector3 direction;
				direction = (transform.position - col.gameObject.transform.position).normalized;

				rb.velocity = direction * knockback;
			}
		}
		if (col.gameObject.CompareTag("solid")){
			speed.y = 0f;
			speed.x = 0f;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag("enemy")){
			if (!hurt){
				hurt = true;
				stopMoving();
				StartCoroutine(stopHurting());

				//ManagerScript.man.changeLivesBy(charaIndex, -1);

				Vector3 direction;
				direction = (transform.position - col.gameObject.transform.position).normalized;

				speed = direction * knockback;
			}
		}
	}

	IEnumerator stopHurting(){
		yield return new WaitForSeconds(0.5f);
		hurt = false;
	}
}
