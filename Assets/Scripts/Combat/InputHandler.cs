using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour {



    public GameObject HudTextPrefab;

    public WeaponScript WeaponHand;
    public GameObject ShieldHand;
    public Transform LockedInTarget;
    public GameObject SelectionProjector;
    private float selectionProjectorHeight = 0f;
    public static InputHandler mSingleton;
    private GameObject trail;

    private Vector3[] SwordStartEndPos;
    private Quaternion[] SwordStartEndQ;
    LerpHandler lerpHandler;


    public GameObject FollowArrowSwords;
    public GameObject GUIPanel;

    float damping = 6.0f;

    
    public bool rightHanded ;

    public GameObject[] Enemies;
    public GameObject[] Allies;
    public List<GameObject> PowerUps;

    private bool PowerAttack = false;
    private bool cancelAttack = false;
    int cont = 0;

    private float StartY;
    private float StartXRot;
	// Use this for initialization
	void Awake()
    {
        if (!SelectionProjector)
            Debug.LogError("Assign Selection Projector");

        StartY = transform.position.y;
        StartXRot = transform.rotation.eulerAngles.x;

        selectionProjectorHeight = 2f;
        lerpHandler = new LerpHandler();
        mSingleton = this;

        SwordStartEndPos = new Vector3[2];
        SwordStartEndQ = new Quaternion[2];
        Enemies = GameObject.FindGameObjectsWithTag(Tags.Enemy);
        Allies = GameObject.FindGameObjectsWithTag(Tags.Ally);
        PowerUps = new List<GameObject>();

        foreach (GameObject enemy in Enemies)
        {
            assignFollowArrowAndHud(enemy);
        }

        foreach (GameObject ally in Allies)
        {
            assignFollowArrowAndHud(ally);
        }

        Debug.Log("Number of enemies in the arena: " + Enemies.Length);
        Debug.Log("Number of allies in the arena: " + Allies.Length);


        StartFight();
	}

    void assignFollowArrowAndHud(GameObject entity)
    {

        entity.GetComponent<EntityScript>().hudText = NGUITools.AddChild(GUIPanel, HudTextPrefab).GetComponent<HUDText>();
        entity.GetComponent<EntityScript>().hudText.GetComponent<UIFollowTarget>().target = entity.GetComponent<EntityScript>().hudTextTarget;
        

        if (entity.tag == Tags.Enemy)
        {
            if (entity.GetComponent<NPCScript>().type == EnemyType.Swordsman)
            {
                entity.GetComponent<NPCScript>().FollowArrow = NGUITools.AddChild(GUIPanel, FollowArrowSwords);

            }
        }
        
    }

    void assignFollowPowerUp()
    {


    }


    void addNewEnemy()
    {


    }

    void StartFight()
    {
        LockInToClosestEnemy();

    }


    public void LockInToClosestEnemy()
    {

        float dist = Mathf.Infinity;
        Transform closestEnemy = null;
        foreach (GameObject enemy in Enemies)
        {
            float enemDist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist > enemDist)
            {
                closestEnemy = enemy.transform;
                Transform target = enemy.transform.FindChild("LockInTarget");
                Vector3 targetPos = target.position;
                targetPos.y = transform.position.y;

             //   target.transform.position = targetPos;

                dist = enemDist;

            }
        }

        LockInTarget(closestEnemy);

    }
	
	// Update is called once per frame
	void Update () 
    {

        Vector3 pos = transform.position;
        pos.y = StartY;

        Vector3 rot = transform.rotation.eulerAngles;
        rot.x = StartXRot;

        transform.position = pos;
   

        lerpHandler.OnUpdate();
        
	}

    void LateUpdate()
    {
        if (LockedInTarget != null)
        {

            Transform target = LockedInTarget.transform.FindChild("LockInTarget");
            Vector3 lockInTargetPlayerLevel = target.position;
            lockInTargetPlayerLevel.y = PlayerScript.mSingleton.transform.position.y;

            target.position = lockInTargetPlayerLevel;

            Quaternion rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }


        UpdateArrows();
    }

    void UpdateArrows()
    {

        IndicatorPanel.mSingleton.UpdateIndicators(Enemies, Allies, PowerUps);

    }

    void OnGUI()
    {
       // GUI.Box(new Rect(0, 0, Screen.width / 2, Screen.height), "osman");
       // GUI.Box(new Rect(Screen.width / 2, 0, Screen.width / 2, Screen.height * 2.35f / 4), "osman2");

    }

    public int RegisterLerpMotionT(Vector3 start, Vector3 end, float time, Transform lerpedObj)
    {

        return lerpHandler.addHelperTransform(start, end, time, lerpedObj);
    }

    public int RegisterLerpMotionQ(Quaternion start, Quaternion end, float time, Transform lerpedObj)
    {

        return lerpHandler.addHelperQuaternion(start, end, time, lerpedObj);
    }

    /// <summary>
    /// with callback
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="time"></param>
    /// <param name="lerpedObj"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public int RegisterLerpMotionT(Vector3 start, Vector3 end, float time, Transform lerpedObj, FinishedCallBack callback)
    {

        return lerpHandler.addHelperTransform(start, end, time, lerpedObj, callback);
    }
    /// <summary>
    /// with callback
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="time"></param>
    /// <param name="lerpedObj"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public int RegisterLerpMotionQ(Quaternion start, Quaternion end, float time, Transform lerpedObj, FinishedCallBack callback)
    {

        return lerpHandler.addHelperQuaternion(start, end, time, lerpedObj, callback);
    }

    public int RegisterLerpMotionBezier(Vector3 start, Vector3 c1, Vector3 c2, Vector3 end, Transform lerpedObj, float time)
    {
        return lerpHandler.addHelperBezier(start, c1, c2, end, time, lerpedObj);
    }

    public int RegisterLerpMotionBezier(Vector3 start, Vector3 c1, Vector3 c2, Vector3 end, Transform lerpedObj, float time, FinishedCallBack callback)
    {
        return lerpHandler.addHelperBezier(start, c1, c2, end, time, lerpedObj, callback);
    }

    public void UnRegisterLerpMotionBezier(int id)
    {
        lerpHandler.removeHelperBezier(id);
    }

    public void UnRegisterLerpMotionT(int id)
    {
        lerpHandler.removeHelperTransform(id);
    }

    public void UnRegisterLerpMotionQ(int id)
    {
        lerpHandler.removeHelperQuaternion(id);

    }


    
    // Subscribe to events
    void OnEnable()
    {

        EasyTouch.On_SwipeStart += On_SwipeStart;
        EasyTouch.On_Swipe += On_Swipe;
        EasyTouch.On_SwipeEnd += On_SwipeEnd;
        EasyTouch.On_DoubleTap += On_DoubleTap;
        
    }

    void OnDisable()
    {
        UnsubscribeEvent();

    }

    void OnDestroy()
    {
        UnsubscribeEvent();
    }

    void UnsubscribeEvent()
    {
        EasyTouch.On_SwipeStart -= On_SwipeStart;
        EasyTouch.On_Swipe -= On_Swipe;
        EasyTouch.On_SwipeEnd -= On_SwipeEnd;
    }

    void Start()
    {
        
      //  EasyTouch.AddReservedArea(new Rect(0, 0, Screen.width / 2, Screen.height));
       // EasyTouch.AddReservedArea(new Rect(Screen.width / 2, Screen.height / 2, Screen.width / 2, Screen.height / 3));
        //textMesh = GameObject.Find("LastSwipeText").transform.gameObject.GetComponent("TextMesh") as TextMesh;
        
    }


    private void On_DoubleTap(Gesture gesture)
    {
        Ray ray = Camera.main.ScreenPointToRay(gesture.position);
        Debug.DrawRay(ray.origin, ray.direction * 110, Color.red, 1f);

        RaycastHit[] rayHits;
  

        rayHits = Physics.RaycastAll(ray, 220f);

        if (rayHits.Length > 0)
        {

            // accept differnet collisions for (buildingfloor -> (world floor)) and for   (wall, stairs -> (building floor))
            foreach (RaycastHit hit in rayHits)
            {
                if (hit.collider.tag == Tags.Enemy)
                {
                    LockInTarget(hit.collider.transform);
                    break;
                }
                else if (hit.collider.tag == Tags.Shield)
                {
                    LockInTarget(hit.collider.transform.parent.transform.parent.transform); // shield < shieldHand < Enemy
                    break;

                }

                else if (hit.collider.tag == Tags.ShieldHand)
                {
                    LockInTarget(hit.collider.transform.parent.transform); //  shieldHand < Enemy
                    break;

                }

                else if (hit.collider.tag == Tags.PowerUp)
                {
                    LockInTarget(hit.collider.transform); //  last check power up
                    break;
                }

            }

        }
    }

    public void LockInTarget(Transform target)
    {

        if (!SelectionProjector.activeSelf)
            SelectionProjector.SetActive(true);

        SelectionProjector.transform.parent = target;
        Vector3 pos = Vector3.zero;
        pos.y = selectionProjectorHeight;
        SelectionProjector.transform.localPosition = pos;
        
        LockedInTarget = target;
    }

    // At the swipe beginning 
    private void On_SwipeStart(Gesture gesture)
    {

        cont = 0;
        // Only for the first finger
        
      //  print(gesture.IsInRect(swipeArea));
        // determine if in normalzied position the point is on top right, for left handed just switch to lesser equals on x
       // print(gesture.NormalizedPosition());
        if (gesture.NormalizedPosition().x >= 0.3f && gesture.NormalizedPosition().y >= 0.35f && trail == null)
        {

            // the world coordinate from touch for z=5
            


            Vector3 position = gesture.GetTouchToWordlPoint(2);
           // WeaponHand.transform.position = position;
           // SwordStartEndPos[0] = position;
           // SwordStartEndQ[0] = Quaternion.identity;

           // PlayerScript.mSingleton.Weapon.startSwingPosition(gesture.NormalizedPosition());

            trail = Instantiate(Resources.Load("Trail"), GUIPanel.transform.position, Quaternion.identity) as GameObject;
            trail.layer = GUIPanel.layer;
            trail.transform.parent = GUIPanel.transform;
            Vector3 newPos = gesture.position;
           
            
            newPos.z = 3f;
            trail.transform.localPosition = newPos;

        }
    }

    // During the swipe
    private void On_Swipe(Gesture gesture)
    {
        if (gesture.NormalizedPosition().x < 0.3f && gesture.NormalizedPosition().y < 0.35f)
            return;

        if (trail != null )
        {
           
            cont++;
            Vector3 position = gesture.GetTouchToWordlPoint(5);
            //trail.transform.position = gesture.position;
            Vector3 newPos = gesture.position;
            newPos.z = 3f;
            trail.transform.localPosition = newPos;
            //PlayerScript.mSingleton.Weapon.swingPosition(gesture.NormalizedPosition());
            

            /*
            if (!PowerAttack)
            {
                // the world coordinate from touch for z=5
                
                Vector3 position = gesture.GetTouchToWordlPoint(5);
                trail.transform.position = position;
                PlayerScript.mSingleton.Weapon.swingPosition(position);'
                if (gesture.actionTime > 1.70f)
                {
                    PowerAttack = true;
                    WeaponHand.Vibrate();
                    trail.renderer.material.SetColor("_TintColor", Color.red);
                    trail.GetComponent<TrailRenderer>().time = 6;

                   

                }
            }
                 cancel after some time is disabled for now, uncomment these sections if you want it back
            else
            {
                if (gesture.actionTime > 6.05f)
                {
                    WeaponHand.StopVibrate();
                    cancelAttack = true;
                }
            }*/
            
        }

        
    }

    // At the swipe end 
    private void On_SwipeEnd(Gesture gesture)
    {
        
       
        if (trail != null)
        {
            if (cancelAttack)
            {
                cancelAttack = false;
                return;

            }
            if(PowerAttack)
                WeaponHand.StopVibrate();
           // WeaponHand.StopVibrate();
            // Get the swipe angle
            float angles = gesture.GetSwipeOrDragAngle();
           // Vector3 position = gesture.GetTouchToWordlPoint(10);
           // PlayerScript.mSingleton.Weapon.endSwingPosition(gesture.NormalizedPosition());
           // SwordStartEndPos[1] = position;
          //  SwordStartEndQ[1] = Quaternion.identity;
            WeaponScript.SwingDirection direction = WeaponScript.SwingDirection.None;
           
           // print("Last swipe : " + gesture.swipe.ToString() + " /  vector : " + gesture.swipeVector.normalized + " / angle : " + angles.ToString("f2") + " / " + gesture.deltaPosition.x.ToString("f5"));
           // print("Swipe Type " + gesture.swipe.ToString() + " Angle " + angles.ToString());

            if (gesture.swipe == EasyTouch.SwipeType.Other)
            {
                if (angles > -180 && angles < -90)
                {
                  //  print("Right up to left down");
                    direction = WeaponScript.SwingDirection.RightTop_LeftBot;

                }
                else if (angles > -90 && angles < 0)
                {
                   // print("Left Up to Right Down");
                    direction = WeaponScript.SwingDirection.LeftTop_RightBot;
                }

                else if (angles > 0 && angles < 90)
                {
                  //  print("Left Down to Right Up");
                    direction = WeaponScript.SwingDirection.LeftBot_RightTop;
                }

                else if (angles > 90 && angles < 180)
                {
                  //  print("Right Down to Left Up");
                    direction = WeaponScript.SwingDirection.RightBot_LeftTop;
                }


            }

            else
            {
                if (gesture.swipe == EasyTouch.SwipeType.Left)
                    direction = WeaponScript.SwingDirection.Right_Left;
                else if (gesture.swipe == EasyTouch.SwipeType.Right)
                    direction = WeaponScript.SwingDirection.Left_Right;                   
                else if (gesture.swipe == EasyTouch.SwipeType.Up)
                    direction = WeaponScript.SwingDirection.Bot_Top;
                else if (gesture.swipe == EasyTouch.SwipeType.Down)
                    direction = WeaponScript.SwingDirection.Top_Bot;
                    

            }


            if (direction != WeaponScript.SwingDirection.None)
            {
                PlayerScript.mSingleton.Weapon.SwingWeapon(direction);
            }

            
            Destroy(trail);
            PowerAttack = false;
        }

    }

    private void weaponPositions()
    {

    }


}
