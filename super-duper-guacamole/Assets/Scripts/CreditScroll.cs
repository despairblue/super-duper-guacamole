using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;

public class CreditScroll : MonoBehaviour
{
    // Start is called before the first frame update
    public int max = 4000;
    void Start()
    {
        System.Action<ITween<Vector2>> setPos = (t) =>
            {
                transform.localPosition = t.CurrentValue;
            };

        TweenFactory.Tween(
            "Scroll",
            transform.localPosition,
            transform.localPosition + new Vector3(0, max, 0),
            max/100,  // 100 units per second
            TweenScaleFunctions.Linear,
            setPos
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
