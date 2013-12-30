using UnityEngine;
using System.Collections;

public class LookAtEnemy : MonoBehaviour {
	
	public GameObject Enemy;
	
	void Awake()
	{
		Enemy = GameObject.FindGameObjectWithTag("Enemy");
		
	}
     
	// Update is called once per frame
	void Update () 
	{
	
	 //Quaternion targetRotation = 
	 		
			
	 transform.LookAt(Enemy.transform);	 
		
	}
}
