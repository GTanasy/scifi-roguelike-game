using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CG.SFRL.Characters;

public class PlayerMovement : MonoBehaviour
{
    public BasicCharacter _moveStats;

    float _horizontal;
    float _vertical;

    Vector2 _movement;

    Rigidbody2D _rb;
    public BoxCollider2D _hitBox;

    Image _imageCoolDownDash;
    TMP_Text _textCoolDownDash;

    Camera _cam;

    Animator _walkingAnimator;
    public AnimationClip rollAnim;

    float _currentMoveSpeed;
    [HideInInspector]
    public CharacterStat normalSpeed;
    [HideInInspector]
    public CharacterStat dashSpeed;
    [HideInInspector]
    public CharacterStat dashLength;
    [HideInInspector]
    public CharacterStat dashCoolDown;

    float _dashCool;
    float _dashCounter;

    private void Awake()
    {
        _cam = GameObject.Find("GameHandler/Main Camera").GetComponent<Camera>();
        _rb = GetComponent<Rigidbody2D>();

        _imageCoolDownDash = GameObject.Find("GameHandler/UI/Canvas/Roll/Button/Image").GetComponent<Image>();
        _textCoolDownDash = GameObject.Find("GameHandler/UI/Canvas/Roll/Button/Text").GetComponent<TMP_Text>();
    }

    void Start()
    {
        normalSpeed.BaseValue = _moveStats.normalSpeed;
        _currentMoveSpeed = normalSpeed.Value;

        _textCoolDownDash.gameObject.SetActive(false);
        _imageCoolDownDash.fillAmount = 0.0f;

        dashSpeed.BaseValue = _moveStats.dashSpeed;
        dashLength.BaseValue = _moveStats.dashLength;
        dashCoolDown.BaseValue = _moveStats.dashCooldown;

        _walkingAnimator = gameObject.GetComponent<Animator>();
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

        _walkingAnimator.SetFloat("Horizontal", _movement.x);
        _walkingAnimator.SetFloat("Vertical", _movement.y);
        _walkingAnimator.SetFloat("Speed", _movement.sqrMagnitude);
    }

    void Dodge()
    {
        if (_dashCool <= 0 && _dashCounter <= 0)
        {
            _walkingAnimator.speed = rollAnim.length / dashLength.Value;
            _walkingAnimator.SetBool("isRolling", true);
            _hitBox.enabled = !_hitBox.enabled;
            _currentMoveSpeed = dashSpeed.Value;
            _dashCounter = dashLength.Value;
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
                _currentMoveSpeed = normalSpeed.Value;
                _walkingAnimator.SetBool("isRolling", false);
                _walkingAnimator.speed = 1;
                _dashCool = dashCoolDown.Value;
                _textCoolDownDash.gameObject.SetActive(false);
                _imageCoolDownDash.fillAmount = 0.0f;
            }
        }

        if (_dashCool > 0)
        {
            _dashCool -= Time.deltaTime;
            _textCoolDownDash.text = Mathf.RoundToInt(_dashCool).ToString();
            _imageCoolDownDash.fillAmount = _dashCool / dashCoolDown.Value;
        }
    }

    public void AdjustSpeed()
    {
        _currentMoveSpeed = normalSpeed.Value;
    }
}
  

