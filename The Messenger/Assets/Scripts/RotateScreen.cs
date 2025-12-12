using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click()
    {
        Debug.Log("Rotate");
        Debug.Log(Screen.orientation);
        if (Screen.orientation == ScreenOrientation.LandscapeLeft)
            Screen.orientation = ScreenOrientation.LandscapeRight;
        else
            Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
}
