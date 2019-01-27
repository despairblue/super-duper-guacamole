using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DigitalRuby.Tween;

public class HomeInteraction : MonoBehaviour
{
    public PersonRepository repo;
    public Image personImage;
    public Text personText;
    public Text personOccupation;
    public Person House;

    public GameObject Like;
    public GameObject Dislike;

    public GameObject completeInfoBox;

    public Text infoText;
    public Button infoButton;
    public GameObject infoPanel;

    private bool HouseSelected = false;
    private Person currentPerson;
    

    public void Start()
    {
        loadNextPerson();       
    }

    public void displayHouse() {
        if (!HouseSelected)
        {
            setPerson(House);
            loadNormalView();
            displayInfo(House);
            disableLiking();
            HouseSelected = true;
        }
        else {
            setPerson(currentPerson);
            loadNormalView();
            enableLiking();
            HouseSelected = false;
        }
    }


    public void likePerson() {
        if (HouseSelected)
        {
            Like.GetComponentInChildren<AudioSource>().Play();
            loadEasterEgg();
        }
        else {
            Like.GetComponentInChildren<AudioSource>().Play();
            loadMessage();
        }
        
    }

    public void dislikePerson()
    {
        Dislike.GetComponentInChildren<AudioSource>().Play();
        loadNormalView();
        if (HouseSelected)
        {
            HouseSelected = false;
            setPerson(currentPerson);
        }
        else {
            Dislike.GetComponentInChildren<AudioSource>().Play();
            loadNextPerson();
        }
    }

    public void displayCurrentPersonInfo() {
        if (!HouseSelected)
        {
            displayInfo(currentPerson);
        }
        else {
            displayInfo(House);
        }
    }

    public void displayInfo(Person displayablePerson) {
        if (infoText.gameObject.activeSelf) {
            loadNormalView();
        }
        else { 
            personText.transform.position = new Vector3(personText.transform.position.x, 3.7f, personText.transform.position.z);
            infoText.gameObject.SetActive(true);
            infoText.text = displayablePerson.InfoText;
            infoButton.gameObject.SetActive(false);
            infoPanel.transform.localScale = new Vector3(infoPanel.transform.localScale.x, 1, infoPanel.transform.localScale.z);
            infoPanel.transform.localPosition = new Vector3(infoPanel.transform.localPosition.x, infoPanel.transform.localPosition.y - 50, infoPanel.transform.localPosition.z);
            personOccupation.gameObject.SetActive(false);
        }
        System.Action<ITween<float>> updateAlpha = (t) =>
        {
            completeInfoBox.GetComponent<CanvasGroup>().alpha = t.CurrentValue;
        };
        TweenFactory.Tween("Reply", 0, 1, 1f, TweenScaleFunctions.QuinticEaseOut, updateAlpha);
    }

    private void loadNormalView()
    {
        if (infoText.gameObject.activeSelf) {
            infoButton.gameObject.SetActive(true);
            infoText.gameObject.SetActive(false);
            personText.transform.localPosition = new Vector3(personText.transform.localPosition.x, 100, personText.transform.position.z);
            infoPanel.transform.localScale = new Vector3(infoPanel.transform.localScale.x, 0.5f, infoPanel.transform.localScale.z);
            infoPanel.transform.localPosition = new Vector3(infoPanel.transform.localPosition.x, infoPanel.transform.localPosition.y + 50, infoPanel.transform.localPosition.z);
            personOccupation.gameObject.SetActive(true);
        }
    }

    private void loadMessage() {
        PlayerPrefs.SetString("name", currentPerson.PersonName);
        SceneManager.LoadScene("Messaging");
    }

    private void loadEasterEgg()
    {
        PlayerPrefs.SetString("name", House.PersonName);
        SceneManager.LoadScene("Messaging");
    }

    private void loadNextPerson()
    {
        Person person = repo.getPerson();
        setPerson(person);
        currentPerson = person;
    }

    private void setPerson(Person person) {
        if (person != null)
        {
            personImage.sprite = person.PersonImage;
            personText.text = person.PersonName + ", " + person.age;
            personOccupation.text = "Occupation: " + person.Occupation;
        }
    }

    private void disableLiking() {
        Like.gameObject.SetActive(false);
        Dislike.gameObject.SetActive(false);
    }

    private void enableLiking()
    {
        Like.gameObject.SetActive(true);
        Dislike.gameObject.SetActive(true);
    }
}
