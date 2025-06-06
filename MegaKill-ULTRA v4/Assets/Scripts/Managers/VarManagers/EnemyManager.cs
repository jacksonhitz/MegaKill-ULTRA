using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [SerializeField] GameObject hands;
    [SerializeField] GameObject enemyHolder;
    public List<Enemy> enemies;
    float spawnInterval = 5f;
    PlayerController player;

    void Awake()
    {
        Instance = this;

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
        if (StateManager.IsActive())
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
        Destroy(enemy.gameObject);

        if (ScenesManager.Instance.lvlType == ScenesManager.WinCon.Elim && enemies.Count == 0)
            StartCoroutine(StateManager.LoadState(StateManager.GameState.SCORE, 2f));
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
            spawnPos.y -= 4f;
            Instantiate(hands, spawnPos, Quaternion.identity);
        }
    }
}