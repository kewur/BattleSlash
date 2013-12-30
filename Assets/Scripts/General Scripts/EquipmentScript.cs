using UnityEngine;
using System.Collections;


public enum EquipType
{
	head,
	shoulder,
	chest,
	legs,
	hands,
	weapon,
	shield
}

public class EquipmentScript : MonoBehaviour {
	
	public int Armor;
	public int Damage;
	public int Health;
	public int Swiftness;
	
	public EquipType Slot;
	
	
	public GameObject guiPrefab;
	public GameObject worldPrefab;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	
	/*
	#region getters
	public int Armor
	{
		get{return _armor;}
		set{_armor = value;}
		
	}
	
	public int Health
	{
		get{return _health;}
		set{_health = value;}
		
	}
	
	public int Damage
	{
		get{return _damage;}
		set{_damage = value;}
		
	}
	
	public int Swiftness
	{
		get{return _swiftness;}
		set{_swiftness = value;}
		
	}
	#endregion */
	
}


