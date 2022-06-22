using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform player;

    public float smoothing;
    public Vector3 offset;  

    void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 _newPosition = Vector3.Lerp(transform.position, player.transform.position + offset, smoothing);
            _newPosition.z = -10;
            transform.position = _newPosition;          
        }
    }
}
