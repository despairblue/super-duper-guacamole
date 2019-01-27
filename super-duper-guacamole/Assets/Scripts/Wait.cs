using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : MonoBehaviour
{
    float phase = 0;
    float y = -10e10f;

    RectTransform[] dots;

    // Start is called before the first frame update
    void Start()
    {
        dots = GetComponentsInChildren<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        phase += Time.deltaTime * 8;
        foreach (RectTransform dot in dots) {
            dot.localPosition = new Vector2(
                dot.localPosition.x,
                Mathf.Sin(dot.localPosition.x/50 * Mathf.PI + phase) * 10 - 20
            );
        }
    }
}
