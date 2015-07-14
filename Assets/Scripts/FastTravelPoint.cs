using UnityEngine;
using System.Collections;

public class FastTravelPoint : MonoBehaviour {
	public GameObject OwnerChunk;
	ChunkGenerator cg;

	void Start(){
		cg = OwnerChunk.GetComponent<ChunkGenerator> ();
	}

	void Update(){
		transform.position = new Vector3 (OwnerChunk.transform.position.x + cg.MaxHeightPoint, cg.MaxHeight + 4.5f, 1);
	}
}
