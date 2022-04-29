using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyPathfinding : MonoBehaviour
{
    GameObject[] players;
    GameObject[] bulletShells;
    
    public Vector3 currentTarget;
    public GameObject targetPlayer;
    public List<string> disregardStates = new List<string>(){"Invisible", "Dead"};
    public float speed = 4f;
    public float targetDistance = 10f;
    public float updatePathRate = 1f;
    public float reachTargetDistance = .8f;
    public float listeningShotsDistance = 20f;
    [System.NonSerialized]
    public bool reachedTarget;
    float nextWaypointDistance = .5f;
    Path path;
    int currentWaypoint = 0;

    public float idleTime = 4f;
    public float idleDistance = 4f;
    float lastIdle;

    Vector2 direction;
    Seeker seeker;
    Rigidbody2D rb;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, updatePathRate);
    }

    void UpdatePath()
    {
        if(currentTarget != null){
            if(seeker.IsDone()){
                seeker.StartPath(rb.position, currentTarget, OnPathComplete);
            }
        }
    }

    void OnPathComplete(Path p)
    {
        if(!p.error){
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if(!targetPlayer){
            LookForPlayers();
        } else {
            if(disregardStates.Contains(targetPlayer.tag)){
                targetPlayer = null;
            } else {
                currentTarget = targetPlayer.transform.position;
            }
        }
        CheckCurrentPath();
    }

    void Idle(){
        if(Time.time > idleTime + lastIdle){
            int count = 0;
            while(true){
                Vector3 randomPos = new Vector3(rb.position.x + Random.Range(-idleDistance, idleDistance), rb.position.y + Random.Range(-idleDistance, idleDistance), 0f);
                if(Physics2D.OverlapCircleAll(randomPos, 2f).Length == 0){
                    currentTarget = randomPos;
                    break;
                } else {
                    count++;
                }
                if(count >= 5){
                    return;
                }
            }
            lastIdle = Time.time + Random.Range(-1f, 1f);
        }
    }

    void LookForPlayers(){
        players = GameObject.FindGameObjectsWithTag("Player");
        bulletShells = GameObject.FindGameObjectsWithTag("BulletShell");
        for(int i=0; i<players.Length; i++){
            if(Vector2.Distance(players[i].transform.position, rb.position) <= targetDistance){
                if(disregardStates.Contains(players[i].tag)){
                    continue;
                } else {
                    targetPlayer = players[i];
                    return;
                }
            } 
            
            PlayerGun gun = players[i].GetComponentInChildren<PlayerGun>();
            if(gun){
                if(gun.isShooting){
                    for(int j=0; j<bulletShells.Length; j++){
                        if(Vector2.Distance(bulletShells[i].transform.position, rb.position) < listeningShotsDistance){
                            currentTarget = bulletShells[i].transform.position;
                            return;
                        }
                    }
                }
            }
        }
        Idle();
    }

    void CheckCurrentPath()
    {
        if(path == null || currentWaypoint >= path.vectorPath.Count){
            return;
        } else if(Vector2.Distance(rb.position, currentTarget) < reachTargetDistance){
            reachedTarget = true;
            direction = Vector2.zero;
            return;
        } else {
            reachedTarget = false;
            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
            //rb.AddForce(direction * speed * 100 * Time.deltaTime);
            //rb.velocity = direction * speed * 50 * Time.deltaTime;
            if(distance <= nextWaypointDistance){
                currentWaypoint++;
            }
        }
    }
}
