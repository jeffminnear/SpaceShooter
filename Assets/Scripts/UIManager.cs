using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _livesImg;
    private float _flickerTime = 0.5f;

    void Start()
    {
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        UpdateScore(0);
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int lives)
    {
        _livesImg.sprite = _liveSprites[lives];

        if(lives <= 0)
        {
            StartCoroutine("GameOverFlicker");
            _restartText.gameObject.SetActive(true);
        }
    }

    IEnumerator GameOverFlicker()
    {
        while(true)
        {
            _gameOverText.gameObject.SetActive(true);

            yield return new WaitForSeconds(_flickerTime);

            _gameOverText.gameObject.SetActive(false);

            yield return new WaitForSeconds(_flickerTime);
        }
    }
}
