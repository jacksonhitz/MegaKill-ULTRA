using System.Collections;
using UnityEngine;

public class EnemyDog : Enemy
{
    
    protected override void DropItem(){}
    protected override void Start()
    {
        attackRange = 2f;
        attackRate = 2f;
        dmg = 10f;
    }
    protected override void CallAttack()
    {
        StartCoroutine(Attack());
    }
    IEnumerator Attack()
    {
        animator.SetTrigger("Atk");
        soundManager.EnemySFX(sfx, attackClip);
        player.health.Hit(dmg);
        soundManager.EnemySFX(sfx, attackClip);

        yield break;
    }
}