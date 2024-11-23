using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnPhasePanel : BasePanel
{
    public void ShowTurnPhase(string name)
    {
        turnPhaseText.text = name;
        turnPhaseAnimator.Play("TurnPhasePanelShow");
    }
    public Animator turnPhaseAnimator;
    public TextMeshProUGUI turnPhaseText;
}
