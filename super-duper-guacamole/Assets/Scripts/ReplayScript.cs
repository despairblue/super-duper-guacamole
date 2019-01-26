using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayScript : MonoBehaviour
{
    public void restart() {
        SceneManager.LoadScene("Home");
    }
}
