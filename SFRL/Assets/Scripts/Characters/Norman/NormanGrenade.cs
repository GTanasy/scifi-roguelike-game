using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormanGrenade : MonoBehaviour
{
    private Vector3 _targetPos;

    public float _speed = 10.0f;

    public GameObject _explosion;

    void Start()
    {
        _targetPos = GameObject.Find("GrenadeFirePoint").transform.position;
    }

    void FixedUpdate()
    {
        if (_speed > 0)
        {
            _speed -= .2f;
            transform.position = Vector2.MoveTowards(transform.position, _targetPos, _speed * Time.deltaTime);
        }
        else if (_speed < 0)
        {
            _speed = 0;
            StartCoroutine(Explode(1));
        }
    }

    IEnumerator Explode(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
        Instantiate(_explosion, transform.position, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy") StartCoroutine(Explode(0));
    }
}
