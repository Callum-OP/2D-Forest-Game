using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// Allows for scene management
using UnityEngine.SceneManagement;

public class AdventureScoreboard : MonoBehaviour
{
    // Controls the displayed level text
    public TextMeshProUGUI levelText;
    // Controls the displayed time text
    public TextMeshProUGUI timeText;
    // Controls the displayed defeats text
    public TextMeshProUGUI defeatsText;
    // Controls the displayed hearts used text
    public TextMeshProUGUI heartsText;
    // Controls the displayed score text
    public TextMeshProUGUI scoreText;
    // The source of audio sounds
    new AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        // Audio equals the audio source on the item
        audio = GetComponent<AudioSource>();

        // Display current level
		levelText.text = "You Completed Level " + Scores.level.ToString();

        // Display time taken
		timeText.text = "Seconds Taken: " + Scores.time.ToString();

        // Display enemies defeated
		defeatsText.text = "Enemies Defeated: " + Scores.defeats.ToString();

        // Display hearts used
		heartsText.text = "Hearts Used: " + Scores.hearts.ToString();

        // Display score
		scoreText.text = "Gold Collected: " + Scores.score.ToString();
    }

    public void NextLevel()
    {
        if (Scores.level == 1) {
            // Play audio when button pressed
            audio.Play();
            // Locate and load scene called Level 2
            SceneManager.LoadScene("Level 2");
        }
    }

    public void Restart()
    {
        // Play audio when button pressed
        audio.Play();
        // Locate and load scene called Level 1
        SceneManager.LoadScene("Level 1");
    }

    public void MainMenu()
    {
        // Play audio when button pressed
        audio.Play();
        // Locate and load scene called Splash Screen
        SceneManager.LoadScene("Splash Screen");
    }
}
