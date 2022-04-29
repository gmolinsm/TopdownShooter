using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactRadius;
    private GameObject[] items;
    public float dropThrowSpeed = 5f;
    public int beltSize = 3;
    public List<GameObject> itemBelt;
    private InputMaster controls;
    private int selectedItem;
    private PlayerAim playerAim;


    void Start()
    {
        controls = this.GetComponent<Player>().controls;
        selectedItem = 0;
        playerAim = GetComponent<PlayerAim>();
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if(child.gameObject.GetComponent<PlayerGun>()){
                Equip(child.gameObject);
            }
        }
        if(itemBelt.Count > 0){
            playerAim.gun = itemBelt[selectedItem].GetComponent<PlayerGun>();
        }
    }
    void Update()
    {
        if(controls.Player.Interact.triggered)
            GrabItem();
        if(controls.Player.Drop.triggered)
            DropItem(selectedItem);
        if(controls.Player.SwitchItem.triggered)
            SwitchItem();
    }

    void GrabItem(){
        items = GameObject.FindGameObjectsWithTag("Interact");
        if(items.Length > 0){
            for(int i=0; i<items.Length; i++){
                if(!itemBelt.Contains(items[i])){
                    if(Vector2.Distance(this.transform.position, items[i].transform.position) <= interactRadius){
                        PlayerGun playerGun = items[i].GetComponent<PlayerGun>();
                        if(playerGun){
                            if(itemBelt.Count == 0){
                                // No items: Equip and set as main gun
                                Equip(items[i]);
                                items[i].SetActive(true);
                                playerAim.gun = playerGun;
                            } else if(itemBelt.Count < beltSize){
                                // Items present: Equip only
                                Equip(items[i]);
                                items[i].SetActive(false);
                            } else if(itemBelt.Count >= beltSize){
                                // Items full: Replace with current
                                DropItem(selectedItem);
                                itemBelt[selectedItem] = items[i];
                                itemBelt[selectedItem].SetActive(true);
                                playerAim.gun = playerGun;
                            }
                            playerGun.animator.enabled = true;
                            Hide hide = GetComponent<Hide>();
                            if(hide)
                                hide.AddItems();
                            break;
                        }
                    }
                }
            }
        }
    }

    void DropItem(int itemIndex){
        if(itemBelt.Count > 0){
            if(itemBelt[itemIndex] != null){
                itemBelt[itemIndex].transform.SetParent(null, true);
                itemBelt[itemIndex].GetComponent<Rigidbody2D>().AddForce(playerAim.aimDir * dropThrowSpeed, ForceMode2D.Impulse);
                itemBelt[itemIndex].SetActive(true);
                itemBelt[itemIndex].GetComponent<BoxCollider2D>().enabled = true;
                PlayerGun playerGun = itemBelt[itemIndex].GetComponent<PlayerGun>();
                if(playerGun){
                    playerGun.gunSprite.sortingOrder = 9;
                    playerGun.gunSprite.sprite = playerGun.alternateSprite;
                    playerAim.gun = null;
                    playerGun.animator.enabled = false;
                }
                playerGun.gunSprite.material.SetFloat("_Fade", 1f);
                itemBelt.RemoveAt(itemIndex);
            }
        }
    }

    void Equip(GameObject item){
        item.transform.position = this.transform.position;
        item.transform.SetParent(this.transform);
        item.GetComponent<BoxCollider2D>().enabled = false;
        itemBelt.Add(item);
    }

    void SwitchItem(){
        if(itemBelt.Count > 0){
            if(selectedItem + 1 >= itemBelt.Count){
                itemBelt[itemBelt.Count-1].SetActive(false);
                selectedItem = 0;
            } else {
                itemBelt[selectedItem].SetActive(false);
                selectedItem += 1;
            }

            if(itemBelt[selectedItem] != null)
                itemBelt[selectedItem].SetActive(true);

            PlayerGun playerGun = itemBelt[selectedItem].GetComponent<PlayerGun>();
            if(playerGun)
                playerAim.gun = playerGun;
        }
        
    }
}
