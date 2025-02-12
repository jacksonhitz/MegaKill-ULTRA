using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject playerObj;
    PlayerController player;
    GameManager gameManager;
    SoundManager soundManager;


    float detectionRange = 30f;
    float pathfindingRange = 30f;
    float attackRange;
    

    bool detectedPlayer = false;
    public bool los = false;
    bool isFiring = false;

    public GameObject blood;
    public GameObject weaponPrefab; 
    public AudioClip rangedSquelch;
    public AudioClip meleeSquelch;

    AudioSource sfx;
    public Animator animator;
    public GameObject model;

    public bool ranged;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = FindAnyObjectByType<PlayerController>();
        sfx = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
        soundManager = FindObjectOfType<SoundManager>();
        
        if (ranged)
        {
            attackRange = Random.Range(10f, 20f);
        }
        else
        {
            attackRange = 0;
        }
    }

    void Update()
    {
        if (player.hasWeapon)
        {
            LOS();
        }
        animator.SetFloat("spd", agent.velocity.magnitude);
        
        float distanceToPlayer = Vector3.Distance(transform.position, playerObj.transform.position);

        if (distanceToPlayer < attackRange && los)
        {
            agent.ResetPath();
            Vector3 targetPosition = playerObj.transform.position;
            Vector3 directionToPlayer = targetPosition - transform.position;

            Vector3 lookDirection = new Vector3(directionToPlayer.x, 0, directionToPlayer.z);
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
        else if (distanceToPlayer <= pathfindingRange && detectedPlayer)
        {
            Vector3 targetPosition = playerObj.transform.position;
            Vector3 directionToPlayer = targetPosition - transform.position;

            Vector3 lookDirection = new Vector3(directionToPlayer.x, 0, directionToPlayer.z);
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            float clampedX = Mathf.Clamp(transform.position.x + directionToPlayer.x, targetPosition.x, targetPosition.x);
            float clampedZ = Mathf.Clamp(transform.position.z + directionToPlayer.z, targetPosition.z, targetPosition.z);

            Vector3 destination = new Vector3(clampedX, transform.position.y, clampedZ);
            agent.SetDestination(destination);
        }
        else
        {
            agent.ResetPath();
        }
    }

    void LOS()
    {
        Vector3 direction = (playerObj.transform.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        LayerMask layerMask = LayerMask.GetMask("Ground", "Player");

        if (Physics.Raycast(ray, out hit, detectionRange, layerMask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                los = true;
                detectedPlayer = true;
            }
            else
            {
                los = false;
            }
        }
    }

    public void Hit()
    {
        Instantiate(blood, transform.position, Quaternion.identity);

        if(ranged)
        {
            soundManager.EnemySFX(sfx, rangedSquelch);
        }
        else
        {
            soundManager.EnemySFX(sfx, meleeSquelch);
        }
        
        model.SetActive(false);
        enabled = false;
        gameManager.Score(100);

        StartCoroutine(Dead());
    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

}

    //public void Hit()
    //{
        //Instantiate(blood, transform.position, Quaternion.identity);
        //if (ranged)
        //{
            //soundManager.PlayRangedDeath();
        //}
        //else
        //{
            //soundManager.PlayMeleeDeath();
        //}
    
        //model.SetActive(false);
        // = false;
        //gameManager.Score(100);

        //StartCoroutine(Dead());
    //}
//}   
