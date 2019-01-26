using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonRepository : MonoBehaviour
{

    public Person[] persons;

    private int i = 0;
    

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
