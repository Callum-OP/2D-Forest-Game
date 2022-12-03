using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// Allows for scene management
using UnityEngine.SceneManagement;

public class SurvivalScoreboard : MonoBehaviour
{
    // Controls the displayed time text
    public TextMeshProUGUI timeText;
    // Controls the displayed defeats text
    public TextMeshProUGUI defeatsText;
    // Controls the displayed score text
    public TextMeshProUGUI scoreText;
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
		timeText.text = "Survived: " + Scores.time.ToString() + " secs";

        // Display enemies defeated
		defeatsText.text = "Enemies Defeated: " + Scores.defeats.ToString();

        // Display score
		scoreText.text = "Gold Collected: " + Scores.score.ToString();

        // Display hearts used
		heartsText.text = "Hearts Used: " + Scores.hearts.ToString();

    }

    public void Restart()
    {
        // Play audio when button pressed
        audio.Play();
        // Locate and load scene called Survival Mode
        SceneManager.LoadScene("Survival Mode");
    }

    public void MainMenu()
    {
        // Play audio when button pressed
        audio.Play();
        // Locate and load scene called Splash Screen
        SceneManager.LoadScene("Splash Screen");
    }
}
