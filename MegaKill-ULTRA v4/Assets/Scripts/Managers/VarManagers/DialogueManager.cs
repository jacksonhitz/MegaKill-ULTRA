using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    DialogueData[] lines;

    void Awake()
    {
        Instance = this;

        lines = Resources.LoadAll<DialogueData>("Dialogue");
        Debug.Log("Loaded Dialogues: " + lines.Length);
    }

    public void PlayRandomLine()
    {
       // var matching = System.Array.FindAll(lines, d => d.trip == TripManager.Instance.trip);
       // string line = matching[Random.Range(0, matching.Length)].line;
       // Dialogue.Instance.TypeText(line, 5f);
    }
}
