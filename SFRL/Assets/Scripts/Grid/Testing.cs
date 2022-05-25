using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public Camera _cam;
    Vector3 _mousePos;

    private Grid _grid;

    // Start is called before the first frame update
    void Start()
    {
        _grid = new Grid(4, 2, 10f, new Vector3(20, 0));        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
            _mousePos.z = 0;
            
            _grid.SetValue(_mousePos, 69);
            
        }

        if (Input.GetMouseButtonDown(1))
        {
            _mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
            _mousePos.z = 0;
            Debug.Log(_grid.GetValue(_mousePos));
        }
    }
}
