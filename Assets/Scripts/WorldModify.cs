using UnityEngine;
using System.Collections;

public class WorldModify : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DestroyVoxel(int x, int y, GameObject ownerChunk){
		ChunkMeshGenerator cg = ownerChunk.GetComponent<ChunkMeshGenerator>();

		
	}
}
