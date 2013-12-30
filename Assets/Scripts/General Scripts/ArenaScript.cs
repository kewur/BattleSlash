using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ArenaScript : MonoBehaviour {

    List<GameObject> PowerUps;
    int ElapsedTime = 0;
    float startTime = 0f;



	// Use this for initialization
	void Start () 
    {
        PowerUps = new List<GameObject>();
        StartCoroutine("OnTick");


	}

    private IEnumerator OnTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            ElapsedTime++;
            print(ElapsedTime);

        }


    }
    
}
