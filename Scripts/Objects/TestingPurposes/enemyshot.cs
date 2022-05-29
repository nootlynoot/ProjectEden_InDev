using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyshot : MonoBehaviour
{
    public float speed;
    public int dmg;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(new Vector2(1f, 0f) * -speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCon PC = other.GetComponent<PlayerCon>();
            PC.TookDmg();
            Destroy(gameObject);
        }
    }
}
