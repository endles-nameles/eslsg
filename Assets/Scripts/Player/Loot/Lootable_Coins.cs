using UnityEngine;
using System.Collections;

public class Lootable_Coins : MonoBehaviour {
	public int Amount;

	void Start(){
		Amount += Random.Range(-10, 10);
	}
}
