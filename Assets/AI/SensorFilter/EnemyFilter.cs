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

[RAINSerializableClass, RAINElement("Enemy Filter")]
public class EnemyFilter : RAINSensorFilter
{
    [RAINSerializableField(Visibility = FieldVisibility.Show, ToolTip = "The team to sense")]
    public string team = "Red";

    public override void Filter(RAINSensor aSensor, List<RAINAspect> aValues)
    {
        //Perform some filtering of the aValues list, leaving only the RAINAspects that you want to keep
        if(aValues.Count > 0)
            Debug.Log(aValues.Count);

    }
}