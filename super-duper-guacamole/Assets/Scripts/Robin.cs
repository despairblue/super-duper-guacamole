using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Robin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var music = GameObject.Find("Audio Source");
        if (music != null)
        {
            var audioSource = music.GetComponent<AudioSource>();
            audioSource.Stop();

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
