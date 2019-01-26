using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.Tween;
using TMPro;

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
    public Text Answer1;
    public Button answer1Button;
    public Text Answer2;
    public Button answer2Button;
    public Text Answer3;
    public Button answer3Button;

    public Image TopProfilePic;

    public RectTransform ScrollViewContent;
    public ScrollRect ScrollRect;

    public RectTransform HouseMessageBox;
    public RectTransform HumansMessageBox;

    private Button[] buttons;

    // Start is called before the first frame update
    void hideButtons() {
        answer1Button.gameObject.SetActive(false);
        answer2Button.gameObject.SetActive(false);
        answer3Button.gameObject.SetActive(false);
    }

    void Start()
    {
        string matchName = PlayerPrefs.GetString("name");

        Sprite profilePic = Resources.Load<Sprite>("Sprites/PB" + matchName);

        TopProfilePic.sprite = profilePic;
        TopProfilePic.GetComponentInChildren<TextMeshProUGUI>().text = matchName;
        // TODO!!!

        currentState = State.Start;


        npc.SetTree(matchName);

        answer1Button = Answer1.GetComponentInParent<Button>();
        answer2Button = Answer2.GetComponentInParent<Button>();
        answer3Button = Answer3.GetComponentInParent<Button>();

        buttons = new Button[] {answer1Button, answer2Button, answer3Button};
        for (int i=0; i<buttons.Length; i++) {
            int index = i;
            buttons[i].onClick.AddListener(() => chooseMessage(index));
        }

        DisplayChoices();
        moveInChoiceButtons();
    }

    public void reply(RectTransform newMessage, string message) {
        newMessage.SetParent(ScrollViewContent, false);
        newMessage.GetComponentInChildren<Text>().text = message;
        newMessage.localScale = new Vector3(1, 1, 1);
    }
    public void houseReply(string message) {
        RectTransform newMessage = (RectTransform) Instantiate(HouseMessageBox, new Vector2(0, 0), ScrollViewContent.rotation);
        reply(newMessage, message);
    }

    public void humanReply(string message) {
        RectTransform newMessage = (RectTransform) Instantiate(HumansMessageBox, new Vector2(0, 0), ScrollViewContent.rotation);
        reply(newMessage, message);
    }

    public void DisplayChoices()
    {
        currentState = State.Choices;
        humanReply(npc.GetCurrentDialogue());
        string[] choices = npc.GetChoices();
        
        for (int i=0; i<choices.Length; i++) {
            buttons[i].GetComponentInChildren<Text>().text = choices[i];
        }

        for (int i=choices.Length; i<buttons.Length; i++) {
            buttons[i].gameObject.SetActive(false);
        }
    }

    public void DisplayDecisions() {
        currentState = State.Decision;
        npc.Next();

        humanReply(npc.GetCurrentDialogue());

        string[] choices = npc.GetChoices();
        
        answer1Button.GetComponentInChildren<Text>().text = choices[0];
        answer2Button.GetComponentInChildren<Text>().text = choices[1];
        answer3Button.gameObject.SetActive(false);
    }

    public void End() {
        currentState = State.End;
        hideButtons();
    }

    public void waitForMessage() {
        
    }

    public void moveInMessage(RectTransform message) {
        System.Action<ITween<float>> move = (t) =>
            {
                message.Translate(new Vector2(t.CurrentValue, 0));
            };
        message.transform.position = new Vector2(-500, 0);

        TweenFactory.Tween("MoveInMessage", 0, 500, 0.4f, TweenScaleFunctions.QuinticEaseOut, move);
    }

    public void moveInChoiceButtons() {
        foreach(Button btn in new List<Button>{answer1Button, answer2Button, answer3Button}) {
            System.Action<ITween<Vector2>> updatePos = (t) =>
            {
                btn.transform.position = t.CurrentValue;
            };
            Vector2 currentPos = btn.transform.position;
            Vector2 endPos = new Vector2(0, currentPos.y);

            // completion defaults to null if not passed in
            TweenFactory.Tween("Move" + btn.gameObject.name, currentPos, endPos, 0.4f, TweenScaleFunctions.QuinticEaseOut, updatePos);
        }
    }


    private void chooseMessage(int i) {
        string[] choices = npc.GetChoices();

        if (i >= choices.Length) {
            Debug.LogError(i.ToString() + ". Choice is out of Bounds");
            return;
        }
        string choice = choices[i];
        houseReply(choice);

        string trigger = npc.GetTrigger();
        Debug.Log(trigger);
        if (trigger == "yes") {
            Debug.Log("YES");
        }
        else if (trigger == "no") {
            Debug.Log("NO");
        }
        else {
            npc.NextChoice(choice);
            humanReply(npc.GetCurrentDialogue());
        }

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
