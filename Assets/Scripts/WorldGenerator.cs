//Written by Andrei Meluta -- 2015

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Biome{
	public float Amplitude, Scale;
	public Texture2D TileSheet;
}

public class WorldGenerator : MonoBehaviour {
	public int World_SizeWidth, World_SizeHeight;
	public int Chunk_SizeWidth, Chunk_SizeHeight;
	
	public List<GameObject> CHUNKS;
	public List<GameObject> ChunkSpawners;

	public Biome[] BIOMES;
	int Biome_Current;
	public int Biome_Frequency;
	public int FastTravel_Frequency;

	public int World_Seed;
	public Material Cube_Material;

	public Transform PlayerObject;

	public GameObject Town;
	public int Town_WorldPosition;

	bool chunkGeneration_Complete = false;

	void Start(){
		if (World_Seed == 0) {
			World_Seed = Random.Range (1, 100000);
		}

		Biome_Current = Random.Range(0, BIOMES.Length);
		Town_WorldPosition = World_SizeWidth/2+1;

		GenerateChunkSpawners();
		GenerateWorld();
		GenerateTown();
		GenerateExtraStructures();

		PlayerObject.transform.position = new Vector3(Town.transform.position.x, 200, 0);
	}

	void CreateCHUNK(Vector3 position, int type, int index){
		GameObject newChunk = new GameObject ("Chunk");

		ChunkMeshGenerator cg = newChunk.AddComponent<ChunkMeshGenerator> ();
		cg.Chunk_SizeWidth = Chunk_SizeWidth;
		cg.Chunk_SizeHeight = Chunk_SizeHeight;
		cg.World_Seed = World_Seed;
		cg.Type = type; //0-normal, 1-flat, 2-platform
		cg.Index = index;

		cg.World_Amplitude = BIOMES[Biome_Current].Amplitude;
		cg.World_Scale = BIOMES[Biome_Current].Scale;

		newChunk.transform.position = position;
		newChunk.GetComponent<Renderer>().material = Cube_Material;
		newChunk.GetComponent<Renderer>().material.mainTexture = BIOMES[Biome_Current].TileSheet;

		if(type == 0 || type == 2){
			newChunk.tag = "Chunk_Surface";
		}else{
			newChunk.tag = "Chunk_Underground";
		}
	
		/*if (building) {
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
		}*/

		CHUNKS.Add (newChunk);
	}

	void GenerateChunkSpawners(){
		for(int i=0;i<World_SizeWidth;i++){
			for(int j=0;j<World_SizeHeight;j++){
				if(i < Town_WorldPosition - 1 || i > Town_WorldPosition + 1){
					GameObject newChunkSpawner = GameObject.CreatePrimitive(PrimitiveType.Cube);
					newChunkSpawner.transform.position = new Vector3(i*Chunk_SizeWidth, -j*Chunk_SizeHeight+ World_SizeHeight*Chunk_SizeHeight, 0);
					newChunkSpawner.transform.localScale = new Vector3(5, 5, 1);
					newChunkSpawner.name = "ChunkSpawner";
					newChunkSpawner.transform.parent = transform;
					newChunkSpawner.GetComponent<Collider>().enabled = false;

					ChunkSpawnerInfo csi = newChunkSpawner.AddComponent<ChunkSpawnerInfo>();

					if(j==0){
						newChunkSpawner.tag = "Chunk_Surface";
					}else{
						newChunkSpawner.tag = "Chunk_Underground";
					}

					if(i < Town_WorldPosition - 1){
						csi.Index = -(i);
					}
					if(i> Town_WorldPosition + 1){
						csi.Index = World_SizeWidth - (i) + 1;
					}

					ChunkSpawners.Add(newChunkSpawner);
				}
			}
		}
	}

	void GenerateWorld(){
		foreach(GameObject spawner in ChunkSpawners){
			if(spawner.GetComponent<Renderer>().enabled){
				ChunkSpawnerInfo csi = spawner.GetComponent<ChunkSpawnerInfo>();
				if(spawner.tag == "Chunk_Surface"){
					if(csi.Index%Biome_Frequency == 0 && csi.Index != 0){
						CreateCHUNK(spawner.transform.position, 2, csi.Index);
						Biome_Current = Random.Range(0, BIOMES.Length);
					}else{
						CreateCHUNK(spawner.transform.position, 0, csi.Index);
					}

					spawner.GetComponent<Renderer>().enabled = false;
				}
				if(spawner.tag == "Chunk_Underground"){
					CreateCHUNK(spawner.transform.position, 1, csi.Index);
					spawner.GetComponent<Renderer>().enabled = false;
				}
			}
		}
	}

	void GenerateTown(){
		Town = GameObject.CreatePrimitive(PrimitiveType.Cube);
		Town.name = "Town";

		Town.transform.position = new Vector3(Town_WorldPosition*Chunk_SizeWidth + Chunk_SizeWidth/2, (Chunk_SizeHeight*World_SizeHeight)/2 + Chunk_SizeHeight, 0);
		Town.transform.localScale = new Vector3(Chunk_SizeWidth*3, Chunk_SizeHeight*World_SizeHeight, 2);

		Town.GetComponent<Renderer>().material = Cube_Material;
	}

	void GenerateExtraStructures(){
		ChunkMeshGenerator cmg;

		foreach (GameObject chunk in CHUNKS) {
			cmg = chunk.GetComponent<ChunkMeshGenerator>();

			//Biome Boundaries - Elevators
			float xpos1, xpos2;
			if(chunk.transform.position.x < Town.transform.position.x){
				xpos1 = chunk.transform.position.x + Chunk_SizeWidth - 2.5f;
				xpos2 = chunk.transform.position.x + 2.5f;
			}else{
				xpos1 = chunk.transform.position.x + 2.5f;
				xpos2 = chunk.transform.position.x + Chunk_SizeWidth - 2.5f;
			}
			if(cmg.Type == 0 && chunk.tag == "Chunk_Surface" && (Mathf.Abs(cmg.Index)+1)%Biome_Frequency == 0){
				GameObject newElevator = GameObject.CreatePrimitive(PrimitiveType.Cube);
				newElevator.name = "Elevator";
				newElevator.tag = "MovingPlatform";
				newElevator.transform.parent = chunk.transform;
				newElevator.transform.localScale = new Vector3(4, 0.75f, 1);
				newElevator.transform.position = new Vector3(xpos1, chunk.transform.position.y + Chunk_SizeHeight - 0.5f, 0);
				newElevator.AddComponent<Elevator>();
			}

			if(cmg.Type == 0 && chunk.tag == "Chunk_Surface" && (Mathf.Abs(cmg.Index)-1)%Biome_Frequency == 0){
				GameObject newElevator = GameObject.CreatePrimitive(PrimitiveType.Cube);
				newElevator.name = "Elevator";
				newElevator.tag = "MovingPlatform";
				newElevator.transform.parent = chunk.transform;
				newElevator.transform.localScale = new Vector3(4, 0.75f, 1);
				newElevator.transform.position = new Vector3(xpos2, chunk.transform.position.y + Chunk_SizeHeight - 0.5f, 0);
				newElevator.AddComponent<Elevator>();
			}
			//Elevators
		}
	}

	void Update(){
		foreach (GameObject chunk in CHUNKS) {
			if(Vector3.Distance (PlayerObject.position, chunk.transform.position) < Chunk_SizeWidth*2.5f){
				if(!chunk.activeSelf){
					chunk.SetActive(true);
				}
			}else{
				if(chunk.activeSelf){
					chunk.SetActive(false);
				}
			}
		}
	}
}
