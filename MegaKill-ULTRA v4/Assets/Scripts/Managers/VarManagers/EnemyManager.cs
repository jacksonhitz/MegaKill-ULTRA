using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public static event System.Action<Enemy> OnDeath;

    //Instance tracking/singleton management
    private static EnemyManager _instance;
    public static EnemyManager Instance
    {
        get
        {
            if (_instance is null) Debug.LogError("Enemy Manager is NULL");
            return _instance;
        }
    }

    [SerializeField] GameObject hands;
    [SerializeField] GameObject enemyHolder;
    public List<Enemy> enemies;
    float spawnInterval = 20f;
    PlayerController player;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        player = FindObjectOfType<PlayerController>();
        enemies = new List<Enemy>();
    }
    void Start()
    {
        Active();
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
        Active();
    }

    void Active()
    {
        enemyHolder.SetActive(true);
        CallHands();
        CollectEnemies();
    }
    void CollectEnemies()
    {
        enemies.Clear(); 
        enemies.AddRange(FindObjectsOfType<Enemy>()); 
    }

    public void Brawl()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy.dosed)
            {
                enemy.currentState = Enemy.EnemyState.Brawl;
                //enemy.StartCoroutine(enemy.StartBrawlAggression());

            }
        }

        List<Enemy> nonDosedEnemies = new List<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            if (!enemy.dosed)
            {
                nonDosedEnemies.Add(enemy);
            }
        }

        int numToConvert = Mathf.CeilToInt(nonDosedEnemies.Count * 0.5f);

        for (int i = 0; i < nonDosedEnemies.Count; i++)
        {
            Enemy temp = nonDosedEnemies[i];
            int randomIndex = Random.Range(i, nonDosedEnemies.Count);
            nonDosedEnemies[i] = nonDosedEnemies[randomIndex];
            nonDosedEnemies[randomIndex] = temp;
        }
        //for (int i = 0; i < numToConvert; i++)
        //{
        //    nonDosedEnemies[i].currentState = Enemy.EnemyState.Brawl;
        //}
    }


    public void Kill(Enemy enemy)
    {
        enemies.Remove(enemy);
        OnDeath?.Invoke(enemy);
        Destroy(enemy.gameObject);
    }


    public void CallHands()
    {
       // StartCoroutine(SpawnHands());
    }

    IEnumerator SpawnHands()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            Vector3 spawnPos = player.transform.position;
            spawnPos.y -= 1f;
            Instantiate(hands, spawnPos, Quaternion.identity);
        }
    }
}