using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public ReligiousPlayerCon RPC;
    LineRenderer LR;
    public int maxLineRange;
    public Transform LaserHit;
    public PolygonCollider2D PC2D;
    public LayerMask HitLayers;
    float _Time;
    float startTime;
    float _Time2;
    float startTime2;
    public int healamt;
    public float cDBetweenTicks;
    [SerializeField] List<Collider2D> players = new List<Collider2D>();
    [SerializeField] List<Collider2D> enemies = new List<Collider2D>();
    List<Vector2> colliderPoints = new List<Vector2>();
    // Start is called before the first frame update
    void Start()
    {
        LR = GetComponent<LineRenderer>();
        LR.enabled = false;
        LR.useWorldSpace = true;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("target"))
        {
            if (!enemies.Contains(other))
            {
                enemies.Add(other);
            }
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            if (!players.Contains(other))
            {
                players.Add(other);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_Time > startTime + cDBetweenTicks)
        {
            print("dmgtick");
            foreach (Collider2D enemy in enemies)
            {
                Targets enemyS = enemy.GetComponent<Targets>();
                enemyS.TakeDamage(RPC.PC.UltimateAttack.Damage);
            }
            startTime = _Time;
        }
        if (_Time2 > startTime2 + cDBetweenTicks)
        {
            print("healtick");
            foreach (Collider2D player in players)
            {
                PlayerCon playerS = player.GetComponent<PlayerCon>();
                playerS.HP += healamt;
            }
            startTime2 = _Time2;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("target"))
        {
            if (enemies.Contains(other))
            {
                enemies.Remove(other);
            }
        }
        if (other.gameObject.CompareTag("Player"))
        {
            if (players.Contains(other))
            {
                players.Remove(other);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        _Time = Time.time;
        _Time2 = Time.time;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, maxLineRange, HitLayers);
        Debug.DrawLine(transform.position, hit.point);
        LaserHit.position = hit.point;
        LR.SetPosition(0, transform.position);
        LR.SetPosition(1, LaserHit.position);

        if (RPC.WC.isBeaming)
        {
            LR.enabled = true;
            PC2D.enabled = true;
            colliderPoints = CalculateColliderPoints();
            PC2D.SetPath(0, colliderPoints.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));
        }
        else
        {
            LR.enabled = false;
            PC2D.enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        if (colliderPoints != null) colliderPoints.ForEach(p => Gizmos.DrawSphere(p, 0.1f));
    }

    private List<Vector2> CalculateColliderPoints()
    {
        //Get All positions on the line renderer
        Vector3[] positions = GetPositions();

        //Get the Width of the Line
        float width = GetWidth();

        //m = (y2 - y1) / (x2 - x1)
        float m = (positions[1].y - positions[0].y) / (positions[1].x - positions[0].x);
        float deltaX = (width / 2f) * (m / Mathf.Pow(m * m + 1, 0.5f));
        float deltaY = (width / 2f) * (1 / Mathf.Pow(1 + m * m, 0.5f));

        //Calculate the Offset from each point to the collision vertex
        Vector3[] offsets = new Vector3[2];
        offsets[0] = new Vector3(-deltaX, deltaY);
        offsets[1] = new Vector3(deltaX, -deltaY);

        //Generate the Colliders Vertices
        List<Vector2> colliderPositions = new List<Vector2> {
            positions[0] + offsets[0],
            positions[1] + offsets[0],
            positions[1] + offsets[1],
            positions[0] + offsets[1]
        };

        return colliderPositions;
    }

    public float GetWidth()
    {
        return LR.startWidth;
    }

    public Vector3[] GetPositions()
    {
        Vector3[] positions = new Vector3[LR.positionCount];
        LR.GetPositions(positions);
        return positions;
    } 

}
