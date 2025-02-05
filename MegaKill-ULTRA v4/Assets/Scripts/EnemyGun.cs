using UnityEngine;
using System.Collections;

public class EnemyGun : MonoBehaviour
{
    float reloadMag = 40f;
    float reloadSpd = 2f;
    float fireRate = 2f;
    float bulletSpd = 50f;
    float range = 1000f;

    public GameObject projectilePrefab;
    public Transform firePoint;
    public TrailRenderer tracerPrefab;

    private Vector3 rot = Vector3.zero;
    private Vector3 originalRot;
    Vector3 originalPos;

    SoundManager soundManager;
    CamController cam;
    Enemy enemy;
    GameObject player;
    bool isAttacking = false;

    AudioSource sfx;
    public AudioClip gunshot;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        sfx = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        cam = FindObjectOfType<CamController>();
        soundManager = FindObjectOfType<SoundManager>();
        

        fireRate = Random.Range(1f, 3f);
    }

    void Update()
    {
        if (enemy.los && InRange() && !isAttacking)
        {
            StartCoroutine(CallAttack());
        }
    }

    IEnumerator CallAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(fireRate);
        if (enemy.enabled)
        {
            Attack();
        }
        isAttacking = false;
    }

    public bool InRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    void Recoil()
    {
        rot += new Vector3(-reloadMag, 0, 0f);
    }

    void Attack()
    {
        soundManager.EnemySFX(sfx, gunshot);

        Vector3 targetDir = (player.transform.position - firePoint.position).normalized;

        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = targetDir * bulletSpd;

        TrailRenderer tracer = Instantiate(tracerPrefab, firePoint.position, Quaternion.identity);
        StartCoroutine(HandleTracer(tracer, bullet));
    }

    IEnumerator HandleTracer(TrailRenderer tracer, GameObject bullet)
    {
        while (bullet != null)
        {
            tracer.transform.position = bullet.transform.position;
            yield return null;
        }

        yield return new WaitForSeconds(tracer.time);

        Destroy(tracer.gameObject);
    }
}