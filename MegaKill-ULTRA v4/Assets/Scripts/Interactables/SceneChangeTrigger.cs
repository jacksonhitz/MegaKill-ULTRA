using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour
{
    //For now this only exists for the trigger from TUTORIAL -> TANGO
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" && StateManager.State == StateManager.GameState.TUTORIAL)
        {
            StateManager.LoadState(StateManager.GameState.TANGO);
        }
    }
}
