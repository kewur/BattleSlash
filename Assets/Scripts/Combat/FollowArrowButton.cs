using UnityEngine;
using System.Collections;

public class FollowArrowButton : MonoBehaviour {

    public float clickTime;
    private Transform target;

    void Awake()
    {

    }

    void OnClick()
    {
        float time = Time.realtimeSinceStartup;

        if (this.clickTime + 0.25f > time)
        {
            OnDoubleClick();

        }
        this.clickTime = time;
        
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    void OnDoubleClick()
    {
        print(target.name);
        InputHandler.mSingleton.LockInTarget(target);
    }

}
