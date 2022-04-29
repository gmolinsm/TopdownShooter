using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private InputMaster controls;
    private Vector2 direction;
    private Rigidbody2D rb_player;
    private Animator animator;
    Rigidbody2D[] allChildren;

    void Start() {
        rb_player = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        controls = this.GetComponent<Player>().controls;
    }

    void Update(){
        direction = controls.Player.Move.ReadValue<Vector2>();
        UpdateState();
    }
    void FixedUpdate() {
        if(direction != Vector2.zero){
            movePlayer();
        }
    }

    void movePlayer(){
        allChildren = GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D child in allChildren)
        {
            child.MovePosition(rb_player.position + direction * speed * Time.deltaTime);
        }
    }

    void UpdateState(){
        if(direction.y != 0 || direction.x != 0){
            animator.SetBool("movement", true);
        } else {
            animator.SetBool("movement", false);            
        }
    }
}
