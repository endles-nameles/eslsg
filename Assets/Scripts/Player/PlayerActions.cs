using UnityEngine;
using System.Collections;

public class PlayerActions : MonoBehaviour {
	GameObject ActionCube;
	ActionCube ac;

	WorldGenerator wg;

	PlayerData pd;
	PlayerInventory pi;	
	
	RaycastHit hit;

	void Start(){
		wg = GameObject.Find("WorldGenerator").GetComponent<WorldGenerator>();
		pi = GetComponent<PlayerInventory>();
		pd = GetComponent<PlayerData>();

		ActionCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		ActionCube.name = "ActionCube";
		ActionCube.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
		ActionCube.GetComponent<Collider>().isTrigger = true;
		ActionCube.GetComponent<Renderer>().material.color = Color.black;
		
		ac = ActionCube.AddComponent<ActionCube>();
		
		Rigidbody ac_r = ActionCube.AddComponent<Rigidbody>();
		ac_r.useGravity = false;
	}

	void Update(){
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = 10;
		Vector3 ActionCube_BasePosition = Camera.main.ScreenToWorldPoint(mousePosition);

		ActionCube.transform.position = new Vector3(ActionCube_BasePosition.x, ActionCube_BasePosition.y, 0);

		if(Input.GetMouseButtonUp(0)){
			if(ac.State == "Loot"){
				if(ac.ObjectName == "Loot_Item"){
					Lootable_Item li = ac.Object.GetComponent<Lootable_Item>();

					if(li){ 
						pi.AddInventoryItem(li.Item); 
						Destroy(ac.Object);
					}
				}

				if(ac.ObjectName == "Loot_Coins"){
					Lootable_Coins lc = ac.Object.GetComponent<Lootable_Coins>();

					if(lc){ 
						pd.Player_Coins += lc.Amount;
						Destroy(ac.Object);
					}
				}
			}
		}
	}
}
