//Written by Andrei Meluta -- 2015

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour {
	public float MoveSpeed = 6.0f;
	public float JumpSpeed = 8.0f;
	public float GravityPower = 20.0f;

	CharacterController controller;
	Vector3 moveDirection = Vector3.zero;

	RaycastHit hit;

	void Start(){
		controller = GetComponent<CharacterController>();

		Physics.IgnoreLayerCollision(8, 9, true);
	}

	void Update(){
		if(controller.isGrounded){
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= MoveSpeed;

			if(Input.GetKey(KeyCode.W)){
				moveDirection.y = JumpSpeed;
			}
		}

		moveDirection.y -= GravityPower * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);

		if(Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f)){
			if(hit.transform.gameObject.tag == "MovingPlatform"){
				transform.parent = hit.transform;
			}else{
				transform.parent = null;
			}
		}
	}
}