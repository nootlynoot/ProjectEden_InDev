using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectClassButton : MonoBehaviour
{
    public Button thisBtn;
    public PlayerStatsClass selectedClass;
    public int PlayerIndex;
    GameObject playerObject;
    LevelManager LM;

    private void Start()
    {
        LM = FindObjectOfType<LevelManager>();
    }
    private void OnEnable()
    {
        thisBtn.onClick.AddListener(() => AssignPlayerStats(selectedClass, PlayerIndex));
    }

    private void AssignPlayerStats(PlayerStatsClass PSC, int PlayerInd)
    {
        PlayerCon curPlayer = LM.ActivePlayers[LM.ActivePlayers.Length - PlayerInd];
        playerObject = curPlayer.gameObject;
        curPlayer.PSC = PSC;
        RemoveAllMovesets();
        switch (PSC.Playertype)
        {
            case PlayerStatsClass.PlayerType.Military:
                curPlayer.gameObject.AddComponent<MilitaryPlayerCon>();
                break;
            case PlayerStatsClass.PlayerType.Religious:
                curPlayer.gameObject.AddComponent<ReligiousPlayerCon>();
                break;
            case PlayerStatsClass.PlayerType.Political:
                curPlayer.gameObject.AddComponent<PoliticalPlayerCon>();
                break;
            case PlayerStatsClass.PlayerType.Outrider:
                curPlayer.gameObject.AddComponent<OutriderPlayerCon>();
                break;
        }
    }

    private void OnDisable()
    {
        thisBtn.onClick.RemoveAllListeners();
    }

    void RemoveAllMovesets()
    {
        if (playerObject.GetComponent<MilitaryPlayerCon>())
        {
            Destroy(playerObject.GetComponent<MilitaryPlayerCon>());
        }
        if (playerObject.GetComponent<ReligiousPlayerCon>())
        {
            Destroy(playerObject.GetComponent<ReligiousPlayerCon>());
        }
        if (playerObject.GetComponent<PoliticalPlayerCon>())
        {
            Destroy(playerObject.GetComponent<PoliticalPlayerCon>());
        }
        if (playerObject.GetComponent<OutriderPlayerCon>())
        {
            Destroy(playerObject.GetComponent<OutriderPlayerCon>());
        }
    }
}
