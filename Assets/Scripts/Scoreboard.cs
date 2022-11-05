using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// Allows for scene management
using UnityEngine.SceneManagement;

public class Scoreboard : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        // Display current level
		levelText.text = "You Completed Level " + Scores.level.ToString();

        // Display time taken
		timeText.text = "Seconds Taken: " + Scores.time.ToString();

        // Display enemies defeated
		defeatsText.text = "Enemies Defeated: " + Defeats.defeats.ToString();

        // Display hearts used
		heartsText.text = "Hearts Used: " + Scores.hearts.ToString();

        // Display score
		scoreText.text = "Gold Collected: " + Scores.score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextLevel()
    {
        if (Scores.level == 1) {
            // Locate and load scene called Level 2
            SceneManager.LoadScene("Level 2");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
