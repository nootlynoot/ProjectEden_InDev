using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class HomingRocket : MonoBehaviour, IPooledObject
{
    public enum RocketStates { standby, lockedon, fire, roaming }
    public RocketStates RocketState = RocketStates.standby;
    MilitaryPlayerCon MPC;
    public OffensiveStatsClass RocketStats;
    Vector2 startPos;
    [Space(10)]
    public List<Transform> enemies = new List<Transform>();
    [Space(10)]
    public float lookAtOffset;
    public float rotateSpeed;
    public float speed;
    [Space(10)]
    public AIDestinationSetter DS;
    public AIPath PA;
    private void Start()
    {
        MPC = FindObjectOfType<MilitaryPlayerCon>();
        StartCoroutine(RocketMovement());
        startPos = transform.position;
        //locateEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        rocketMain();
    }

    void rocketMain()
    {
        switch (RocketState)
        {
            case RocketStates.standby:
                locateEnemies();
                break;
            case RocketStates.lockedon:
                lookTowardsEnemies();
                break;
            case RocketStates.fire:
                lookTowardsEnemies();
                OnObjectSpawn();
                adjustTarget();
                break;
            case RocketStates.roaming:
                roaming();
                break;
        }

        if(enemies.Count <= 0){
            RocketState = RocketStates.roaming;
        }
    }

    void adjustTarget()
    {
        if(!enemies[0].gameObject.activeInHierarchy)
        {
            enemies.RemoveAt(0);
        }
    }

    void locateEnemies()
    {
        enemies.Clear();
        enemies.AddRange(MPC.storedTargets);
        transform.position += transform.up * Time.deltaTime * (speed-7);
        //enemies.RemoveAt(0);
    }

    void lookTowardsEnemies()
    {
        Vector3 targetPos = enemies[0].position;
        Vector3 thisPos = transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle + lookAtOffset)), rotateSpeed*Time.deltaTime);
    }

    public void OnObjectSpawn()
    {
        DS.target = enemies[0];
    }

    void roaming(){
        DS.enabled = false;
        PA.enabled = false;
        transform.position += transform.right *Time.deltaTime * speed;
        Destroy(gameObject,3);
    }

    IEnumerator RocketMovement()
    {
        RocketState = RocketStates.standby;
        yield return new WaitForSeconds(1f);
        RocketState = RocketStates.lockedon;
        yield return new WaitForSeconds(1f);
        RocketState = RocketStates.fire;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("target"))
        {
            Targets target = other.gameObject.GetComponent<Targets>();
            target.TakeDamage(RocketStats.Damage);
            gameObject.SetActive(false);
        }
    }
}
