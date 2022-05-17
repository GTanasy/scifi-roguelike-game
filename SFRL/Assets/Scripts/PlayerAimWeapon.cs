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
        _mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        Vector3 _lookDirection = (_mousePos - transform.position).normalized;

        // This gets the angle required to rotate the player using Atan2 and converts it into degrees
        float _angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg - 0f;
        _aimTransform.eulerAngles = new Vector3(0, 0, _angle);
            
        if (_angle < -90 || _angle > 90)
        {
            _gun.flipY = true;
        }
        else
        {
            _gun.flipY = false;            
        }
    }
}