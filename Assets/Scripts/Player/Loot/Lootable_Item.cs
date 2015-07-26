using UnityEngine;
using System.Collections;

public class Lootable_Item : MonoBehaviour {
	public InventoryItem Item;

	void Start(){
		GetComponent<Renderer>().material.mainTexture = Item.Icon.texture;
	}
}
