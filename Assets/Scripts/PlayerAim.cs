using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    
    public Camera cam;
    private Animator playerAnimator;
    private SpriteRenderer playerSprite;

    [System.NonSerialized]
    public PlayerGun gun;
    [System.NonSerialized]
    public float angle;
    [System.NonSerialized]
    public Vector2 mousePos;

    private InputMaster controls;

    [System.NonSerialized]
    public Vector2 aimDir;
    Vector2 movement;
    private void Start() {
        playerAnimator = this.GetComponentInParent<Animator>();
        playerSprite = this.GetComponent<SpriteRenderer>();
        controls = this.GetComponent<Player>().controls;
    }

    private void Update() {
        if(cam)
            Aim();  
        UpdateState();
    }
    
    void Aim(){
        
        // Perspective camera mode only
        //mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.transform.position.z));
        
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        aimDir = mousePos - (Vector2)transform.position;
        angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;

        if(gun){
            if(gun.isAuto){
                if(controls.Player.Shoot.ReadValue<float>() == 1){
                    gun.Fire();
                }
            } else if(controls.Player.Shoot.triggered) {
                //controls.Player.Shoot.started += _ => 
                gun.Fire();
            }
            
            /*if(Input.GetButtonDown("Reload")){
                gun.Reload();
                StartCoroutine(gun.Reload());
            }*/

            if(controls.Player.Reload.triggered){
                StartCoroutine(gun.Reload());
            }
        }
    }

    void UpdateState(){
        if(67.5f >= angle && angle > 22.5f) {
            playerAnimator.SetInteger("orientation", 1);
        } else if(22.5f >= angle && angle > -22.5f) {
            playerAnimator.SetInteger("orientation", 0);
        } else if(-22.5f >= angle && angle > -67.5f) {
            playerAnimator.SetInteger("orientation", 1);
        } else if(-67.5f >= angle && angle > -112.5f) {
            playerAnimator.SetInteger("orientation", 2);
        } else if(-112.5f >= angle && angle > -157.5f) {
            playerAnimator.SetInteger("orientation", 3);
        } else if(-157.5f >= angle && angle > -202.5f) {
            playerAnimator.SetInteger("orientation", 4);
        } else if(-202.5f >= angle && angle > -247.5f) {
            playerAnimator.SetInteger("orientation", 3);
        } else if(-247.5f >= angle || angle > 67.5f) {
            playerAnimator.SetInteger("orientation", 2);
        }

        if(0 >= angle && angle > -180) {
            playerSprite.flipX = false;
        } else {
            playerSprite.flipX = true;
        }

        
    }
    
}
