using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {
	Vector3 InitialPosition;
	float MoveSpeed = 5.0f;

	bool standing = false;
	int direction = 2; //1-up, 2-down

	float standingTime = 5;
	float timer;

	void Start(){
		InitialPosition = transform.position;
		InitialPosition -= new Vector3(0, 1f, 0);

		timer = standingTime;
	}
	
	void Update () {
		if(standing){
			if(timer < standingTime){
				timer += Time.deltaTime*2;
			}else{
				timer = 0;
				if(direction == 1){
					direction = 2;
				}else{
					direction = 1;
				}
				standing = false;
			}
		}else{
			if(direction == 1){
				transform.position += new Vector3(0, MoveSpeed * Time.deltaTime, 0);

				if(transform.position.y > InitialPosition.y){
					standing = true;
				}
			}else{
				transform.position -= new Vector3(0, MoveSpeed * Time.deltaTime, 0);

				if(Physics.Raycast(transform.position, Vector3.down, 1f)){
					standing = true;
				}
			}
		}	
	}
}
