using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayGun : MonoBehaviour
{
    public Transform firePoint;
    public float fireRate = 3f;
    public SpriteRenderer gunSprite;
    private float lastShot;
    private float angle;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        angle = this.GetComponentInParent<PlayerAim>().angle;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        UpdateState();
    }

    public void Fire()
    {
        Debug.Log("Fire!");
        if(Time.time > fireRate + lastShot){
            RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.up);

            if(hitInfo){
                Debug.Log(hitInfo.transform.name);
            }
            lastShot = Time.time;
        }
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
    }
}
