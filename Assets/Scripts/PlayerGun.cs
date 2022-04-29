using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [Header("General")]
    public int totalAmmo = 300;
    public int magAmmo = 30;
    public float reloadTime = 5f;
    public bool isAuto;
    public Transform firePoint;
    public float fireRate = 3f;
    public SpriteRenderer gunSprite;
    public Sprite alternateSprite;
    [System.NonSerialized]
    public Animator animator;
    private float lastShot;
    private float angle;

    [Header("Raycasting")]
    //Use these for raycasting
    public bool isRayGun;
    public int rayGunDamage;
    public int rayMaxDistance;
    public string[] layers;
    public GameObject rayGunHitEffect;
    public float rayGunHitEffectLength;
    public GameObject laserEffect;
    public float laserEffectLength;

    [Header("Colliders")]
    //And these for object based
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public Transform ejectPoint;
    public GameObject bulletShellPrefab;
    public float shellDespawnTime = 10f;


    public GameObject magPrefab;
    public float magDespawnTime = 20f;

    int tempMax;
    int tempAmmo;
    [System.NonSerialized]
    public bool isShooting;
    bool isReloading;
    [System.NonSerialized]
    public Hide hide;

    private void Start() {
        animator = this.GetComponentInChildren<Animator>();
        tempMax = totalAmmo;
        tempAmmo = magAmmo;
        hide = this.GetComponentInParent<Hide>();

        if(!GetComponentInParent<Player>()){
            if(animator){
                animator.enabled = false;
            }

            gunSprite.sortingOrder = 9;
            GetComponent<BoxCollider2D>().enabled = true;
        } else {
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    void Update()
    {
        if(GetComponentInParent<Player>()){
            hide = this.GetComponentInParent<Hide>();
            angle = this.GetComponentInParent<PlayerAim>().angle;
            UpdateState();
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        isShooting = false;
    }

    public void Fire()
    {
        if(tempAmmo > 0 && !isReloading && !hide.isHiding){
            isShooting = true;
            if(Time.time > fireRate + lastShot){
                if(isRayGun){
                    tempAmmo -= 1;
                    GameObject laser = Instantiate(this.laserEffect, firePoint.position, Quaternion.identity);
                    LineRenderer lineRenderer = laser.GetComponent<LineRenderer>();
                    LayerMask mask = LayerMask.GetMask(layers);
                    RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.up, rayMaxDistance, mask);
                    if(hitInfo){
                        lineRenderer.SetPosition(0, firePoint.position);
                        lineRenderer.SetPosition(1, hitInfo.point);
                        Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
                        if(enemy){
                            enemy.TakeDamage(rayGunDamage);
                            if(enemy.hitEffect){
                                GameObject effect = Instantiate(enemy.hitEffect, hitInfo.point, Quaternion.identity);
                                Destroy(effect, rayGunHitEffectLength);
                            } else {
                                DefaultHit(hitInfo.point);
                            }
                        } else {
                            DefaultHit(hitInfo.point);
                        }
                    } else {
                        lineRenderer.SetPosition(0, firePoint.position);
                        lineRenderer.SetPosition(1, firePoint.up + firePoint.up * 100);
                    }
                    Destroy(laser, laserEffectLength);
                    
                } else {
                    tempAmmo -= 1;
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
                    bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed, ForceMode2D.Impulse);
                    if(bulletShellPrefab){
                        GameObject shell = Instantiate(bulletShellPrefab, ejectPoint.position, ejectPoint.rotation);
                        shell.GetComponent<Rigidbody2D>().AddForce(ejectPoint.right * 5f, ForceMode2D.Impulse);
                        shell.GetComponent<Rigidbody2D>().centerOfMass -= new Vector2(0f, -0.1f);
                        Destroy(shell, shellDespawnTime);
                    }
                }
                lastShot = Time.time;
            }
        }
    }

    public IEnumerator Reload(){
        if(tempMax > 0 && tempAmmo < magAmmo && !isReloading){
            isReloading = true;
            if(magPrefab){
                GameObject mag = Instantiate(magPrefab, ejectPoint.position, ejectPoint.rotation);
                mag.GetComponent<Rigidbody2D>().AddForce(-ejectPoint.right * 5f, ForceMode2D.Impulse);
                Destroy(mag, magDespawnTime);
            }

            yield return new WaitForSeconds(reloadTime);

            tempMax -= magAmmo - tempAmmo;
            tempAmmo = magAmmo;
            isReloading = false;
        }
    }

    void DefaultHit(Vector2 hitpoint){
        GameObject effect = Instantiate(rayGunHitEffect, hitpoint, Quaternion.identity);
        Destroy(effect, rayGunHitEffectLength);
    }

    void UpdateState() {
        if(0 >= angle && angle > -180) {
            gunSprite.flipY = false;
        } else {
            gunSprite.flipY = true;
        }

        if(22.5f >= angle && angle > -22.5) {
            gunSprite.sortingOrder = 9;
        } else {
            gunSprite.sortingOrder = 11;
        }

        if(animator){
            if(isShooting){
                animator.SetBool("isShooting", true);
            } else {
                animator.SetBool("isShooting", false);
            }

            if(isReloading){
                animator.SetBool("isReloading", true);
            } else {
                animator.SetBool("isReloading", false);
            }
        }
    }
}
