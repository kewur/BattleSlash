using UnityEngine;
using System.Collections;

public class EntityScript : MonoBehaviour {

    protected int ShieldonGoingLerpTID = -1;
    protected int ShieldonGoingLerpQID = -1;
    public GameObject ShieldHand;
    public GameObject WeaponHand;
    public Transform[] shieldStartEnd;
    public float shieldReactionTime = 1f;
    public HUDText hudText;
    public Transform hudTextTarget;

    public int health = 30;
    


    protected virtual void Awake()
    {
        WeaponHand = FindChildWithTag(Tags.WeaponHand);
        ShieldHand = FindChildWithTag(Tags.ShieldHand);

    }

    public virtual void ApplyDamage(int amount, bool isCritical)
    {
        health -= amount;
        if(isCritical == false)
            hudText.Add(amount.ToString(), Color.red, 0.2f);

        else
            hudText.Add(amount.ToString() + "!", Color.magenta, 0.4f);


        if (health <= 0)
            print("Die");



    }

    public void Block()
    {
        if (ShieldonGoingLerpTID != -1) // if the shield is already moving unregister the current lerp so that it doesn't intervene with each other
        {
            InputHandler.mSingleton.UnRegisterLerpMotionT(ShieldonGoingLerpTID);
        }

        ShieldonGoingLerpTID = InputHandler.mSingleton.RegisterLerpMotionT(ShieldHand.transform.localPosition, shieldStartEnd[1].localPosition, shieldReactionTime, ShieldHand.transform, this.BlockTFinished);


        if (ShieldonGoingLerpQID != -1)
        {
            InputHandler.mSingleton.UnRegisterLerpMotionQ(ShieldonGoingLerpQID);
        }

        ShieldonGoingLerpQID = InputHandler.mSingleton.RegisterLerpMotionQ(ShieldHand.transform.localRotation, shieldStartEnd[1].localRotation, shieldReactionTime, ShieldHand.transform, this.BlockQFinished);

    }

    public void UnBlock()
    {
        if (ShieldonGoingLerpTID != -1) // if the shield is already moving unregister the current lerp so that it doesn't intervene with each other
        {
           // print(ShieldonGoingLerpTID);
            InputHandler.mSingleton.UnRegisterLerpMotionT(ShieldonGoingLerpTID);
        }

        ShieldonGoingLerpTID = InputHandler.mSingleton.RegisterLerpMotionT(ShieldHand.transform.localPosition, shieldStartEnd[0].localPosition, shieldReactionTime, ShieldHand.transform, this.BlockTFinished);


        if (ShieldonGoingLerpQID != -1)
        {
            InputHandler.mSingleton.UnRegisterLerpMotionQ(ShieldonGoingLerpQID);
        }

        ShieldonGoingLerpQID = InputHandler.mSingleton.RegisterLerpMotionQ(ShieldHand.transform.localRotation, shieldStartEnd[0].localRotation, shieldReactionTime, ShieldHand.transform, this.BlockQFinished);

    }


    public void BlockTFinished()
    {      
        ShieldonGoingLerpTID = -1;
    }

    public void BlockQFinished()
    {
        ShieldonGoingLerpQID = -1;
    }
    
    GameObject FindChildWithTag(string tag)
    {
        GameObject obj = null;

        foreach (Transform child in transform)
        {
            if (child.tag == tag)
            {
                obj = child.gameObject;
                break;
            }
        }
        if (obj == null)
        {

            Debug.LogError("Couldn't find child with tag: " + tag);
        }
        return obj;
    }
}
