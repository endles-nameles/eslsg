//Written by Andrei Meluta -- 2015

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Biome{
	public float Amplitude, Scale;
}

public class WorldGenerator : MonoBehaviour {
	public int World_SizeWidth, World_SizeHeight;
	public int Chunk_SizeWidth, Chunk_SizeHeight;
	
	public List<GameObject> CHUNKS;
	public List<GameObject> ChunkSpawners;

	public Biome[] BIOMES;
	int Biome_CurrentRight, Biome_CurrentLeft;
	public int Biome_Frequency;
	public int FastTravel_Frequency;

	public int World_Seed;
	public Material Cube_Material;

	public Transform PlayerObject;

	void Start(){
		if (World_Seed == 0) {
			World_Seed = Random.Range (1, 100000);
		}

		for(int i=0;i<World_SizeWidth;i++){
			for(int j=0;j<World_SizeHeight;j++){
				GameObject newChunkSpawner = GameObject.CreatePrimitive(PrimitiveType.Cube);
				newChunkSpawner.transform.position = new Vector3(i*Chunk_SizeWidth, -j*Chunk_SizeHeight, 0);
				newChunkSpawner.transform.localScale = new Vector3(5, 5, 1);
				newChunkSpawner.name = "ChunkSpawner";
				newChunkSpawner.transform.parent = transform;
				newChunkSpawner.GetComponent<Collider>().enabled = false;

				if(j==0){
					newChunkSpawner.tag = "Chunk_Surface";
				}else{
					newChunkSpawner.tag = "Chunk_Underground";
				}

				ChunkSpawners.Add(newChunkSpawner);
			}
		}
	}

	void CreateCHUNK(Vector3 position, bool flat, bool building, int extraStruct){
		GameObject newChunk = new GameObject ("Chunk");

		ChunkGenerator cg = newChunk.AddComponent<ChunkGenerator> ();
		cg.Chunk_SizeWidth = Chunk_SizeWidth;
		cg.Chunk_SizeHeight = Chunk_SizeHeight;
		cg.World_Seed = World_Seed;
		cg.Cube_Material = Cube_Material;

		cg.World_Amplitude = BIOMES[Biome_CurrentRight].Amplitude;
		cg.World_Scale = BIOMES[Biome_CurrentRight].Scale;

		newChunk.transform.position = position;

		/*
		if(Chunks_Right%Biome_Frequency == 0){
			flat = true;
			Biome_CurrentRight = Random.Range(0, BIOMES.Length);
		}else{
			if(Chunks_Right%FastTravel_Frequency == 0){
				GameObject newFastTravel = GameObject.CreatePrimitive(PrimitiveType.Cube);
				newFastTravel.name = "Fast Travel";
				newFastTravel.transform.localScale = new Vector3(3f, 10f, 1);
				newFastTravel.transform.parent = newChunk.transform;
				newFastTravel.GetComponent<Renderer>().material = Cube_Material;
				newFastTravel.GetComponent<Renderer>().material.color = Color.black;

				FastTravelPoint ftp = newFastTravel.AddComponent<FastTravelPoint>();
				ftp.OwnerChunk = newChunk;
			}
		} 
		*/
	
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
			newBuilding.GetComponent<Renderer>().material = Cube_Material;
			newBuilding.GetComponent<Renderer>().material.color = Color.black;
		}

		if(extraStruct == 1){
			cg.hasElevator = true;
		}

		cg.Flat = flat;

		CHUNKS.Add (newChunk);
	}

	void Update(){
		foreach (GameObject chunk in CHUNKS) {
			if(Vector3.Distance (PlayerObject.position, chunk.transform.position) < Chunk_SizeWidth*4.5f){
				if(!chunk.activeSelf){
					chunk.SetActive(true);
				}
			}else{
				if(chunk.activeSelf){
					chunk.SetActive(false);
				}
			}
		}

		foreach(GameObject spawner in ChunkSpawners){
			if(Vector3.Distance (PlayerObject.position, spawner.transform.position) < Chunk_SizeWidth*7){
				if(spawner.GetComponent<Renderer>().enabled){
					if(spawner.tag == "Chunk_Surface"){
						CreateCHUNK(spawner.transform.position, false, false, 0);
						spawner.GetComponent<Renderer>().enabled = false;
					}
					if(spawner.tag == "Chunk_Underground"){
						CreateCHUNK(spawner.transform.position, true, false, 0);
						spawner.GetComponent<Renderer>().enabled = false;
					}
				}
			}
		}
	}
}
