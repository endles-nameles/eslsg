using UnityEngine;
using System.Collections;

[System.Serializable]
public class InventoryItem{
	public string Name;
	public string Type;
	public string Rarity;
	public bool Stackable;
	public int Quantity;
	public Texture2D Icon;
}

public class PlayerInventory : MonoBehaviour {
	public InventoryItem[] INVENTORY;
	
	public int InventorySize;
	int InventoryFreeSlot;

	void Start(){
		INVENTORY = new InventoryItem[InventorySize];
	}

	void AddInventoryItem(string Name, string Type, string Rarity, bool Stackable, Texture2D Icon){
		for(int i=0;i<INVENTORY.Length;i++){
			if(INVENTORY[i].Name == Name && INVENTORY[i].Stackable){
				INVENTORY[i].Quantity += 1;
				return;
			}
		}

		if(InventoryFreeSlot < InventorySize){
			INVENTORY[InventoryFreeSlot].Name = Name;
			INVENTORY[InventoryFreeSlot].Type = Type;
			INVENTORY[InventoryFreeSlot].Rarity = Rarity;
			INVENTORY[InventoryFreeSlot].Stackable = Stackable;
			INVENTORY[InventoryFreeSlot].Quantity = 1;
			INVENTORY[InventoryFreeSlot].Icon = Icon;

			InventoryFreeSlot++;
		}
	}
}
