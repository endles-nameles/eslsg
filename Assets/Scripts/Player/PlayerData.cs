using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {
	public float Player_MaxHP;
	public float Player_CurHP;

	public int Player_Coins;

	void Start(){
		Player_CurHP = Player_MaxHP;
	}
}
