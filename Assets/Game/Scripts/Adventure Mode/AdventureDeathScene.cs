using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// Allows for scene management
using UnityEngine.SceneManagement;

public class AdventureDeathScene : MonoBehaviour
{
        // Controls the displayed time text
        public TextMeshProUGUI timeText;
        // Controls the displayed defeats text
        public TextMeshProUGUI defeatsText;
        // Controls the displayed hearts used text
        public TextMeshProUGUI heartsText;
        // Controls the displayed score text
        public TextMeshProUGUI scoreText;

    public void Restart()
    {
        // Play audio when button pressed
        GetComponent<AudioSource>().Play();
        // Locate and load scene called World
        SceneManager.LoadScene("World");
    }

    public void MainMenu()
    {
        // Play audio when button pressed
        GetComponent<AudioSource>().Play();
        // Locate and load scene called Splash Screen
        SceneManager.LoadScene("Splash Screen");
    }

    // Start is called before the first frame update
    void Start()
    {
        // Display time taken
		timeText.text = "Seconds Taken: " + Scores.time.ToString();

        // Display enemies defeated
		defeatsText.text = "Enemies Defeated: " + Scores.defeats.ToString();

        // Display hearts used
		heartsText.text = "Potions Used: " + Scores.hearts.ToString();

        // Display score
		scoreText.text = "Gems Collected: " + Scores.gems.ToString();
    }
}
