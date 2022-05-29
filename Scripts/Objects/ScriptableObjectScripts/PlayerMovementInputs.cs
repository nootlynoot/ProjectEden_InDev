using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerControl", menuName = "PlayerInputs")]
public class PlayerMovementInputs : ScriptableObject
{
    public string HorizontalMovementAxis;
    public string VerticalMovementAxis;
    [Space(10)]
    public string HorizontalAimingAxis;
    public string VerticalAimingAxis;
    [Space(10)]
    public string JumpButton;
    public string SpecialMovementButton;
    [Space(10)]
    public string PrimaryAttackButton;
    public string SecondaryAttackButton;
    public string UltimateAttackButton;
}
