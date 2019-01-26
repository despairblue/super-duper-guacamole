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

    public Text infoText;

    private Person currentPerson;

    public void Start()
    {
        loadNextPerson();       
    }

    public void likePerson() {
        loadMessage();
    }

    public void dislikePerson()
    {
        loadNormalView();
        loadNextPerson();
    }

    public void displayInfo() {
        if (infoText.gameObject.activeSelf) {
            loadNormalView();
        }
        else { 
            personText.transform.position = new Vector3(personText.transform.position.x, -1.9f, personText.transform.position.z);
            infoText.gameObject.SetActive(true);
            infoText.text = currentPerson.InfoText;
        }
    }

    private void loadNormalView()
    {
        if (infoText.gameObject.activeSelf) {
            infoText.gameObject.SetActive(false);
            personText.transform.position = new Vector3(personText.transform.position.x, -2.7f, personText.transform.position.z);
        }
    }

    private void loadMessage() {
        PlayerPrefs.SetString("name", currentPerson.PersonName);
        SceneManager.LoadScene("Messaging");
    }

    private void loadNextPerson()
    {
        Person person = repo.getPerson();
        if (person != null)
        {
            currentPerson = person;
            personImage.sprite = person.PersonImage;
            personText.text = person.PersonName;
        }
    }
}
