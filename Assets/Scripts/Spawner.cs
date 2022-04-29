using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawn;
    public float safeZoneRadius = 5f;
    public float spawnRate;
    public int maxSpawnCount = 40;

    float lastSpawnTime;
    int spawned = 0;
    GameObject lastSpawn;
    Collider2D[] obstructions;
    // Update is called once per frame

    private void Start() {
        lastSpawn = Instantiate(spawn, this.transform.position, Quaternion.identity);
    }

    void Update()
    {
        if(Time.time > spawnRate + lastSpawnTime && spawned < maxSpawnCount){
            obstructions = Physics2D.OverlapCircleAll(this.transform.position, safeZoneRadius);
            for(int i=0; i<obstructions.Length; i++){
                if(obstructions[i].tag == "Enemy"){
                    return;
                }
            }
            spawn.GetComponent<SpriteRenderer>().sortingOrder += 1;
            Instantiate(spawn, this.transform.position, Quaternion.identity);
            lastSpawn = spawn;
            lastSpawnTime = Time.time;
            spawned++;
        }
    }
}
