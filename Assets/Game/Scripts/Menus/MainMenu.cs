using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Allows for scene management
using UnityEngine.SceneManagement;

public class Settings
{
    // Controls difficulty level
    public static int difficulty;
    // Controls game mode
    public static string gameMode;
}

public class MainMenu : MonoBehaviour
{
    // The source of audio sounds
    new AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        // Don't destroy canvas so audio will still play
        DontDestroyOnLoad(this.gameObject);
        // Audio equals the audio source on the enemy
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        // Play audio when button pressed
        audio.Play();
        // Locate and load scene called Select Mode
        SceneManager.LoadScene("Select Mode");
        // Destroy canvas
        Destroy(this.gameObject,0.03f);
    }

    public void QuitGame()
    {
        // Play audio when button pressed
        audio.Play();
        // Exit the game
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void AdventureMode()
    {
        // Play audio when button pressed
        audio.Play();
        Settings.gameMode = "Adventure";
        // Locate and load scene called Select Difficulty
        SceneManager.LoadScene("Select Difficulty");
        // Destroy canvas
        Destroy(this.gameObject,0.03f);
    }

    public void SurvivalMode()
    {
        // Play audio when button pressed
        audio.Play();
        Settings.gameMode = "Survival";
        // Locate and load scene called Select Difficulty
        SceneManager.LoadScene("Select Difficulty");
        // Destroy canvas
        Destroy(this.gameObject,0.03f);
    }

    public void EasyGame()
    {
        // Stop splash screen music from playing
        Destroy(GameObject.FindGameObjectWithTag("SplashScreenMusic"));
        // Play audio when button pressed
        audio.Play();
        // Set difficulty to easy
        Settings.difficulty = 1;
        if(Settings.gameMode == "Adventure") {
            // Locate and load scene called Level 1
            SceneManager.LoadScene("Level 1");
        } else if(Settings.gameMode == "Survival") {
            // Locate and load scene called SurvivalMode
            SceneManager.LoadScene("Survival Mode");
        }
        // Destroy canvas
        Destroy(this.gameObject,0.03f);
    }

    public void NormalGame()
    {
        // Stop splash screen music from playing
        Destroy(GameObject.FindGameObjectWithTag("SplashScreenMusic"));
        // Play audio when button pressed
        audio.Play();
        // Set difficulty to normal
        Settings.difficulty = 2;
        if(Settings.gameMode == "Adventure") {
            // Locate and load scene called Level 1
            SceneManager.LoadScene("Level 1");
        } else if(Settings.gameMode == "Survival") {
            // Locate and load scene called SurvivalMode
            SceneManager.LoadScene("Survival Mode");
        }
        // Destroy canvas
        Destroy(this.gameObject,0.03f);
    }

    public void HardGame()
    {
        // Stop splash screen music from playing
        Destroy(GameObject.FindGameObjectWithTag("SplashScreenMusic"));
        // Play audio when button pressed
        audio.Play();
        // Set difficulty to hard
        Settings.difficulty = 3;
        if(Settings.gameMode == "Adventure") {
            // Locate and load scene called Level 1
            SceneManager.LoadScene("Level 1");
        } else if(Settings.gameMode == "Survival") {
            // Locate and load scene called SurvivalMode
            SceneManager.LoadScene("Survival Mode");
        }
        // Destroy canvas
        Destroy(this.gameObject,0.03f);
    }
}
