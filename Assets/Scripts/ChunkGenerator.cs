//Written by Andrei Meluta -- 2015

using UnityEngine;
using System.Collections;

public class ChunkGenerator : MonoBehaviour {
	public int Chunk_SizeWidth, Chunk_SizeHeight;
	public float World_Amplitude, World_Scale;
	public int World_Seed;
	public Material Cube_Material;

	public float MaxHeight, MaxHeightPoint;

	public bool Flat;

	public bool hasElevator;

	public GameObject[,] Chunk;


	void Start(){
		Chunk = new GameObject[Chunk_SizeWidth, Chunk_SizeHeight];

		for(int i=0;i<Chunk_SizeWidth;i++){
			for(int j=0;j<Chunk_SizeHeight;j++){
				if(!Flat){
					float posY = Noise(World_Seed+i+(int)(transform.position.x), World_Seed+j, World_Scale, World_Amplitude, 1);

					if(j < posY){
						Chunk[i, j] = GameObject.CreatePrimitive(PrimitiveType.Cube);

						Chunk[i, j].transform.position = new Vector3(transform.position.x + i, transform.position.y + j, 0);
						Chunk[i, j].transform.parent = transform;
						Chunk[i, j].GetComponent<Renderer>().material = Cube_Material;
						Chunk[i, j].GetComponent<Renderer>().material.color = new Color (Random.value, Random.value, Random.value);

						if(j > MaxHeight){
							MaxHeight = j;
							MaxHeightPoint = i;
						}
						Chunk[i, j].GetComponent<Renderer>().material.color = new Color (Random.value, Random.value, Random.value);
					}else{
						Chunk[i, j] = null;
					}
				}else{
					if(j < Chunk_SizeHeight){
						Chunk[i, j] = GameObject.CreatePrimitive(PrimitiveType.Cube);
						
						Chunk[i, j].transform.position = new Vector3(transform.position.x + i, transform.position.y + j, 0);
						Chunk[i, j].transform.parent = transform;
						Chunk[i, j].GetComponent<Renderer>().material = Cube_Material;
						Chunk[i, j].GetComponent<Renderer>().material.color = new Color (Random.value, Random.value, Random.value);

						if(j > MaxHeight){
							MaxHeight = j;
							MaxHeightPoint = i;
						}
					}
				}
			}
		}
	}

	int Noise (int x, int y, float scale, float amp, float exp){
		return (int) (Mathf.Pow ((Mathf.PerlinNoise(x/scale,y/scale)*amp),(exp))); 
		
	}
}
