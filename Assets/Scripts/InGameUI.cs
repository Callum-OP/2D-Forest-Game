using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// Allows for scene management
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    [Header("User Interface")]
    // Images used for rendering the five hearts in the UI
    public Image heart1;
    public Image heart2;
    public Image heart3;
    public Image heart4;
    public Image heart5;
    public Image heart6;
    public Image heart7;
    public Image heart8;
    public Image heart9;
    public Image heart10;
    // Sprites used to change the display of the hearts in the UI
    public Sprite fullHeart;
    public Sprite emptyHeart;

    // Image used beside arrow text
    public Image arrowImage;
    // Controls the displayed arrow text
    public TextMeshProUGUI arrowText;

    // Image used beside score text
    public Image scoreImage;
    // Sprites used to display image beside score in UI
    public Sprite scoreSprite;
    // Controls the displayed score text
    public TextMeshProUGUI scoreText;

    // Image used beside gems text
    public Image gemsImage;
    // Sprites used to display image beside gems in UI
    public Sprite gemsSprite;
    // Controls the displayed gems text
    public TextMeshProUGUI gemsText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Used to set the text in the UI to the correct amount of health player has
    public void SetHealthText()
	{
        // Change hearts in UI based on max health
        switch (Player.maxHealth) {
            // If max health is 1
            case 1:
                // Show first heart
                heart1.enabled = true;
                heart2.enabled = false;
                heart3.enabled = false;
                heart4.enabled = false;
                heart5.enabled = false;
                heart6.enabled = false;
                heart7.enabled = false;
                heart8.enabled = false;
                heart9.enabled = false;
                heart10.enabled = false;
                break;
            // If max health is 2
            case 2:
                // Show first 2 hearts
                heart1.enabled = true;
                heart2.enabled = true;
                heart3.enabled = false;
                heart4.enabled = false;
                heart5.enabled = false;
                heart6.enabled = false;
                heart7.enabled = false;
                heart8.enabled = false;
                heart9.enabled = false;
                heart10.enabled = false;
                break;
            // If max health is 3
            case 3:
                // Show first 3 hearts
                heart1.enabled = true;
                heart2.enabled = true;
                heart3.enabled = true;
                heart4.enabled = false;
                heart5.enabled = false;
                heart6.enabled = false;
                heart7.enabled = false;
                heart8.enabled = false;
                heart9.enabled = false;
                heart10.enabled = false;
                break;
            // If max health is 4
            case 4:
                // Show first 4 hearts
                heart1.enabled = true;
                heart2.enabled = true;
                heart3.enabled = true;
                heart4.enabled = true;
                heart5.enabled = false;
                heart6.enabled = false;
                heart7.enabled = false;
                heart8.enabled = false;
                heart9.enabled = false;
                heart10.enabled = false;
                break;
            // If max health is 5
            case 5:
                // Show first 5 hearts
                heart1.enabled = true;
                heart2.enabled = true;
                heart3.enabled = true;
                heart4.enabled = true;
                heart5.enabled = true;
                heart6.enabled = false;
                heart7.enabled = false;
                heart8.enabled = false;
                heart9.enabled = false;
                heart10.enabled = false;
                break;
            // If max health is 6
            case 6:
                // Show first 6 hearts
                heart1.enabled = true;
                heart2.enabled = true;
                heart3.enabled = true;
                heart4.enabled = true;
                heart5.enabled = true;
                heart6.enabled = true;
                heart7.enabled = false;
                heart8.enabled = false;
                heart9.enabled = false;
                heart10.enabled = false;
                break;
            // If max health is 7
            case 7:
                // Show first 7 hearts
                heart1.enabled = true;
                heart2.enabled = true;
                heart3.enabled = true;
                heart4.enabled = true;
                heart5.enabled = true;
                heart6.enabled = true;
                heart7.enabled = true;
                heart8.enabled = false;
                heart9.enabled = false;
                heart10.enabled = false;
                break;
            // If max health is 8
            case 8:
                // Show first 8 hearts
                heart1.enabled = true;
                heart2.enabled = true;
                heart3.enabled = true;
                heart4.enabled = true;
                heart5.enabled = true;
                heart6.enabled = true;
                heart7.enabled = true;
                heart8.enabled = true;
                heart9.enabled = false;
                heart10.enabled = false;
                break;
            // If max health is 9
            case 9:
                // Show first 9 hearts
                heart1.enabled = true;
                heart2.enabled = true;
                heart3.enabled = true;
                heart4.enabled = true;
                heart5.enabled = true;
                heart6.enabled = true;
                heart7.enabled = true;
                heart8.enabled = true;
                heart9.enabled = true;
                heart10.enabled = false;
                break;
            // If max health is 10
            case 10:
                // Show all 10 hearts
                heart1.enabled = true;
                heart2.enabled = true;
                heart3.enabled = true;
                heart4.enabled = true;
                heart5.enabled = true;
                heart6.enabled = true;
                heart7.enabled = true;
                heart8.enabled = true;
                heart9.enabled = true;
                heart10.enabled = true;
                break;
        }
        // Change hearts in UI based on current health
        switch (Player.health) {
            // If health is 0
            case 0:
                // Display current health
                heart1.sprite = emptyHeart;
                heart2.sprite = emptyHeart;
                heart3.sprite = emptyHeart;
                heart4.sprite = emptyHeart;
                heart5.sprite = emptyHeart;
                heart6.sprite = emptyHeart;
                heart7.sprite = emptyHeart;
                heart8.sprite = emptyHeart;
                heart9.sprite = emptyHeart;
                heart10.sprite = emptyHeart;
                // Locate and load scene called Death Scene
                SceneManager.LoadScene("Death");
                break;
            // If health is 1
            case 1:
                heart1.sprite = fullHeart;
                heart2.sprite = emptyHeart;
                heart3.sprite = emptyHeart;
                heart4.sprite = emptyHeart;
                heart5.sprite = emptyHeart;
                heart6.sprite = emptyHeart;
                heart7.sprite = emptyHeart;
                heart8.sprite = emptyHeart;
                heart9.sprite = emptyHeart;
                heart10.sprite = emptyHeart;
                break;
            // If health is 2
            case 2:
                heart1.sprite = fullHeart;
                heart2.sprite = fullHeart;
                heart3.sprite = emptyHeart;
                heart4.sprite = emptyHeart;
                heart5.sprite = emptyHeart;
                heart6.sprite = emptyHeart;
                heart7.sprite = emptyHeart;
                heart8.sprite = emptyHeart;
                heart9.sprite = emptyHeart;
                heart10.sprite = emptyHeart;
                break;
            // If health is 3
            case 3:
                heart1.sprite = fullHeart;
                heart2.sprite = fullHeart;
                heart3.sprite = fullHeart;
                heart4.sprite = emptyHeart;
                heart5.sprite = emptyHeart;
                heart6.sprite = emptyHeart;
                heart7.sprite = emptyHeart;
                heart8.sprite = emptyHeart;
                heart9.sprite = emptyHeart;
                heart10.sprite = emptyHeart;
                break;
            // If health is 4
            case 4:
                heart1.sprite = fullHeart;
                heart2.sprite = fullHeart;
                heart3.sprite = fullHeart;
                heart4.sprite = fullHeart;
                heart5.sprite = emptyHeart;
                heart6.sprite = emptyHeart;
                heart7.sprite = emptyHeart;
                heart8.sprite = emptyHeart;
                heart9.sprite = emptyHeart;
                heart10.sprite = emptyHeart;
                break;
            // If health is 5
            case 5:
                heart1.sprite = fullHeart;
                heart2.sprite = fullHeart;
                heart3.sprite = fullHeart;
                heart4.sprite = fullHeart;
                heart5.sprite = fullHeart;
                heart6.sprite = emptyHeart;
                heart7.sprite = emptyHeart;
                heart8.sprite = emptyHeart;
                heart9.sprite = emptyHeart;
                heart10.sprite = emptyHeart;
                break;
            // If health is 6
            case 6:
                heart1.sprite = fullHeart;
                heart2.sprite = fullHeart;
                heart3.sprite = fullHeart;
                heart4.sprite = fullHeart;
                heart5.sprite = fullHeart;
                heart6.sprite = fullHeart;
                heart7.sprite = emptyHeart;
                heart8.sprite = emptyHeart;
                heart9.sprite = emptyHeart;
                heart10.sprite = emptyHeart;
                break;
            // If health is 7
            case 7:
                heart1.sprite = fullHeart;
                heart2.sprite = fullHeart;
                heart3.sprite = fullHeart;
                heart4.sprite = fullHeart;
                heart5.sprite = fullHeart;
                heart6.sprite = fullHeart;
                heart7.sprite = fullHeart;
                heart8.sprite = emptyHeart;
                heart9.sprite = emptyHeart;
                heart10.sprite = emptyHeart;
                break;
            // If health is 8
            case 8:
                heart1.sprite = fullHeart;
                heart2.sprite = fullHeart;
                heart3.sprite = fullHeart;
                heart4.sprite = fullHeart;
                heart5.sprite = fullHeart;
                heart6.sprite = fullHeart;
                heart7.sprite = fullHeart;
                heart8.sprite = fullHeart;
                heart9.sprite = emptyHeart;
                heart10.sprite = emptyHeart;
                break;
            // If health is 9
            case 9:
                heart1.sprite = fullHeart;
                heart2.sprite = fullHeart;
                heart3.sprite = fullHeart;
                heart4.sprite = fullHeart;
                heart5.sprite = fullHeart;
                heart6.sprite = fullHeart;
                heart7.sprite = fullHeart;
                heart8.sprite = fullHeart;
                heart9.sprite = fullHeart;
                heart10.sprite = emptyHeart;
                break;
            // If health is 10
            case 10:
                heart1.sprite = fullHeart;
                heart2.sprite = fullHeart;
                heart3.sprite = fullHeart;
                heart4.sprite = fullHeart;
                heart5.sprite = fullHeart;
                heart6.sprite = fullHeart;
                heart7.sprite = fullHeart;
                heart8.sprite = fullHeart;
                heart9.sprite = fullHeart;
                heart10.sprite = fullHeart;
                break;
        }
	}
    // Used to set the text in the UI to the number of arrows user can fire before cooldown
    public void SetArrowsText() {
        // Change arrows text in UI based on current arrows
		arrowText.text = Player.arrows.ToString() + "/" + Player.maxArrows.ToString();
    }
    // Used to set the text in the UI to the correct number of gold collected
    public void SetScoreText()
	{
        // Display image for score
        scoreImage.sprite = scoreSprite;
        // Display current Scores.score
		scoreText.text = Scores.score.ToString();
	}
    // Used to set the text in the UI to the correct number of gems collected and needed
    public void SetGemsText()
    {
        // Display image for score
        gemsImage.sprite = gemsSprite;
        // Display current Scores.score
		gemsText.text = Scores.gems.ToString() + "/" + Scores.gemsToGet;
        
        // When gems is more than gems to get
		if (Scores.gameWon == true) 
		{
            // Locate and load scene called Level Scoreboard
            SceneManager.LoadScene("End");
		}
    }
}
