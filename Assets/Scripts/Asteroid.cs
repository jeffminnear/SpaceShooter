using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private float _rotationSpeed = 0.05f;
    [SerializeField]
    private GameObject _explosion;
    private bool _dead = false;
    private SpawnManager _spawnManager;

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.back * _rotationSpeed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_dead)
        {
            return;
        }

        if (other.transform.tag == "Laser")
        {
            Destroy(other.gameObject);
            Death();
        }
        else if (other.transform.tag == "Beam")
        {
            Death();
        }
    }

    void Death()
    {
        _spawnManager.StartSpawning();
        
        _dead = true;
        
        GameObject explosion = Instantiate(_explosion, transform.position, Quaternion.identity);

        Destroy(explosion, 2.8f);
        Destroy(this.gameObject, 0.2f);
    }
}
