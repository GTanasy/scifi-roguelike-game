using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    Transform _aimTransform;

    Vector3 _mousePos;

    public SpriteRenderer _gun;
    public Camera _cam;

    void Awake()
    {
        _aimTransform = transform.Find("Aim");
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        HandleAim();
    }

    void HandleAim()
    {
        _mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 _lookDirection = (_mousePos - transform.position).normalized;

        // This gets the angle required to rotate the player using Atan2 and converts it into degrees
        float _angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg - 0f;
        _aimTransform.eulerAngles = new Vector3(0, 0, _angle);

        Vector3 _scale = Vector3.one;

        if (_angle < -90 || _angle > 90)
        {
            _scale.y = -1.0f;
        }
        else
        {
            _scale.y = +1.0f;
        }
        _aimTransform.localScale = _scale;
    }

    void FlipGun(float angle)
    {
        if (angle < -90 || angle > 90)
        {
            _gun.flipY = true;
        }
        else
        {
            _gun.flipY = false;
        }
    }
}
