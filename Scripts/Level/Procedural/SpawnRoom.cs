using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : MonoBehaviour
{
    public LayerMask whatIsRoom;
    public LevelGen LG;

    // Update is called once per frame
    void Update()
    {
        Collider2D roomDetect = Physics2D.OverlapCircle(transform.position, 1, whatIsRoom);
        if (roomDetect == null && LG.stopGen == true)
        {
            //SPAWN RAND ROOM
            int rand = Random.Range(0, LG.rooms.Length);
            Instantiate(LG.rooms[rand], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
