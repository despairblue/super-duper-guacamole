using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour
{
    public void Start() {
        StartCoroutine(AutoStart());
    }

    public IEnumerator AutoStart() {
        yield return new WaitForSeconds(30);
        startGame();
    }

    public void startGame() {
        SceneManager.LoadScene("Home");
    }
}
