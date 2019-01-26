using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public enum State
    {
        Start,
        Choices,
        Choose,
        Decision,
        End
    }
    public State currentState;
    public Dialogues npc;
    public Text ChatBoxHouse;
    private string lastHouseMessage;
    public Text Answer1;
    private Button answer1Button;
    public Text Answer2;
    private Button answer2Button;
    public Text Answer3;
    private Button answer3Button;

    public Text AnswerBox;

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Start;
        npc.SetTree("Tenant 1");

        answer1Button = Answer1.GetComponentInParent<Button>();
        answer2Button = Answer2.GetComponentInParent<Button>();
        answer3Button = Answer3.GetComponentInParent<Button>();

        answer1Button.onClick.AddListener(delegate {chooseMessage(0); });
        answer2Button.onClick.AddListener(delegate {chooseMessage(1); });
        answer3Button.onClick.AddListener(delegate {chooseMessage(2); });

        lastHouseMessage = npc.GetCurrentDialogue();

        DisplayChoices();
    }

    public void DisplayChoices()
    {
        currentState = State.Choices;
        ChatBoxHouse.text = lastHouseMessage;
        string[] choices = npc.GetChoices();
        
        Answer1.text = choices[0];
        Answer2.text = choices[1];
        Answer3.text = choices[2];
    }

    public void DisplayDecisions() {
        currentState = State.Decision;
        string dialogue = npc.GetCurrentDialogue();
        AnswerBox.text = dialogue;
        string[] choices = npc.GetChoices();
        Debug.Log(choices.Length);
        answer3Button.gameObject.SetActive(false);
        Answer1.text = choices[0];
        Answer2.text = choices[1];
    }

    public void End() {
        currentState = State.End;
        answer1Button.gameObject.SetActive(false);
        answer2Button.gameObject.SetActive(false);
        answer3Button.gameObject.SetActive(false);
    }

    private void chooseMessage(int i) {
        string[] choices = npc.GetChoices();

        if (i >= choices.Length) {
            Debug.LogError("Choice is out of Bounds");
            return;
        }
        string choice = choices[i];
        Debug.Log(choice);
        npc.NextChoice(choice);
        lastHouseMessage = choice;

        AnswerBox.text = npc.GetCurrentDialogue();
        npc.Next();

        switch (currentState) {
            case State.Choices: 
                DisplayDecisions();
                break;
            case State.Decision:
                End();
                break;
            default:
                break;
        }
    }
}
