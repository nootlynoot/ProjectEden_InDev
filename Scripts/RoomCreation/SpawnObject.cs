using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject[] objs;

    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, objs.Length);
        GameObject instance = (GameObject)Instantiate(objs[rand], transform.position, Quaternion.identity);
        instance.transform.parent = transform;
    }
}
