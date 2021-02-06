using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject _laser;
    [SerializeField]
    private GameObject _tripleShot;
    [SerializeField]
    private GameObject _playerShield;
    [SerializeField]
    private GameObject _leftWingDamage;
    [SerializeField]
    private GameObject _rightWingDamage;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private GameObject _thruster;
    private int _bonusShieldScore = 100;
    [SerializeField]
    private float _baseSpeed = 5.5f;
    private float _currentSpeed;
    [SerializeField]
    private float _speedBoost = 5f;
    private float _minYPosition = -3.5f;
    private float _maxYPosition = 5.5f;
    private float _minXPosition = -11.5f;
    private float _maxXPosition = 11.5f;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFireTime = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private GameObject _currentWeapon;
    [SerializeField]
    private float _powerUpDuration = 5f;
    [SerializeField]
    private bool _startsWithShield = false;
    private int _shieldStrength;
    private int _maxShieldStrength = 3;
    [SerializeField]
    private Color[] _shieldColors;
    [SerializeField]
    private int _score = 0;
    private UIManager _uiManager;
    private GameManager _gameManager;
    private AudioManager _audio;
    private BoxCollider2D _collider;
    

    void Start()
    {
        transform.position = new Vector3(0, -2.5f, 0);
        InitializePlayer();
        InitializeComponents();
    }

    void Update()
    {
        Move();

        if (isFirePressed() && Time.time > _canFireTime)
        {
            Fire();
        }
    }

    void InitializePlayer()
    {
        _currentWeapon = _laser;
        _currentSpeed = _baseSpeed;
        _shieldStrength = _startsWithShield ? _maxShieldStrength : 0;
        _playerShield.SetActive(_startsWithShield);
        SetShieldColorByStrength(_maxShieldStrength);
        _leftWingDamage.SetActive(false);
        _rightWingDamage.SetActive(false);
        _thruster.SetActive(true);
    }

    void InitializeComponents()
    {
        _collider = gameObject.GetComponent<BoxCollider2D>();
        _collider.enabled = true;
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _uiManager.UpdateLives(_lives);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void VerifyComponent(object component)
    {
        if (component == null)
        {
            Debug.LogError("The " + component.ToString() + " is NULL");
        }
    }

    bool isFirePressed()
    {
        return (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 2"));
    }

    bool isBoostPressed()
    {
        return (Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("Boost") > 0);
    }

    void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        float boost = isBoostPressed() ? _speedBoost : 0f;

        transform.Translate(direction * (_currentSpeed + boost) * Time.deltaTime);

        ConstrainPosition();
    }

    void ConstrainPosition()
    {
        float _y = Mathf.Clamp(transform.position.y, _minYPosition, _maxYPosition);
        float _x = transform.position.x;
        float _z = transform.position.z;

        if (_x < _minXPosition)
        {
            _x = _maxXPosition;
        }
        else if (_x > _maxXPosition)
        {
            _x = _minXPosition;
        }

        transform.position = new Vector3(_x, _y, _z);
    }

    void Fire()
    {
        Vector3 weaponOffset = _currentWeapon == _laser ?  new Vector3(0, 1.02f, 0) : Vector3.zero;
        Instantiate(_currentWeapon, transform.position + weaponOffset, Quaternion.identity);
        _canFireTime = Time.time + _fireRate;
        _audio.Laser();
    }

    public void Damage()
    {
        if (_shieldStrength > 0)
        {
            DamageShield();
            return;
        }

        _lives--;
        _uiManager.UpdateLives(_lives);

        switch(_lives)
        {
            case 2:
                _leftWingDamage.SetActive(true);
                break;
            case 1:
                _rightWingDamage.SetActive(true);
                break;
        }

        if (_lives <= 0)
        {
            Death();
        }
    }

    void SetShieldColorByStrength(int strength)
    {
        _playerShield.GetComponent<SpriteRenderer>().color = _shieldColors[Mathf.Clamp(strength - 1, 0, _shieldColors.Length - 1)];
    }

    void DamageShield()
    {
        _shieldStrength--;

        if (_shieldStrength <= 0)
        {
            _playerShield.SetActive(false);
        }
        else {
            SetShieldColorByStrength(_shieldStrength);
        }
    }

    void Death()
    {
        _collider.enabled = false;
        GameObject explosion = Instantiate(_explosion, transform.position, Quaternion.identity);
        explosion.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        StartCoroutine(Disappear());

        _spawnManager.StopSpawning();
        _gameManager.EndGame();

        Destroy(explosion, 3f);
        Destroy(this.gameObject, 8f);
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(0.02f);

        _thruster.SetActive(false);
        _leftWingDamage.SetActive(false);
        _rightWingDamage.SetActive(false);
        gameObject.GetComponent<Renderer>().enabled = false;
    }

    public void GetPowerUp(PowerUp.PowerUpType type)
    {
        switch (type)
        {
            case PowerUp.PowerUpType.TripleShot:
                _currentWeapon = _tripleShot;
                StartCoroutine(ResetWeapon());
                break;
            case PowerUp.PowerUpType.Speed:
                _currentSpeed = _baseSpeed * 1.5f;
                StartCoroutine(ResetSpeed());
                break;
            case PowerUp.PowerUpType.Shield:
                if (_shieldStrength == _maxShieldStrength)
                {
                    AddToScore(_bonusShieldScore);
                }
                else
                {
                    _shieldStrength = _maxShieldStrength;
                    SetShieldColorByStrength(_shieldStrength);
                    _playerShield.SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    public void AddToScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public int Score()
    {
        return _score;
    }

    public int Lives()
    {
        return _lives;
    }

    IEnumerator ResetWeapon()
    {
        yield return new WaitForSeconds(_powerUpDuration);

        _currentWeapon = _laser;
    }

    IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(_powerUpDuration);

        _currentSpeed = _baseSpeed;
    }
}
