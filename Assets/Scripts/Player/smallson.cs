using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class smallson : MonoBehaviour
{

    public float maxJumpHeight = 4;
    public float minJumpHeight = 0.6f;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 6;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .3f;
    float timeToWallUnstick;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

    Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;
    int lastWallDir;

	public CircleCollider2D hands;
	bool holdin = false; //you holdin?
	Throwable heldObj;
	public LayerMask throwable; //i dont feel like being thrown
	public float pushDelay = 0.25f;

    Animator animator;

    bool stunned;
    bool controllable;

    void Start()
    {
        controller = GetComponent<Controller2D>();
		hands = transform.Find ("Hands").GetComponent<CircleCollider2D> ();
			
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        CalculateVelocity();
        HandleWallSliding();

        controller.Move(velocity * Time.deltaTime, directionalInput);
        animator.SetBool("running", directionalInput.x != 0);
        animator.SetBool("grounded", controller.collisions.below);

        if (controller.collisions.above || controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
            }
        }
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    public void OnJumpInputDown()
    {
        if (wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                timeToWallUnstick = 0;
                velocity.x = -lastWallDir * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        }
        if (controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
                { 
                    velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                    velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                }
            }
            else
            {
                velocity.y = maxJumpVelocity;
            }
        }
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

	public void PickUp()
	{
		if (holdin)
			return;


		var other = Physics2D.OverlapCircle (hands.transform.position, hands.radius, throwable);
        if (other)
        {
            if (other.tag != "BIG BOY")
            {
                heldObj = other.GetComponent<Throwable>();
                //heldObj.collider.enabled = false;
                heldObj.gameObject.transform.position = hands.transform.position;
                heldObj.PickUp(this);
				holdin = true;
				Debug.Log(other);
            }
        }
	}

	public void Drop()
	{
		//StartCoroutine ("ResetCollider");
		heldObj.Drop();
		holdin = false;

	}

	public void Throw()
	{
		if (!holdin) {			
			Push ();
		}
		else {
			heldObj.Throw ();
		}
	}

	void Push(){
		
	}


    void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;

        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y <= Mathf.Sqrt(Mathf.Abs(gravity)) && (directionalInput.x == wallDirX || lastWallDir != 0))
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                if (lastWallDir == 0 && directionalInput.x == wallDirX)
                {
                    lastWallDir = wallDirX;
                }

                velocityXSmoothing = 0;
                velocity.x = 0;

                if (directionalInput.x != wallDirX) //&& directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                lastWallDir = 0;
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }

    public bool isHoldin()
    {
        return holdin;
    }
}