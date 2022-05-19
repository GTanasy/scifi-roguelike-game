using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] _bottomRooms;
    public GameObject[] _topRooms;
    public GameObject[] _leftRooms;
    public GameObject[] _rightRooms;

    public GameObject _closedRoom;

    public List<GameObject> _rooms;

    public float _waitTime;
    private bool _spawnedBoss;
    public GameObject _boss;

    void Update()
    {
        if (_waitTime <= 0 && _spawnedBoss == false)
        {
            for (int i = 0; i < _rooms.Count; i++)
            {
                if (i == _rooms.Count - 1)
                {
                    Instantiate(_boss, _rooms[i].transform.position, Quaternion.identity);
                    _spawnedBoss = true;
                }
            }
        }
        else
        {
            _waitTime -= Time.deltaTime;
        }
    }
}
