using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    public enum CollectType { Money, Ammo, Coolant, Essence }
    public CollectType PickupType = CollectType.Money;
    [Space(10)]
    public int minAmt;
    public int maxAmt;
    public float despawnTime;
    [Space(10)]
    [Header("Drop")]
    public float upForce;
    public float sideForce;


    private void Start()
    {
        Destroy(gameObject, despawnTime);
        float xForce = Random.Range(-sideForce, sideForce);
        float yForce = Random.Range(upForce / 2f, upForce);
        Vector3 force = new Vector2(xForce, yForce);

        GetComponent<Rigidbody2D>().velocity = force;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<MilitaryPlayerCon>() != null)
        {
            MilitaryPlayerCon player = other.GetComponent<MilitaryPlayerCon>();
            int Amt = Random.Range(minAmt, maxAmt);
            player.AmmoCount += Amt;
            Destroy(gameObject);
        }
    }
}
