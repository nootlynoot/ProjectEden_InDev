using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Block : MonoBehaviour
{
    ChanceBlock Chance;
    public GameObject Object;
    public Sprite[] Artwork;
    private void Start()
    {
        int i = Random.Range(0, Artwork.Length);
        Object.GetComponent<SpriteRenderer>().sprite = Artwork[i];
        if (Chance.isChance)
        {
            int ind = Random.Range(0, Chance.chances);
            if (ind == 1)
            {
                Instantiate(Object, transform.position, Quaternion.identity);
            }
            else
            {
                return;
            }
        }
    }

    public class ChanceBlock : MonoBehaviour
    {
        public bool isChance;
        public int chances;
    }

    [CustomEditor(typeof(ChanceBlock))]
    public class BlockEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            var Props = target as ChanceBlock;
            Props.isChance = GUILayout.Toggle(Props.isChance, "IsChaneBlock");

            if (Props.isChance)
            {
                Props.chances = EditorGUILayout.IntSlider("Chances of Appearing", Props.chances, 2, 10);
            }
        }
    }
}
