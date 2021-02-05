using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _minXPosition = -9.3f;
    private float _maxXPosition = 9.3f;
    private float _maxYPosition = 8f;
    private float _minYPosition = -5.57f;
    [SerializeField]
    private float _baseSpeed = 2.75f;
    public float _currentSpeed;
    private float _minSpeed = 2f;
    [SerializeField]
    private float _maxSpeed = 12f;
    private Player _player;
    [SerializeField]
    private int _pointValue = 10;
    private Animator _anim;
    private bool _dead = false;
    private AudioManager _audio;


    void Start()
    {
        _anim = gameObject.GetComponent<Animator>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _currentSpeed = _baseSpeed + (Time.timeSinceLevelLoad / 15);
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.Translate(Vector3.down * _currentSpeed * Time.deltaTime);

        if (!_dead && transform.position.y < _minYPosition)
        {
            Reset();
        }
    }

    void Reset()
    {
        float randomX = Random.Range(_minXPosition, _maxXPosition);
        transform.position = new Vector3(randomX, _maxYPosition, 0);

        if (_currentSpeed < _maxSpeed)
        {
            _currentSpeed = Mathf.Clamp(_currentSpeed * 1.1f, _minSpeed, _maxSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();
            }

            Death();
        }
        else if (other.transform.tag == "Laser")
        {
            if (_player != null)
            {
                _player.AddToScore(Mathf.RoundToInt(_pointValue * (_currentSpeed / 2)));
            }

            Destroy(other.gameObject);
            Death();
        }
    }

    private void Death()
    {
        _dead = true;
        Destroy(gameObject.GetComponent<BoxCollider2D>());
        _audio.Explosion();
        _anim.SetTrigger("OnEnemyDeath");
        Destroy(this.gameObject, 2.4f);
    }
}
