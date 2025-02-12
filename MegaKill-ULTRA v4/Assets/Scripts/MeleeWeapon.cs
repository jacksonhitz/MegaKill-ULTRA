using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeWeapon : MonoBehaviour
{
    public Transform weapon;
    public LayerMask enemyLayer;
    public Animator animator;
    public PlayerController player;
    public SphereCollider hitbox;
    SoundManager soundManager;
    GameManager gameManager;
    bool isSwinging;

    void Start()
    {
        Debug.Log("MeleeWeapon Start called");
        soundManager = FindObjectOfType<SoundManager>();
        gameManager = FindObjectOfType<GameManager>();
        animator.SetBool("Swing", false);
        isSwinging = false;
    }

    public void Attack()
    {
        Debug.Log("Attack called");
        if (!isSwinging)
        {
            Debug.Log("Starting swing animation");
            animator.SetBool("Swing", true);
            StartCoroutine(Swing(true));
        }
    }

    IEnumerator Swing(bool playSound = false)
    {
        Debug.Log("Swing coroutine started");
        isSwinging = true;
        yield return new WaitForSeconds(0.3f);
        Hit();
        if (playSound)
        {
            Debug.Log("Playing bat swing sound");
            soundManager.BatSwing();
        }
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Swing", false);
        isSwinging = false;
        Debug.Log("Swing coroutine ended");
    }

    void Hit()
    {

        Collider[] colliders = Physics.OverlapSphere(hitbox.bounds.center, hitbox.radius * hitbox.transform.lossyScale.x);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("NPC"))
            {
                Enemy enemy = collider.transform.parent?.parent?.GetComponent<Enemy>();
                if (enemy == null)
                {
                    enemy = collider.transform.GetComponent<Enemy>();
                }
                enemy?.Hit();
            }
        }
    }
}
