using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.Tween;
using TMPro;

public class Dialog : MonoBehaviour
{

    public const string TRIGGER_OUT = "out";
    public const string TRIGGER_IN = "in";

    public enum State
    {
        Human,
        Choice,
        End
    }
    public State currentState;
    public Dialogues npc;

    public Image TopProfilePic;

    public RectTransform ScrollViewContent;

    public RectTransform HouseMessageBox;
    public RectTransform HumansMessageBox;

    public Button responseButton;
    private List<Button> buttons;

    // Start is called before the first frame update
    void deleteButtons() {
        foreach(Button btn in buttons) {
            Debug.Log(btn);
            Destroy(btn.gameObject);
        }

        buttons.Clear();
    }

    public void startDialog() {
        currentState = State.Human;
        continueDialog();
    }

    public void Update() {
        // continueDialog();
    }

    void continueDialog() {
        if (currentState == State.Human) {
            humanReply(npc.GetCurrentDialogue());
            int next = npc.Next();
            if (next == 0) {
                continueDialog();
            }
            else if (next == -1) {
                End();
            }
            else {
                DisplayChoices();
            }
        }
        
    }

    void Start()
    {
        string matchName = PlayerPrefs.GetString("name");

        Sprite profilePic = Resources.Load<Sprite>("Sprites/PB" + matchName);

        TopProfilePic.sprite = profilePic;
        TopProfilePic.GetComponentInChildren<TextMeshProUGUI>().text = matchName;

        npc.SetTree(matchName);
        npc.SetTree("Jerry");

        startDialog();
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
        currentState = State.Choice;

        string[] choices = npc.GetChoices();

        buttons = new List<Button> {};

        foreach (string choice in choices) {
            Button button = (Button) Instantiate(responseButton, new Vector2(0, 0), ScrollViewContent.rotation);
            button.transform.SetParent(ScrollViewContent, false);
            button.transform.localScale = new Vector3(1, 1, 1);
            button.onClick.AddListener(() => chooseMessage(choice));
            button.GetComponentInChildren<Text>().text = choice;
            buttons.Add(button);
        }
    }

    public void End() {
        currentState = State.End;
    }

    public void moveInChoiceButtons() {
        foreach(Button btn in buttons) {
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


    private void chooseMessage(string choice) {
        houseReply(choice);
        npc.NextChoice(choice);

        deleteButtons();

        currentState = State.Human;

        continueDialog();
    }
}
