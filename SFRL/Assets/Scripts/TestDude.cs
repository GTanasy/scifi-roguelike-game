using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDude : MonoBehaviour
{

    float _horizontal;
    float _vertical;

    Vector2 _movement;
    Vector2 _mousePos;

    public Rigidbody2D _rb;

    public Camera _cam;

    public float _speed = 3.0f;    

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        _movement.x = _horizontal;
        _movement.y = _vertical;

        _mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement * _speed * Time.fixedDeltaTime);

        Vector2 _lookDirection = _mousePos - _rb.position;

        // This gets the angle required to rotate the player using Atan2 and converts it into degrees
        float _angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg - 0f;

        _rb.rotation = _angle;
    }
}
  

