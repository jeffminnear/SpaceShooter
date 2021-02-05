using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver;

    void Start()
    {
        _isGameOver = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }

        if (_isGameOver)
        {
            if (isRestartPressed())
            {
                ResetGame();
            }
        }
    }

    bool isRestartPressed()
    {
        return (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown("joystick button 7"));
    }

    void ResetGame()
    {
        SceneManager.LoadScene(1); // Current Game scene
    }

    void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void EndGame()
    {
        _isGameOver = true;
    }
}
