using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class ChunkMeshGenerator : MonoBehaviour {
	public int Chunk_SizeWidth, Chunk_SizeHeight;
	public float World_Amplitude, World_Scale;
	public int World_Seed;
	public int Type;

	[System.NonSerialized]
	public List<Vector3> newVertices = new List<Vector3>();
	[System.NonSerialized]
	public List<int> newTriangles = new List<int>();
	[System.NonSerialized]
	public List<Vector2> newUV = new List<Vector2>();
	[System.NonSerialized]
	public List<Vector3> colVertices = new List<Vector3>();
	[System.NonSerialized]
	public List<int> colTriangles = new List<int>();

	Mesh mesh;
	MeshCollider col;

	int squareCount;
	int colCount;

	float tileUnit = 0.25f;
	Vector3 tile_Underground = new Vector2(0, 0);
	Vector3 tile_Surface = new Vector2(0, 1);
	Vector3 tile_Solid = new Vector2(0, 2);

	public bool update = false;

	public byte[,] Blocks;
	public int Index;

	void Start(){
		mesh = GetComponent<MeshFilter>().mesh;
		col = GetComponent<MeshCollider>();

		GenerateChunk();
		BuildMesh();
		UpdateMesh();
	}

	void Update(){
		if(update){
			BuildMesh();
			UpdateMesh();
			update = false;
		}
	}

	void UpdateMesh(){
		mesh.Clear();
		mesh.vertices = newVertices.ToArray();
		mesh.triangles = newTriangles.ToArray();
		mesh.uv = newUV.ToArray();
		mesh.Optimize();
		mesh.RecalculateNormals();

		squareCount = 0;
		newVertices.Clear();
		newTriangles.Clear();
		newUV.Clear();

		Mesh newMesh = new Mesh();
		newMesh.vertices = colVertices.ToArray();
		newMesh.triangles = colTriangles.ToArray();
		col.sharedMesh = newMesh;

		colVertices.Clear();
		colTriangles.Clear();
		colCount = 0;
	}

	void GenerateBlock(int x, int y, Vector2 texture){
		newVertices.Add(new Vector3(x, y, 0));
		newVertices.Add(new Vector3(x+1, y, 0));
		newVertices.Add(new Vector3(x+1, y-1, 0));
		newVertices.Add(new Vector3(x, y-1, 0));

		newTriangles.Add((squareCount*4));
		newTriangles.Add((squareCount*4)+1);
		newTriangles.Add((squareCount*4)+3);
		newTriangles.Add((squareCount*4)+1);
		newTriangles.Add((squareCount*4)+2);
		newTriangles.Add((squareCount*4)+3);

		newUV.Add(new Vector2(tileUnit*texture.x, tileUnit*texture.y + tileUnit));
		newUV.Add(new Vector2(tileUnit*texture.x + tileUnit, tileUnit*texture.y + tileUnit));
		newUV.Add(new Vector2(tileUnit*texture.x + tileUnit, tileUnit*texture.y));
		newUV.Add(new Vector2(tileUnit*texture.x, tileUnit*texture.y));

		squareCount++;
	}

	void GenerateChunk(){
		Blocks = new byte[Chunk_SizeWidth, Chunk_SizeHeight];

		for(int px=0;px<Blocks.GetLength(0);px++){
			for(int py=0;py<Blocks.GetLength(1);py++){
				if(Type == 0){
					float posY = Noise(World_Seed+px+(int)(transform.position.x), World_Seed+py, World_Scale, World_Amplitude, 1);
					if(py < posY){
						Blocks[px, py] = 1;		
					}else{
						Blocks[px, py] = 0;
					}
				}

				if(Type == 1){
					float posY = Noise(World_Seed+px+(int)(transform.position.x), World_Seed+py+(int)(transform.position.y), 45, World_Amplitude, 1);
					if(posY > 4.5f){
						Blocks[px, py] = 1;
					}else{
						Blocks[px, py] = 0;
					}
				}

				if(Type == 2){
					if(py < Blocks.GetLength(1)-1){
						Blocks[px, py] = 3;
					}else{
						Blocks[px, py] = 2;
					}
				}
			}
		}

		for(int px=0;px<Blocks.GetLength(0);px++){
			for(int py=0;py<Blocks.GetLength(1);py++){
				if(Blocks[px, py] == 1){
					if(py != Blocks.GetLength(1)-1){
						if(Blocks[px, py+1] == 0){
							Blocks[px, py] = 2;
						}
					}else{
						if(Type == 0){
							Blocks[px, py] = 2;
						}
					}
				}
			}
		}
	}

	void BuildMesh(){
		for(int px=0;px<Blocks.GetLength(0);px++){
			for(int py=0;py<Blocks.GetLength(1);py++){
				if(Blocks[px, py] != 0){
					GenerateCollider(px, py);

					if(Blocks[px, py] == 1){
						GenerateBlock(px, py, tile_Underground);
					}
					if(Blocks[px, py] == 2){
						GenerateBlock(px, py, tile_Surface);
					}
					if(Blocks[px, py] == 3){
						GenerateBlock(px, py, tile_Solid);
					}
				}
			}
		}
	}

	void GenerateCollider(int x, int y){
		colVertices.Add( new Vector3 (x  , y-1, 0));
	  	colVertices.Add( new Vector3 (x + 1 , y-1  , 0));
	  	colVertices.Add( new Vector3 (x + 1 , y  , 0 ));
	  	colVertices.Add( new Vector3 (x  , y  , 0 ));
	   
		ColliderTriangles();
		colCount++;

		//top col mesh
		//if(BlockBoundaries(x,y+1)==0){
	  		colVertices.Add( new Vector3 (x  , y  , 1));
	  		colVertices.Add( new Vector3 (x + 1 , y  , 1));
	  		colVertices.Add( new Vector3 (x + 1 , y  , 0 ));
	  		colVertices.Add( new Vector3 (x  , y  , 0 ));
	   
		    ColliderTriangles();
		    colCount++;
		//}
	   
	    //bot col mesh
	    //if(BlockBoundaries(x,y-1)==0){
		    colVertices.Add( new Vector3 (x, y-1, 0));
		    colVertices.Add( new Vector3 (x + 1, y-1, 0));
		    colVertices.Add( new Vector3 (x + 1, y-1, 1));
		    colVertices.Add( new Vector3 (x, y-1, 1));
		   
		    ColliderTriangles();
		    colCount++;
		//}
	   
	    //left col mesh
	    //if(BlockBoundaries(x-1,y)==0){
		    colVertices.Add( new Vector3 (x  , y -1 , 1));
	        colVertices.Add( new Vector3 (x  , y  , 1));
		    colVertices.Add( new Vector3 (x  , y  , 0 ));
		    colVertices.Add( new Vector3 (x  , y -1 , 0 ));
		   
		    ColliderTriangles();
		    colCount++;
		//}
	   
	    //right col mesh
	    //if(BlockBoundaries(x+1,y)==0){
		    colVertices.Add( new Vector3 (x +1 , y  , 1));
		    colVertices.Add( new Vector3 (x +1 , y -1 , 1));
		    colVertices.Add( new Vector3 (x +1 , y -1 , 0 ));
		    colVertices.Add( new Vector3 (x +1 , y  , 0 ));
		   
		    ColliderTriangles();
		    colCount++;
		//}
	}

	void ColliderTriangles(){
 		colTriangles.Add(colCount*4);
 		colTriangles.Add((colCount*4)+1);
 		colTriangles.Add((colCount*4)+3);
 		colTriangles.Add((colCount*4)+1);
 		colTriangles.Add((colCount*4)+2);
 		colTriangles.Add((colCount*4)+3);
	}

	byte BlockBoundaries(int x, int y){
 		if(x==-1 || x==Blocks.GetLength(0) ||   y==-1 || y==Blocks.GetLength(1)){
  			return (byte)1;
 		}
  
 		return 0;
	}

	int Noise (int x, int y, float scale, float amp, float exp){
		return (int) (Mathf.Pow ((Mathf.PerlinNoise(x/scale,y/scale)*amp),(exp)));	
	}
}
