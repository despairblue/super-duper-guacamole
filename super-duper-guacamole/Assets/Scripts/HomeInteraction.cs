using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeInteraction : MonoBehaviour
{
    public PersonRepository repo;
    public Image personImage;
    public Text personText;
    public Person House;

    public GameObject Like;
    public GameObject Dislike;

    public Text infoText;

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
            Debug.Log(currentPerson.PersonName);
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
        loadMessage();
    }

    public void dislikePerson()
    {
        loadNormalView();
        loadNextPerson();
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
        }
    }

    private void loadNormalView()
    {
        if (infoText.gameObject.activeSelf) {
            infoText.gameObject.SetActive(false);
            personText.transform.position = new Vector3(personText.transform.position.x, 3f, personText.transform.position.z);
        }
    }

    private void loadMessage() {
        PlayerPrefs.SetString("name", currentPerson.PersonName);
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
            personText.text = person.PersonName;
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
