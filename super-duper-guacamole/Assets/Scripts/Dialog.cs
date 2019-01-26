using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.Tween;
using TMPro;
using UnityEngine.SceneManagement;


public class Dialog : MonoBehaviour
{

    public const string TRIGGER_OUT = "out";
    public const string TRIGGER_WIN = "win";
    public const string TRIGGER_LOSE = "lose";

    public enum State
    {
        Human,
        Waiting,
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
    public ScrollRect scrollRect;

    public Dialogues carl;

    bool chosen = true;

    // Start is called before the first frame update
    void deleteButtons() {
        foreach(Button btn in buttons) {
            Destroy(btn.gameObject);
        }

        buttons.Clear();
    }

    public void startDialog() {
        currentState = State.Human;
        continueDialog();
    }

    void continueDialog() {
        if (currentState == State.Human && chosen) {
            humanReply(npc.GetCurrentDialogue());
            int next = npc.Next();
            if (next > 0) {
                chosen = false;
                DisplayChoices();
            }
            else if (next == -1) {
                End();
            }
        }
        
    }

    void Start()
    {
        string matchName = PlayerPrefs.GetString("name");
        Debug.Log(matchName);

        Sprite profilePic = Resources.Load<Sprite>("Sprites/PB" + matchName);

        TopProfilePic.sprite = profilePic;
        TopProfilePic.GetComponentInChildren<TextMeshProUGUI>().text = matchName;

        npc = GameObject.Find(matchName).GetComponent<Dialogues>();
        
        startDialog();
    }

    public void reply(RectTransform newMessage, string message) {
        newMessage.SetParent(ScrollViewContent, false);
        newMessage.GetComponentInChildren<Text>().text = message;
        newMessage.localScale = new Vector3(1, 1, 1);
        
        System.Action<ITween<float>> updatePos = (t) =>
            {
                newMessage.GetComponent<CanvasGroup>().alpha = t.CurrentValue;
            };

        System.Action<ITween<float>> finishTween = (t) =>
            {
                if (currentState == State.Waiting) {
                    currentState = State.Human;
                    
                };
                continueDialog();
            };
        
        currentState = State.Waiting;
        TweenFactory.Tween("Reply", 0, 1, Mathf.Clamp(Mathf.Sqrt(message.Length), 0.1f, 1.5f), TweenScaleFunctions.QuinticEaseOut, updatePos, finishTween);
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

        HashSet<string> usedChoices = new HashSet<string>();

        foreach (string choice in choices) {
            if (!usedChoices.Contains(choice)) {
                usedChoices.Add(choice);
                Button button = (Button) Instantiate(responseButton, new Vector2(0, 0), ScrollViewContent.rotation);
                button.transform.SetParent(ScrollViewContent, false);
                button.transform.localScale = new Vector3(1, 1, 1);
                button.onClick.AddListener(() => chooseMessage(choice));
                button.GetComponentInChildren<Text>().text = choice;
                
                System.Action<ITween<float>> setAlpha = (t) =>
                {
                    button.GetComponent<CanvasGroup>().alpha = t.CurrentValue;
                };
                System.Action<ITween<float>> finish = (t) =>
                {
                    button.interactable = true;
                };
                button.interactable = false;
                TweenFactory.Tween("FadeIn" + choice, 0f, 1f, 0.7f, TweenScaleFunctions.QuinticEaseOut, setAlpha, finish);

                buttons.Add(button);
            }
        }
    }

    public void End() {
        Debug.Log("FINISHED WITH: " + npc.GetTrigger());
        currentState = State.End;
        if (npc.GetTrigger() == TRIGGER_WIN) {
            StartCoroutine(loadScene("Winning Endscreen"));
        } 
        else {
            StartCoroutine(loadScene("Home"));
        }
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

    private IEnumerator loadScene(string scene) {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(scene);
    }

    private void chooseMessage(string choice) {
        houseReply(choice);
        npc.NextChoice(choice);

        deleteButtons();

        currentState = State.Human;

        chosen = true;
    }
}
