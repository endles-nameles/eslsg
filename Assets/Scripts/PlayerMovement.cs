using UnityEngine;
using System.Collections;

public class playermovement : MonoBehaviour {
	public float movespeed=10f, jumpheight=1000f;

	public bool grounded = false;
	RaycastHit hit;

	void Update(){
			if (grounded==true)		{
				if(Input.GetKey (KeyCode.D)){
					transform.Translate (-Vector3.left * movespeed * Time.deltaTime);
				}
				if(Input.GetKey (KeyCode.A)){
					transform.Translate (Vector3.left * movespeed * Time.deltaTime);
				}
			}
		}

	void FixedUpdate(){
		if(Physics.Raycast(transform.position, -Vector2.up, out hit, 0.5f)){
			if(hit.transform.gameObject.tag == "ground"){
				grounded = true;
			}	
		}
		if (grounded==true && Input.GetKeyDown (KeyCode.W)) {
			GetComponent<Rigidbody> ().AddForce (Vector2.up * jumpheight);
			grounded = false;
		}
	}
}