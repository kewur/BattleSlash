using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponScript : MonoBehaviour {

    public enum SwingDirection
    {
        None,
        Right_Left,
        Left_Right,
        Top_Bot,
        Bot_Top,
        RightTop_LeftBot,
        LeftTop_RightBot,
        LeftBot_RightTop,
        RightBot_LeftTop
    }

    public enum WeaponType
    { 
        None,
        MeleeWeapon,      
        Ranged
    }

    public float pathVal = 0f;

    public List<GameObject> enemiesHitInSwing;
    public WeaponType EquippedWeaponType;
	public AudioClip clashSound;
	public AudioClip woundSound;
	public ParticleSystem woundEffect;

    private Vector3 oldPosition;

    private Vector3 weaponDefaultPosition;
    private Quaternion weaponDefaultRotation;

    private List<Vector2> swingPositions;

    public float AttackSpeed = 0.5f;
    public float CriticalChance = 0.2f;
    public int minDamage = 5;
    public int maxDamage = 12;


    public GameObject WeaponPositions; //TO-DO leave this for debug, instantiate this according to the weapon type used 
    int vibrateLerpId = -1;
    int swingLerpId = -1;

    bool vibrating = false;
    bool Attacking = false;

    SwingDirection swipeInQueue = SwingDirection.None;

    void Awake()
    {
        enemiesHitInSwing = new List<GameObject>();
        weaponDefaultPosition = transform.localPosition;
        weaponDefaultRotation = transform.localRotation;
        
        if (EquippedWeaponType == WeaponType.MeleeWeapon || EquippedWeaponType == WeaponType.None)
        {
            //WeaponPositions = Resources.Load("SwingmeleeWeaponPositions");
            //positions.transform.parent = transform.parent;
 
        }
    }

    void initializeRealSwingPositions()
    {


    }

	void Start () 
	{
        swingPositions = new List<Vector2>();
   
	}


   

    
	// Update is called once per frame
	void OnTriggerEnter(Collider other)
	{
        
        if (Attacking) //if not attacking don't do anything
        {
            if (EnemyAlreadyHit(other.gameObject) == true)
            {
                print("Enemy already hit");
                return;

            }

            List<string> tagToCheck = new List<string>();

            if (transform.parent.tag == Tags.Player || transform.parent.tag == Tags.Ally)
            {
                tagToCheck.Add(Tags.Enemy);
            }
            else
            {
                tagToCheck.Add(Tags.Ally);
                tagToCheck.Add(Tags.Player);
            }

            if (other.tag == "Weapon")
            {

                if (EnemyAlreadyHit(other.transform.parent.transform.parent.gameObject) == true) // if the enemy is already hit, don't play the clash sound
                    return;

                Debug.Log("Hit Sword");
                return;
            }

            foreach (string tag in tagToCheck)
            {
                if (other.tag == tag )
                {
                    enemiesHitInSwing.Add(other.gameObject);

                    int baseDamage = Random.Range(minDamage, maxDamage);
                    float crit = Random.Range(0f, 1f);
                    bool isCritical = false;

                    if (crit <= CriticalChance)
                    {
                        isCritical = true;
                        baseDamage *= 2;
                    }

                    other.GetComponent<EntityScript>().ApplyDamage(baseDamage, isCritical);
                    return;

                }
            }
        }	
	}

    bool EnemyAlreadyHit(GameObject obj)
    {
        foreach(GameObject enemy in enemiesHitInSwing)
        {
            if (enemy.name == obj.name)
                return true;

        }

        return false;

    }

    public void Vibrate()
    {
        if (vibrating)
            return;
        vibrating = true;
        oldPosition = transform.localPosition;

        Vector3 vibratePos = transform.localPosition;

       
        vibratePos.x += Random.Range(0.03f, 0.05f);
        vibratePos.y += Random.Range(0.03f, 0.05f);
        vibratePos.z += Random.Range(0.03f, 0.05f);
      

        vibrateLerpId = InputHandler.mSingleton.RegisterLerpMotionT(transform.localPosition, vibratePos, 0.05f, transform, this.vibrateComeBackFinished);
    }

    public void StopVibrate()
    {
        if (vibrateLerpId != -1)
            InputHandler.mSingleton.UnRegisterLerpMotionT(vibrateLerpId);
        transform.localPosition = oldPosition;
        vibrateLerpId = -1;
        vibrating = false;
    }

    public void vibrateFinished()
    {
       
        vibrating = false;
        vibrateLerpId = -1;
        Vibrate();

    }

    public void vibrateComeBackFinished()
    {
       
        vibrateLerpId = InputHandler.mSingleton.RegisterLerpMotionT(transform.localPosition, oldPosition, 0.05f, transform, this.vibrateFinished);
    }

	public void Attack()
	{
		
	}

    public void startSwingPosition(Vector2 pos)
    {
        swingPositions.Clear(); // clear all previous swing positions

        swingPositions.Add(pos); // need to find the actual value before putting it here
        //print("Start Swing Pos: " + pos);

    }


    public void endSwingPosition(Vector2 pos)
    {
        /*
        swingPositions.Add(pos); // get the values we are going to use and swing the sword

        Vector3[] points = new Vector3[4];

        int oneThird =(int)Mathf.Floor(swingPositions.Count / 3f);

        if (swingPositions.Count < 4)
            return;

        float distance = Mathf.Infinity;
        int SwingStyle = 0;

        int swingCount = swingPositions.Count;


        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < System.Enum.GetNames(typeof(SwingPosition)).Length; j++) // first
            {
                if (Vector2.Distance(swingPositions[(int)Mathf.Floor(swingCount * ( i / 3))], swingTresholds[j]) < distance)
                {
                    SwingStyle = j;
                }

            }
            points[i] = swingTresholds[SwingStyle];
        }
        
        
       
        /*
        points[0] = swingPositions[0];
        points[0].z = 0.1f;

        points[1] = swingPositions[oneThird];
        points[1].z = 0.3f;

        points[2] = swingPositions[oneThird * 2];
        points[2].z = 0.6f;

        points[3] = pos;
        points[3].z = 1f;
       
         

        if (swingLerpId != -1)
        {
            InputHandler.mSingleton.UnRegisterLerpMotionBezier(swingLerpId);
        }
        SwingWeapon(points);
        print("End Swing Pos: " + pos);
         *  */
    }

    public void SwingWeapon(Vector3[] points)
    {
        swingLerpId = InputHandler.mSingleton.RegisterLerpMotionBezier(points[0], points[1], points[1], points[2], transform, AttackSpeed, this.swingFinished);

    }

    public void SwingWeapon(SwingDirection direction)
    {
        print(direction.ToString());
        string[] dirStr = direction.ToString().Split('_');
        string fromPos = dirStr[0]; string toPos = dirStr[1]; string fromMidToPos = fromPos + "Mid" + toPos;

     
     
        // gameObject.MoveTo(iTweenPath.GetPath("RightTopLeftBot"), 1, 0, EaseType.easeOutCubic);
        Transform[] swingPositions = new Transform[3];
        //print(fromPos);
        swingPositions[0] = WeaponPositions.transform.FindChild(fromPos);

        //print(fromMidToPos);
        swingPositions[1] = WeaponPositions.transform.FindChild(fromMidToPos);

        //print(toPos);
        swingPositions[2] = WeaponPositions.transform.FindChild(toPos);
        
        Vector3[] vectPosSwing = new Vector3[3];
        vectPosSwing[0] = swingPositions[0].localPosition;
        vectPosSwing[1] = swingPositions[1].localPosition;
        vectPosSwing[2] = swingPositions[2].localPosition;

        Vector3[] vectRotSwing = new Vector3[2];
        vectRotSwing[0] = weaponDefaultRotation.eulerAngles;
        vectRotSwing[1] = swingPositions[2].eulerAngles;
      //  iTween.MoveTo(gameObject, iTween.Hash());
        

       // SwingWeapon(swingPositions);
        
        // WeaponPositions

       // iTween.PutOnPath(gameObject, swingPositions, pathVal);
       // gameObject.MoveTo(swingPositions,1f, 0f);

        if (Attacking == false) //if currently not attacking, attack
        {

          //  gameObject.MoveTo(vectPosSwing[0], 0.1f, 0);
            gameObject.RotateTo(vectRotSwing[1], AttackSpeed , 0);
            
            iTween.MoveTo(gameObject, iTween.Hash("path", vectPosSwing, "time", AttackSpeed , "islocal", true, "oncomplete", "SwingCompleted", "easetype", "easeOutBack"));
            Attacking = true;
        }
        else // if already attacking save this swipe and use it after it's finished
        {
            swipeInQueue = direction;
            return;
        }

        GameObject weaponTrail = transform.GetChild(0).transform.FindChild("Trail").gameObject;

        if (weaponTrail != null)
        {
            weaponTrail.GetComponent<TrailRenderer>().enabled = true;
        }

    }

    public void RealSwing()
    {


    }

    public void SwingCompleted()
    {

        GameObject weaponTrail = transform.GetChild(0).transform.FindChild("Trail").gameObject;

        if (weaponTrail != null)
        {
            weaponTrail.GetComponent<TrailRenderer>().enabled = false;
        }
        iTween.MoveTo(gameObject, iTween.Hash("position", weaponDefaultPosition, "time", 0.2f, "islocal", true, "easetype", "easeInExpo", "oncomplete", "SwordInPosition"));
        transform.localRotation = weaponDefaultRotation;

       
    }
    public void SwordInPosition()
    {
        Attacking = false;
        enemiesHitInSwing.Clear();
        

        if (swipeInQueue != SwingDirection.None)
        {           
            SwingWeapon(swipeInQueue);
            swipeInQueue = SwingDirection.None;
        }
    }


    public void swingFinished()
    {
        transform.localPosition = weaponDefaultPosition;
        transform.localRotation = weaponDefaultRotation;
        swingLerpId = -1;
        
    }
    
	
}
