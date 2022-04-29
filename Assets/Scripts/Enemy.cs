using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public int health;
    //Don't remove. Part of Bullet.cs behaviour
    public GameObject hitEffect;

    public GameObject deathEffect;
    public float deathEffectLength;

    private void Update()
    {
        UpdateState();
    }

    public void TakeDamage(int damage)
    {
        this.health -= damage;

        if(health <= 0){
            Die();
        }
    }

    void Die()
    {
        if(deathEffect){
            Instantiate(deathEffect, this.transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject, deathEffectLength);
    }

    void UpdateState()
    {
    }
}
