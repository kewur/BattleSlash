using UnityEngine;
using System.Collections;

public class VitalScript : MonoBehaviour {
	
	public int currentHealth;
	public int maxHealth;
	public int armor;
	
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	
	public void damageTaken(int damage)
	{
		currentHealth -= damage;
		if(currentHealth < 1)
			Destroy(this);
	}
	
	
		
	
}
