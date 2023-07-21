using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_function : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Time.timeScale == 0) Time.timeScale = 1;
    }

    public GameObject pauseUI;
    public GameObject gameUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0;
            pauseUI.SetActive(true);
            gameUI.SetActive(false);
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseUI.SetActive(false);
        gameUI.SetActive(true);
    }
}
