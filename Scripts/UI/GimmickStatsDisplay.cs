using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GimmickStatsDisplay : MonoBehaviour
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
        if (LM.ActivePlayers.Length != 0)
        {
            if (LM.ActivePlayers[LM.ActivePlayers.Length - 1].gameObject.GetComponent<MilitaryPlayerCon>() != null)
            {
                MilitaryPlayerCon MP = LM.ActivePlayers[LM.ActivePlayers.Length - 1].gameObject.GetComponent<MilitaryPlayerCon>();
                DebugStats.text = ($"Gimmick Status\n{MP.AmmoCount}");
            }
        }
    }
}
