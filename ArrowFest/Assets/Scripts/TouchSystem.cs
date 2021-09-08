using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TouchSystemHandler();
public class TouchSystem : MonoBehaviour
{
    public static event TouchSystemHandler TouchDowned;
    public static event TouchSystemHandler TouchUpped;
    public static event TouchSystemHandler Touching;
    private static Vector3 distancePosTwoFrame, distancePosFirstActive;
    Vector3 firstPos, secondPos,firstclick;

    void Update()
    {
        #if UNITY_EDITOR
        PC_Control();
        #else
        Mobile_Control();
        #endif
    }

    public static Vector3 TouchPosition
    {
        #if UNITY_EDITOR
        get { return Input.mousePosition;}
        #else 
        get {return Input.GetTouch(0).position; }
        #endif
    }

    public static Vector3 DistanceBetweenTwoFrame
    {
        get { return distancePosTwoFrame; }
    }

    public static Vector3 DistanceBetweenFirstTouchAndActive
    {
        get { return distancePosFirstActive; }
    }

    public void PC_Control()
    {
        #if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            firstPos = TouchPosition;
            firstclick = firstPos;
            secondPos = firstPos;
            if (TouchDowned != null)
                TouchDowned();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (TouchUpped != null)
                TouchUpped();
        }
        else if (Input.GetMouseButton(0))
        {
            firstPos = TouchPosition;
            //Debug.Log(firstPos);
            distancePosTwoFrame = firstPos - secondPos;
            distancePosFirstActive = firstPos - firstclick;
            if (Touching != null)
                Touching();
        }
        secondPos = firstPos;
        #endif
    }
    
    public void Mobile_Control()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                firstPos = TouchPosition;
                firstclick = firstPos;
                secondPos = firstPos;
                if (TouchDowned != null)
                    TouchDowned();
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (TouchUpped != null)
                    TouchUpped();
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                firstPos = TouchPosition;
                distancePosTwoFrame = firstPos - secondPos;
                distancePosFirstActive = firstPos - firstclick;
                if (Touching != null)
                    Touching();
            }
            secondPos = firstPos;
        }
    }
}
