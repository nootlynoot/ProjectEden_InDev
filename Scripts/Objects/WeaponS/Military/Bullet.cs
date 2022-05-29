using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject
{
    public float speed;
    public float Dmg;
    //MilitaryPool MP;

    public void FixedUpdate()
    {
        OnObjectSpawn();
    }

    public void OnObjectSpawn()
    {
        transform.Translate(new Vector2(1f, 0f) * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("target"))
        {
            Targets target = other.gameObject.GetComponent<Targets>();
            target.TakeDamage(Dmg);
            gameObject.SetActive(false);
        }
    }
}
