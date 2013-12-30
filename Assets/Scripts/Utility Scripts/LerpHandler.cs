using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void FinishedCallBack();

public class LerpHandler
{

    struct LerpingItems
    {
       public HelperUtil helper;
       public Transform lerpedObj;
       public int lerpID;
       public FinishedCallBack finished;
    }

    List<LerpingItems> ActiveHelpersTransform;
    List<LerpingItems> ActiveHelpersQuaternion;
    List<LerpingItems> ActiveHelpersBezier;

    int lerpCountT = 0;
    int lerpCountQ = 0;
    int lerpCountB = 0;

    public LerpHandler()
    {
        ActiveHelpersTransform = new List<LerpingItems>();
        ActiveHelpersQuaternion = new List<LerpingItems>();
        ActiveHelpersBezier = new List<LerpingItems>();
    }

    public int addHelperBezier(Vector3 start, Vector3 c1, Vector3 c2, Vector3 end, float time, Transform lerpedObj)
    {
        lerpCountB++;
        LerpingItems newItem;
        HelperUtil newHelper = new HelperUtil();

        newItem.helper = newHelper;
        newItem.lerpedObj = lerpedObj;
        newItem.lerpID = lerpCountB;
        newItem.helper.StartBezier(start, c1, c2, end, time);
        newItem.finished = null;

        ActiveHelpersBezier.Add(newItem);

        return newItem.lerpID;
    }

    public int addHelperBezier(Vector3 start, Vector3 c1, Vector3 c2, Vector3 end, float time, Transform lerpedObj, FinishedCallBack callback)
    {

        lerpCountB++;
        LerpingItems newItem;
        HelperUtil newHelper = new HelperUtil();

        newItem.helper = newHelper;
        newItem.lerpedObj = lerpedObj;
        newItem.lerpID = lerpCountB;
        newItem.helper.StartBezier(start, c1, c2, end, time);
        newItem.finished = callback;

        ActiveHelpersBezier.Add(newItem);

        return newItem.lerpID;
    }

    public int addHelperTransform(Vector3 start, Vector3 end, float time, Transform lerpedObj)
    {
        lerpCountT++;
       
        LerpingItems newItem;
        HelperUtil newHelper = new HelperUtil();

        newItem.helper = newHelper;
        newItem.lerpedObj = lerpedObj;
        newItem.lerpID = lerpCountT;
        newItem.helper.StartLerp(start, end, time);
        newItem.finished = null;

        ActiveHelpersTransform.Add(newItem);

        return newItem.lerpID;
       
    }

    public int addHelperTransform(Vector3 start, Vector3 end, float time, Transform lerpedObj, FinishedCallBack callback)
    {
        lerpCountT++;
        LerpingItems newItem;
        HelperUtil newHelper = new HelperUtil();

        newItem.helper = newHelper;
        newItem.lerpedObj = lerpedObj;
        newItem.lerpID = lerpCountT;
        newItem.helper.StartLerp(start, end, time);
        newItem.finished = callback;

        ActiveHelpersTransform.Add(newItem);

        return newItem.lerpID;

    }


    public int addHelperQuaternion(Quaternion start, Quaternion end, float time, Transform lerpedObj)
    {
        lerpCountQ++;
        LerpingItems newItem;
        HelperUtil newHelper = new HelperUtil();

        newItem.helper = newHelper;
        newItem.lerpedObj = lerpedObj;
        newItem.lerpID = lerpCountQ;

        newItem.helper.StartLerpQ(start, end, time);
        newItem.finished = null;
        ActiveHelpersQuaternion.Add(newItem);

        return newItem.lerpID;
    }

    public int addHelperQuaternion(Quaternion start, Quaternion end, float time, Transform lerpedObj, FinishedCallBack callback)
    {
        lerpCountQ++;
        LerpingItems newItem;
        HelperUtil newHelper = new HelperUtil();

        newItem.helper = newHelper;
        newItem.lerpedObj = lerpedObj;
        newItem.lerpID = lerpCountQ;

        newItem.helper.StartLerpQ(start, end, time);
        newItem.finished = callback;
        ActiveHelpersQuaternion.Add(newItem);

        return newItem.lerpID;
    }


    public void removeHelperBezier(int lerperID)
    {
        for (int i = 0; i < ActiveHelpersBezier.Count; i++)
        {

            if (ActiveHelpersBezier[i].lerpID == lerperID)
            {
                ActiveHelpersBezier.RemoveAt(i);
                return;
            }
        }

        Debug.LogError("lerper not found in active lerps (Bezier)");
    }


    public void removeHelperTransform(int lerperID)
    {
        for (int i = 0; i < ActiveHelpersTransform.Count; i++ )
        {
            
            if (ActiveHelpersTransform[i].lerpID == lerperID)
            {
                ActiveHelpersTransform.RemoveAt(i);
                return;
            }
        }

        Debug.LogError("lerper not found in active lerps (Transform)");
    }

    public void removeHelperQuaternion(int lerperID)
    {
        for (int i = 0; i < ActiveHelpersQuaternion.Count; i++)
        {

            if (ActiveHelpersQuaternion[i].lerpID == lerperID)
            {
                ActiveHelpersQuaternion.RemoveAt(i);
                return;
            }
        }

        Debug.LogError("lerper not found in active lerps (Quaternion)");
    }


    void handleVectorLerps()
    {
        for (int i = 0; i < ActiveHelpersTransform.Count; i++)
        {
            if (ActiveHelpersTransform[i].helper.lerpFinished())
            {
                if (ActiveHelpersTransform[i].finished != null)
                {
                    ActiveHelpersTransform[i].finished(); // use the call back if provided

                }

                ActiveHelpersTransform.RemoveAt(i);
                continue;
            }

            else
            {

                ActiveHelpersTransform[i].lerpedObj.localPosition = ActiveHelpersTransform[i].helper.getLerp();
            }


        }

        if (ActiveHelpersTransform.Count == 0)
            lerpCountT = 0;

    }

    void handleBezierLerps()
    {

        for (int i = 0; i < ActiveHelpersBezier.Count; i++)
        {
            if (ActiveHelpersBezier[i].helper.lerpFinished())
            {
                if (ActiveHelpersBezier[i].finished != null)
                    ActiveHelpersBezier[i].finished(); // use the call back if provided
                ActiveHelpersBezier.RemoveAt(i);
                continue;
            }

            else
            {
                ActiveHelpersBezier[i].lerpedObj.localPosition = ActiveHelpersBezier[i].helper.getBezier();
            }

        }

        if (ActiveHelpersBezier.Count == 0)
            lerpCountB = 0;

    }


    void handleQuaternionLerps()
    {
        for (int i = 0; i < ActiveHelpersQuaternion.Count; i++)
        {
            if (ActiveHelpersQuaternion[i].helper.lerpFinished())
            {
                if (ActiveHelpersQuaternion[i].finished != null)
                    ActiveHelpersQuaternion[i].finished(); // use the call back if provided
                ActiveHelpersQuaternion.RemoveAt(i);
                continue;
            }

            else
            {
                ActiveHelpersQuaternion[i].lerpedObj.localRotation = ActiveHelpersQuaternion[i].helper.getLerpQ();
            }

        }

        if (ActiveHelpersQuaternion.Count == 0)
            lerpCountQ = 0;

    }


    

    public void OnUpdate()
    {

        handleVectorLerps();

        handleQuaternionLerps();

        handleBezierLerps();

    }
	

}
