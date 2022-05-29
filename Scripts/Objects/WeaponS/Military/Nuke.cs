using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuke : MonoBehaviour
{
    public string[] tags;

    void Start(){
        Destroy(gameObject,1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        for (int i = 0; i < tags.Length; i++)
        {
            if (other.CompareTag(tags[i]))
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}
