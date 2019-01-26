using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.Tween;

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
    private Button answer1Button;
    public Text Answer2;
    private Button answer2Button;
    public Text Answer3;
    private Button answer3Button;

    public Image TopProfilePic;

    public RectTransform ScrollViewContent;
    public Scrollbar ScrollBar;

    public RectTransform HouseMessageBox;
    public RectTransform HumansMessageBox;

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
        // TopProfilePic.GetComponentInChildren<TextMeshProGUI>().text = matchName;
        // TODO!!!

        currentState = State.Start;


        npc.SetTree(matchName);

        answer1Button = Answer1.GetComponentInParent<Button>();
        answer2Button = Answer2.GetComponentInParent<Button>();
        answer3Button = Answer3.GetComponentInParent<Button>();

        answer1Button.onClick.AddListener(delegate {chooseMessage(0); });
        answer2Button.onClick.AddListener(delegate {chooseMessage(2); });
        answer3Button.onClick.AddListener(delegate {chooseMessage(1); });

        DisplayChoices();
        moveInChoiceButtons();
    }

    public void reply(RectTransform newMessage, string message) {
        newMessage.SetParent(ScrollViewContent, false);
        newMessage.GetComponentInChildren<Text>().text = message;
        newMessage.localScale = new Vector3(1, 1, 1);
        ScrollBar.value = 1;
        // moveInMessage(newMessage);
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
        
        Answer1.text = choices[0];
        Answer2.text = choices[2];
        Answer3.text = choices[1];
    }

    public void DisplayDecisions() {
        currentState = State.Decision;
        npc.Next();

        humanReply(npc.GetCurrentDialogue());

        string[] choices = npc.GetChoices();
        answer2Button.gameObject.SetActive(false);
        Answer1.text = choices[0];
        Answer3.text = choices[1];
    }

    public void End() {
        currentState = State.End;
        hideButtons();
    }

    public void waitForMessage() {
        
    }

    public void moveInMessage(RectTransform message) {
        System.Action<ITween<Vector2>> updatePos = (t) =>
            {
                message.position = t.CurrentValue;
            };
        Vector2 startPos = new Vector2(-500, message.transform.position.y);
        Vector2 endPos = message.transform.position;

        TweenFactory.Tween("MoveInMessage", startPos, endPos, 0.4f, TweenScaleFunctions.QuinticEaseOut, updatePos);
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
            Debug.LogError("Choice is out of Bounds");
            return;
        }
        string choice = choices[i];
        houseReply(choice);

        npc.NextChoice(choice);
        humanReply(npc.GetCurrentDialogue());

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
