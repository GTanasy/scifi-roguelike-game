using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool QKey { get; private set; }
    public bool EKey { get; private set; }
    public bool RKey { get; private set; }
    public bool ShiftKey { get; private set; }
    public bool LeftClick { get; private set; }
    public bool RightClick { get; private set; }
    public bool RightClickHeld { get; private set; }
    public bool RightClickRelease { get; private set; }
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public Vector3 MousePosition { get; private set; }

    // Update is called once per frame
    void Update()
    {
        QKey = Input.GetKeyDown(KeyCode.Q);
        EKey = Input.GetKeyDown(KeyCode.E);
        RKey = Input.GetKeyDown(KeyCode.R);
        ShiftKey = Input.GetKeyDown(KeyCode.LeftShift);
        LeftClick = Input.GetButton("Fire1");
        RightClick = Input.GetButton("Fire2");
        RightClickHeld = Input.GetButtonDown("Fire2");
        RightClickRelease = Input.GetButtonUp("Fire2");
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        MousePosition = Input.mousePosition;
    }
}
