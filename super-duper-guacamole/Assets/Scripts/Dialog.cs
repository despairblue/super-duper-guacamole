using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{

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
        ChatBoxHouse.text = lastHouseMessage;
        string[] choices = npc.GetChoices();

        answer1Button.gameObject.SetActive(true);
        answer2Button.gameObject.SetActive(true);
        answer3Button.gameObject.SetActive(true);
        
        switch (choices.Length) {
        case 3: 
            Answer1.text = choices[0];
            Answer2.text = choices[1];
            Answer3.text = choices[2];
            break;
        case 2:
            Answer1.text = choices[0];
            Answer2.text = choices[1];
            Answer3.gameObject.SetActive(false);
            break;
        default:
            answer1Button.gameObject.SetActive(false);
            answer2Button.gameObject.SetActive(false);
            answer3Button.gameObject.SetActive(false);
            break;
        }
        
    }

    private void chooseMessage(int i) {
        string[] choices = npc.GetChoices();
        Debug.Log(choices.Length);
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

        // answer1Button.gameObject.SetActive(false);
        // answer2Button.gameObject.SetActive(false);
        answer3Button.gameObject.SetActive(false);

        DisplayChoices();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
