using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void NextScene() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);

    }

    public void QuitGame() {

        Debug.Log("QUIT!");
        Application.Quit();

    }
}
