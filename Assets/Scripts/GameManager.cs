using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    private bool gameWon;

    private void Awake()
    {
        if (_instance != null && _instance != this) {
            Destroy(this);
        }
        else {
            _instance = this;
        }
    }

    public void GameWon() {
        gameWon = true;
    }

    public bool GetGameWon() {
        return gameWon;
    }
}
