using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{

    private RoomTemplates _templates;

    void Start()
    {
        _templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        _templates._rooms.Add(this.gameObject);
    }
}
