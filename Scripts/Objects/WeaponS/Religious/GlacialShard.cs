using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlacialShard : MonoBehaviour
{
    public ReligiousPlayerCon RPC;
    public OffensiveStatsClass glacial;
    public Vector2 startpos;
    Rigidbody2D rb;
    public enum ShardsState { spawned, standby, fire}
    public ShardsState ShardState = ShardsState.spawned;
    public int shardInd;
    public Transform Target;
    public float speed;
    float Ospeed;
    public int shieldValue;
    public bool deducted;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        Ospeed = speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Targets t = other.GetComponent<Targets>();
        if(t != null)
        {
            t.TakeDamage(glacial.Damage);
            gameObject.SetActive(false);
        }
        else
        {
            if (!deducted)
            {
                RPC.PC.HP -= shieldValue;
                deducted = true;
            }
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (ShardState)
        {
            case ShardsState.spawned:
                moveTowards();
                break;
            case ShardsState.standby:
                moveTowards();
                break;
            case ShardsState.fire:
                movetoEnemy();
                break;
        }
    }

    void moveTowards()
    {
        speed = Ospeed;
        transform.position = Vector2.Lerp(transform.position, Target.position, speed * Time.deltaTime);
        float distance = Vector2.Distance(transform.position, Target.position);
        if (distance <= 0.1f)
        {
            ShardState = ShardsState.standby;
        }
    }
    void movetoEnemy()
    {
        rb.velocity = transform.right * speed;
    }
}
