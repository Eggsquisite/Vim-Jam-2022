using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] List<GameObject> escapeMenu = null;

    public static bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject child in escapeMenu)
        {
            child.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = true;
            Time.timeScale = 0f;
            foreach (GameObject child in escapeMenu)
            {
                child.SetActive(true);
            }
        }
        else if (isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = false;
            Time.timeScale = 1f;
            foreach (GameObject child in escapeMenu)
            {
                child.SetActive(false);
            }
        }
    }

    public void UnPause()
    {
        isPaused = false;
        Time.timeScale = 1f;
        foreach (GameObject child in escapeMenu)
        {
            child.SetActive(false);
        }
    }

    public void AboutScreen()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
