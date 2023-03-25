using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Allows for scene management
using UnityEngine.SceneManagement;
// Allows for UI
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject controls;

    public static bool menuOpen = false;

    void Start() {
        menuOpen = false;
        menu.SetActive(false);
        controls.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (menuOpen == true) {
                CloseMenu();
            } else {
                OpenMenu();
            }
        }
    }

    public void OpenMenu() {
        Time.timeScale = 0f;
        menuOpen = true;
        menu.SetActive(true);
    }

    public void CloseMenu() {
        Time.timeScale = 1f;
        menuOpen = false;
        menu.SetActive(false);
    }

    public void OpenControls() {
        menu.SetActive(false);
        controls.SetActive(true);
    }

    public void CloseControls() {
        menu.SetActive(true);
        controls.SetActive(false);
    }

    public void RestartGame() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        // Load the title scene
        SceneManager.LoadScene("Title");
    }
}

