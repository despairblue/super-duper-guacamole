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
                StartCoroutine(DisplayChoices());
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
        
        System.Action<ITween<float>> updateAlpha = (t) =>
            {
                newMessage.GetComponent<CanvasGroup>().alpha = t.CurrentValue;
            };

        System.Action<ITween<float>> finishTween = (t) =>
            {
                if (currentState == State.Waiting) {
                    currentState = State.Human;
                    
                };
                continueDialog();
                scrollRect.verticalNormalizedPosition = 0;
            };
        
        currentState = State.Waiting;
        TweenFactory.Tween("Reply", 0, 1, 1f, TweenScaleFunctions.QuinticEaseOut, updateAlpha, finishTween);
        
    }
    public void houseReply(string message) {
        RectTransform newMessage = (RectTransform) Instantiate(HouseMessageBox, new Vector2(0, 0), ScrollViewContent.rotation);
        reply(newMessage, message);
    }

    public void humanReply(string message) {
        RectTransform newMessage = (RectTransform) Instantiate(HumansMessageBox, new Vector2(0, 0), ScrollViewContent.rotation);
        reply(newMessage, message);
    }

    public IEnumerator DisplayChoices()
    {
        currentState = State.Choice;
        yield return new WaitForSeconds(0.8f);

        string[] choices = npc.GetChoices();

        buttons = new List<Button> {};
        

        foreach (string choice in choices) {
            Button button = (Button) Instantiate(responseButton, new Vector2(0, 0), ScrollViewContent.rotation);
            button.transform.SetParent(ScrollViewContent, false);
            button.transform.localScale = new Vector3(1, 1, 1);
            button.onClick.AddListener(() => chooseMessage(choice));
            button.GetComponentInChildren<Text>().text = choice;
            buttons.Add(button);
            button.GetComponent<CanvasGroup>().alpha = 0;
            
            System.Action<ITween<float>> setAlpha = (t) =>
            {
                if (button != null) {
                    button.GetComponent<CanvasGroup>().alpha = t.CurrentValue;
                }
            };
            yield return new WaitForSeconds(0.1f);
            TweenFactory.Tween("FadeIn" + choice, 0, 1f, 1f, TweenScaleFunctions.QuinticEaseOut, setAlpha);
        }
    }

    public void End() {
        Debug.Log("FINISHED WITH: " + npc.GetTrigger());
        currentState = State.End;
        string trigger = npc.GetTrigger();
        switch (trigger) {
            case TRIGGER_WIN:
                StartCoroutine(loadScene("Winning Endscreen"));
                break;
            case TRIGGER_LOSE:
                StartCoroutine(loadScene("Losing Endscreen"));
                break;
            case TRIGGER_OUT:
                StartCoroutine(loadScene("Home"));
                break;
            default:
                Debug.LogError("Unknown trigger: " + trigger);
                break;
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
