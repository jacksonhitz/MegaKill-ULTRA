using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class EnemyFlyingEye : Enemy
{
    [Header("References")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    Rigidbody rb;

    Vector3 rot = Vector3.zero;
    float moveSpeed = 9f;
    float hoverHeight = 3f;
    float hoverSmoothness = 8f;
    float bulletSpd = 18f;
    float targetAdjust = 0.3f;
    
    protected override void DropItem() {}

    protected override void Start()
    {
        base.Start(); // important!

        rb = GetComponent<Rigidbody>();
        attackRange = 25f;
        attackRate = 5f;
        dmg = 20f;

        currentState = EnemyState.Active;
        target = PlayerController.Instance.gameObject;
    }

    protected override void CallAttack()
    {
        StartCoroutine(Attack());
    }
    protected override void Update()
    {
        if (isDead) return;
        LOS();
        Pathfind();

        // Optional: animate speed using Rigidbody velocity
        if (animator != null)
        {
            float speed = rb.velocity.magnitude;
            animator.SetFloat("Spd", speed);
        }
    }

    void Pathfind()
    {
        if (los && Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            target = player.gameObject;
            LookTowards(player.transform.position);
            StartCoroutine(AttackCheck());

        }
        else if (detectedPlayer && Vector3.Distance(transform.position, player.transform.position) <= detectionRange)
        {
            Vector3 targetPosition = player.transform.position + Vector3.up * hoverHeight;
            Vector3 direction = (targetPosition - transform.position).normalized;
            rb.velocity = new Vector3(direction.x * moveSpeed, Mathf.Lerp(rb.velocity.y, direction.y * moveSpeed, Time.deltaTime * hoverSmoothness), direction.z * moveSpeed);
            LookTowards(player.transform.position);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    IEnumerator Attack()
    {
        Debug.Log("EnemyFlyingEye Attack");
        animator.SetTrigger("Atk");
       // SoundManager.Instance.EnemySFX(sfx, attackClip);

        Vector3 targetPos = player.transform.position;
        targetPos.y += targetAdjust;

        Vector3 targetDir = (targetPos - firePoint.position).normalized;

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        Rigidbody rb = bulletObj.GetComponent<Rigidbody>();

        rb.velocity = targetDir * bulletSpd;

        bullet.vel = bulletSpd;
        bullet.dir = targetDir;
        bullet.dmg = dmg;

        yield break;
    }
}
