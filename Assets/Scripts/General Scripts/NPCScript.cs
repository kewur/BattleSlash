using UnityEngine;
using System.Collections;
using RAIN.Core;
using RAIN.Action;
using RAIN.Representation;

public class NPCScript : EntityScript
{
    public GameObject followArrow;
    private UILabel followArrowLabel;
    public EnemyType type;
    private AIRig aiRig = null;
    // Use this for initialization

    protected override void Awake()
    {
        base.Awake();
        aiRig = gameObject.GetComponentInChildren<AIRig>();

       
    }


    public GameObject FollowArrow
    {
        get { return followArrow; }

        set
        {
            followArrow = value;
            followArrow.GetComponent<FollowArrowButton>().SetTarget(transform); //for followarrow button function
            DistanceText = followArrow.transform.FindChild("Label").GetComponent<UILabel>(); // cache label for use in the indicatorPanel script

        }
    }

    public UILabel DistanceText
    {
        get { return followArrowLabel; }
        set { followArrowLabel = value; }

    }

    public void Update()
    {
        aiRig.AI.WorkingMemory.SetItem("health", health);

    }


}


