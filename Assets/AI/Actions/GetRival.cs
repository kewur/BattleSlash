using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;
using RAIN.Representation;
using RAIN.Entities.Aspects;
using RAIN.Serialization;
using RAIN.Perception.Sensors.Filters;
using RAIN.Perception.Sensors;

[RAINAction]
public class GetRival : RAINAction
{
    RAIN.Perception.Sensors.Filters.RAINSensorFilter filter;
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
        IList<RAINAspect> objs = ai.Senses.Match("Eyes", "Player");
        IList<RAINAspect> allies = ai.Senses.Match("Eyes", "Enemy");

        
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



