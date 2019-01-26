using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public Dialogues npc;
    public Text ChatBoxHouse;
    public Text Answer1;
    public Text Answer2;
    public Text Answer3;

    // Start is called before the first frame update
    void Start()
    {
        npc.SetTree("Tenant 1");
        Display();
    }

    public void Display()
    {
        ChatBoxHouse.text = npc.GetCurrentDialogue();
        Answer1.text = npc.GetChoices()[0];
        Answer2.text = npc.GetChoices()[1];
        Answer3.text = npc.GetChoices()[2];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
