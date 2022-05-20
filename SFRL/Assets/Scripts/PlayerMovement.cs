using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    float _horizontal;
    float _vertical;

    Vector2 _movement;

    public Rigidbody2D _rb;

    public Camera _cam;

    public Animator _animator;

    public float _speed = 3.0f;    

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement * _speed * Time.fixedDeltaTime);       
    }

    void HandleMovement()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        _movement.x = _horizontal;
        _movement.y = _vertical;

        _animator.SetFloat("Horizontal", _movement.x);
        _animator.SetFloat("Vertical", _movement.y);
        _animator.SetFloat("Speed", _movement.sqrMagnitude);
    }
}
  

