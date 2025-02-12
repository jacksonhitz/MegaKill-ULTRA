using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI title;
    public TextMeshProUGUI creditsBox;
    public string[] lines;
    public float textSpeed;

    int index;
    bool started = false;
    bool waiting = false;
    GameManager gameManager;

    public bool intro;
    public bool tutorial;
    public bool passed;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (!tutorial)
        {
            passed = true;
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !started && !intro)
        {
            started = true;
            textComponent.text = string.Empty;
            creditsBox.text = string.Empty;
            title.text = string.Empty;
            StartDialogue();
        }

        if (Input.GetKeyDown(KeyCode.Return) && waiting)
        {
            waiting = false;
            NextLine();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.C) && !started)
        {
            //SceneManager.LoadScene(2);
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(1f);
        waiting = true;
        NextLine();
    }

    void NextLine()
    {
        if (index < lines.Length - 1 && passed)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());

            if (tutorial)
            {
                passed = false;
            }
        }
        else
        {
            StartCoroutine(LoadNext());
        }
    }

    IEnumerator LoadNext()
    {
        yield return new WaitForSeconds(1f);

        if (intro)
        {
            textComponent.text = string.Empty;
            gameManager.Tutorial();
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
