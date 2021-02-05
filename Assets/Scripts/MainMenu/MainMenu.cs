using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Update()
    {
        if (isStartPressed())
        {
            LoadGame();
        }
    }

    bool isStartPressed()
    {
        return (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 7"));
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(1); // Main Game Scene
    }
}
