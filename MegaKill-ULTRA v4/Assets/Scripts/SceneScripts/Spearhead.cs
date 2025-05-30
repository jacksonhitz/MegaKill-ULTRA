using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spearhead : ScenesManager
{
    void Awake()
    {
        if (SceneManager.GetActiveScene().name != "SPEARHEAD")
        {
            SoundManager.Instance?.Play("DJ");
            StartCoroutine(StateManager.LoadState(StateManager.GameState.SPEARHEAD, 0f));
        }
        else StateManager.LoadSilent(StateManager.GameState.SPEARHEAD);
    }
}
