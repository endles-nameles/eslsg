using UnityEngine;
using System.Collections;

public class PlayerActions : MonoBehaviour {
	GameObject ActionCube;
	ActionCube ac;

	WorldGenerator wg;	
	
	RaycastHit hit;

	void Start(){
		wg = GameObject.Find("WorldGenerator").GetComponent<WorldGenerator>();

		ActionCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		ActionCube.name = "ActionCube";
		ActionCube.transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);
		ActionCube.GetComponent<Collider>().isTrigger = true;
		ActionCube.GetComponent<Renderer>().material.color = Color.black;
		
		ac = ActionCube.AddComponent<ActionCube>();
		
		Rigidbody ac_r = ActionCube.AddComponent<Rigidbody>();
		ac_r.useGravity = false;
	}

	void Update(){
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = 10;
		Vector3 ActionCube_BasePosition = Camera.main.ScreenToWorldPoint(mousePosition);

		ActionCube.transform.position = new Vector3(ActionCube_BasePosition.x, ActionCube_BasePosition.y, 0);
	}
}
