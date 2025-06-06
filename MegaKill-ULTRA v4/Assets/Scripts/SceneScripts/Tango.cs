using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tango : ScenesManager
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] PopUp popUp;

    int dosedCount;
    bool started;

    protected override void Awake()
    {
        base.Awake();
        StateManager.lvl = StateManager.GameState.TANGO;
        if (StateManager.State != StateManager.GameState.FILE)
            StateManager.StartLvl();
    }

    protected override void Update()
    {
        base.Update();

    }

    void Start() //STUPID AND BAD
    {
        StateChange(StateManager.State);
    }

    void OnEnable()
    {
        StateManager.OnStateChanged += StateChange;
    }
    void OnDisable()
    {
        StateManager.OnStateChanged -= StateChange;
    }
    void StateChange(StateManager.GameState state)
    {
        switch (state)
        {
            case StateManager.GameState.TANGO: StartLvl(); break;
            case StateManager.GameState.SCORE: NewsDialogue(); break;
        }
    }

    void StartLvl()
    {
        dialogue.TypeText("F TO GIVE DRUGS", 0f);
    }

    //DOSED WITH MKU
    public override void Interact()
    {
        if (dosedCount == 0)
            dialogue.Off();

        DialogueManager.Instance.PlayRandomLine();

        popUp.UpdatePopUp("MKU DISTRIBUTED");
        dosedCount++;
        Debug.Log("dosed");

        if (dosedCount > 10 && !started)
            StartCoroutine(Countdown());
    }

    //YO WHAT THE HELL WAS I THINKING WHEN I MADE THIS BULLSHIT
    IEnumerator Countdown()
    {
        started = true;

        Info.Instance.TypeText("LADIES AND GENTLEMEN! THE GROOVES WILL START IN 1 MINUTE, MAKE YOUR WAY TO THE MAIN STAGE!", 0f);
        yield return new WaitForSeconds(10f);
        Info.Instance.Off();
        yield return new WaitForSeconds(20f);
        Info.Instance.TypeText("30 SECONDS!", 0f);
        yield return new WaitForSeconds(10f);
        Info.Instance.Off();
        yield return new WaitForSeconds(10f);
        Info.Instance.TypeText("10!", 0f);
        yield return new WaitForSeconds(1f);
        Info.Instance.TypeText("9!", 0f);
        yield return new WaitForSeconds(1f);
        Info.Instance.TypeText("8!", 0f);
        yield return new WaitForSeconds(1f);
        Info.Instance.TypeText("7!", 0f);
        yield return new WaitForSeconds(1f);
        Info.Instance.TypeText("6!", 0f);
        yield return new WaitForSeconds(1f);
        Info.Instance.TypeText("5!", 0f);
        yield return new WaitForSeconds(1f);
        Info.Instance.TypeText("4!", 0f);
        yield return new WaitForSeconds(1f);
        Info.Instance.TypeText("3!", 0f);
        yield return new WaitForSeconds(1f);
        Info.Instance.TypeText("2!", 0f);
        yield return new WaitForSeconds(1f);
        Info.Instance.TypeText("1!", 0f);
        yield return new WaitForSeconds(1f);
        Info.Instance.Off();

        StateManager.State = StateManager.GameState.TANGO2;

        SoundManager.Instance.Play("Magic");
        InteractionManager.Instance.ExtractOn();
        EnemyManager.Instance.Brawl();

        Info.Instance.TypeText("F ON ANY VAN TO EXTRACT", 0f);
    }

    void NewsDialogue()
    {
        dialogue.TypeText("We are just now receiving reports from the authorities that an underground USSR base has been discovered operating out of the abandoned downtown subway system - that's right folks, Reds here on American soil...  ", 2f);
    }
}
