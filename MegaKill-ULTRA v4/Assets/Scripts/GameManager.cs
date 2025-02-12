using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int phase;

    public int score = 0;

    Volume volume;
    UX ux;
    GameObject player;
    SoundManager soundManager;
    Tutorial tutorial;

    List<NPCAI> civs;
    List<Enemy> enemies;

    public Pointer pointer;
    public CamController cam;
    public Material camMat;

    float currentLerp;
    float currentFrequency;
    float currentAmplitude;
    float currentSpeed;

    public bool fadeOut;

    public bool isIntro = false;

    public GameObject bat;
    public GameObject cia;
    public GameObject black;

    public Dialogue intro;



    void Awake()
    {
        civs = new List<NPCAI>(FindObjectsOfType<NPCAI>()); 
        enemies = new List<Enemy>(FindObjectsOfType<Enemy>()); 
        soundManager = FindObjectOfType<SoundManager>(); 
        volume = FindObjectOfType<Volume>(); 
        ux = FindObjectOfType<UX>(); 
        tutorial = FindObjectOfType<Tutorial>(); 
        intro = FindObjectOfType<Dialogue>(); 
        player = GameObject.FindGameObjectWithTag("Player");

        camMat.SetFloat("_Lerp", currentLerp);
        camMat.SetFloat("_Frequency", currentFrequency);
        camMat.SetFloat("_Amplitude", currentAmplitude);
        camMat.SetFloat("_Speed", currentSpeed);
    }

    void Start()
    {
        intro.CallDialogue();
    }

    public void Tutorial()
    {
        isIntro = false;
        StartCoroutine(FlashIn());
    }

    IEnumerator FlashIn()
    {
        black.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        black.SetActive(false);
        yield return new WaitForSeconds(0.15f);
        black.SetActive(true);
        cia.SetActive(false);
        bat.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        black.SetActive(false);
        ux.gameObject.SetActive(true);
        tutorial.CallDialogue();
    }

    public void ScareNPCs()
    {
        foreach (NPCAI civ in civs)
        {
            civ.CallFlee(player.transform.position);
        }
    }
    void Update()
   {
       if (!fadeOut)
       {
           float currentLerp = camMat.GetFloat("_Lerp");
           float capLerp = 0.0125f * phase;
           float currentFrequency = camMat.GetFloat("_Frequency");
           float capFrequency = 1.25f * phase;
           float currentAmplitude = camMat.GetFloat("_Amplitude");
           float capAmplitude = 0.025f * phase;
           float currentSpeed = camMat.GetFloat("_Speed");
           float capSpeed = 0.025f * phase;


           if (currentLerp < capLerp)
           {
               currentLerp += 0.00001f;
               camMat.SetFloat("_Lerp", currentLerp);
           }
           if (currentFrequency < capFrequency)
           {
               currentFrequency += 0.001f;
               camMat.SetFloat("_Frequency", currentFrequency);
           }
           if (currentAmplitude < capAmplitude)
           {
               currentAmplitude += 0.00002f;
               camMat.SetFloat("_Amplitude", currentAmplitude);
           }
           if (currentSpeed < capSpeed)
           {
               currentSpeed += 0.00002f;
               camMat.SetFloat("_Speed", currentSpeed);
           }
       }
       else
       {
           float accLerp = camMat.GetFloat("_Lerp");
           float accFrequency = camMat.GetFloat("_Frequency");


           accLerp += 0.0025f;
           accFrequency += 0.025f;


           camMat.SetFloat("_Lerp", accLerp);
           camMat.SetFloat("_Frequency", accFrequency);
       }
   }

    IEnumerator UpPhase()
    {
        while (true)
        {
            phase++;
            Debug.Log("Phase: " + phase);
            yield return new WaitForSeconds(30f);
        }
    }

    public void Score(int newScore)
    {
        Debug.Log("score");

        score += newScore;
        
        ux.Score();
        ux.PopUp(newScore);
    }
}
