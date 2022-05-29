using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject player;
    public enum SpawnState {TopSpawn, BtmSpawn}
    public SpawnState SpawnLocal = SpawnState.TopSpawn;
    [Space(10)]
    public GameObject[] playerTopSpawn;
    public GameObject[] playerBtmSpawn;

    public void SpawnPlayer()
    {
        if(SpawnLocal == SpawnState.TopSpawn)
        {
            int ind = Random.Range(0, playerTopSpawn.Length);
            Instantiate(player, playerTopSpawn[ind].transform.position, Quaternion.identity);
        }
        if(SpawnLocal == SpawnState.BtmSpawn)
        {
            int ind = Random.Range(0, playerBtmSpawn.Length);
            Instantiate(player, playerBtmSpawn[ind].transform.position, Quaternion.identity);
        }
    }
}
