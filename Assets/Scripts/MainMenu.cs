using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Allows for scene management
using UnityEngine.SceneManagement;

public class Settings
{
    // Controls difficulty level
    public static int difficulty;
}

public class MainMenu : MonoBehaviour
{
    public GameObject playMenu;
    public GameObject controls;
    public GameObject difficultyMenu;

    // Start is called before the first frame update
    void Start()
    {
        playMenu.SetActive(true);
        difficultyMenu.SetActive(false);
        controls.SetActive(false);
    }

    public void OpenControls() {
        playMenu.SetActive(false);
        controls.SetActive(true);
    }

    public void CloseControls() {
        playMenu.SetActive(true);
        controls.SetActive(false);
    }

    public void PlayGame()
    {
        playMenu.SetActive(false);
        difficultyMenu.SetActive(true);
    }

    public void EasyGame()
    {
        // Set difficulty to easy
        Settings.difficulty = 1;
        // Load the game
        SceneManager.LoadScene("World");
    }

    public void NormalGame()
    {
        // Set difficulty to normal
        Settings.difficulty = 2;
        // Load the game
        SceneManager.LoadScene("World");
    }

    public void HardGame()
    {
        // Set difficulty to hard
        Settings.difficulty = 3;
        // Load the game
        SceneManager.LoadScene("World");
    }

    public void QuitGame()
    {
        // Exit the game
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
