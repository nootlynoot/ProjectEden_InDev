using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatusDisplay : MonoBehaviour
{
    public LevelManager LM;

    public TMP_Text DebugStats;

    // Update is called once per frame
    void Update()
    {
        DisplayStatus();
    }

    void DisplayStatus()
    {
        if(LM.ActivePlayers.Length != 0)
        {
            DebugStats.text = ($"Player Status\nClassType:{LM.ActivePlayers[LM.ActivePlayers.Length - 1].PSC}\nHealth:{LM.ActivePlayers[LM.ActivePlayers.Length - 1].HP}\nMOVEMENT STATUS:{LM.ActivePlayers[LM.ActivePlayers.Length - 1].PMC.MoveState}\n{LM.ActivePlayers[LM.ActivePlayers.Length - 1].PMC.IsGrounded()}\n{LM.ActivePlayers[LM.ActivePlayers.Length - 1].PMC.MoveState}\n{LM.ActivePlayers[LM.ActivePlayers.Length - 1].PMC.rb.velocity}\nLOADOUT STATUS:\nPirmaryAttack:{LM.ActivePlayers[LM.ActivePlayers.Length - 1].PrimaryAttack}\nSecondaryAttack:{LM.ActivePlayers[LM.ActivePlayers.Length - 1].SecondaryAttack}\nUltimateAttack:{LM.ActivePlayers[LM.ActivePlayers.Length - 1].UltimateAttack}");
        }
    }
}
