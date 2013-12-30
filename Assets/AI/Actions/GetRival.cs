using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;
using RAIN.Representation;

[RAINAction]
public class GetRival : RAINAction
{

    public Expression target;

    public GetRival()
    {
        actionName = "GetRival";
    }

    public override void Start(AI ai)
    {

        base.Start(ai);
    }

    public override ActionResult Execute(AI ai)
    {

        GameObject enemyObject = target.Evaluate(ai.DeltaTime, ai.WorkingMemory).GetValue<GameObject>();
        if (enemyObject != null)
        {

            Debug.Log(enemyObject.name);
        }
       

  
       // HelperUtil.print(enemyObject.name); // (enemyObject.name);


        return ActionResult.SUCCESS;
    }

    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }
}