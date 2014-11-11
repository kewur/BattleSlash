using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Team
{
    None,
    Red,
    Blue
}


public class ArenaScript : MonoBehaviour {

    List<GameObject> PowerUps;
    int ElapsedTime = 0;
    float startTime = 0f;

    public List<EntityScript> BlueTeam;
    public List<EntityScript> RedTeam;
    public List<EntityScript> NoTeamEnemies;

    public static ArenaScript Instance = null;

	// Use this for initialization
	void Start () 
    {
        Instance = this;
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
