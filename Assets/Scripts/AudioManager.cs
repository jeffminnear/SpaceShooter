using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _laser;
    [SerializeField]
    private AudioSource _explosion;
    [SerializeField]
    private AudioSource _powerUpPickedUp;

    public void Laser()
    {
        _laser.Play();
    }

    public void Explosion()
    {
        _explosion.Play();
    }

    public void PowerUpPickedUp()
    {
        _powerUpPickedUp.Play();
    }
}
