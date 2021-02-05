using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    [SerializeField]
    private float _range = 10f;

    private float _distanceTravelled = 0f;
    private Vector3 _lastPosition;

    void Start()
    {
        _lastPosition = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        _distanceTravelled += (transform.position - _lastPosition).magnitude;
        _lastPosition = transform.position;

        if (_distanceTravelled > _range)
        {
            if (transform.parent)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }
}
