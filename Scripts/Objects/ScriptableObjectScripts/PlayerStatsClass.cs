using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStatus", menuName ="Players")]
public class PlayerStatsClass : ScriptableObject
{
    public enum PlayerType { Military, Religious, Political, Outrider, Summons, Default}
    public PlayerType Playertype = PlayerType.Military;
    [Space(10)]
    public int PlayerHealth;

    public float PlayerSpeed;

    //To Add Player abilities to the list after making the script;
}
