using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DigitalRuby.Tween;


public class Swipable : EventTrigger
{
    private int dragMargin = 500;
    private Vector2 startPos;
    private Vector2 dragStartPos;
    private HomeInteraction dependency;
    public Transform canvasTransform;

    void Start()
    {
        canvasTransform = transform.parent;
        startPos = transform.position;
    }

    public override void OnBeginDrag(PointerEventData data) {
        Debug.Log("Start Drag");
        dragStartPos = data.position;
    }

    public override void OnDrag(PointerEventData data)
    {
        Debug.Log(data.position - dragStartPos);
        transform.localPosition = data.position - dragStartPos;
    }

    public override void OnEndDrag(PointerEventData data)
    {
        if ((data.position - dragStartPos).x >= dragMargin) {
            swipeRight();
        }
        if ((data.position - dragStartPos).x <= -dragMargin) {
            swipeLeft();
        }
        else {
            moveBack();
        }
        transform.position = startPos;
    }

    public void swipeLeft() {
        dependency = GameObject.Find("Homescreen").GetComponent<HomeInteraction>();
        dependency.dislikeLogic();
    }

    public void swipeRight() {
        dependency = GameObject.Find("Homescreen").GetComponent<HomeInteraction>();
        dependency.likeLogic();
    }

    private void moveBack() {
        System.Action<ITween<Vector3>> updatePos = (t) =>
            {
                transform.position = t.CurrentValue;
            };

            Vector2 currentPos = transform.position;
            Vector2 endPos = startPos;

            // completion defaults to null if not passed in
            TweenFactory.Tween("MoveCircle", currentPos, endPos, 0.15f, TweenScaleFunctions.CubicEaseIn, updatePos);
    }
}
