using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnPhasePanel : BasePanel
{
    public void ShowTurnPhase(string name)
    {
        turnPhaseText.text = name;
        turnPhaseAnimator.SetBool("Turn Phase Show", true);
        Timer.Register(1f, () =>
        {
            turnPhaseAnimator.SetBool("Turn Phase Show", false);
        });
    }
    public Animator turnPhaseAnimator;
    public TextMeshProUGUI turnPhaseText;
}
