using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public GameObject FollowObject;
	public float Smoothness = 6;

	void Update(){
		transform.position = Vector3.Lerp(transform.position, new Vector3(FollowObject.transform.position.x, FollowObject.transform.position.y, -10), Time.deltaTime*Smoothness); 
	}
}
