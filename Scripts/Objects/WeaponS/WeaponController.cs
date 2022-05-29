using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class WeaponController : MonoBehaviour
{
    [HideInInspector]
    public float dirX = 0f;
    [HideInInspector]
    public float dirY = 0f;
    public PlayerCon PC;
    public SummonCon SC;
    public bool isSummon;
    public bool isReligious;
    public bool isBeaming;
    Player player;
    public int playerId;
    [Space(10)]
    public float rotateSpeed = 10f;

    private void Awake()
    {
        playerId = PC.PMC.playerId;
        player = ReInput.players.GetPlayer(playerId);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReligious)
        {
            getInput();
        }
        if (isReligious)
        {
            if (isBeaming)
            {
                getInput();
            }
            else
            {
                dirX = 0;
                dirY = 0;
            }
        }
    }

    void getInput()
    {
        dirX = player.GetAxisRaw("Aim X") * Time.deltaTime * rotateSpeed;
        dirY = player.GetAxisRaw("Aim Y") * Time.deltaTime * rotateSpeed;
    }

    public void RotateAll()
    {
        float angle = Mathf.Atan2(dirX, dirY) * Mathf.Rad2Deg;
        if (PC.PMC.sprite.flipX)
        {
            if (angle >= 0)
            {
                PC.PMC.flipRight();
                //angle = 0;
            }
            if (angle <= -180)
            {
                PC.PMC.flipRight();
                //angle = -180;
            }
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 180, -(angle + 90)));
        }
        else
        {
            if (angle >= 180)
            {
                PC.PMC.flipLeft();
                //angle = 180;
            }
            if (angle <= 0)
            {
                PC.PMC.flipLeft();
                //angle = 0;
            }
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 0, angle - 90));
        }
        //print(angle);
    }
    public void RotateR()
    {
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 180, transform.rotation.z));
    }

    public void RotateL()
    {
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 0, transform.rotation.z));
    }
}
