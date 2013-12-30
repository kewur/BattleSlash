using UnityEngine;
using System.Collections;

public enum PowerUp
{
    Heal,
    Fire

}

public class PowerUpScript : MonoBehaviour {

	// Use this for initialization

    public PowerUp name = PowerUp.Heal;
   

	void Start () 
    {

        GameObject powerCube = transform.FindChild("PowerUp").gameObject;

        Transform Top = powerCube.transform.FindChild("Top");
        Vector3 pos = Top.position;
        Vector3 rot = Top.rotation.eulerAngles;


	  // iTween.RotateTo(gameObject, iTween.Hash("rotation", rot, "time", 2, "looptype" , LoopType.pingPong));
       powerCube.RotateTo(rot, 2, 0, EaseType.easeInOutQuad, LoopType.pingPong);
       powerCube.MoveTo(pos, 2, 0, EaseType.easeInOutQuad, LoopType.pingPong);

	}


    void OnTriggerEnter(Collider other)
    {

        print("yoho");

        if (other.collider.tag == Tags.Player)
        {
            other.gameObject.GetComponent<InputHandler>().LockInToClosestEnemy();
            PlayerScript.mSingleton.activatePowerUp(name);
            Destroy(gameObject);


        }


    }
	
}
