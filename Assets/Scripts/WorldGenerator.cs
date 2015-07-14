using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Biome{
	public float Amplitude, Scale;
}

public class WorldGenerator : MonoBehaviour {
	public int Chunk_SizeWidth, Chunk_SizeHeight;
	
	//[System.NonSerialized]
	public List<GameObject> CHUNKS;

	public Biome[] BIOMES;
	int Biome_CurrentRight, Biome_CurrentLeft;
	public int Biome_Frequency;
	public int FastTravel_Frequency;

	public int World_Seed;
	public Material Cube_Material;

	

	public Transform chunk_NextRight, chunk_NextLeft;
	int Chunks_Right, Chunks_Left;

	public Transform PlayerObject;

	void Start(){
		if (World_Seed == 0) {
			World_Seed = Random.Range (1, 100000);
		}

		Biome_CurrentRight = 0;
		Biome_CurrentLeft = 0;

		CreateCHUNK (1, true, true, 0);
		CreateCHUNK (1, true, true, 1);
		CreateCHUNK (1, true, true, 0);
		CreateCHUNK (2, true, true, 0);
		CreateCHUNK (2, true, true, 1);
	}

	void CreateCHUNK(int direction, bool flat, bool building, int extraStruct){
		GameObject newChunk = new GameObject ("Chunk");

		ChunkGenerator cg = newChunk.AddComponent<ChunkGenerator> ();
		cg.Chunk_SizeWidth = Chunk_SizeWidth;
		cg.Chunk_SizeHeight = Chunk_SizeHeight;
		cg.World_Seed = World_Seed;
		cg.Cube_Material = Cube_Material;

		//1 - right; 2- left; 3 - down
		if (direction == 1) {
			cg.World_Amplitude = BIOMES[Biome_CurrentRight].Amplitude;
			cg.World_Scale = BIOMES[Biome_CurrentRight].Scale;

			newChunk.transform.position = new Vector2 (chunk_NextRight.position.x, chunk_NextRight.position.y);
			chunk_NextRight.position += new Vector3(Chunk_SizeWidth, 0, 0);
			Chunks_Right++;

			if(Chunks_Right%Biome_Frequency == 0){
				flat = true;
				Biome_CurrentRight = Random.Range(0, BIOMES.Length);
			}else{
				if(Chunks_Right%FastTravel_Frequency == 0){
					GameObject newFastTravel = GameObject.CreatePrimitive(PrimitiveType.Cube);
					newFastTravel.name = "Fast Travel";
					newFastTravel.transform.localScale = new Vector3(3f, 10f, 1);
					newFastTravel.transform.parent = newChunk.transform;

					FastTravelPoint ftp = newFastTravel.AddComponent<FastTravelPoint>();
					ftp.OwnerChunk = newChunk;
				}
			}
		} 
		if (direction == 2) {
			cg.World_Amplitude = BIOMES[Biome_CurrentLeft].Amplitude;
			cg.World_Scale = BIOMES[Biome_CurrentLeft].Scale;

			newChunk.transform.position = new Vector2 (chunk_NextLeft.position.x, chunk_NextLeft.position.y);
			chunk_NextLeft.position -= new Vector3(Chunk_SizeWidth, 0, 0);
			Chunks_Left++;

			if(Chunks_Left%Biome_Frequency == 0){
				flat = true;
				Biome_CurrentLeft = Random.Range(0, BIOMES.Length);
			}else{
				if(Chunks_Left%FastTravel_Frequency == 0){
					GameObject newFastTravel = GameObject.CreatePrimitive(PrimitiveType.Cube);
					newFastTravel.name = "Fast Travel";
					newFastTravel.transform.localScale = new Vector3(3f, 10f, 1);
					newFastTravel.transform.parent = newChunk.transform;

					FastTravelPoint ftp = newFastTravel.AddComponent<FastTravelPoint>();
					ftp.OwnerChunk = newChunk;
				}
			}
		}

		if (building) {
			GameObject newBuilding = GameObject.CreatePrimitive(PrimitiveType.Cube);
			newBuilding.name = "Building";

			float buildingHeight = Random.Range(10, 16);
			if(Random.Range(0, 1000) < 100){
				buildingHeight = Random.Range(22, 26);
			}
			newBuilding.transform.position = new Vector3(newChunk.transform.position.x + Random.Range (-2f, 2f) + Chunk_SizeWidth/2, newChunk.transform.position.y+Chunk_SizeHeight - Chunk_SizeHeight/4+buildingHeight/2-0.5f, 1);
			newBuilding.transform.localScale = new Vector3(Random.Range(Chunk_SizeWidth-6, Chunk_SizeWidth-4), buildingHeight, 1);
			newBuilding.transform.parent = newChunk.transform;
		}

		if(extraStruct == 1){
			cg.hasElevator = true;
		}

		cg.Flat = flat;

		CHUNKS.Add (newChunk);
	}

	void Update(){
		if (Input.GetMouseButtonUp (0)) {
			Application.LoadLevel(Application.loadedLevel);
		}

		foreach (GameObject chunk in CHUNKS) {
			if(Vector3.Distance (PlayerObject.position, chunk.transform.position) < Chunk_SizeWidth*35){
				if(!chunk.activeSelf){
					chunk.SetActive(true);
				}
			}else{
				if(chunk.activeSelf){
					chunk.SetActive(false);
				}
			}
		}

		if (Vector3.Distance (PlayerObject.position, chunk_NextRight.position) < Chunk_SizeWidth * 5f) {
			CreateCHUNK(1, false, false, 0);
		}

		if (Vector3.Distance (PlayerObject.position, chunk_NextLeft.position) < Chunk_SizeWidth * 5f) {
			CreateCHUNK(2, false, false, 0);
		}
	}
}
