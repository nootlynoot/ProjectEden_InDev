using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectWeaponBtn : MonoBehaviour
{
    public Button thisBtn;
    public OffensiveStatsClass selectedWeapon;
    public int PlayerIndex;
    LevelManager LM;
    // Start is called before the first frame update
    void Start()
    {
        LM = FindObjectOfType<LevelManager>();
    }

    private void OnEnable()
    {
        thisBtn.onClick.AddListener(() => AssignPlayerWeapon(selectedWeapon, PlayerIndex));
    }

    void AssignPlayerWeapon(OffensiveStatsClass OSC, int PlayerInd)
    {
        PlayerCon curPlayer = LM.ActivePlayers[LM.ActivePlayers.Length - PlayerInd];
        switch (selectedWeapon.AttackType)
        {
            case OffensiveStatsClass.OffenceType.Primary:
                curPlayer.PrimaryAttack = selectedWeapon;
                break;
            case OffensiveStatsClass.OffenceType.Secondary:
                curPlayer.SecondaryAttack = selectedWeapon;
                break;
            case OffensiveStatsClass.OffenceType.Ultimate:
                curPlayer.UltimateAttack = selectedWeapon;
                break;
        }
    }

    private void OnDisable()
    {
        thisBtn.onClick.RemoveAllListeners();
    }
}
