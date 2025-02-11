using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SoundManager : MonoBehaviour
{
    public enum GameSpeed
    {
        Regular,
        Slow
    }

    public AudioSource music;
    public AudioSource sfx;
    public AudioSource enemySfx;

    public AudioClip title;
    public AudioClip acid;
    public AudioClip witch;
    public AudioClip hott;
    public AudioClip threes;
    public AudioClip life;
    public AudioClip real;
    public AudioClip could;
    public AudioClip dj;
    public AudioClip four;
    public AudioClip all;

    public AudioClip revShot;
    public AudioClip revReload;
    public AudioClip revEmpty;

    public AudioClip shotShot;
    public AudioClip shotReload;
    public AudioClip shotEmpty;
    
    public AudioClip batSwing;

    public AudioClip meleeSquelch;
    public AudioClip rangedSquelch;

    public AudioClip heartbeat;
    public AudioClip flatline;

    public AudioClip squelch;

    public GameManager gameManager;
    public bool controller;

    private List<AudioClip> tracks;
    private int trackIndex = 0;

    public GameSpeed currentSpeed = GameSpeed.Regular;

    private float originalMusicVolume;
    private float originalSfxVolume;
    private float originalEnemySfxVolume;

    private bool shouldMuteSounds = false;
    private bool isFlatlining = false;

    private static SoundManager instance;

    void Awake()
    {
        Debug.Log("SoundManager Awake called");
        
        if (instance != null && instance != this)
        {
            Debug.Log("Destroying duplicate SoundManager");
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        gameManager = FindObjectOfType<GameManager>();
        tracks = new List<AudioClip> { acid, witch, could, dj, all, hott, threes, life, real, four };

        originalMusicVolume = music.volume;
        originalSfxVolume = sfx.volume;
        originalEnemySfxVolume = enemySfx.volume;
    }

    void OnEnable()
    {
        Debug.Log("SoundManager OnEnable - Adding scene load callback");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        Debug.Log("SoundManager OnDisable - Removing scene load callback");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        music.volume = originalMusicVolume;
        sfx.volume = originalSfxVolume;
        enemySfx.volume = originalEnemySfxVolume;

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Debug.Log("In title scene, playing title music");
            music.clip = title;
            music.Play();
        }
    }

    public void EnemySFX(AudioSource source, AudioClip sound)
    {
        if (!shouldMuteSounds)
        {
            source.clip = sound;
            source.volume = originalEnemySfxVolume;
            source.pitch = currentSpeed == GameSpeed.Slow ? 0.75f : 1f;
            source.Play();  
        }
    }

    public void Stop()
    {
        music.Stop();
    }

    public void NewTrack()
    {
        trackIndex = (trackIndex + 1) % tracks.Count;
        music.clip = tracks[trackIndex];
        music.Play();

        music.volume = (music.clip == dj || music.clip == four) ? 0.5f : 0.1f;
    }

    public void RevShot() => PlaySfx(revShot);
    public void RevReload() => PlaySfx(revReload);
    public void RevEmpty() => PlaySfx(revEmpty);

    public void ShotShot() => PlaySfx(shotShot);
    public void ShotReload() => PlaySfx(shotReload);
    public void ShotEmpty() => PlaySfx(shotEmpty);
    public void BatSwing() => PlaySfx(batSwing);

    public void Heartbeat() => PlaySfx(heartbeat);
    
    public void Flatline()
    {
        if (isFlatlining) return;
        
        isFlatlining = true;
        shouldMuteSounds = true;
        
        sfx.volume = 0f;
        enemySfx.volume = 0f;
        
        sfx.clip = flatline;
        sfx.volume = originalSfxVolume;
        sfx.Play();
    }

    void PlaySfx(AudioClip clip)
    {
        if (!shouldMuteSounds || clip == flatline)
        {
            sfx.clip = clip;
            sfx.volume = originalSfxVolume;
            sfx.Play();
        }
    }

    private IEnumerator MuteAfterFlatline()
    {
        yield return new WaitForSeconds(sfx.clip.length);
    
        sfx.volume = 0f;
    }

    private void StopAllSounds()
    {
        if (sfx.isPlaying && sfx.clip != flatline)
        {
        sfx.Stop();
        }
        if (enemySfx.isPlaying)
        {
        enemySfx.Stop();
        }
    
    sfx.volume = 0f;
    enemySfx.volume = 0f;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}, BuildIndex: {scene.buildIndex}");
        
        ResetMute();
        
        if (scene.buildIndex == 0)
        {
            Debug.Log("Loading title scene, playing title music");
            music.Stop(); 
            music.clip = title;
            music.Play();
        }
    }

    public void ResetMute()
    {
        shouldMuteSounds = false;
        isFlatlining = false;
        sfx.volume = originalSfxVolume;
        enemySfx.volume = originalEnemySfxVolume;
    }
    
    public void SetSpeed(GameSpeed speed)
    {
        currentSpeed = speed;

        float pitchValue = speed == GameSpeed.Slow ? 0.75f : 1f;
        music.pitch = pitchValue;
        sfx.pitch = pitchValue;
        enemySfx.pitch = pitchValue;
    }
}
