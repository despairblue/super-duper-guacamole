using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Person : MonoBehaviour
{
    [SerializeField]
    public Sprite PersonImage;
    [SerializeField]
    public string PersonName;
    public string InfoText;
    //-341,74
    public void MarkObject() {
        DontDestroyOnLoad(gameObject);
    }
}
