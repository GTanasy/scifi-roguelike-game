using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{

    float _horizontal;
    float _vertical;

    Vector2 _movement;

    public Rigidbody2D _rb;
    public BoxCollider2D _hitBox;

    [SerializeField] private Image _imageCoolDownDash;
    [SerializeField] private TMP_Text _textCoolDownDash;

    public Camera _cam;

    public Animator _animator;
    public AnimationClip _rollAnim;

    float _currentMoveSpeed;

    public float _normalSpeed = 3.0f;

    public float _dashSpeed = 9.0f;
    public float _dashLength = 0.5f;
    public float _dashCoolDown = 1.0f;

    float _dashCool;
    float _dashCounter;

    void Start()
    {
        _currentMoveSpeed = _normalSpeed;

        _textCoolDownDash.gameObject.SetActive(false);
        _imageCoolDownDash.fillAmount = 0.0f;
    }
    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dodge();
        }
        DashCoolDown();
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement * _currentMoveSpeed * Time.fixedDeltaTime);       
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

    void Dodge()
    {
        if (_dashCool <= 0 && _dashCounter <= 0)
        {
            _animator.speed = _rollAnim.length / _dashLength;
            _animator.SetBool("isRolling", true);
            _hitBox.enabled = !_hitBox.enabled;
            _currentMoveSpeed = _dashSpeed;
            _dashCounter = _dashLength;
        }
    }

    void DashCoolDown()
    {
        if (_dashCounter > 0)
        {
            _dashCounter -= Time.deltaTime;           

            if (_dashCounter <= 0)
            {
                _hitBox.enabled = !_hitBox.enabled;
                _currentMoveSpeed = _normalSpeed;
                _animator.SetBool("isRolling", false);
                _animator.speed = 1;
                _dashCool = _dashCoolDown;
                _textCoolDownDash.gameObject.SetActive(false);
                _imageCoolDownDash.fillAmount = 0.0f;
            }
        }

        if (_dashCool > 0)
        {
            _dashCool -= Time.deltaTime;
            _textCoolDownDash.text = Mathf.RoundToInt(_dashCool).ToString();
            _imageCoolDownDash.fillAmount = _dashCool / _dashCoolDown;
        }
    }
}
  

