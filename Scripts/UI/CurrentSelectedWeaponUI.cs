using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentSelectedWeaponUI : MonoBehaviour
{
    public LevelManager LM;
    public int whichPlayer;
    PlayerCon PC;
    // Start is called before the first frame update
    void Start()
    {
        PC = LM.ActivePlayers[LM.ActivePlayers.Length - whichPlayer];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
