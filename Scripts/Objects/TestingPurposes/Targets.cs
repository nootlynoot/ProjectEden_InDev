using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targets : MonoBehaviour
{
    public float health = 20;

    public GameObject[] Drops;

    private void Update()
    {
        if(health <= 0)
        {
            Dieded();
        }
    }

    void Dieded()
    {
        gameObject.SetActive(false);
        for (int i = 0; i < Drops.Length; i++)
        {
            Instantiate(Drops[i],transform.position,Quaternion.identity);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
