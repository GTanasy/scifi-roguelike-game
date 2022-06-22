using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormanGrenade : MonoBehaviour
{
    Vector3 _targetPos;

    public float speed;

    public GameObject explosion;

    void Start()
    {
        _targetPos = GameObject.Find("GrenadeFirePoint").transform.position;
    }

    void FixedUpdate()
    {
        if (speed > 0)
        {
            speed -= .2f;
            transform.position = Vector2.MoveTowards(transform.position, _targetPos, speed * Time.deltaTime);
        }
        else if (speed < 0)
        {
            speed = 0;
            StartCoroutine(Explode(1));
        }
    }

    IEnumerator Explode(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
        Instantiate(explosion, transform.position, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy") StartCoroutine(Explode(0));
    }
}
