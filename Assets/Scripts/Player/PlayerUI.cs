using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour {
	public GameObject Canvas_PlayerHUD;
	public GameObject Canvas_PlayerScreen;

	public GameObject PlayerScreen_Character;
	public GameObject PlayerScreen_Inventory;

	public Color[] Inventory_ItemRarity_Colors;
	GameObject Inventory_ItemName, Inventory_ItemTexture, Inventory_ItemTypes, Inventory_ItemQuantity;
	GameObject Inventory_ItemNext_ToLeft, Inventory_ItemNext_ToRight;
	GameObject EmptyInventory, CurrentItem;
	int Inventory_CurrentItem = 0;
	
	GameObject HUD_HPBox, HUD_HP, HUD_Coins;

	PlayerData pd;
	PlayerInventory pi;

	void Start(){
		Inventory_ItemName = PlayerScreen_Inventory.transform.Find("Item_Name").gameObject;
		Inventory_ItemTexture = PlayerScreen_Inventory.transform.Find("Item_Texture").gameObject;
		Inventory_ItemTypes = PlayerScreen_Inventory.transform.Find("Item_Types").gameObject;
		Inventory_ItemQuantity = Inventory_ItemTexture.transform.Find("Item_Quantity").gameObject;
		Inventory_ItemNext_ToLeft = PlayerScreen_Inventory.transform.Find("Item_ToLeft").gameObject;
		Inventory_ItemNext_ToRight = PlayerScreen_Inventory.transform.Find("Item_ToRight").gameObject;
		EmptyInventory = PlayerScreen_Inventory.transform.Find("EmptyInventory").gameObject;
		CurrentItem = PlayerScreen_Inventory.transform.Find("CurrentItem").gameObject;

		HUD_HPBox = Canvas_PlayerHUD.transform.Find("HP_Box").Find("HP").gameObject;
		HUD_HP = HUD_HPBox.transform.Find("Text").gameObject;
		HUD_Coins = Canvas_PlayerHUD.transform.Find("Coins_Box").Find("Text").gameObject;

		pi = GetComponent<PlayerInventory>();
		pd = GetComponent<PlayerData>();
	}

	void Update(){
		//if(Canvas_PlayerScreen.activeSelf){
			if(PlayerScreen_Inventory.activeSelf){
				if(!EmptyInventory.activeSelf){
					if(Inventory_CurrentItem == 0){
						Inventory_ItemNext_ToLeft.GetComponent<Button>().interactable = false;
					}else{
						Inventory_ItemNext_ToLeft.GetComponent<Button>().interactable = true;
					}

					if(Inventory_CurrentItem == pi.InventoryFreeSlot-1){
						Inventory_ItemNext_ToRight.GetComponent<Button>().interactable = false;
					}else{
						Inventory_ItemNext_ToRight.GetComponent<Button>().interactable = true;
					}
				}

				if(pi.InventoryFreeSlot > 0){
					if(EmptyInventory.activeSelf){
						Inventory_ItemName.SetActive(true);
						Inventory_ItemTexture.SetActive(true);
						Inventory_ItemTypes.SetActive(true);
						Inventory_ItemNext_ToRight.SetActive(true);
						Inventory_ItemNext_ToLeft.SetActive(true);
						CurrentItem.SetActive(true);

						EmptyInventory.SetActive(false);
					}
				}else{
					if(!EmptyInventory.activeSelf){
						Inventory_ItemName.SetActive(false);
						Inventory_ItemTexture.SetActive(false);
						Inventory_ItemTypes.SetActive(false);
						Inventory_ItemNext_ToRight.SetActive(false);
						Inventory_ItemNext_ToLeft.SetActive(false);
						CurrentItem.SetActive(false);

						EmptyInventory.SetActive(true);
					}
				}
			}
		//}

		if(Canvas_PlayerHUD.activeSelf){
			HUD_HPBox.GetComponent<Image>().fillAmount = pd.Player_CurHP / pd.Player_MaxHP;
			HUD_HP.GetComponent<Text>().text = (int)pd.Player_CurHP + " / " + (int)pd.Player_MaxHP;
			HUD_Coins.GetComponent<Text>().text = pd.Player_Coins + "";
		}
	}

	public void UpdateInventoryInfo(){
		Inventory_ItemName.GetComponent<Text>().text = pi.INVENTORY[Inventory_CurrentItem].Name;
		Inventory_ItemTexture.GetComponent<Image>().sprite = pi.INVENTORY[Inventory_CurrentItem].Icon;
		Inventory_ItemTypes.GetComponent<Text>().text = pi.INVENTORY[Inventory_CurrentItem].Type + " (" + pi.INVENTORY[Inventory_CurrentItem].SubType + ")";

		if(pi.INVENTORY[Inventory_CurrentItem].Quantity > 1){
			Inventory_ItemQuantity.SetActive(true);
			Inventory_ItemQuantity.transform.Find("Text").gameObject.GetComponent<Text>().text = pi.INVENTORY[Inventory_CurrentItem].Quantity + "";
		}else{
			Inventory_ItemQuantity.SetActive(false);
		}

		CurrentItem.transform.Find("Text").gameObject.GetComponent<Text>().text = (Inventory_CurrentItem+1) + " / " + pi.InventoryFreeSlot;

		int rarity = 0;
		if(pi.INVENTORY[Inventory_CurrentItem].Rarity == "Common"){ rarity = 0;}
		if(pi.INVENTORY[Inventory_CurrentItem].Rarity == "Uncommon"){ rarity = 1;}
		if(pi.INVENTORY[Inventory_CurrentItem].Rarity == "Rare"){ rarity = 2;}
		if(pi.INVENTORY[Inventory_CurrentItem].Rarity == "Mythic"){ rarity = 3;}
		PlayerScreen_Inventory.GetComponent<Image>().color = Inventory_ItemRarity_Colors[rarity];
	}

	public void Button_Inventory_NextItem_ToRight(){
		Inventory_CurrentItem++;
		UpdateInventoryInfo();
	}
	public void Button_Inventory_NextItem_ToLeft(){
		Inventory_CurrentItem--;
		UpdateInventoryInfo();	
	}

	public void Button_Close(){
		Canvas_PlayerScreen.SetActive(false);
		Canvas_PlayerHUD.SetActive(true);
	}

	public void Button_Open(){
		Canvas_PlayerScreen.SetActive(true);
		Canvas_PlayerHUD.SetActive(false);
		Button_Character();
	}

	public void Button_Character(){
		PlayerScreen_Character.SetActive(true);
		PlayerScreen_Inventory.SetActive(false);
	}

	public void Button_Inventory(){
		PlayerScreen_Character.SetActive(false);
		PlayerScreen_Inventory.SetActive(true);
		UpdateInventoryInfo();
	}

}
