using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StateManager
{
    public enum GameState
    {
        TITLE,
        TUTORIAL,
        REHEARSAL,
        TANGO,
        TANGO2,
        SABLE,
        SPEARHEAD,

        TRANSITION,
        FILE,
        SCORE
    }

    public static GameState state;
    public static GameState previous;
    public static GameState lvl;

    public static GameState PREVIOUS => previous;
    public static GameState STATE => state;
    public static GameState LVL => lvl;

    public static event Action<GameState> OnStateChanged;
    public static event Action<GameState> OnSilentChanged;

    static readonly List<GameState> Order = new()
    {
        GameState.TITLE,
        GameState.TUTORIAL,
        GameState.REHEARSAL,
        GameState.TANGO,
        GameState.TANGO2,
        GameState.SABLE,
        GameState.SPEARHEAD
    };

    static readonly HashSet<GameState> Scene = new()
    {
        GameState.TITLE,
        GameState.TUTORIAL,
        GameState.REHEARSAL,
        GameState.TANGO,
        GameState.SABLE,
        GameState.SPEARHEAD,
    };

    static readonly HashSet<GameState> Transition = new()
    {
        GameState.TITLE,
        GameState.TUTORIAL,
        GameState.REHEARSAL,
        GameState.TANGO,
        GameState.SABLE,
        GameState.SPEARHEAD,
        GameState.SCORE
    };

    static readonly HashSet<GameState> Active = new()
    {
        GameState.TUTORIAL,
        GameState.REHEARSAL,
        GameState.TANGO,
        GameState.TANGO2,
        GameState.SABLE,
        GameState.SPEARHEAD,
    };

    static readonly HashSet<GameState> Passive = new()
    {
        GameState.TITLE,
        GameState.FILE,
        GameState.TRANSITION,
        GameState.SCORE
    };

    public static GameState State
    {
        get => state;
        set
        {
            previous = state;
            state = value;

            OnStateChanged?.Invoke(state);
            Debug.Log($"State changed to {state}");
        }
    }
    public static GameState SilentState
    {
        get => state;
        set
        {
            previous = state;
            state = value;

            OnSilentChanged?.Invoke(state);
            Debug.Log($"State changed to {state}");

        }
    }

    public static bool GroupCheck(HashSet<GameState> group) => group.Contains(state);

    public static bool IsActive() => GroupCheck(Active);
    public static bool IsPassive() => GroupCheck(Passive);
    public static bool IsScene() => GroupCheck(Scene);


    //this is the worst fucking thing ive ever made
    public static IEnumerator LoadState(GameState newState, float delay)
    {
        if (Transition.Contains(newState))
        {
            State = GameState.TRANSITION;
            yield return new WaitForSeconds(delay);
        }

        if (newState == GameState.SCORE)
        {
            State = newState;
            yield break;
        }

        if (Scene.Contains(newState))
        {
            Debug.Log("Loading Scene: " + newState);
            var loadingOperation = SceneManager.LoadSceneAsync(newState.ToString(), LoadSceneMode.Single);
            while (!loadingOperation?.isDone ?? true) 
            {
                yield return null;
            }
            if (Active.Contains(newState) && lvl != newState)
            {
                State = GameState.FILE;
                lvl = newState;
            }
        }
        else 
            State = newState;
    }
    public static void StartLvl() => State = lvl;

    public static void NextState(MonoBehaviour caller)
    {
        int currentIndex = Order.IndexOf(state);
        int nextIndex = (currentIndex + 1) % Order.Count;
        SettingsManager.Instance.StartCoroutine(LoadState(Order[nextIndex], 2f)); //TEMP FIX THIS IS FUCKED
    }
}
