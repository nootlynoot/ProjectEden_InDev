using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    public PlayerMovement PMC;
    public PlayerMovementInputs PMI;
    public PlayerStatsClass PSC;
    [Space(10)]
    public OffensiveStatsClass PrimaryAttack;
    public OffensiveStatsClass SecondaryAttack;
    public OffensiveStatsClass UltimateAttack;
    [Space(10)]
    public GameObject WP;
    [Header("Health")]
    public int HP;
    public float invinceWindow;
    // Start is called before the first frame update
    void Start()
    {
        HP = PSC.PlayerHealth;
        PMC = GetComponent<PlayerMovement>();
    }

    public void TookDmg()
    {
        HP -= 1;
        StartCoroutine(invinWindow());
    }

    IEnumerator invinWindow()
    {
        gameObject.tag = "Untagged";
        yield return new WaitForSeconds(invinceWindow);
        gameObject.tag = "Player";
    }
}
