using UnityEngine;
using System.Collections;

public class Swipe : MonoBehaviour {

	private TextMesh textMesh;
	private GameObject trail;

    public EasyJoystick moveJoy;
	// Subscribe to events
	void OnEnable(){
        
		EasyTouch.On_SwipeStart += On_SwipeStart;
		EasyTouch.On_Swipe += On_Swipe;
		EasyTouch.On_SwipeEnd += On_SwipeEnd;		
	}

	void OnDisable(){
		UnsubscribeEvent();
		
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_SwipeStart -= On_SwipeStart;
		EasyTouch.On_Swipe -= On_Swipe;
		EasyTouch.On_SwipeEnd -= On_SwipeEnd;	
	}
	
	void Start()
    {
        
        EasyTouch.AddReservedArea(new Rect(0, Screen.height, Screen.width, 400f));
		//textMesh = GameObject.Find("LastSwipeText").transform.gameObject.GetComponent("TextMesh") as TextMesh;
	}
	
	// At the swipe beginning 
	private void On_SwipeStart( Gesture gesture){


		// Only for the first finger
		if (!gesture.isHoverReservedArea && trail==null){ 
			
			// the world coordinate from touch for z=5
			Vector3 position = gesture.GetTouchToWordlPoint(0);
			trail = Instantiate( Resources.Load("Trail"),position,Quaternion.identity) as GameObject;
		}
	}
	
	// During the swipe
	private void On_Swipe(Gesture gesture){
		
		if (trail!=null){
			
			// the world coordinate from touch for z=5
			Vector3 position = gesture.GetTouchToWordlPoint(5);
			trail.transform.position = position;
		}
	}
	
	// At the swipe end 
	private void On_SwipeEnd(Gesture gesture){
		
		if (trail!=null){
			Destroy(trail);
			
			// Get the swipe angle
			float angles = gesture.GetSwipeOrDragAngle();
			//textMesh.text = "Last swipe : " + gesture.swipe.ToString() + " /  vector : " + gesture.swipeVector.normalized + " / angle : " + angles.ToString("f2") + " / " + gesture.deltaPosition.x.ToString("f5");
		}
				
	}
}
