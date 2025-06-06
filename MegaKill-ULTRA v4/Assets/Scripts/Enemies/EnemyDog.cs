using System.Collections;
using UnityEngine;

public class EnemyDog : Enemy
{
    
    protected override void DropItem(){}
    protected override void Start()
    {
        base.Start();

        attackRange = 2f;
        attackRate = 2f;
        dmg = 10f;

        currentState = EnemyState.Active;
        target = PlayerController.Instance.gameObject;
    }

    protected override void CallAttack()
    {
        StartCoroutine(Attack());
    }
    IEnumerator Attack()
    {
        animator.SetTrigger("Atk");

        PlayerController.Instance.health.Hit(dmg);
        SoundManager.Instance.Play("Bite");

        yield break;
    }
}