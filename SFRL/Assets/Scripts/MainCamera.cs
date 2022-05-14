using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform _player;

    public float _smoothing;
    public Vector3 _offset;


    void FixedUpdate()
    {
        if (_player != null)
        {
            Vector3 _newPosition = Vector3.Lerp(transform.position, _player.transform.position + _offset, _smoothing);
            transform.position = _newPosition;
        }
    }
}
