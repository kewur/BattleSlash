using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class HelperUtil
{
    private float startTime = 0f;
    private float t = 0f;
    private float duration = -1f;

    private Vector3 startPos = Vector3.zero, endPos = Vector3.zero;
    private Vector3[] BezierValues;


    private Quaternion startRot = Quaternion.identity, endRot = Quaternion.identity;

    public HelperUtil()
    {
        BezierValues = new Vector3[4];
    }
    /// <summary>
    /// use this if you want to feed the positions every frame
    /// </summary>
    /// <param name="mdur"></param>
    public void StartLerp(float mdur)
    {
        startTime = Time.time;
        duration = mdur;

    }

    public static void print(object message)
    {

        Debug.Log(message);

    }

    /// <summary>
    /// use this if you want to feed the positions only once
    /// </summary>
    /// <param name="mdur"></param>
    public void StartLerp( Vector3 start, Vector3 end, float mdur)
    {
        t = 0;
        startTime = Time.time;
        duration = mdur;
        startPos = start;
        endPos = end;


    }
  

    public void StartLerpQ(Quaternion start, Quaternion end, float mdur)
    {
        t = 0f;
        startTime = Time.time;
        duration = mdur;
        startRot = start;
        endRot = end;

    }

    public void StartBezier(Vector3 start, Vector3 c1, Vector3 c2, Vector3 end, float mdur)
    {
        t = 0f;
        startTime = Time.time;
        duration = mdur;
        BezierValues[0] = start;
        BezierValues[1] = c1;
        BezierValues[2] = c2;
        BezierValues[3] = end;

    }


    public Vector3 getBezier()
    {
        Vector3 result = Vector3.zero;
        t = (Time.time - startTime) / duration;
        if (duration == -1)
        {
            Debug.LogError("Lerp not started, use StartLerp function to initialize");
            return result;
        }
        result = Bezier3(BezierValues[0], BezierValues[1], BezierValues[2], BezierValues[3], (Time.time - startTime) / duration);

        return result;
    }

    /// <summary>
    /// feed positions
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    /// 
    public Vector3 getLerp(Vector3 start, Vector3 end)
    {
        Vector3 result = Vector3.zero;

        if (duration == -1)
        {
            Debug.LogError("Lerp not started, use StartLerp function to initialize");
            return result;
        }
        result = Vector3.Lerp(start, end, (Time.time - startTime) / duration);

        return result;

    }

    public Vector3 getLerp()
    {
        Vector3 result = Vector3.zero;
        t = (Time.time - startTime) / duration;
        if (duration == -1)
        {
            Debug.LogError("Lerp not started, use StartLerp function to initialize");
            return result;
        }
        result = Vector3.Lerp(startPos, endPos, (Time.time - startTime) / duration);

        
       
      
        return result;

    }

    public Quaternion getLerpQ()
    {
        Quaternion result = Quaternion.identity;
        t = (Time.time - startTime) / duration;
        if (duration == -1)
        {
            Debug.LogError("Lerp not started, use StartLerpQ function to initialize");
            return result;
        }

        result = Quaternion.Lerp(startRot, endRot, (Time.time - startTime) / duration);

        return result;

    }

    Vector3 Bezier3(Vector3 s, Vector3 st, Vector3 et, Vector3 e, float t)
    {
        return (((-s + 3 * (st - et) + e) * t + (3 * (s + et) - 6 * st)) * t + 3 * (st - s)) * t + s;
    }

     Vector3 Bezier2(Vector3 Start, Vector3 Control, Vector3 End, float t)
    {
        return (((1-t)*(1-t)) * Start) + (2 * t * (1 - t) * Control) + ((t * t) * End);
    }

   
  

    public void resetTimer()
    {
        startTime = Time.time;
        t = 0f;
    }

    public void ChangeLerpDirection()
    {
        Vector3 tmp;
        tmp = startPos;
        endPos = startPos;
        startPos = tmp;
    }

    public void ChangeLerpRotation()
    {
        Quaternion tmp;
        tmp = startRot;
        endRot = startRot;
        startRot = tmp;

    }

    public void ChangeLerpDirectionAndReset()
    {
        ChangeLerpDirection();
        resetTimer();

    }

    public void resetLerper()
    {
        t = 0f;
        duration = -1;
        startPos = Vector3.zero;
        endPos = Vector3.zero;
        startRot = Quaternion.identity;
        endRot = Quaternion.identity;

    }
    /// <summary>
    /// checks whether the lerp has ended, if ended resets and stops the lerp. use for one time transitions
    /// </summary>
    /// <returns></returns>
    public bool checkLerpAndReset()
    {
        if (t < 1f)
        {

            resetLerper();
            return false;

        }

        return true;

    }

    public bool lerpFinished()
    {
       

        if (t < 1f)
            return false;

        return true;

    }
    /// <summary>
    /// /* Counts the ON bits in n. Use this if you know n is mostly 0s */
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public int bitCount(int n)
    {
        int tot = 0;

        while (n != 0)
        {
            ++tot;
            n &= n - 1;
        }

        return tot;
    }
    /// <summary>
    /// returns the smallest on bits value e.g 0011 = 1, 1110 = 2, 1100 = 4 ...
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public int smallestFlag(int n)
    {
        int tot = 0;
        int one = 1;

        while ((n & one) != one)
        {
            n = n >> 1;
            tot++;
        }

        return (int)Mathf.Pow(2, tot);
    }
	
}
