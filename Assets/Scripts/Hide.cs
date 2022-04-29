using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    public float hideSpeed = 1;
    public float maxHidingTime = 5f;
    public float timeThreshold = 0.4f;
    
    private InputMaster controls;
    float lastHid;
    SpriteRenderer[] elements;
    [System.NonSerialized]
    public bool isHiding = false;
    bool depleted = false;
    float fade = 1f;
    float hidingTime;

    void Start()
    {
        hidingTime = maxHidingTime;
        AddItems();
        controls = this.GetComponent<Player>().controls;
    }

    public void AddItems(){
        elements = this.GetComponentsInChildren<SpriteRenderer>();
    }
    
    void Update()
    {
        /*if(Input.GetButtonDown("Hide")){
            isHiding = changeMode();
        }*/

        if(controls.Player.Hide.triggered){
            isHiding = changeMode();
        }
            
        if(isHiding){
            fade -= hideSpeed * Time.deltaTime;

            if(fade <= 0f){
                hidingTime -= Time.time - lastHid;
                fade = 0f;
                this.gameObject.tag = "Invisible";
            }

            for(int i=0; i<elements.Length; i++){
                elements[i].material.SetFloat("_Fade", fade);
            }

            if(hidingTime >= maxHidingTime*timeThreshold){
                depleted = false;
            }

            if(hidingTime <= 0f){
                hidingTime = 0f;
                depleted = true;
                isHiding = false;
            }
        } else {
            fade += hideSpeed * Time.deltaTime;
            
            if(fade >= .5f){
                this.gameObject.tag = "Player";
            }

            if(fade >= 1f){
                fade = 1f;
                hidingTime += Time.time - lastHid;
            }

            for(int i=0; i<elements.Length; i++){
                elements[i].material.SetFloat("_Fade", fade);
            }

            if(hidingTime >= maxHidingTime*timeThreshold){
                depleted = false;
            }

            if(hidingTime >= maxHidingTime){
                hidingTime = maxHidingTime;
            }
        }
        lastHid = Time.time;
    }
 
    bool changeMode(){
        if(isHiding){
            return false;
        } else{
            if(depleted){
                return false;
            }
            return true;
        }
    }

}
