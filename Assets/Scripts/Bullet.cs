using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    public float hitEffectLength;
    public int damage;

    private void OnCollisionEnter2D() {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy"){
            Enemy enemy = other.GetComponent<Enemy>();
            if(enemy){
                enemy.TakeDamage(damage);
                if(enemy.hitEffect){
                    GameObject effect = Instantiate(enemy.hitEffect, transform.position, Quaternion.identity);
                    Destroy(effect, hitEffectLength);
                } else {
                    defaultHit();
                }
            }
        } else {
            defaultHit();
        }
        Destroy(this.gameObject);
    }

    void defaultHit(){
        GameObject effect = Instantiate(this.hitEffect, this.transform.position, Quaternion.identity);
        Destroy(effect, hitEffectLength);
    }
}
