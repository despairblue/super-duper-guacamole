using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeInteraction : MonoBehaviour
{
    public PersonRepository repo;

    public Image personImage;
    public Text text;

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
        loadNextPerson();
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
            text.text = person.PersonName;
        }
    }
}
