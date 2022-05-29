using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGen : MonoBehaviour
{
    public Transform[] startingPos;
    public GameObject[] rooms; // ind0 = LR, ind1 = LRB, ind2 = LRT, ind3 = LRTB
    [Space(10)]
    int dir;
    public float moveDAmt;
    public float moveAmt;
    [Space(10)]
    float timeBtRoom;
    public float startTimeBtRoom;
    [Space(10)]
    public float minX;
    public float maxX;
    public float minY;
    public bool stopGen;
    [Space(10)]
    public LayerMask room;
    [Space(10)]
    public PlayerSpawn PS;
    [Space(10)]
    int downCounter;

    // Start is called before the first frame update
    void Start()
    {
        int randStartPos = Random.Range(0, startingPos.Length);
        transform.position = startingPos[randStartPos].position;
        Instantiate(rooms[0], transform.position, Quaternion.identity);

        dir = Random.Range(1, 8);
    }

    void Update()
    {
        if(timeBtRoom <= 0 && !stopGen)
        {
            Move();
            timeBtRoom = startTimeBtRoom;
        }
        else
        {
            timeBtRoom -= Time.deltaTime;
        }
    }

    void Move()
    {
        if (dir == 1 || dir == 2 || dir == 3) //Move RIGHT
        {
            if (transform.position.x < maxX)
            {
                downCounter = 0;
                Vector2 newPos = new Vector2(transform.position.x + moveAmt, transform.position.y);
                transform.position = newPos;

                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                dir = Random.Range(1, 8);
                if(dir == 4 || dir == 5)
                {
                    dir = 2;
                }
                else if(dir == 6)
                {
                    dir = 7;
                }
            }
            else
            {
                dir = 7;
            }

        }else if (dir == 4 || dir == 5 || dir == 6) //Move LEFT
        {
            if (transform.position.x > minX)
            {
                downCounter = 0;
                Vector2 newPos = new Vector2(transform.position.x - moveAmt, transform.position.y);
                transform.position = newPos;

                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                dir = Random.Range(4, 8);
            }
            else
            {
                dir = 7;
            }

        }else if(dir == 7) //Move DOWN
        {
            downCounter ++;

            if (transform.position.y > minY)
            {
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
                if(roomDetection.GetComponent<RoomType>().type != 1 && roomDetection.GetComponent<RoomType>().type != 4)
                {
                    if (downCounter >=2)
                    {
                        roomDetection.GetComponent<RoomType>().RoomDestory();
                        Instantiate(rooms[3], transform.position, Quaternion.identity);
                    }
                    else
                    {
                        roomDetection.GetComponent<RoomType>().RoomDestory();

                        int randBtmRoom = Random.Range(1, 4);
                        if (randBtmRoom == 2)
                        {
                            randBtmRoom = 1;
                        }
                        Instantiate(rooms[randBtmRoom], transform.position, Quaternion.identity);
                    }
                }

                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveDAmt);
                transform.position = newPos;

                int rand = Random.Range(2, 4);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                dir = Random.Range(1, 6);
            }
            else
            {
                //STOP LEVEL GEN
                stopGen = true;
                PS.SpawnPlayer();
                print("LEVEL GENERATION COMPLETE");
            }
        }
    }
}
