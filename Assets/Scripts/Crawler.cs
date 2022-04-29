using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : Enemy
{
    public int attackDamage;
    public float attackRate;
    public float attackDuration;
    public float attackKnockBack;
    private float lastAttack;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    Vector2 direction;
    EnemyPathfinding pathfinding;
    Rigidbody2D targetRb;
    bool isAttacking;

    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        animator = this.GetComponent<Animator>();
        pathfinding = this.GetComponent<EnemyPathfinding>();
    }

    private void Update()
    {
        UpdateState();
        if(pathfinding.targetPlayer){
            if(!targetRb)
                targetRb = pathfinding.targetPlayer.GetComponent<Rigidbody2D>();
            
            if(pathfinding.reachedTarget && pathfinding.targetPlayer.tag == "Player"){
                if(Vector2.Distance(pathfinding.targetPlayer.transform.position, transform.position) <= pathfinding.reachTargetDistance){
                    if(!isAttacking)
                        StartCoroutine(Attack());
                }
            }
        } else {
            targetRb = null;
        }
    }

    private IEnumerator Attack()
    {
        if(Time.time > attackRate + lastAttack){
            isAttacking = true;
            yield return new WaitForSeconds(attackDuration);
            isAttacking = false;
            
            if(pathfinding.targetPlayer){
                pathfinding.targetPlayer.GetComponent<Player>().TakeDamage(attackDamage);
                lastAttack = Time.time;
                if(attackKnockBack > 0){
                    if(targetRb)
                        targetRb.AddForce(direction * attackKnockBack, ForceMode2D.Impulse);
                }
            }
            
        }
    }

    void UpdateState()
    {
        if(Vector2.Distance(pathfinding.currentTarget, transform.position) >= pathfinding.reachTargetDistance + 0.1f){
            animator.SetBool("moving", true);
            direction = (pathfinding.currentTarget - transform.position).normalized;

            if(direction.x > 0){
                spriteRenderer.flipX = true;
            } else {
                spriteRenderer.flipX = false;
            }

            if(direction.y > 0){
                animator.SetBool("goingDown", false);
            } else {
                animator.SetBool("goingDown", true);
            }
        } else {
            animator.SetBool("moving", false);

            if(isAttacking){
                animator.SetBool("isAttacking", true);
            } else {
                animator.SetBool("isAttacking", false);
            }
        }
    }
}
