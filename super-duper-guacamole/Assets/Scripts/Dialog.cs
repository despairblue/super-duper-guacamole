using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    public Dialogues npc;

    // Start is called before the first frame update
    void Start()
    {
        npc.SetTree("Tenant 1");
        Display();
    }

    public void Display()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
