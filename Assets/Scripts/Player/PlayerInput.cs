using UnityEngine;
using System.Collections;

[RequireComponent (typeof (smallson))]
public class PlayerInput : MonoBehaviour {

	smallson player;

	void Start () {
		player = GetComponent<smallson> ();
	}

	void Update () {
		Vector2 directionalInput = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		player.SetDirectionalInput (directionalInput);

		if (Input.GetButtonDown("Jump")) {
			player.OnJumpInputDown ();
		}
		if (Input.GetButtonUp ("Jump")) {
			player.OnJumpInputUp ();
		}

 		     
		if (Input.GetButtonUp("Fire2") || Input.GetKeyDown(KeyCode.X))
        {
			if(player.isHoldin()){
				player.Drop ();
			}else{
				player.PickUp();
			}
        }
        if (Input.GetButtonDown("Fire1"))
        {
            player.Throw();
        }
       
	}
}