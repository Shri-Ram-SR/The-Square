using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetJoystickValue : MonoBehaviour
{
    Joystick JS;
    public static GetJoystickValue Instance;
    private void Awake()
    {
        Instance = this;
        JS = GetComponent<Joystick>();
    }
    public int GetDirection()
    {
        if (JS.Direction.x > 0)
            return 1;
        else if (JS.Direction.x < 0)
            return -1;
        else
            return 0;
    }
}
