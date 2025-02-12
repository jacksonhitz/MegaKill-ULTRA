using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string[] lines;
    public float textSpeed;

    int index;
    bool waiting;
    GameManager gameManager;

    public enum State
    {
        None,
        Start,
        WASD,
        Jump,
        Slow,
        Grab,
        Swap,
        Kill,
        Reload,
        Off
    }

    bool passed;

    public State currentState;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void States(State newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case State.WASD:
                Debug.Log("Tutorial: WASD");
                break;
            case State.Jump:
                Debug.Log("Tutorial: Jump");
                break;
            case State.Grab:
                Debug.Log("Tutorial: Grab");
                break;
            case State.Slow:
                Debug.Log("Tutorial: Slow");
                break;
            case State.Swap:
                Debug.Log("Tutorial: Swap");
                break;
            case State.Kill:
                Debug.Log("Tutorial: Kill");
                break;
            case State.Reload:
                Debug.Log("Tutorial: Reload");
                break;
            case State.Off:
                Debug.Log("Tutorial: Off");
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && waiting)
        {
            waiting = false;
            NextLine();
        }
    }
    public void CallDialogue()
    {
        StartCoroutine(DelayDialogue());
    }
    IEnumerator DelayDialogue()
    {
        yield return new WaitForSeconds(1.5f);
        StartDialogue();
    }
    public void StartDialogue()
    {
        index = 0;
        text.text = "";
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(1f);
        waiting = true;
        if(index == 0)
        {
            NextLine();
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            text.text = "";
            StartCoroutine(TypeLine());
        }
    }
}
