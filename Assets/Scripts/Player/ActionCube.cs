using UnityEngine;
using System.Collections;

public class ActionCube : MonoBehaviour {
	public string ObjectName, ObjectTag;
	public GameObject Object;
	public string State;

	void OnTriggerEnter(Collider other){
		ObjectName = other.gameObject.name;
		ObjectTag = other.gameObject.tag;
		Object = other.gameObject;
	}

	void OnTriggerExit(){
		ObjectName = "NO OBJECT";
		ObjectTag = "NO OBJECT";
		Object = null; 
	}

	void Update(){
		State = "Shoot";
		if(ObjectTag == "Loot"){ State = "Loot";}
		if(ObjectTag == "Resource"){ State = "Gather";}
		if(ObjectTag == "Structure"){ State = "Enter/Use";}

		if(Object == null){
			State = "Shoot";
			ObjectName = "NO OBJECT";
			ObjectTag = "NO OBJECT";
		}
	}
}

/*int posX, posY;
ChunkMeshGenerator cmg = Object.GetComponent<ChunkMeshGenerator>();

posX = Mathf.RoundToInt(transform.position.x - Object.transform.position.x - 0.5f);
posY = Mathf.RoundToInt(transform.position.y - Object.transform.position.y + 0.5f);
*/
