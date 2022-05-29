using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseCannon : MonoBehaviour
{
    public OutriderPlayerCon OPC;
    public GameObject shotpointPos;
    public GameObject shotpointend;
    LineRenderer LR;
    public int maxLineRange;
    public Transform LaserHit;
    public PolygonCollider2D PC2D;
    public LayerMask HitLayers;
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
            Targets t = other.GetComponent<Targets>();
            t.TakeDamage(OPC.PC.UltimateAttack.Damage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, maxLineRange, HitLayers);
        Debug.DrawLine(transform.position, hit.point);
        LaserHit.position = hit.point;
        LR.SetPosition(0, shotpointPos.transform.position);
        LR.SetPosition(1, shotpointend.transform.position);

        if (OPC.WC.isBeaming)
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
