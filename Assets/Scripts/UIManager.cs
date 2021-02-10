using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    private bool _canFlashScoreText = true;
    [SerializeField]
    private Text _ammoText;
    private bool _canFlashAmmoText = true;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Slider _engineTempSlider;
    [SerializeField]
    private GameObject _engineTempSliderFill;
    private Image _engineTempSliderFillImage;
    private float _flickerTime = 0.5f;
    private float _textFlashOnDelay = 0.3f;
    private float _textFlashBetweenDelay = 0.2f;
    private int _currentScore = 0;
    private int _currentAmmo = 15;
    
    public static Color GreenText = new Color(0.04138483f, 0.8773585f, 0.114081f, 1f);
    public static Color YellowText = new Color(0.8080425f, 0.8784314f, 0.04313725f, 1f);
    public static Color RedText = new Color(0.8207547f, 0f, 0f, 1f);
    public static Color CoolEngine = new Color(0f, 0.1843137f, 1f, 1f);
    public static Color WarmEngine = new Color(1f, 0.4558313f, 0f, 1f);
    public static Color HotEngine = new Color(1f, 0.0630439f, 0f, 1f);

    void Awake()
    {
        _engineTempSliderFillImage = _engineTempSliderFill.GetComponent<Image>();
    }

    void Start()
    {
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        UpdateScore(0);
    }

    public void UpdateScore(int newScore)
    {
        if (_canFlashScoreText && newScore != _currentScore)
        {
            Color color;
            if (newScore > _currentScore)
            {
                color = GreenText;
            }
            else
            {
                color = RedText;
            }

            CanFlash handler = CanFlashScore;
            StartCoroutine(FlashText(_scoreText, handler, color));
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
        if (_canFlashAmmoText && newAmmo > _currentAmmo)
        {
            CanFlash handler = CanFlashAmmo;
            StartCoroutine(FlashText(_ammoText, handler, GreenText));
        }
        _currentAmmo = newAmmo;
        _ammoText.text = "Ammo: " + _currentAmmo;
    }

    public void UpdateEngineTemp(float newEngineTemp)
    {
        _engineTempSlider.value = newEngineTemp;
        if (newEngineTemp <= 50)
        {
            _engineTempSliderFillImage.color = Color.Lerp(CoolEngine, WarmEngine, newEngineTemp / 50);
        }
        else
        {
            _engineTempSliderFillImage.color = Color.Lerp(WarmEngine, HotEngine, (newEngineTemp - 50) / 50);
        }
    }

    delegate void CanFlash(bool val);
    void CanFlashAmmo(bool val)
    {
        _canFlashAmmoText = val;
    }

    void CanFlashScore(bool val)
    {
        _canFlashScoreText = val;
    }

    public void ShowOutOfAmmo()
    {
        if (_canFlashAmmoText)
        {
            CanFlash handler = CanFlashAmmo;
            StartCoroutine(FlashText(_ammoText, handler, RedText, 3));
        }
    }

    IEnumerator FlashText(Text text, CanFlash canFlash, Color flashColor, int count = 1)
    {
        canFlash(false);

        Color originalColor = text.color;

        while (count > 0)
        {
            count--;
            text.color = flashColor;

            yield return new WaitForSeconds(_textFlashOnDelay);

            text.color = originalColor;

            yield return new WaitForSeconds(_textFlashBetweenDelay);
        }

        canFlash(true);
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
