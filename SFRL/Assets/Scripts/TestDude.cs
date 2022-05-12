using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDude : MonoBehaviour
{

    float _horizontal;
    float _vertical;

    Vector2 _position;

    public float _speed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        _position = transform.position;

        _position.x = _position.x + _speed * _horizontal * Time.deltaTime;
        _position.y = _position.y + _speed * _vertical * Time.deltaTime;
        transform.position = _position;
    }
}
  

