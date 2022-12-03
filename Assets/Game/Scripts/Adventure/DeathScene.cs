using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Allows for scene management
using UnityEngine.SceneManagement;

public class DeathScene : MonoBehaviour
{
    public void Restart()
    {
        // Play audio when button pressed
        GetComponent<AudioSource>().Play();
        // Locate and load scene called Level 1
        SceneManager.LoadScene("Level 1");
    }

    public void MainMenu()
    {
        // Play audio when button pressed
        GetComponent<AudioSource>().Play();
        // Locate and load scene called Splash Screen
        SceneManager.LoadScene("Splash Screen");
    }
}
