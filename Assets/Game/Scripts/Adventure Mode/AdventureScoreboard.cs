using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// Allows for scene management
using UnityEngine.SceneManagement;

public class AdventureScoreboard : MonoBehaviour
{
    // Controls the displayed time text
    public TextMeshProUGUI timeText;
    // Controls the displayed defeats text
    public TextMeshProUGUI defeatsText;
    // Controls the displayed hearts used text
    public TextMeshProUGUI heartsText;
    // The source of audio sounds
    new AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        // Audio equals the audio source on the item
        audio = GetComponent<AudioSource>();

        // Display time taken
		timeText.text = "Seconds Taken: " + Scores.time.ToString();

        // Display enemies defeated
		defeatsText.text = "Enemies Defeated: " + Scores.defeats.ToString();

        // Display hearts used
		heartsText.text = "Hearts Used: " + Scores.hearts.ToString();
    }

    public void MainMenu()
    {
        // Play audio when button pressed
        audio.Play();
        // Locate and load scene called Splash Screen
        SceneManager.LoadScene("Splash Screen");
    }

        public void QuitGame()
    {
        // Play audio when button pressed
        audio.Play();
        // Exit the game
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
