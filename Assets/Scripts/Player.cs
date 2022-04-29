using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;
    public GameObject hitEffect;
    public float hitEffectLength;
    public GameObject deathEffect;
    public float deathEffectLength;
    private Animator animator;

    [System.NonSerialized]
    public InputMaster controls;

    private void Awake() {
        controls = new InputMaster();    
    }

    private void Start() {
    }

    public void TakeDamage(int damage){
        this.health -= damage;

        if(hitEffect){
            GameObject effect = Instantiate(this.hitEffect, this.transform.position, Quaternion.identity);
            Destroy(effect, hitEffectLength);
        }

        if(health <= 0){
            health = 0;
            Die();
        }
    }

    void Die(){
        this.gameObject.SetActive(false);
        if(deathEffect){
            Instantiate(deathEffect, this.transform.position, Quaternion.identity);
            Destroy(deathEffect, deathEffectLength);
        }
        this.gameObject.tag = "Dead";
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }
}
