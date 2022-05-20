using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAimWeapon : MonoBehaviour
{
    Transform _aimTransform;
    public Transform _player;

    public SpriteRenderer _gun;
    public Camera _cam;

    void Awake()
    {
        _aimTransform = transform.Find("Aim");
    }

    void FixedUpdate()
    {
        if (_player != null)
        {
            HandleAim();
        }       
    }

    void HandleAim()
    {
        Vector3 _lookDirection = (_player.position - transform.position).normalized;

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
}
