using System.Collections;
using UnityEngine;

public class Melee : MonoBehaviour
{
    float fireRate = 1f;
    float range = 2f;
    Enemy enemy;
    PlayerController player;

    public Animator animator;
    bool isAttacking = false;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        player = FindObjectOfType<PlayerController>(); 
    }

    void Update()
    {
        if (enemy.los && InRange() && !isAttacking)
        {
            StartCoroutine(CallAttack());
        }
    }

    bool InRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    IEnumerator CallAttack()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.2f);
        Attack();
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("isAttacking", false);
        isAttacking = false;
    }

    void Attack()
    {
        player.Hit();
    }
}