using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.SFRL.Characters;

public class Bullet : MonoBehaviour
{


    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
