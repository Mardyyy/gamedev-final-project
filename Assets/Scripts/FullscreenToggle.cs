using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FullscreenToggle : MonoBehaviour
{

    public void Fullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        Debug.Log("Fullscreen is " + isFullscreen);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
