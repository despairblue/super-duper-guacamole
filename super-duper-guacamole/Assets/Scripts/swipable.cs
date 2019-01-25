using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DigitalRuby.Tween;


public class swipable : EventTrigger
{
    private int dragMargin = 300;
    private Vector2 startPos;
    void Start()
    {
        startPos = transform.position;
    }

    public override void OnDrag(PointerEventData data)
    {
        transform.position = data.position;
    }

    public override void OnEndDrag(PointerEventData data)
    {
        if ((data.position - startPos).x >= dragMargin) {
            swipeRight();
        }
        else if ((data.position - startPos).x <= -dragMargin) {
            swipeLeft();
        }
        else {
            moveBack();
        }
        // transform.position = startPos;
    }

    public void swipeLeft() {
        Debug.Log("SWIPE LEFT");
    }

    public void swipeRight() {
        Debug.Log("SWIPE RIGHT");
    }

    private void moveBack() {
        System.Action<ITween<Vector3>> updatePos = (t) =>
            {
                transform.position = t.CurrentValue;
            };

            Vector2 currentPos = transform.position;
            Vector2 endPos = startPos;
            // currentPos.z = endPos.z = 0.0f;

            // completion defaults to null if not passed in
            TweenFactory.Tween("MoveCircle", currentPos, endPos, 0.15f, TweenScaleFunctions.CubicEaseIn, updatePos);
    }
}
