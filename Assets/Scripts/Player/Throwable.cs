using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : PhysicsObject {

	public float damage = 2f;
	public float weight = 1;

	float gravity;
	Vector3 velocity;
	float velocityXSmoothing;
	Controller2D controller;


	[HideInInspector]
	public smallson owner;
	public smallson lastOwner;

    bool held = false;

	// Use this for initialization
	void Start () {
		gravity = -(2 * 0.6f) / Mathf.Pow(0.2f, 2);
		controller = gameObject.GetComponent<Controller2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (held) {
			velocity = Vector3.zero;
			transform.position = owner.hands.gameObject.transform.position;
		} else {
			CalculateVelocity ();
			controller.Move(velocity * Time.deltaTime, Vector2.zero);

		}
	}

    public void PickUp(smallson owner)
    {
        this.owner = owner;
        held = true;
	//	gameObject.layer = 
    }

    public void Drop()
    {
		lastOwner = this.owner;
        owner = null;
        held = false;
    }

    public void Throw()
    {
		lastOwner = this.owner;
		owner = null;
		held = false;
		velocity.x = 20;
    }

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject != owner){
			
		}
	}

	void CalculateVelocity()
	{
		if (controller.collisions.above || controller.collisions.below) {
			if (controller.collisions.slidingDownMaxSlope) {
				velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
			} else {
				velocity.y = 0;
				velocity.x = 0;
			}
		} else {
			velocity.y += gravity * Time.deltaTime;
		}
	}
}
