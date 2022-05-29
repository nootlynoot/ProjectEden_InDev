using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChainLightning : MonoBehaviour
{
    ReligiousPlayerCon RPC;
    Rigidbody2D rb;
    Transform myTarget;
    public OffensiveStatsClass LightningStats;
    [SerializeField] private float speed;
    [Space(10)]
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layerMask;
    List<Transform> targets = new List<Transform>();
    protected bool hit;
    [SerializeField] private bool entered;
    protected int targetIndex;
    [Space(10)]
    [SerializeField] private int chainCount;
    public int maxChainCount;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        RPC = FindObjectOfType<ReligiousPlayerCon>();
        myTarget = RPC.lightningTarget;
        transform.LookAt(myTarget);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (myTarget != null)
        {
            move();
        }
        else
        {
            RPC.lightningTarget = null;
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        if (chainCount == maxChainCount)
        {
            RPC.lightningTarget = null;
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        float distance = 0;
        if (myTarget != null)
        {
            distance = Vector2.Distance(transform.position, myTarget.position);
        }
        if (distance <= 0.5f)
        {
            //if hit a target and there are still more targets to go to
            if (hit && targetIndex < targets.Count && chainCount < maxChainCount)
            {
                pickTarget(myTarget.GetComponent<Collider2D>());
            }
            else if (myTarget != null)
            {
                if (entered)
                {
                    Targets t = myTarget.gameObject.GetComponent<Targets>();
                    speed = 0;
                    t.TakeDamage(LightningStats.Damage);
                    rb.velocity = Vector2.zero;
                    myTarget = null;
                }
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        entered = true;
        if (!hit && other.tag == "target" && other.transform == myTarget)
        {
            hit = true;

            Collider2D[] tmp = Physics2D.OverlapCircleAll(other.transform.position, radius, layerMask);
            foreach (Collider2D collider in tmp)
            {
                if(collider.transform != myTarget && collider.transform != transform && collider.tag == "target")
                {
                    targets.Add(collider.transform);
                }
            }
            pickTarget(other);
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "target")
        {
            entered = false;
        }
    }

    void pickTarget(Collider2D other)
    {
        chainCount += 1;
        Targets t = other.GetComponent<Targets>();
        t.TakeDamage(LightningStats.Damage);
        myTarget = targets[targetIndex];
        targetIndex++;
    }

    private void OnDrawGizmos()
    {
        Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, radius);
    }

    void move()
    {
        Vector2 dir = myTarget.position - transform.position;
        rb.velocity = dir.normalized * speed;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
