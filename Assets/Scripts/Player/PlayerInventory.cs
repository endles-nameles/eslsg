using UnityEngine;
using System.Collections;

[System.Serializable]
public class InventoryItem{
	public string Name;
	public string Type;
	public string SubType;
	public string Rarity;
	public bool Stackable;
	public int Quantity;
	public Sprite Icon;

	public InventoryItem(){
		Name = "";
		Type = "";
		SubType = "";
		Rarity = "";
		Stackable = false;
		Quantity = 0;
		Icon = null;
	}
}

public class PlayerInventory : MonoBehaviour {
	public InventoryItem[] INVENTORY;
	
	public int InventorySize;
	[System.NonSerialized]
	public int InventoryFreeSlot;

	void Start(){
		INVENTORY = new InventoryItem[InventorySize];
		for(int i=0;i<INVENTORY.Length;i++){
			INVENTORY[i] = new InventoryItem();
		}
	}

	public void AddInventoryItem(InventoryItem Item){
		for(int i=0;i<INVENTORY.Length;i++){
			if(INVENTORY[i].Name == Item.Name && INVENTORY[i].Stackable){
				INVENTORY[i].Quantity += 1;

				print(Item.Name + " ADDED TO INVENTORY.");

				return;
			}
		}

		if(InventoryFreeSlot < InventorySize){
			INVENTORY[InventoryFreeSlot] = Item;

			print(Item.Name + " ADDED TO INVENTORY.");

			InventoryFreeSlot++;

			return;
		}

		print("INVENTORY FULL.");
	}
}
