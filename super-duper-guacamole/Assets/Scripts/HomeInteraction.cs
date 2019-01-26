using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeInteraction : MonoBehaviour
{
    public PersonRepository repo;

    public Image personImage;
    public Text text;


    public void changePerson() {
        Person person = repo.getPerson();
        if (person != null) {
            personImage.sprite = person.PersonImage;
            text.text = person.PersonName;
        }
    }
}
