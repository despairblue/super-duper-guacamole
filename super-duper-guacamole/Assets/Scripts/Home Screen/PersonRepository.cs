using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonRepository : MonoBehaviour
{

    public Person[] persons;

    private int i = 0;

    public void shuffle(Person[] list) {
        int n = list.Length;
        while (n > 1) {
            n--;
            int k = Random.Range(0, n);
            Person value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void Start() {
        shuffle(persons);
    }
    

    public Person getPerson() {
        if (i < persons.Length) {
            Person result = persons[i];
            i++;
            return result;
        }
        if (persons.Length <= 0)
        {
            return null;
        }
        else {
            Person result = persons[0];
            i = 1;
            return result;
        }
       
    }
}
