using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReligiousInputDisplay : MonoBehaviour
{
    ReligiousPlayerCon RPC;
    public Sprite up, down, left, right;
    Image SR;
    [Range(1, 3)] public int imageInd;
    // Start is called before the first frame update
    void Start()
    {
        if(FindObjectOfType<ReligiousPlayerCon>() != null)
        {
            RPC = FindObjectOfType<ReligiousPlayerCon>();
        }
        SR = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<ReligiousPlayerCon>() != null)
        {
            updateSprites();
        }
    }

    void updateSprites()
    {
        if(imageInd == 1)
        {
            switch (RPC.firstCast)
            {
                case "castup":
                    SR.sprite = up;
                    break;
                case "castdown":
                    SR.sprite = down;
                    break;
                case "castleft":
                    SR.sprite = left;
                    break;
                case "castright":
                    SR.sprite = right;
                    break;
                case "":
                    SR.sprite = null;
                    break;
            }
        }
        if (imageInd == 2)
        {
            switch (RPC.secondCast)
            {
                case "castup":
                    SR.sprite = up;
                    break;
                case "castdown":
                    SR.sprite = down;
                    break;
                case "castleft":
                    SR.sprite = left;
                    break;
                case "castright":
                    SR.sprite = right;
                    break;
                case "":
                    SR.sprite = null;
                    break;
            }
        }
        if (imageInd == 3)
        {
            switch (RPC.thirdCast)
            {
                case "castup":
                    SR.sprite = up;
                    break;
                case "castdown":
                    SR.sprite = down;
                    break;
                case "castleft":
                    SR.sprite = left;
                    break;
                case "castright":
                    SR.sprite = right;
                    break;
                case "":
                    SR.sprite = null;
                    break;
            }
        }
    }
}
