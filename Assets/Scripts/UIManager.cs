using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private FlashText _scoreText;
    [SerializeField]
    private FlashText _ammoText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _livesImg;
    private float _flickerTime = 0.5f;
    private float _textFlashOnDelay = 0.3f;
    private float _textFlashBetweenDelay = 0.2f;
    private int _currentScore = 0;
    private int _currentAmmo = 15;
    
    public static Color GreenText = new Color(0.04138483f, 0.8773585f, 0.114081f, 1f);
    public static Color YellowText = new Color(0.8080425f, 0.8784314f, 0.04313725f, 1f);
    public static Color RedText = new Color(0.8207547f, 0f, 0f, 1f);

    void Start()
    {
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        UpdateScore(0);
    }

    public void UpdateScore(int newScore)
    {
        if (_scoreText.flashable)
        {
            Color color;
            if (newScore > _currentScore)
            {
                color = GreenText;
            }
            else if (newScore < _currentScore)
            {
                color = RedText;
            }
            else
            {
                color = _scoreText.color;
            }

            StartCoroutine(FlashText(_scoreText, color, 1));
        }
        _currentScore = newScore;
        _scoreText.text = "Score: " + _currentScore;
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

    public void UpdateAmmo(int newAmmo)
    {
        if (_ammoText.flashable)
        {
            if (newAmmo > _currentAmmo)
            {
                StartCoroutine(FlashText(_ammoText, GreenText, 1));
            }
        }
        _currentAmmo = newAmmo;
        _ammoText.text = "Ammo: " + _currentAmmo;
    }

    public void ShowOutOfAmmo()
    {
        if (_ammoText.flashable)
        {
            StartCoroutine(FlashText(_ammoText, RedText));
        }
    }

    IEnumerator FlashText(FlashText text, Color flashColor, int count = 3)
    {
        text.flashable = false;

        Color originalColor = text.color;

        while (count > 0)
        {
            count--;
            text.color = flashColor;

            yield return new WaitForSeconds(_textFlashOnDelay);

            text.color = originalColor;

            yield return new WaitForSeconds(_textFlashBetweenDelay);
        }

        text.flashable = true;
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);

            yield return new WaitForSeconds(_flickerTime);

            _gameOverText.gameObject.SetActive(false);

            yield return new WaitForSeconds(_flickerTime);
        }
    }
}
