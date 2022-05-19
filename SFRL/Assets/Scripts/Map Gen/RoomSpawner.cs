using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int _openingDirection;
    // 1 need bottom door
    // 2 need top door
    // 3 need left door
    // 4 need right door

    private RoomTemplates _templates;
    private int _rand;
    private bool _spawned = false;

    public float _waitTime = 4f;

    void Start()
    {
        Destroy(gameObject, _waitTime);
        _templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    void Spawn()
    {
        if (_spawned == false)
        {
            if (_openingDirection == 1)
            {
                // Need Bottom door
                _rand = Random.Range(0, _templates._bottomRooms.Length);
                Instantiate(_templates._bottomRooms[_rand], transform.position,
                    _templates._bottomRooms[_rand].transform.rotation);
            }
            else if (_openingDirection == 2)
            {
                // Need top door
                _rand = Random.Range(0, _templates._topRooms.Length);
                Instantiate(_templates._topRooms[_rand], transform.position,
                    _templates._topRooms[_rand].transform.rotation);
            }
            else if (_openingDirection == 3)
            {
                // Need left door
                _rand = Random.Range(0, _templates._leftRooms.Length);
                Instantiate(_templates._leftRooms[_rand], transform.position,
                     _templates._leftRooms[_rand].transform.rotation);
            }
            else if (_openingDirection == 4)
            {
                // Need right door
                _rand = Random.Range(0, _templates._rightRooms.Length);
                Instantiate(_templates._rightRooms[_rand], transform.position,
                     _templates._rightRooms[_rand].transform.rotation);
            }
            _spawned = true;
        }
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SpawnPoint"))
        {
            if(collision.GetComponent<RoomSpawner>()._spawned == false && _spawned == false)
            {
                // Spawn walls blocking off openings
                _templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
                Instantiate(_templates._closedRoom, transform.position, Quaternion.identity);
            }
            _spawned = true;
        }
    }
}
