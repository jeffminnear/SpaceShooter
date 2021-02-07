using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType {
        TripleShot,
        Speed,
        Shield,
        Ammo,
        Life
    }

    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private PowerUpType _powerUpType;
    private AudioManager _audio;

    void Start()
    {
        _audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8f)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                _audio.PowerUpPickedUp();

                player.GetPowerUp(_powerUpType);
                Destroy(this.gameObject);
            }
        }
    }
}
