using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IndicatorPanel : MonoBehaviour {


   public static IndicatorPanel mSingleton;

    void Awake()
    {
        mSingleton = this;

    }

    public void UpdateIndicators(GameObject[] Enemies, GameObject[] Allies, List<GameObject> PowerUps)
    {
       
        foreach (GameObject enemy in Enemies)
        {
            NPCScript entity = enemy.GetComponent<NPCScript>();
            //print("Enemy " + i + " pos: " + (transform.position - enemy.transform.position).normalized);

            Vector3 screenPos = Camera.main.WorldToScreenPoint(enemy.transform.position);

            if (screenPos.z > 0 && // ONSCREEN
                screenPos.x > 0 && screenPos.x < Screen.width &&
                screenPos.y > 0 && screenPos.y < Screen.height)
            {
                if (entity.FollowArrow.activeSelf)
                    entity.FollowArrow.SetActive(false);
            }


            else // OFFSCREEN
            {

                if (!entity.FollowArrow.activeSelf)
                    entity.FollowArrow.SetActive(true);

                if (screenPos.z < 0)
                {
                    screenPos *= -1;
                }

                Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;

                screenPos -= screenCenter;

                float angle = Mathf.Atan2(screenPos.y, screenPos.x);
                angle -= 90 * Mathf.Deg2Rad;

                float cos = Mathf.Cos(angle);
                float sin = -Mathf.Sin(angle);

                screenPos = screenCenter + new Vector3(sin * 150, cos * 150, 0);

                float m = cos / sin;

                

                Vector3 screenBounds = screenCenter * 0.9f;
                
                if (cos > 0)
                { // check up and down
                 //   print("up");
                    screenPos = new Vector3(screenBounds.y / m, screenBounds.y, 0);
                }
                else
                { // down
                   // print("down");
                    screenPos = new Vector3(-screenBounds.y / m, -screenBounds.y, 0);
                }

                //if out of bounds, get point on appropriate side

                if (screenPos.x > screenBounds.x)
                {
                    screenPos = new Vector3(screenBounds.x, screenBounds.x * m, 0);
                }
                else if (screenPos.x < -screenBounds.x) //behind
                {

                    screenPos = new Vector3(-screenBounds.x, -screenBounds.x * m, 0);
                }

                screenPos += screenCenter;



                entity.FollowArrow.transform.localPosition = screenPos;
                entity.FollowArrow.transform.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
                setDistanceText(entity);
              //  print(enemy.GetComponent<EntityScript>().followArrow.transform.localPosition);

            }
            //  direction.x *= Screen.width / 2;
            //   direction.y *= Screen.height / 2;


        }

        foreach (GameObject ally in Allies)
        {

        }

        foreach (GameObject PowerUp in PowerUps)
        {

        }


    }


    void setDistanceText(NPCScript entity)
    {
        int distance = (int)Mathf.Floor(Vector3.Distance(entity.transform.position, PlayerScript.mSingleton.transform.position));

        if (distance < 13)
        {
            entity.DistanceText.color = Color.red;
        }
        else if (distance >= 13 && distance < 22)
        {
            entity.DistanceText.color = Color.yellow;
        }

        else if (distance >= 22)
        {
            entity.DistanceText.color = Color.green;
        }
        entity.DistanceText.text = distance.ToString() + " m";

    }
	
}
