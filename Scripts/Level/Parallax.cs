using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Parallax : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.3f;
    [SerializeField] GameObject viewtarget;

    Tilemap tileMap;

    private void Start()
    {
        tileMap = GetComponent<Tilemap>();
    }

    private void Update()
    {
        float newXPos = (viewtarget.transform.position.x + 47.8f) * scrollSpeed;

        tileMap.transform.position = new Vector3(newXPos, tileMap.transform.position.y, tileMap.transform.position.z);
    }
}
