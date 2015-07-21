using UnityEngine;
using System.Collections;

public class ActionCube : MonoBehaviour {
	public string ObjectName;
	public GameObject Object;

	void OnTriggerEnter(Collider other){
		if(other){
			ObjectName = other.gameObject.name;
			Object = other.gameObject;
		}else{
			ObjectName = "NO OBJECT";
			Object = null; 
		}
	}

	void Update(){
		if(ObjectName == "Chunk"){
			int posX, posY;
			ChunkMeshGenerator cmg = Object.GetComponent<ChunkMeshGenerator>();

			posX = Mathf.RoundToInt(transform.position.x - Object.transform.position.x - 0.5f);
			posY = Mathf.RoundToInt(transform.position.y - Object.transform.position.y + 0.5f);
		}
	}
}
